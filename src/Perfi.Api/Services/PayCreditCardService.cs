using CSharpFunctionalExtensions;
using Perfi.Api.Commands;
using Perfi.Api.Exceptions;
using Perfi.Api.Responses;
using Perfi.Core.Accounting;
using Perfi.Core.Accounting.AccountingTransactionAggregate;
using Perfi.Core.Accounts.AccountingTransactionAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.Core.Accounts.CreditCardAggregate;
using Perfi.Core.Expenses;

namespace Perfi.Api.Services
{
    public class PayCreditCardService : IPayCreditCardService
    {
        private readonly ICreditCardAccountRepository _creditCardAccountRepository;
        private readonly ICashAccountRepository _cashAccountRepository;
        private readonly IExpenseRepository _expenseRepository;
        private readonly ITransactionalExpenseCategoryRepository _transactionalExpenseCategoryRepository;
        private readonly IAccountingTransactionRepository _accountingTransactionRepository;

        public PayCreditCardService(ICreditCardAccountRepository creditCardAccountRepository,
            ICashAccountRepository cashAccountRepository,
            IExpenseRepository expenseRepository,
            ITransactionalExpenseCategoryRepository transactionalExpenseCategoryRepository,
            IAccountingTransactionRepository accountingTransactionRepository)
        {
            _creditCardAccountRepository = creditCardAccountRepository;
            _cashAccountRepository = cashAccountRepository;
            _expenseRepository = expenseRepository;
            _transactionalExpenseCategoryRepository = transactionalExpenseCategoryRepository;
            _accountingTransactionRepository = accountingTransactionRepository;
        }

        public async Task<NewExpenseAddedResponse> PayAsync(PayCreditCardCommand payCreditCardCommand)
        {
            CreditCardAccount creditCardAccount = await FindCreditCardAccountByIdAsync(payCreditCardCommand.CreditCardId);
            Expense expense = await AddExpenseAsync(payCreditCardCommand, creditCardAccount);
            AccountingTransaction accountingTransaction = await AddAccountingTransactionAsync(creditCardAccount, expense);
            await SetExpenseTransaction(expense, accountingTransaction);
            TransactionalExpenseCategory transactionalExpenseCategory = await FindTransactionExpenseCategoryByCodeAsync(expense.ExpenseCategoryCode);
            return NewExpenseAddedResponse.From(expense, transactionalExpenseCategory);
        }

        private async Task SetExpenseTransaction(Expense expense, AccountingTransaction accountingTransaction)
        {
            expense.SetAccountingTransaction(accountingTransaction);
            _expenseRepository.Update(expense);
            await _expenseRepository.UnitOfWork.SaveChangesAsync();
        }

        private async Task<TransactionalExpenseCategory> FindTransactionExpenseCategoryByCodeAsync(ExpenseCategoryCode expenseCategoryCode)
        {
            Maybe<TransactionalExpenseCategory> maybeTransactionalExpenseCategory = await _transactionalExpenseCategoryRepository.GetByCodeAsync(expenseCategoryCode);
            if (maybeTransactionalExpenseCategory.HasNoValue)
            {
                throw new ResourceNotFoundException($"Expense category with code '{expenseCategoryCode}' not found");
            }
            TransactionalExpenseCategory transactionalExpenseCategory = maybeTransactionalExpenseCategory.Value;
            return transactionalExpenseCategory;
        }

        private async Task<AccountingTransaction> AddAccountingTransactionAsync(CreditCardAccount creditCardAccount, Expense expense)
        {
            AccountingEntry debitAccountingEntry = AccountingEntry.Debit(creditCardAccount.AssociatedAccountNumber,
                                                                                amount: expense.Amount,
                                                                                transactionDate: expense.TransactionDate);

            AccountingEntry creditAccountingEntry = AccountingEntry.Credit(expense.PaymentMethod.GetAssociatedAccountNumber(),
                                                                    amount: expense.Amount,
                                                                    transactionDate: expense.TransactionDate);

            AccountingTransaction accountingTransaction = AccountingTransaction.New(expense.TransactionDate,
                                                                                    expense.Description,
                                                                                    debitAccountingEntry,
                                                                                    creditAccountingEntry
                                                                                    );
            accountingTransaction = _accountingTransactionRepository.Add(accountingTransaction);
            await _accountingTransactionRepository.UnitOfWork.SaveChangesAsync();
            return accountingTransaction;
        }

        private async Task<Expense> AddExpenseAsync(PayCreditCardCommand payCreditCardCommand, CreditCardAccount creditCardAccount)
        {
            CashAccount cashAccount = await FindCashAccountByIdAsync(payCreditCardCommand.PayWithCashAccountId);
            Expense expense = Expense.NewCashAccountExpense(description: $"Credit Card '{creditCardAccount.Name}-{creditCardAccount.LastFourDigits}' Payment",
                                                            transactionDate: DateTimeOffset.FromUnixTimeMilliseconds(payCreditCardCommand.TransactionUnixTimeMilliseconds),
                                                            expenseCategoryCode: ExpenseCategoryCode.DebtPayment,
                                                            amount: Money.UsdFrom(payCreditCardCommand.Amount),
                                                            cashAccount);
            return expense;
        }

        private async Task<CashAccount> FindCashAccountByIdAsync(int payWithCashAccountId)
        {
            Maybe<CashAccount> maybeCashAccount = await _cashAccountRepository.GetByIdAsync(payWithCashAccountId);
            if (maybeCashAccount.HasNoValue)
            {
                throw new ResourceNotFoundException($"Cash account with id '{payWithCashAccountId}' not found");
            }
            return maybeCashAccount.Value;
        }

        private async Task<CreditCardAccount> FindCreditCardAccountByIdAsync(int creditCardId)
        {
            Maybe<CreditCardAccount> maybeCreditCardAccount = await _creditCardAccountRepository.GetByIdAsync(creditCardId);
            if (maybeCreditCardAccount.HasNoValue)
            {
                throw new ResourceNotFoundException($"Credit card account with id '{creditCardId}' not found");
            }
            return maybeCreditCardAccount.Value;
        }
    }
}
