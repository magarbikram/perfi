using CSharpFunctionalExtensions;
using Perfi.Api.Commands;
using Perfi.Api.Exceptions;
using Perfi.Api.Responses;
using Perfi.Core.Accounting;
using Perfi.Core.Accounting.AccountingTransactionAggregate;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.AccountingTransactionAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.Core.Accounts.CreditCardAggregate;
using Perfi.Core.Accounts.LoanAggregate;
using Perfi.Core.Expenses;
using Perfi.Core.MoneyTransfers;
using Perfi.Core.Payments.LoanPayments;
using Perfi.Core.Payments.OutgoingPayments;
using System.Text;

namespace Perfi.Api.Services
{
    public class PayLoanService : IPayLoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IExpenseRepository _expenseRepository;
        private readonly ICashAccountRepository _cashAccountRepository;
        private readonly ICreditCardAccountRepository _creditCardAccountRepository;
        private readonly IAccountingTransactionRepository _accountingTransactionRepository;
        private readonly ITransactionalExpenseCategoryRepository _transactionalExpenseCategoryRepository;
        private readonly IAddOutgoingPaymentService _addOutgoingPaymentService;
        private readonly ILogger<PayLoanService> _logger;
        private readonly ILoanPaymentRepository _loanPaymentRepository;
        private readonly IOutgoingPaymentRepository _outgoingPaymentRepository;

        public PayLoanService(
            ILoanRepository loanRepository,
            IExpenseRepository expenseRepository,
            ICashAccountRepository cashAccountRepository,
            ICreditCardAccountRepository creditCardAccountRepository,
            IAccountingTransactionRepository accountingTransactionRepository,
            ITransactionalExpenseCategoryRepository transactionalExpenseCategoryRepository,
            IAddOutgoingPaymentService addOutgoingPaymentService,
            ILogger<PayLoanService> logger,
            ILoanPaymentRepository loanPaymentRepository,
            IOutgoingPaymentRepository outgoingPaymentRepository)
        {
            _loanRepository = loanRepository;
            _expenseRepository = expenseRepository;
            _cashAccountRepository = cashAccountRepository;
            _creditCardAccountRepository = creditCardAccountRepository;
            _accountingTransactionRepository = accountingTransactionRepository;
            _transactionalExpenseCategoryRepository = transactionalExpenseCategoryRepository;
            _addOutgoingPaymentService = addOutgoingPaymentService;
            _logger = logger;
            _loanPaymentRepository = loanPaymentRepository;
            _outgoingPaymentRepository = outgoingPaymentRepository;
        }

        public async Task<NewLoanPaymentAddedResponse> PayAsync(PayLoanCommand payLoanCommand)
        {
            CashAccount cashAccount = await FindCashAccountById(payLoanCommand.PayWithCashAccountId);
            LoanPayment loanPayment = AddLoanPayment(payLoanCommand, cashAccount);
            RecordOutgoingPaymentOf(loanPayment);
            Maybe<Expense> maybeExpense = RecordExpenseOf(loanPayment, cashAccount);
            AccountingTransaction accountingTransaction = await AddAccoutingTransactionForLoanPaymentAsync(loanPayment);
            SetAccountingTransactionOfExplense(maybeExpense, accountingTransaction);
            await SetAccountingTransactionOfLoanPaymentAsync(loanPayment, accountingTransaction);
            return NewLoanPaymentAddedResponse.From(loanPayment);
        }

        private async Task SetAccountingTransactionOfLoanPaymentAsync(LoanPayment loanPayment, AccountingTransaction accountingTransaction)
        {
            loanPayment.SetAccountingTransaction(accountingTransaction);
            _loanPaymentRepository.Update(loanPayment);
            await _loanPaymentRepository.UnitOfWork.SaveChangesAsync();
        }

        private void SetAccountingTransactionOfExplense(Maybe<Expense> maybeExpense, AccountingTransaction accountingTransaction)
        {
            if (maybeExpense.HasValue)
            {
                maybeExpense.Value.SetAccountingTransaction(accountingTransaction);
                _expenseRepository.Update(maybeExpense.Value);
            }
        }

        private LoanPayment AddLoanPayment(PayLoanCommand payLoanCommand, CashAccount cashAccount)
        {
            LoanPayment loanPayment = LoanPayment.NewMortgagePayment(
                                                    payLoanCommand.LoanId,
                                                    LoanPaymentMethod.From(cashAccount),
                                                    principalAmount: Money.UsdFrom(payLoanCommand.PrincipalAmount),
                                                    interestAmount: GetInterestAmount(payLoanCommand),
                                                    escrowAmount: GetEscrowAmount(payLoanCommand),
                                                    feeAmount: GetFeeAmount(payLoanCommand),
                                                    transactionDate: DateTimeOffset.FromUnixTimeMilliseconds(payLoanCommand.TransactionUnixTimeMilliseconds));


            loanPayment = _loanPaymentRepository.Add(loanPayment);
            return loanPayment;
        }

        private static Money? GetEscrowAmount(PayLoanCommand payLoanCommand)
        {
            return payLoanCommand.EscrowAmount.HasValue ? Money.UsdFrom(payLoanCommand.EscrowAmount.Value) : null;
        }

        private static Money? GetInterestAmount(PayLoanCommand payLoanCommand)
        {
            return payLoanCommand.InterestAmount.HasValue ? Money.UsdFrom(payLoanCommand.InterestAmount.Value) : null;
        }

