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
using Perfi.Core.Expenses;

namespace Perfi.Api.Services
{
    public class AddNewExpenseService : IAddNewExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IAccountingTransactionRepository _accountingTransactionRepository;
        private readonly ICreditCardAccountRepository _creditCardAccountRepository;
        private readonly ICashAccountRepository _cashAccountRepository;
        private readonly ILogger<AddNewExpenseService> _logger;
        private readonly ITransactionalAccountRepository _transactionalAccountRepository;
        private readonly ITransactionalExpenseCategoryRepository _transactionalExpenseCategoryRepository;

        public AddNewExpenseService(
            IExpenseRepository expenseRepository,
            IAccountingTransactionRepository accountingTransactionRepository,
            ICreditCardAccountRepository creditCardAccountRepository,
            ICashAccountRepository cashAccountRepository,
            ILogger<AddNewExpenseService> logger,
            ITransactionalAccountRepository transactionalAccountRepository,
            ITransactionalExpenseCategoryRepository transactionalExpenseCategoryRepository)
        {
            _expenseRepository = expenseRepository;
            _accountingTransactionRepository = accountingTransactionRepository;
            _creditCardAccountRepository = creditCardAccountRepository;
            _cashAccountRepository = cashAccountRepository;
            _logger = logger;
            _transactionalAccountRepository = transactionalAccountRepository;
            _transactionalExpenseCategoryRepository = transactionalExpenseCategoryRepository;
        }
        public async Task<NewExpenseAddedResponse> AddAsync(AddNewExpenseCommand addNewExpenseCommand)
        {
            Expense expense = await AddNewExpenseAsync(addNewExpenseCommand);
            AccountingTransaction accountingTransaction = await AddAccountingTransactionRelatedToExpenseAsync(expense);
            await SetAccountingTransactionInExpense(expense, accountingTransaction);
            return NewExpenseAddedResponse.From(expense);
        }

        private async Task SetAccountingTransactionInExpense(Expense expense, AccountingTransaction accountingTransaction)
        {
            expense.SetAccountingTransaction(accountingTransaction);
            await _expenseRepository.UnitOfWork.SaveChangesAsync();
        }

        private async Task<Expense> AddNewExpenseAsync(AddNewExpenseCommand addNewExpenseCommand)
        {
            if (addNewExpenseCommand.PaymentMethod.PaymentMethodType == PaymentMethodType.CreditCard)
            {
                return await AddCreditCardExpenseAsync(addNewExpenseCommand);
            }
            return await AddCashAccountExpenseAsync(addNewExpenseCommand);

        }

        private async Task<Expense> AddCreditCardExpenseAsync(AddNewExpenseCommand addNewExpenseCommand)
        {
            CreditCardAccount creditCardAccount = await FindCreditCardAccountAsync(addNewExpenseCommand.PaymentMethod.CreditCardAccountId.Value);
            Expense newExpense = Expense.NewCreditCardExpense(description: addNewExpenseCommand.Description,
                                             transactionDate: DateTimeOffset.FromUnixTimeMilliseconds(addNewExpenseCommand.TransactionDateUnixTimeStamp),
                                             expenseCategoryCode: ExpenseCategoryCode.From(addNewExpenseCommand.ExpenseCategoryCode),
                                             amount: Money.UsdFrom(addNewExpenseCommand.Amount),
                                             creditCardAccount
                                             );
            newExpense = _expenseRepository.Add(newExpense);
            await _expenseRepository.UnitOfWork.SaveChangesAsync();

            _logger.LogInformation("New expense {Id} added", newExpense.Id);
            return newExpense;
        }

        private async Task<Expense> AddCashAccountExpenseAsync(AddNewExpenseCommand addNewExpenseCommand)
        {
            CashAccount cashAccount = await FindCashAccountByIdAsync(addNewExpenseCommand.PaymentMethod.CashAccountId.Value);
            Expense newExpense = Expense.NewCashAccountExpense(description: addNewExpenseCommand.Description,
                                             transactionDate: DateTimeOffset.FromUnixTimeMilliseconds(addNewExpenseCommand.TransactionDateUnixTimeStamp),
                                             expenseCategoryCode: ExpenseCategoryCode.From(addNewExpenseCommand.ExpenseCategoryCode),
                                             amount: Money.UsdFrom(addNewExpenseCommand.Amount),
                                             cashAccount
                                             );
            newExpense = _expenseRepository.Add(newExpense);
            await _expenseRepository.UnitOfWork.SaveChangesAsync();

            _logger.LogInformation($"New expense [{newExpense.Id}] added");
            return newExpense;
        }

