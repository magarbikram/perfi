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
using System.Text;

namespace Perfi.Api.Services
{
    public class PayMortgageService : IPayMortgageService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IExpenseRepository _expenseRepository;
        private readonly ICashAccountRepository _cashAccountRepository;
        private readonly ICreditCardAccountRepository _creditCardAccountRepository;
        private readonly IAccountingTransactionRepository _accountingTransactionRepository;
        private readonly ITransactionalExpenseCategoryRepository _transactionalExpenseCategoryRepository;
        private readonly ILogger<PayMortgageService> _logger;

        public PayMortgageService(
            ILoanRepository loanRepository,
            IExpenseRepository expenseRepository,
            ICashAccountRepository cashAccountRepository,
            ICreditCardAccountRepository creditCardAccountRepository,
            IAccountingTransactionRepository accountingTransactionRepository,
            ITransactionalExpenseCategoryRepository transactionalExpenseCategoryRepository,
            ILogger<PayMortgageService> logger)
        {
            _loanRepository = loanRepository;
            _expenseRepository = expenseRepository;
            _cashAccountRepository = cashAccountRepository;
            _creditCardAccountRepository = creditCardAccountRepository;
            _accountingTransactionRepository = accountingTransactionRepository;
            _transactionalExpenseCategoryRepository = transactionalExpenseCategoryRepository;
            _logger = logger;
        }

        public async Task<NewExpenseAddedResponse> PayAsync(PayMortgageCommand payMortgageCommand)
        {
            Loan mortgageLoan = await FindMortgageLoanByIdAsync(payMortgageCommand.MortgageLoanId);
            Expense expense = await AddNewExpenseAsync(mortgageLoan, payMortgageCommand);
            AccountingTransaction accountingTransaction = await AddAccoutingTransactionAsync(expense, mortgageLoan, payMortgageCommand);
            await SetExpenseAccountingTransaction(expense, accountingTransaction);
            TransactionalExpenseCategory transactionalExpenseCategory = await FindTransactionExpenseCategoryAsync(expense.ExpenseCategoryCode);
            return NewExpenseAddedResponse.From(expense, transactionalExpenseCategory);
        }

        private async Task<TransactionalExpenseCategory> FindTransactionExpenseCategoryAsync(ExpenseCategoryCode expenseCategoryCode)
        {
            Maybe<TransactionalExpenseCategory> maybeTransactionalExpenseCategory = await _transactionalExpenseCategoryRepository.GetByCodeAsync(expenseCategoryCode);
            if (maybeTransactionalExpenseCategory.HasNoValue)
            {
                throw new ResourceNotFoundException($"Expense category with code '{expenseCategoryCode}' not found");
            }
            return maybeTransactionalExpenseCategory.Value;
        }

        private async Task SetExpenseAccountingTransaction(Expense expense, AccountingTransaction accountingTransaction)
        {
            expense.SetAccountingTransaction(accountingTransaction);
            _expenseRepository.Update(expense);
            await _expenseRepository.UnitOfWork.SaveChangesAsync();
        }

        private async Task<AccountingTransaction> AddAccoutingTransactionAsync(Expense expense, Loan mortgageLoan, PayMortgageCommand payMortgageCommand)
        {
            AccountingTransaction accountingTransaction = BuildAccountingTransaction(expense, mortgageLoan, payMortgageCommand);

            accountingTransaction = _accountingTransactionRepository.Add(accountingTransaction);
            _accountingTransactionRepository.Add(accountingTransaction);
            await _accountingTransactionRepository.UnitOfWork.SaveChangesAsync();
            return accountingTransaction;
        }

        private static AccountingTransaction BuildAccountingTransaction(Expense expense, Loan mortgageLoan, PayMortgageCommand payMortgageCommand)
        {
            List<AccountingEntry> accountingEntries = BuildAccountingEntries(expense, mortgageLoan, payMortgageCommand);

            AccountingTransaction accountingTransaction = AccountingTransaction.New(transactionDate: expense.TransactionDate,
                                                                                        description: expense.Description,
                                                                                        accountingEntries.ToArray());
            return accountingTransaction;
        }

        private static List<AccountingEntry> BuildAccountingEntries(Expense expense, Loan mortgageLoan, PayMortgageCommand payMortgageCommand)
        {
            List<AccountingEntry> accountingEntries = new();
            AccountingEntry creditAccountingEntry = AccountingEntry.Credit(expense.PaymentMethod.GetAssociatedAccountNumber(),
                                                                           expense.Amount,
                                                                           transactionDate: expense.TransactionDate);
            accountingEntries.Add(creditAccountingEntry);

            AccountingEntry debitPrincipalAmountAccountingEntry = AccountingEntry.Debit(mortgageLoan.AssociatedAccountNumber,
                                                                    Money.UsdFrom(payMortgageCommand.PrincipalAmount),
                                                                    expense.TransactionDate);
            accountingEntries.Add(debitPrincipalAmountAccountingEntry);

            AccountingEntry debitInterestAmountAccountingEntry = AccountingEntry.Debit(TransactionalAccount.DefaultAccountNumbers.GetInterestPaid(),
                                                                    Money.UsdFrom(payMortgageCommand.InterestAmount),
                                                                    expense.TransactionDate);
            accountingEntries.Add(debitInterestAmountAccountingEntry);

            AccountingEntry debitEscrowAmountAccountingEntry = AccountingEntry.Debit(TransactionalAccount.DefaultAccountNumbers.GetEscrowPaid(),
                                                                    Money.UsdFrom(payMortgageCommand.EscrowAmount),
                                                                    expense.TransactionDate);
            accountingEntries.Add(debitEscrowAmountAccountingEntry);

            if (payMortgageCommand.FeeAmount.HasValue && payMortgageCommand.FeeAmount.Value > 0)
            {
                AccountingEntry debitFeeAmountAccountingEntry = AccountingEntry.Debit(TransactionalAccount.DefaultAccountNumbers.GetFeePaid(),
                                                                       Money.UsdFrom(payMortgageCommand.FeeAmount.Value),
                                                                       expense.TransactionDate);
                accountingEntries.Add(debitFeeAmountAccountingEntry);
            }

            return accountingEntries;
        }