        private Maybe<Expense> RecordExpenseOf(LoanPayment loanPayment, CashAccount cashAccount)
        {
            if (loanPayment.HasNoExpenseWith())
            {
                return Maybe<Expense>.None;
            }
            Expense expense = Expense.NewCashAccountExpense(description: BuildDebtPaymentExpenseDescription(loanPayment),
                                                                transactionDate: loanPayment.TransactionDate,
                                                                expenseCategoryCode: ExpenseCategoryCode.DebtPayment,
                                                                amount: loanPayment.GetExpenseAmount(),
                                                                cashAccount);
            _expenseRepository.Add(expense);
            return expense;
        }

        private static string BuildDebtPaymentExpenseDescription(LoanPayment loanPayment)
        {
            StringBuilder stringBuilder = new StringBuilder($"Repayment expense of loan with id '{loanPayment.LoanId}'");
            if (loanPayment.InterestAmount != null)
            {
                stringBuilder.Append($" ,Interest({loanPayment.InterestAmount.Value})");
            }
            if (loanPayment.EscrowAmount != null)
            {
                stringBuilder.Append($" ,Escrow({loanPayment.EscrowAmount.Value})");
            }
            if (loanPayment.FeeAmount != null)
            {
                stringBuilder.Append($" ,Fee({loanPayment.FeeAmount.Value})");
            }
            return stringBuilder.ToString();
        }

        private void RecordOutgoingPaymentOf(LoanPayment loanPayment)
        {
            OutgoingPayment outgoingPayment = OutgoingPayment.From(loanPayment);
            _outgoingPaymentRepository.Add(outgoingPayment);
        }

        private Money? GetFeeAmount(PayLoanCommand payLoanCommand)
        {
            if (payLoanCommand.FeeAmount.HasValue)
            {
                return Money.UsdFrom(payLoanCommand.FeeAmount.Value);
            }
            return null;
        }

        private async Task<AccountingTransaction> AddAccoutingTransactionForLoanPaymentAsync(LoanPayment loanPayment)
        {
            Loan mortgageLoan = await FindLoanByIdAsync(loanPayment.LoanId);
            AccountingTransaction accountingTransaction = BuildAccountingTransaction(loanPayment, mortgageLoan);

            accountingTransaction = _accountingTransactionRepository.Add(accountingTransaction);
            _accountingTransactionRepository.Add(accountingTransaction);
            await _accountingTransactionRepository.UnitOfWork.SaveChangesAsync();
            return accountingTransaction;
        }

        private static AccountingTransaction BuildAccountingTransaction(LoanPayment loanPayment, Loan loan)
        {
            List<AccountingEntry> accountingEntries = BuildAccountingEntries(loanPayment, loan);

            AccountingTransaction accountingTransaction = AccountingTransaction.New(transactionDate: loanPayment.TransactionDate,
                                                                                        description: $"Loan payment for '{loan.Id}-{loan.Name}'",
                                                                                        accountingEntries.ToArray());
            return accountingTransaction;
        }

        private static List<AccountingEntry> BuildAccountingEntries(LoanPayment loanPayment, Loan mortgageLoan)
        {
            List<AccountingEntry> accountingEntries = new();
            AccountingEntry creditAccountingEntry = AccountingEntry.Credit(loanPayment.PaymentMethod.CashAccountNumber,
                                                                           loanPayment.TotalPaymentAmount,
                                                                           transactionDate: loanPayment.TransactionDate);
            accountingEntries.Add(creditAccountingEntry);

            AccountingEntry debitPrincipalAmountAccountingEntry = AccountingEntry.Debit(mortgageLoan.AssociatedAccountNumber,
                                                                    loanPayment.PrincipalAmount,
                                                                    loanPayment.TransactionDate);
            accountingEntries.Add(debitPrincipalAmountAccountingEntry);

            if (loanPayment.InterestAmount != null)
            {
                AccountingEntry debitInterestAmountAccountingEntry = AccountingEntry.Debit(TransactionalAccount.DefaultAccountNumbers.GetInterestPaid(),
                                                                    loanPayment.InterestAmount,
                                                                    loanPayment.TransactionDate);
                accountingEntries.Add(debitInterestAmountAccountingEntry);

            }

            if (loanPayment.EscrowAmount != null)
            {
                AccountingEntry debitEscrowAmountAccountingEntry = AccountingEntry.Debit(TransactionalAccount.DefaultAccountNumbers.GetEscrowPaid(),
                                                                   loanPayment.EscrowAmount,
                                                                   loanPayment.TransactionDate);
                accountingEntries.Add(debitEscrowAmountAccountingEntry);
            }


            if (loanPayment.FeeAmount != null)
            {
                AccountingEntry debitFeeAmountAccountingEntry = AccountingEntry.Debit(TransactionalAccount.DefaultAccountNumbers.GetFeePaid(),
                                                                       loanPayment.FeeAmount,
                                                                       loanPayment.TransactionDate);
                accountingEntries.Add(debitFeeAmountAccountingEntry);
            }

            return accountingEntries;
        }

        private async Task<CashAccount> FindCashAccountById(int cashAccountId)
        {
            Maybe<CashAccount> cashAccount = await _cashAccountRepository.GetByIdAsync(cashAccountId);
            if (cashAccount.HasNoValue)
            {
                throw new ResourceNotFoundException($"Cash account with id '{cashAccountId}' not found");
            }
            return cashAccount.Value;
        }

        private async Task<Loan> FindLoanByIdAsync(int loanId)
        {
            Maybe<Loan> maybeLoan = await _loanRepository.GetByIdAsync(loanId);
            if (!maybeLoan.HasValue)
            {
                throw new ResourceNotFoundException($"Loan with id '{loanId}' not found");
            }
            return maybeLoan.Value;
        }
    }
}