        private async Task<CreditCardAccount> FindCreditCardAccountAsync(int creditCardAccountId)
        {
            Maybe<CreditCardAccount> maybeCreditCardAccount = await _creditCardAccountRepository.GetByIdAsync(creditCardAccountId);
            if (maybeCreditCardAccount.HasNoValue)
            {
                throw new ResourceNotFoundException($"Credit card account with id '{creditCardAccountId}' not found");
            }
            return maybeCreditCardAccount.Value;
        }

        private async Task<CashAccount> FindCashAccountByIdAsync(int cashAccountId)
        {
            Maybe<CashAccount> maybeCashAccount = await _cashAccountRepository.GetByIdAsync(cashAccountId);
            if (maybeCashAccount.HasNoValue)
            {
                throw new ResourceNotFoundException($"Cash account with id '{cashAccountId}' not found");
            }
            return maybeCashAccount.Value;
        }

        private async Task<AccountingTransaction> AddAccountingTransactionRelatedToExpenseAsync(Expense expense)
        {
            _logger.LogDebug($"Adding accounting transaction for expense '{expense.Id}'");
            AccountingTransaction accountingTransaction = await BuildAccountingTransactionForExpenseAsync(expense);
            accountingTransaction = _accountingTransactionRepository.Add(accountingTransaction);
            await _accountingTransactionRepository.UnitOfWork.SaveChangesAsync();
            _logger.LogInformation($"Added transaction '{accountingTransaction.Id}' for expense '{expense.Id}'");
            return accountingTransaction;
        }

        private async Task<AccountingTransaction> BuildAccountingTransactionForExpenseAsync(Expense expense)
        {
            AccountingEntry creditAccountingEntry = await BuildCreditAccountingEntry(expense);
            AccountingEntry debitAccountingEntry = await BuildDebitAccountingEntry(expense);
            return AccountingTransaction.New(expense.TransactionDate, expense.Description,
                                             creditAccountingEntry, debitAccountingEntry);
        }

        private async Task<AccountingEntry> BuildDebitAccountingEntry(Expense expense)
        {
            TransactionalAccount expenseAccount = await FindExpenseAccountForCategoryAsync(expense.ExpenseCategoryCode);
            AccountingEntry debitAccountingEntry = AccountingEntry.Debit(expenseAccount, expense.Amount, expense.TransactionDate);
            return debitAccountingEntry;
        }

        private async Task<AccountingEntry> BuildCreditAccountingEntry(Expense expense)
        {
            TransactionalAccount paymentAccount = await FindAccountByNumberAsync(expense.PaymentMethod.GetAssociatedAccountNumber());
            AccountingEntry creditAccountingEntry = AccountingEntry.Credit(paymentAccount, expense.Amount, expense.TransactionDate);
            return creditAccountingEntry;
        }

        private async Task<TransactionalAccount> FindAccountByNumberAsync(AccountNumber accountNumber)
        {
            Maybe<TransactionalAccount> maybeAccount = await _transactionalAccountRepository.GetByAccountNumberAsync(accountNumber);
            if (maybeAccount.HasNoValue)
            {
                throw new ResourceNotFoundException($"Account with number '{accountNumber}' not found");
            }
            return maybeAccount.Value;
        }

        private async Task<TransactionalAccount> FindExpenseAccountForCategoryAsync(ExpenseCategoryCode expenseCategoryCode)
        {
            TransactionalExpenseCategory transactionalExpenseCategory = await FindTransactionalExpenseCategoryByCodeAsync(expenseCategoryCode);
            return await FindAccountByNumberAsync(transactionalExpenseCategory.AssociatedExpenseAccountNumber);
        }

        private async Task<TransactionalExpenseCategory> FindTransactionalExpenseCategoryByCodeAsync(ExpenseCategoryCode expenseCategoryCode)
        {
            Maybe<TransactionalExpenseCategory> maybeTransactionalExpenseCategory = await _transactionalExpenseCategoryRepository.GetByCodeAsync(expenseCategoryCode);
            if (maybeTransactionalExpenseCategory.HasNoValue)
            {
                throw new ResourceNotFoundException($"TransactionalExpenseCategory with code '{expenseCategoryCode}' not found");
            }
            return maybeTransactionalExpenseCategory.Value;
        }
    }
}