        private async Task<Expense> AddNewExpenseAsync(Loan mortgageLoan, PayMortgageCommand payMortgageCommand)
        {
            Expense expense = await BuildExpenseAsync(mortgageLoan, payMortgageCommand);
            expense = _expenseRepository.Add(expense);
            await _expenseRepository.UnitOfWork.SaveChangesAsync();
            _logger.LogDebug("New Expense with id '{ExpenseId}' added", new { ExpenseId = expense.Id });
            return expense;
        }

        private Task<Expense> BuildExpenseAsync(Loan mortgageLoan, PayMortgageCommand payMortgageCommand)
        {
            if (payMortgageCommand.PaymentMethod.PaymentMethodType == PaymentMethodType.CreditCard)
            {
                return BuildNewExpenseWithCreditCardAsync(mortgageLoan, payMortgageCommand);
            }
            return BuildNewExpenseWithBankCashAccontAsync(mortgageLoan, payMortgageCommand);
        }

        private async Task<Expense> BuildNewExpenseWithBankCashAccontAsync(Loan mortgageLoan, PayMortgageCommand payMortgageCommand)
        {
            CashAccount cashAccount = await FindCashAccountById(payMortgageCommand.PaymentMethod.CashAccountId!.Value);
            Expense expense = Expense.NewCashAccountExpense(
                BuildExpenseDescriptionFor(mortgageLoan, payMortgageCommand),
                transactionDate: DateTimeOffset.FromUnixTimeMilliseconds(payMortgageCommand.TransactionUnixTimeMilliseconds),
                expenseCategoryCode: ExpenseCategoryCode.MortgagePayment,
                amount: CalculateTotalMortgagePayment(payMortgageCommand),
                cashAccount);
            return expense;
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

        private static Money CalculateTotalMortgagePayment(PayMortgageCommand payMortgageCommand)
        {
            Money totalAmount = Money.UsdFrom(payMortgageCommand.PrincipalAmount);
            totalAmount += Money.UsdFrom(payMortgageCommand.InterestAmount);
            totalAmount += Money.UsdFrom(payMortgageCommand.EscrowAmount);
            if (payMortgageCommand.FeeAmount.HasValue && payMortgageCommand.FeeAmount.Value > 0)
            {
                totalAmount += Money.UsdFrom(payMortgageCommand.FeeAmount.Value);
            }
            return totalAmount;
        }

        private static string BuildExpenseDescriptionFor(Loan mortgageLoan, PayMortgageCommand payMortgageCommand)
        {
            StringBuilder description = new StringBuilder($"Mortgage payment of '{mortgageLoan.Name}'");
            description.Append($"Principal (${payMortgageCommand.PrincipalAmount})");
            description.Append($"Interest (${payMortgageCommand.InterestAmount})");
            description.Append($"Escrow (${payMortgageCommand.EscrowAmount})");
            if (payMortgageCommand.FeeAmount.HasValue && payMortgageCommand.FeeAmount.Value > 0)
            {
                description.Append($"Fee (${payMortgageCommand.FeeAmount})");
            }
            return description.ToString();
        }

        private async Task<Expense> BuildNewExpenseWithCreditCardAsync(Loan mortgageLoan, PayMortgageCommand payMortgageCommand)
        {
            CreditCardAccount creditCardAccount = await FindCreditCardAccountByIdAsync(payMortgageCommand.PaymentMethod.CreditCardAccountId!.Value);
            return Expense.NewCreditCardExpense(description: BuildExpenseDescriptionFor(mortgageLoan, payMortgageCommand),
                                                transactionDate: DateTimeOffset.FromFileTime(payMortgageCommand.TransactionUnixTimeMilliseconds),
                                                expenseCategoryCode: ExpenseCategoryCode.MortgagePayment,
                                                amount: CalculateTotalMortgagePayment(payMortgageCommand),
                                                creditCardAccount);
        }

        private async Task<CreditCardAccount> FindCreditCardAccountByIdAsync(int creditCardAccountId)
        {
            Maybe<CreditCardAccount> maybeCreditCardAccount = await _creditCardAccountRepository.GetByIdAsync(creditCardAccountId);
            if (maybeCreditCardAccount.HasNoValue)
            {
                throw new ResourceNotFoundException($"Credit card account with id '{creditCardAccountId}' not found");
            }
            return maybeCreditCardAccount.Value;
        }

        private async Task<Loan> FindMortgageLoanByIdAsync(int mortgageLoanId)
        {
            Maybe<Loan> maybeMortgageLoan = await _loanRepository.GetByIdAsync(mortgageLoanId);
            if (!maybeMortgageLoan.HasValue)
            {
                throw new ResourceNotFoundException($"Morgage loan with id '{mortgageLoanId}' not found");
            }
            return maybeMortgageLoan.Value;
        }
    }
}
