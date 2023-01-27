using CSharpFunctionalExtensions;
using Perfi.Api.Commands;
using Perfi.Api.Exceptions;
using Perfi.Api.Responses;
using Perfi.Core.Accounting;
using Perfi.Core.Accounting.AccountingTransactionAggregate;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.AccountingTransactionAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.Core.MoneyTransfers;
using Perfi.Core.Payments.IncomingPayments;
using Perfi.Core.Payments.OutgoingPayments;
using Perfi.Core.SplitPartners;

namespace Perfi.Api.Services
{
    public class TransferMoneyService : ITransferMoneyService
    {
        private readonly IMoneyTransferRepository _moneyTransferRepository;
        private readonly ISplitPartnerRepository _splitPartnerRepository;
        private readonly ICashAccountRepository _cashAccountRepository;
        private readonly IAccountingTransactionRepository _accountingTransactionRepository;
        private readonly IOutgoingPaymentRepository _outgoingPaymentRepository;
        private readonly IIncomingPaymentRepository _incomingPaymentRepository;

        public TransferMoneyService(IMoneyTransferRepository moneyTransferRepository,
            ISplitPartnerRepository splitPartnerRepository,
            ICashAccountRepository cashAccountRepository,
            IAccountingTransactionRepository accountingTransactionRepository,
            IOutgoingPaymentRepository outgoingPaymentRepository,
            IIncomingPaymentRepository incomingPaymentRepository)
        {
            _moneyTransferRepository = moneyTransferRepository;
            _splitPartnerRepository = splitPartnerRepository;
            _cashAccountRepository = cashAccountRepository;
            _accountingTransactionRepository = accountingTransactionRepository;
            _outgoingPaymentRepository = outgoingPaymentRepository;
            _incomingPaymentRepository = incomingPaymentRepository;
        }
        public async Task<NewMoneyTransferResponse> TransferAsync(TransferMoneyCommand transferMoneyCommand)
        {
            MoneyTransfer moneyTransfer = await AddMoneyTransferAsync(transferMoneyCommand);
            AddPayment(transferMoneyCommand, moneyTransfer);
            AccountingTransaction accountingTransaction = await AddAccountingTransactionAsync(moneyTransfer);
            await SetAccountTransactionInMoneyTransfer(moneyTransfer, accountingTransaction);
            return NewMoneyTransferResponse.From(moneyTransfer);
        }

        private void AddPayment(TransferMoneyCommand transferMoneyCommand, MoneyTransfer moneyTransfer)
        {
            if (IsOutgoingPayment(transferMoneyCommand))
            {
                AddOutgoingPayment(transferMoneyCommand, moneyTransfer);
            }
            else if (IsIncomingPayment(transferMoneyCommand))
            {
                AddIncomingPayment(transferMoneyCommand, moneyTransfer);
            }
        }

        private static bool IsIncomingPayment(TransferMoneyCommand transferMoneyCommand)
        {
            return transferMoneyCommand.FromAccount.Type == TransferAccountType.SplitPartner &&
                                 transferMoneyCommand.ToAccount.Type == TransferAccountType.CashAccount;
        }

        private static bool IsOutgoingPayment(TransferMoneyCommand transferMoneyCommand)
        {
            return transferMoneyCommand.FromAccount.Type == TransferAccountType.CashAccount &&
                            transferMoneyCommand.ToAccount.Type == TransferAccountType.SplitPartner;
        }

        private void AddOutgoingPayment(TransferMoneyCommand transferMoneyCommand, MoneyTransfer moneyTransfer)
        {
            OutgoingPayment outgoingPayment = OutgoingPayment.From(description: $"Money transfer for {transferMoneyCommand.Remarks} from cash account: '{transferMoneyCommand.FromAccount.CashAccountId}', to split partner: '{transferMoneyCommand.ToAccount.SplitPartnerId}'",
                                                                   amount: Money.UsdFrom(transferMoneyCommand.Amount),
                                                                   paidFromAccountNumber: moneyTransfer.FromAccountNumber,
                                                                   transactionDate: moneyTransfer.TransactionDate);
            _outgoingPaymentRepository.Add(outgoingPayment);
        }

        private void AddIncomingPayment(TransferMoneyCommand transferMoneyCommand, MoneyTransfer moneyTransfer)
        {
            IncomingPayment incomingPayment = IncomingPayment.From(description: $"Money transfer for {transferMoneyCommand.Remarks} from split partner: '{transferMoneyCommand.ToAccount.SplitPartnerId}' to cash account: '{transferMoneyCommand.FromAccount.CashAccountId}'",
                                                                   amount: Money.UsdFrom(transferMoneyCommand.Amount),
                                                                   depositedToAccountNumber: moneyTransfer.ToAccountNumber,
                                                                   transactionDate: moneyTransfer.TransactionDate);
            _incomingPaymentRepository.Add(incomingPayment);
        }

