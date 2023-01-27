using CSharpFunctionalExtensions;
using Perfi.Api.Commands;
using Perfi.Api.Responses;
using Perfi.Core.Accounting;
using Perfi.Core.Accounting.AccountingTransactionAggregate;
using Perfi.Core.Accounts.AccountingTransactionAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.Core.Earnings;
using Perfi.Core.Earnings.IncomeSources;
using Perfi.Core.Payments.IncomingPayments;

namespace Perfi.Api.Services
{
    public class AddNewIncomeService : IAddNewIncomeService
    {
        private readonly IIncomeDocumentRepository _incomeDocumentRepository;
        private readonly ICashAccountRepository _cashAccountRepository;
        private readonly IIncomeSourceRepository _incomeSourceRepository;
        private readonly IAccountingTransactionRepository _accountingTransactionRepository;
        private readonly IIncomingPaymentRepository _incomingPaymentRepository;

        private Money RemainderDebitAmount { get; set; }
        public AddNewIncomeService(
            IIncomeDocumentRepository incomeDocumentRepository,
            ICashAccountRepository cashAccountRepository,
            IIncomeSourceRepository incomeSourceRepository,
            IAccountingTransactionRepository accountingTransactionRepository,
            IIncomingPaymentRepository incomingPaymentRepository)
        {
            _incomeDocumentRepository = incomeDocumentRepository;
            _cashAccountRepository = cashAccountRepository;
            _incomeSourceRepository = incomeSourceRepository;
            _accountingTransactionRepository = accountingTransactionRepository;
            _incomingPaymentRepository = incomingPaymentRepository;
        }
        public async Task<NewIncomeAddedResponse> AddAsync(AddNewIncomeCommand addJobIncomeCommand)
        {
            IncomeDocument incomeDocument = await AddIncomeDocumentAsync(addJobIncomeCommand);
            AddIncomingPaymentAsync(incomeDocument);
            AccountingTransaction transaction = await AddTransactionForIncomeDocumentAsync(incomeDocument);
            await SetIncomeDocumentTransactionAsync(incomeDocument, transaction);
            return NewIncomeAddedResponse.From(incomeDocument);
        }

        private void AddIncomingPaymentAsync(IncomeDocument incomeDocument)
        {
            IncomingPayment incomingPayment = IncomingPayment.From(incomeDocument);
            _incomingPaymentRepository.Add(incomingPayment);
        }

        private async Task SetIncomeDocumentTransactionAsync(IncomeDocument incomeDocument, AccountingTransaction transaction)
        {
            incomeDocument.SetTransaction(transaction);
            _incomeDocumentRepository.Update(incomeDocument);
            await _incomeDocumentRepository.UnitOfWork.SaveChangesAsync();
        }

        private async Task<AccountingTransaction> AddTransactionForIncomeDocumentAsync(IncomeDocument incomeDocument)
        {
            AccountingTransaction accountingTransaction = await BuildAccountingTransactionAsync(incomeDocument);
            accountingTransaction = _accountingTransactionRepository.Add(accountingTransaction);
            await _accountingTransactionRepository.UnitOfWork.SaveChangesAsync();
            return accountingTransaction;
        }

        private async Task<AccountingTransaction> BuildAccountingTransactionAsync(IncomeDocument incomeDocument)
        {
            IncomeSource incomeSource = await FindIncomeSourceByIdAsync(incomeDocument.Source.IncomeSourceId);
            AccountingEntry creditEntry = AccountingEntry.Credit(incomeSource.AssociatedAccountNumber, incomeDocument.Amount, incomeDocument.TransactionDate);
            AccountingEntry debitEntry = AccountingEntry.Debit(incomeDocument.PaymentDeposition.GetAssociatedAccountNumber(), incomeDocument.Amount, incomeDocument.TransactionDate);

            AccountingTransaction accountingTransaction = AccountingTransaction.New(incomeDocument.TransactionDate, description: $"Income from '{incomeDocument.Source.Name}'", creditEntry, debitEntry);
            return accountingTransaction;
        }

        private async Task<IncomeSource> FindIncomeSourceByIdAsync(int incomeSourceId)
        {
            Maybe<IncomeSource> maybeIncomeSource = await _incomeSourceRepository.GetByIdAsync(incomeSourceId);
            if (maybeIncomeSource.HasNoValue)
            {
                throw new ArgumentException(message: $"IncomeSource with id '{incomeSourceId}' not found", paramName: nameof(maybeIncomeSource));
            }
            IncomeSource incomeSource = maybeIncomeSource.Value;
            return incomeSource;
        }

        private async Task<IncomeDocument> AddIncomeDocumentAsync(AddNewIncomeCommand addIncomeToBankCashAccountCommand)
        {
            CashAccount cashAccount = await FindCashAccountByIdAsync(addIncomeToBankCashAccountCommand.CashAccountIdToDeposit);
            IncomeSource incomeSource = await FindIncomeSourceByIdAsync(addIncomeToBankCashAccountCommand.IncomeSourceId);
            IncomeDocument incomeDocument = IncomeDocument.NewJobIncome(incomeSource,
                                                                        incomeAmount: Money.UsdFrom(addIncomeToBankCashAccountCommand.Amount),
                                                                        cashAccountForDeposit: cashAccount,
                                                                        transactionDate: DateTimeOffset.FromUnixTimeMilliseconds(addIncomeToBankCashAccountCommand.TransactionUnixTimeMilliseconds));
            incomeDocument = _incomeDocumentRepository.Add(incomeDocument);
            await _incomeDocumentRepository.UnitOfWork.SaveChangesAsync();
            return incomeDocument;
        }

        private async Task<CashAccount> FindCashAccountByIdAsync(int cashAccountId)
        {
            Maybe<CashAccount> maybeCashAccount = await _cashAccountRepository.GetByIdAsync(cashAccountId);
            if (maybeCashAccount.HasNoValue)
            {
                throw new ArgumentException(message: $"Invalid cash account id", paramName: nameof(cashAccountId));
            }
            return maybeCashAccount.Value;
        }
    }
}
