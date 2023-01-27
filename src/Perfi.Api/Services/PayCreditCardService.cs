using CSharpFunctionalExtensions;
using Perfi.Api.Commands;
using Perfi.Api.Exceptions;
using Perfi.Api.Responses;
using Perfi.Core.Accounting;
using Perfi.Core.Accounting.AccountingTransactionAggregate;
using Perfi.Core.Accounts.AccountingTransactionAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.Core.Accounts.CreditCardAggregate;
using Perfi.Core.MoneyTransfers;
using Perfi.Core.Payments.OutgoingPayments;

namespace Perfi.Api.Services
{
    public class PayCreditCardService : IPayCreditCardService
    {
        private readonly ICreditCardAccountRepository _creditCardAccountRepository;
        private readonly ICashAccountRepository _cashAccountRepository;
        private readonly IAccountingTransactionRepository _accountingTransactionRepository;
        private readonly IMoneyTransferRepository _moneyTransferRepository;
        private readonly IOutgoingPaymentRepository _outgoingPaymentRepository;

        public PayCreditCardService(ICreditCardAccountRepository creditCardAccountRepository,
            ICashAccountRepository cashAccountRepository,
            IAccountingTransactionRepository accountingTransactionRepository,
            IMoneyTransferRepository moneyTransferRepository,
            IOutgoingPaymentRepository outgoingPaymentRepository)
        {
            _creditCardAccountRepository = creditCardAccountRepository;
            _cashAccountRepository = cashAccountRepository;
            _accountingTransactionRepository = accountingTransactionRepository;
            _moneyTransferRepository = moneyTransferRepository;
            _outgoingPaymentRepository = outgoingPaymentRepository;
        }

        public async Task<NewMoneyTransferResponse> PayAsync(PayCreditCardCommand payCreditCardCommand)
        {
            MoneyTransfer moneyTransfer = await AddMoneyTransferAsync(payCreditCardCommand);
            AddOutgoingPayment(moneyTransfer);
            AccountingTransaction accountingTransaction = await AddAccountingTransactionAsync(moneyTransfer);
            await SetAccountingTransactionAsync(moneyTransfer, accountingTransaction);
            return NewMoneyTransferResponse.From(moneyTransfer);
        }

        private async Task<MoneyTransfer> AddMoneyTransferAsync(PayCreditCardCommand payCreditCardCommand)
        {
            CreditCardAccount creditCardAccount = await FindCreditCardAccountByIdAsync(payCreditCardCommand.CreditCardId);
            CashAccount cashAccount = await FindCashAccountByIdAsync(payCreditCardCommand.PayWithCashAccountId);
            MoneyTransfer moneyTransfer = MoneyTransfer.New(remarks: $"Credit card '{creditCardAccount.Name}' payment",
                                                            amount: Money.UsdFrom(payCreditCardCommand.Amount),
                                                            fromAccountNumber: cashAccount.AssociatedAccountNumber,
                                                            from: $"Cash Account '{cashAccount.Name}'",
                                                            toAccountNumber: creditCardAccount.AssociatedAccountNumber,
                                                            to: $"Credit card '{creditCardAccount.Name}-{creditCardAccount.LastFourDigits}'",
                                                            transactionDate: DateTimeOffset.FromUnixTimeMilliseconds(payCreditCardCommand.TransactionUnixTimeMilliseconds));
            _moneyTransferRepository.Add(moneyTransfer);
            return moneyTransfer;
        }

        private void AddOutgoingPayment(MoneyTransfer moneyTransfer)
        {
            OutgoingPayment outgoingPayment = OutgoingPayment.From(description: $"Paid by cash: '{moneyTransfer.From}' for credit card: {moneyTransfer.To}",
                                                                              amount: moneyTransfer.Amount,
                                                                              paidFromAccountNumber: moneyTransfer.FromAccountNumber,
                                                                              transactionDate: moneyTransfer.TransactionDate);
            _outgoingPaymentRepository.Add(outgoingPayment);
        }

        private async Task SetAccountingTransactionAsync(MoneyTransfer moneyTransfer, AccountingTransaction accountingTransaction)
        {
            moneyTransfer.SetAccountingTransaction(accountingTransaction);
            await _moneyTransferRepository.UnitOfWork.SaveChangesAsync();
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