        private async Task SetAccountTransactionInMoneyTransfer(MoneyTransfer moneyTransfer, AccountingTransaction accountingTransaction)
        {
            moneyTransfer.SetAccountingTransaction(accountingTransaction);
            _moneyTransferRepository.Update(moneyTransfer);
            await _moneyTransferRepository.UnitOfWork.SaveChangesAsync();
        }

        private async Task<MoneyTransfer> AddMoneyTransferAsync(TransferMoneyCommand transferMoneyCommand)
        {
            MoneyTransfer moneyTransfer = await BuildMoneyTransfer(transferMoneyCommand);
            moneyTransfer = _moneyTransferRepository.Add(moneyTransfer);
            return moneyTransfer;
        }

        private async Task<MoneyTransfer> BuildMoneyTransfer(TransferMoneyCommand transferMoneyCommand)
        {
            Money transferAmount = Money.UsdFrom(transferMoneyCommand.Amount);
            AccountNumber fromAccountNumber = await GetAccountNumberAsync(transferMoneyCommand.FromAccount);
            string from = await GetAccountNameAsync(transferMoneyCommand.FromAccount);
            AccountNumber toAccountNumber = await GetAccountNumberAsync(transferMoneyCommand.ToAccount);
            string to = await GetAccountNameAsync(transferMoneyCommand.ToAccount);
            DateTimeOffset transactionDate = DateTimeOffset.FromUnixTimeMilliseconds(transferMoneyCommand.TransactionUnixTimeMilliseconds);

            MoneyTransfer moneyTransfer = MoneyTransfer.New(remarks: transferMoneyCommand.Remarks,
                                                            transferAmount,
                                                            fromAccountNumber: fromAccountNumber,
                                                            from: from,
                                                            toAccountNumber: toAccountNumber,
                                                            to: to,
                                                            transactionDate);
            return moneyTransfer;
        }

        private async Task<AccountingTransaction> AddAccountingTransactionAsync(MoneyTransfer moneyTransfer)
        {
            AccountingEntry debitEntry = AccountingEntry.Debit(moneyTransfer.ToAccountNumber, moneyTransfer.Amount, moneyTransfer.TransactionDate);
            AccountingEntry creditEntry = AccountingEntry.Credit(moneyTransfer.FromAccountNumber, moneyTransfer.Amount, moneyTransfer.TransactionDate);
            AccountingTransaction accountingTransaction = AccountingTransaction.New(moneyTransfer.TransactionDate, moneyTransfer.Remarks, debitEntry, creditEntry);
            accountingTransaction = _accountingTransactionRepository.Add(accountingTransaction);
            await _accountingTransactionRepository.UnitOfWork.SaveChangesAsync();
            return accountingTransaction;
        }

        private async Task<AccountNumber> GetToAccountNumberAsync(TransferAccount toTransferAccount)
        {
            if (toTransferAccount.Type == TransferAccountType.CashAccount)
            {
                CashAccount cashAccount = await FindCashAccountByIdAsync(toTransferAccount.CashAccountId!.Value);
                return cashAccount.AssociatedAccountNumber;
            }
            else
            {
                SplitPartner splitPartner = await FindSplitPartnerByIdAsync(toTransferAccount.SplitPartnerId!.Value);
                return splitPartner.ReceivableAccountNumber;
            }
        }

        private async Task<SplitPartner> FindSplitPartnerByIdAsync(int value)
        {
            Maybe<SplitPartner> maybeSplitPartner = await _splitPartnerRepository.GetByIdAsync(value);
            if (maybeSplitPartner.HasNoValue)
            {
                throw new ResourceNotFoundException($"SplitPartner with id '{value}' not found");
            }
            return maybeSplitPartner.Value;
        }

        private async Task<CashAccount> FindCashAccountByIdAsync(int value)
        {
            Maybe<CashAccount> maybeCashAccount = await _cashAccountRepository.GetByIdAsync(value);
            if (maybeCashAccount.HasNoValue)
            {
                throw new ResourceNotFoundException($"CashAccount with id '{value}' not found");
            }
            return maybeCashAccount.Value;
        }

        private async Task<string> GetAccountNameAsync(TransferAccount transferAccount)
        {
            if (transferAccount.Type == TransferAccountType.CashAccount)
            {
                CashAccount cashAccount = await FindCashAccountByIdAsync(transferAccount.CashAccountId!.Value);
                return cashAccount.Name;
            }
            else
            {
                SplitPartner splitPartner = await FindSplitPartnerByIdAsync(transferAccount.SplitPartnerId!.Value);
                return splitPartner.Name;
            }
        }

        private async Task<AccountNumber> GetAccountNumberAsync(TransferAccount transferAccount)
        {
            if (transferAccount.Type == TransferAccountType.CashAccount)
            {
                CashAccount cashAccount = await FindCashAccountByIdAsync(transferAccount.CashAccountId!.Value);
                return cashAccount.AssociatedAccountNumber;
            }
            else
            {
                SplitPartner splitPartner = await FindSplitPartnerByIdAsync(transferAccount.SplitPartnerId!.Value);
                return splitPartner.ReceivableAccountNumber;
            }
        }
    }
}
