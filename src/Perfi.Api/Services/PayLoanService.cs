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
    public class PayLoanService : IPayLoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IExpenseRepository _expenseRepository;
        private readonly ICashAccountRepository _cashAccountRepository;
        private readonly ICreditCardAccountRepository _creditCardAccountRepository;
        private readonly IAccountingTransactionRepository _accountingTransactionRepository;
        private readonly ITransactionalExpenseCategoryRepository _transactionalExpenseCategoryRepository;
        private readonly ILogger<PayMortgageService> _logger;

        public PayLoanService(
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

        public async Task<NewExpenseAddedResponse> PayAsync(PayLoanCommand payLoanCommand)
        {
            Loan mortgageLoan = await FindMortgageLoanByIdAsync(payLoanCommand.LoanId);
            Expense expense = await AddNewExpenseAsync(mortgageLoan, payLoanCommand);
            AccountingTransaction accountingTransaction = await AddAccoutingTransactionAsync(expense, mortgageLoan, payLoanCommand);
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

        private async Task<AccountingTransaction> AddAccoutingTransactionAsync(Expense expense, Loan mortgageLoan, PayLoanCommand payMortgageCommand)
        {
            AccountingTransaction accountingTransaction = BuildAccountingTransaction(expense, mortgageLoan, payMortgageCommand);

            accountingTransaction = _accountingTransactionRepository.Add(accountingTransaction);
            _accountingTransactionRepository.Add(accountingTransaction);
            await _accountingTransactionRepository.UnitOfWork.SaveChangesAsync();
            return accountingTransaction;
        }

        private static AccountingTransaction BuildAccountingTransaction(Expense expense, Loan mortgageLoan, PayLoanCommand payMortgageCommand)
        {
            List<AccountingEntry> accountingEntries = BuildAccountingEntries(expense, mortgageLoan, payMortgageCommand);

            AccountingTransaction accountingTransaction = AccountingTransaction.New(transactionDate: expense.TransactionDate,
                                                                                        description: expense.Description,
                                                                                        accountingEntries.ToArray());
            return accountingTransaction;
        }

        private static List<AccountingEntry> BuildAccountingEntries(Expense expense, Loan mortgageLoan, PayLoanCommand payMortgageCommand)
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

            if (payMortgageCommand.FeeAmount.HasValue && payMortgageCommand.FeeAmount.Value > 0)
            {
                AccountingEntry debitFeeAmountAccountingEntry = AccountingEntry.Debit(TransactionalAccount.DefaultAccountNumbers.GetFeePaid(),
                                                                       Money.UsdFrom(payMortgageCommand.FeeAmount.Value),
                                                                       expense.TransactionDate);
                accountingEntries.Add(debitFeeAmountAccountingEntry);
            }

            return accountingEntries;
        }

        private async Task<Expense> AddNewExpenseAsync(Loan mortgageLoan, PayLoanCommand payLoanCommand)
        {
            Expense expense = await BuildExpenseAsync(mortgageLoan, payLoanCommand);
            expense = _expenseRepository.Add(expense);
            await _expenseRepository.UnitOfWork.SaveChangesAsync();
            _logger.LogDebug("New Expense with id '{ExpenseId}' added", new { ExpenseId = expense.Id });
            return expense;
        }

        private Task<Expense> BuildExpenseAsync(Loan mortgageLoan, PayLoanCommand payLoanCommand)
        {
            if (payLoanCommand.PaymentMethod.PaymentMethodType == PaymentMethodType.CreditCard)
            {
                return BuildNewExpenseWithCreditCardAsync(mortgageLoan, payLoanCommand);
            }
            return BuildNewExpenseWithBankCashAccontAsync(mortgageLoan, payLoanCommand);
        }

        private async Task<Expense> BuildNewExpenseWithBankCashAccontAsync(Loan mortgageLoan, PayLoanCommand payLoanCommand)
        {
            CashAccount cashAccount = await FindCashAccountById(payLoanCommand.PaymentMethod.CashAccountId!.Value);
            Expense expense = Expense.NewCashAccountExpense(
                BuildExpenseDescriptionFor(mortgageLoan, payLoanCommand),
                transactionDate: DateTimeOffset.FromUnixTimeMilliseconds(payLoanCommand.TransactionUnixTimeMilliseconds),
                expenseCategoryCode: ExpenseCategoryCode.MortgagePayment,
                amount: CalculateTotalMortgagePayment(payLoanCommand),
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

        private static Money CalculateTotalMortgagePayment(PayLoanCommand payMortgageCommand)
        {
            Money totalAmount = Money.UsdFrom(payMortgageCommand.PrincipalAmount);
            totalAmount += Money.UsdFrom(payMortgageCommand.InterestAmount);
            if (payMortgageCommand.FeeAmount.HasValue && payMortgageCommand.FeeAmount.Value > 0)
            {
                totalAmount += Money.UsdFrom(payMortgageCommand.FeeAmount.Value);
            }
            return totalAmount;
        }

        private static string BuildExpenseDescriptionFor(Loan loan, PayLoanCommand payLoanCommand)
        {
            StringBuilder description = new StringBuilder($"Loan payment of '{loan.Name}'");
            description.Append($"Principal (${payLoanCommand.PrincipalAmount})");
            description.Append($"Interest (${payLoanCommand.InterestAmount})");
            if (payLoanCommand.FeeAmount.HasValue && payLoanCommand.FeeAmount.Value > 0)
            {
                description.Append($"Fee (${payLoanCommand.FeeAmount})");
            }
            return description.ToString();
        }

        private async Task<Expense> BuildNewExpenseWithCreditCardAsync(Loan mortgageLoan, PayLoanCommand payLoanCommand)
        {
            CreditCardAccount creditCardAccount = await FindCreditCardAccountByIdAsync(payLoanCommand.PaymentMethod.CreditCardAccountId!.Value);
            return Expense.NewCreditCardExpense(description: BuildExpenseDescriptionFor(mortgageLoan, payLoanCommand),
                                                transactionDate: DateTimeOffset.FromFileTime(payLoanCommand.TransactionUnixTimeMilliseconds),
                                                expenseCategoryCode: ExpenseCategoryCode.MortgagePayment,
                                                amount: CalculateTotalMortgagePayment(payLoanCommand),
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
