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
using Perfi.Core.Payments.OutgoingPayments;
using Perfi.Core.SplitPartners;

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
        private readonly ISplitPartnerRepository _splitPartnerRepository;
        private readonly IAddOutgoingPaymentService _addOutgoingPaymentService;
        private readonly IOutgoingPaymentRepository _outgoingPaymentRepository;

        public AddNewExpenseService(
            IExpenseRepository expenseRepository,
            IAccountingTransactionRepository accountingTransactionRepository,
            ICreditCardAccountRepository creditCardAccountRepository,
            ICashAccountRepository cashAccountRepository,
            ILogger<AddNewExpenseService> logger,
            ITransactionalAccountRepository transactionalAccountRepository,
            ITransactionalExpenseCategoryRepository transactionalExpenseCategoryRepository,
            ISplitPartnerRepository splitPartnerRepository,
            IAddOutgoingPaymentService addOutgoingPaymentService)
        {
            _expenseRepository = expenseRepository;
            _accountingTransactionRepository = accountingTransactionRepository;
            _creditCardAccountRepository = creditCardAccountRepository;
            _cashAccountRepository = cashAccountRepository;
            _logger = logger;
            _transactionalAccountRepository = transactionalAccountRepository;
            _transactionalExpenseCategoryRepository = transactionalExpenseCategoryRepository;
            _splitPartnerRepository = splitPartnerRepository;
            _addOutgoingPaymentService = addOutgoingPaymentService;
        }
        public async Task<NewExpenseAddedResponse> AddAsync(AddNewExpenseCommand addNewExpenseCommand)
        {
            Expense expense = await AddNewExpenseAsync(addNewExpenseCommand);
            AddOutgoingPayment(expense, Money.UsdFrom(addNewExpenseCommand.Amount));
            AccountingTransaction accountingTransaction = await AddAccountingTransactionRelatedToExpenseAsync(expense);
            await SetAccountingTransactionInExpense(expense, accountingTransaction);
            TransactionalExpenseCategory transactionalExpenseCategory = await FindTransactionalExpenseCategoryByCodeAsync(expense.ExpenseCategoryCode);
            return NewExpenseAddedResponse.From(expense, transactionalExpenseCategory);
        }

        private void AddOutgoingPayment(Expense expense, Money totalOutgoingPaymentAmount)
        {
            _addOutgoingPaymentService.AddOutGoingPaymentFor(expense, totalOutgoingPaymentAmount);
        }

        private async Task SetAccountingTransactionInExpense(Expense expense, AccountingTransaction accountingTransaction)
        {
            expense.SetAccountingTransaction(accountingTransaction);
            await _expenseRepository.UnitOfWork.SaveChangesAsync();
        }

        private async Task<Expense> AddNewExpenseAsync(AddNewExpenseCommand addNewExpenseCommand)
        {
            Expense newExpense = await BuildNewExpenseWith(addNewExpenseCommand);
            newExpense = _expenseRepository.Add(newExpense);
            return newExpense;
        }

        private Task<Expense> BuildNewExpenseWith(AddNewExpenseCommand addNewExpenseCommand)
        {
            if (addNewExpenseCommand.IsSplitExpense())
            {
                return BuildNewSplitExpense(addNewExpenseCommand);
            }
            else
            {
                return BuildNewExpenseWithNoSplit(addNewExpenseCommand);
            }
        }

        private Task<Expense> BuildNewSplitExpense(AddNewExpenseCommand addNewExpenseCommand)
        {
            if (addNewExpenseCommand.SplitPayment.IsPaidBySplitPartner)
            {
                return BuildNewSplitExpensePaidBySplitPartner(addNewExpenseCommand);
            }
            else
            {
                return BuildNewExpenseWithSplitAndPaidByAccountHolder(addNewExpenseCommand);
            }
        }

        private async Task<Expense> BuildNewExpenseWithSplitAndPaidByAccountHolder(AddNewExpenseCommand addNewExpenseCommand)
        {
            if (addNewExpenseCommand.PaymentMethod.PaymentMethodType == PaymentMethodType.CreditCard)
            {
                return await BuildNewSplitExpenseWithCreditCardPaymentAsync(addNewExpenseCommand);
            }
            else
            {
                return await BuildNewSplitExpenseWithCashAccountPaymentAsync(addNewExpenseCommand);
            }

        }

        private async Task<Expense> BuildNewSplitExpenseWithCashAccountPaymentAsync(AddNewExpenseCommand addNewExpenseCommand)
        {
            Money ownerShareExpenseAmount = Money.UsdFrom(addNewExpenseCommand.SplitPayment!.PaymentShare.OwnerShare);
            if (ownerShareExpenseAmount.IsZero())
            {
                throw new ArgumentException($"Account holder expense amount should not be zero");
            }
            Money splitPartnerShareExpenseAmount = Money.UsdFrom(addNewExpenseCommand.SplitPayment!.PaymentShare.SplitPartnerShare);
            CashAccount cashAccount = await FindCashAccountByIdAsync(addNewExpenseCommand.PaymentMethod!.CashAccountId!.Value);
            SplitPartner splitPartner = await FindSplitPartnerByIdAsync(addNewExpenseCommand.SplitPayment.SplitPartnerId);
            Expense newExpense = Expense.NewSplitExpenseWithCashAccountPayment(description: addNewExpenseCommand.Description,
                                             transactionDate: DateTimeOffset.FromUnixTimeMilliseconds(addNewExpenseCommand.TransactionDateUnixTimeStamp),
                                             expenseCategoryCode: ExpenseCategoryCode.From(addNewExpenseCommand.ExpenseCategoryCode),
                                             ownerShareExpenseAmount: Money.UsdFrom(addNewExpenseCommand.Amount),
                                             cashAccount,
                                             splitPartner,
                                             splitPartnerShareExpenseAmount
                                             );
            return newExpense;
        }

        private async Task<Expense> BuildNewSplitExpenseWithCreditCardPaymentAsync(AddNewExpenseCommand addNewExpenseCommand)
        {
            Money ownerShareExpenseAmount = Money.UsdFrom(addNewExpenseCommand.SplitPayment.PaymentShare.OwnerShare);
            if (ownerShareExpenseAmount.IsZero())
            {
                throw new ArgumentException($"Account holder expense amount should not be zero");
            }
            Money splitPartnerShareExpenseAmount = Money.UsdFrom(addNewExpenseCommand.SplitPayment.PaymentShare.SplitPartnerShare);
            CreditCardAccount creditCardAccount = await FindCreditCardAccountAsync(addNewExpenseCommand.PaymentMethod.CreditCardAccountId!.Value);
            SplitPartner splitPartner = await FindSplitPartnerByIdAsync(addNewExpenseCommand.SplitPayment.SplitPartnerId);
            Expense newExpense = Expense.NewSplitExpenseWithCreditCardPayment(description: addNewExpenseCommand.Description,
                                             transactionDate: DateTimeOffset.FromUnixTimeMilliseconds(addNewExpenseCommand.TransactionDateUnixTimeStamp),
                                             expenseCategoryCode: ExpenseCategoryCode.From(addNewExpenseCommand.ExpenseCategoryCode),
                                             ownerShareExpenseAmount: ownerShareExpenseAmount,
                                             creditCardAccount,
                                             splitPartner,
                                             splitPartnerShareExpenseAmount
                                             );
            return newExpense;
        }

        private async Task<Expense> BuildNewSplitExpensePaidBySplitPartner(AddNewExpenseCommand addNewExpenseCommand)
        {
            Money ownerShareExpenseAmount = Money.UsdFrom(addNewExpenseCommand.SplitPayment.PaymentShare.OwnerShare);
            if (ownerShareExpenseAmount.IsZero())
            {
                throw new ArgumentException($"Account holder expense amount should not be zero");
            }
            Money splitPartnerShareExpenseAmount = Money.UsdFrom(addNewExpenseCommand.SplitPayment.PaymentShare.OwnerShare);
            SplitPartner splitPartner = await FindSplitPartnerByIdAsync(addNewExpenseCommand.SplitPayment.SplitPartnerId);
            return Expense.NewSplitExpensePaidBySplitPartner(description: addNewExpenseCommand.Description,
                                             transactionDate: DateTimeOffset.FromUnixTimeMilliseconds(addNewExpenseCommand.TransactionDateUnixTimeStamp),
                                             expenseCategoryCode: ExpenseCategoryCode.From(addNewExpenseCommand.ExpenseCategoryCode),
                                             ownerShareExpenseAmount: ownerShareExpenseAmount,
                                             splitPartner,
                                             splitPartnerShareExpenseAmount);
        }

        private Task<Expense> BuildNewExpenseWithNoSplit(AddNewExpenseCommand addNewExpenseCommand)
        {
            if (addNewExpenseCommand.PaymentMethod.PaymentMethodType == PaymentMethodType.CreditCard)
            {
                return BuildExpenseWithCreditCardPaymentAsync(addNewExpenseCommand);
            }
            return BuildExpenseWithCashAccountPaymentAsync(addNewExpenseCommand);
        }

        private async Task<SplitPartner> FindSplitPartnerByIdAsync(int splitPartnerId)
        {
            Maybe<SplitPartner> splitPartner = await _splitPartnerRepository.GetByIdAsync(splitPartnerId);
            if (splitPartner.HasNoValue)
            {
                throw new ResourceNotFoundException($"Split partner with id '{splitPartnerId}' not found");
            }
            return splitPartner.Value;
        }

        private async Task<Expense> BuildExpenseWithCreditCardPaymentAsync(AddNewExpenseCommand addNewExpenseCommand)
        {
            CreditCardAccount creditCardAccount = await FindCreditCardAccountAsync(addNewExpenseCommand.PaymentMethod.CreditCardAccountId!.Value);
            Expense newExpense = Expense.NewCreditCardExpense(description: addNewExpenseCommand.Description,
                                             transactionDate: DateTimeOffset.FromUnixTimeMilliseconds(addNewExpenseCommand.TransactionDateUnixTimeStamp),
                                             expenseCategoryCode: ExpenseCategoryCode.From(addNewExpenseCommand.ExpenseCategoryCode),
                                             amount: Money.UsdFrom(addNewExpenseCommand.Amount),
                                             creditCardAccount
                                             );
            return newExpense;
        }

        private async Task<Expense> BuildExpenseWithCashAccountPaymentAsync(AddNewExpenseCommand addNewExpenseCommand)
        {
            CashAccount cashAccount = await FindCashAccountByIdAsync(addNewExpenseCommand.PaymentMethod.CashAccountId!.Value);
            Expense newExpense = Expense.NewCashAccountExpense(description: addNewExpenseCommand.Description,
                                             transactionDate: DateTimeOffset.FromUnixTimeMilliseconds(addNewExpenseCommand.TransactionDateUnixTimeStamp),
                                             expenseCategoryCode: ExpenseCategoryCode.From(addNewExpenseCommand.ExpenseCategoryCode),
                                             amount: Money.UsdFrom(addNewExpenseCommand.Amount),
                                             cashAccount
                                             );
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
            if (expense.IsSplit())
            {
                return await BuildAccountingTransationWithSplitAsync(expense);
            }
            else
            {
                return await BuildAccountingTransationWithNoSplitAsync(expense);
            }
        }

        private async Task<AccountingTransaction> BuildAccountingTransationWithSplitAsync(Expense expense)
        {
            AccountingEntry creditAccountingEntry = await BuildCreditAccountingEntryForSplitExpense(expense);
            AccountingEntry debitAccountingEntry = await BuildDebitAccountingEntry(expense);
            if (!expense.SplitPayment!.SplitPartnerShare.IsZero())
            {
                AccountingEntry splitPartnerDebitAccountingEntry = await BuildSplitPartnerDebitAccountingEntry(expense);
                return AccountingTransaction.New(expense.TransactionDate, expense.Description,
                                             creditAccountingEntry, debitAccountingEntry, splitPartnerDebitAccountingEntry);
            }
            return AccountingTransaction.New(expense.TransactionDate, expense.Description,
                                             creditAccountingEntry, debitAccountingEntry);
        }

        private async Task<AccountingEntry> BuildSplitPartnerDebitAccountingEntry(Expense expense)
        {
            TransactionalAccount accountsPayable = await FindAccountByNumberAsync(expense.SplitPayment!.SplitPartnerReceivableAccountNumber);
            AccountingEntry debitAccountingEntry = AccountingEntry.Debit(accountsPayable, expense.SplitPayment.SplitPartnerShare, expense.TransactionDate);
            return debitAccountingEntry;
        }

        private async Task<AccountingTransaction> BuildAccountingTransationWithNoSplitAsync(Expense expense)
        {
            AccountingEntry creditAccountingEntry = await BuildCreditAccountingEntry(expense);
            AccountingEntry debitAccountingEntry = await BuildDebitAccountingEntry(expense);
            return AccountingTransaction.New(expense.TransactionDate, expense.Description,
                                             creditAccountingEntry, debitAccountingEntry);
        }

        private async Task<AccountingEntry> BuildDebitAccountingEntry(Expense expense)
        {
            TransactionalAccount expenseAccount = await FindExpenseAccountForCategoryAsync(expense.ExpenseCategoryCode);
            if (expense.IsSplit())
            {
                return AccountingEntry.Debit(expenseAccount, expense.SplitPayment!.OwnerShare, expense.TransactionDate);
            }
            return AccountingEntry.Debit(expenseAccount, expense.Amount, expense.TransactionDate);
        }

        private async Task<AccountingEntry> BuildCreditAccountingEntry(Expense expense)
        {
            TransactionalAccount paymentAccount = await FindAccountByNumberAsync(expense.PaymentMethod.GetAssociatedAccountNumber());
            AccountingEntry creditAccountingEntry = AccountingEntry.Credit(paymentAccount, expense.Amount, expense.TransactionDate);
            return creditAccountingEntry;
        }
        private async Task<AccountingEntry> BuildCreditAccountingEntryForSplitExpense(Expense expense)
        {
            TransactionalAccount paymentAccount = await FindAccountByNumberAsync(expense.PaymentMethod.GetAssociatedAccountNumber());
            AccountingEntry creditAccountingEntry = AccountingEntry.Credit(paymentAccount, expense.SplitPayment!.GetTotalAmount(), expense.TransactionDate);
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
