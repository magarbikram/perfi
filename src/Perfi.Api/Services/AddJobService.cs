using CSharpFunctionalExtensions;
using Perfi.Api.Commands;
using Perfi.Api.Exceptions;
using Perfi.Api.Responses;
using Perfi.Core.Accounting;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.Jobs;

namespace Perfi.Api.Services
{
    public class AddJobService : IAddJobService
    {
        private readonly ITransactionalAccountRepository _transactionalAccountRepository;
        private readonly IGetNextAccountNumberService _getNextAccountNumberService;
        private readonly IJobRepository _jobRepository;

        public AddJobService(
            ITransactionalAccountRepository transactionalAccountRepository,
            IGetNextAccountNumberService getNextAccountNumberService,
            IJobRepository jobRepository)
        {
            _transactionalAccountRepository = transactionalAccountRepository;
            _getNextAccountNumberService = getNextAccountNumberService;
            _jobRepository = jobRepository;
        }

        public async Task<NewJobAddedResponse> AddAsync(AddNewJobCommand addNewJobCommand)
        {
            TransactionalAccount jobServiceAccount = await AddJobServiceAccountAsync(addNewJobCommand);
            Job job = await AddJobAsync(addNewJobCommand, jobServiceAccount);
            return NewJobAddedResponse.From(job);
        }

        private async Task<Job> AddJobAsync(AddNewJobCommand addNewJobCommand, TransactionalAccount jobServiceAccount)
        {
            Job job = Job.From(addNewJobCommand.JobHolder, addNewJobCommand.Employee, jobServiceAccount);
            foreach (JobIncomeSplitCommand jobIncomeSplitCommand in addNewJobCommand.jobIncomeSplits)
            {
                job.AddIncomeSplit(await BuildIncomeSplitFromAsync(jobIncomeSplitCommand));
            }
            job = _jobRepository.Add(job);
            await _jobRepository.UnitOfWork.SaveChangesAsync();
            return job;
        }

        private async Task<JobIncomeSplit> BuildIncomeSplitFromAsync(JobIncomeSplitCommand jobIncomeSplitCommand)
        {
            Maybe<TransactionalAccount> maybeBankCashAccount = await FindTransactionalAccountAsync(jobIncomeSplitCommand.CashAccountNumber);
            if (maybeBankCashAccount.HasNoValue)
            {
                throw new ResourceNotFoundException($"Cash account with number '{jobIncomeSplitCommand.CashAccountNumber}' not found");
            }
            TransactionalAccount bankCashAccount = maybeBankCashAccount.Value;
            await GuardAgainstInvalidValidCashAccountNumberAsync(jobIncomeSplitCommand.CashAccountNumber);
            if (jobIncomeSplitCommand.SplitRemainderAmount)
            {
                return JobIncomeSplit.ForSplitRemainderAmount(bankCashAccount);
            }
            else
            {
                return JobIncomeSplit.From(Money.UsdFrom(jobIncomeSplitCommand.SplitAmount!.Value), bankCashAccount);
            }
        }

        private async Task GuardAgainstInvalidValidCashAccountNumberAsync(string cashAccountNumber)
        {
            Maybe<TransactionalAccount> maybeTransactionalAccount = await FindTransactionalAccountAsync(cashAccountNumber);
            if (maybeTransactionalAccount.Value.ParentAccountNumber != AccountNumber.From(SummaryAccount.DefaultAccountNumbers.BankCashAccount))
            {
                throw new ArgumentException($"Cash account with number '{cashAccountNumber}' not found");
            }
        }

        private async Task<Maybe<TransactionalAccount>> FindTransactionalAccountAsync(string cashAccountNumber)
        {
            Maybe<TransactionalAccount> maybeTransactionalAccount = await _transactionalAccountRepository.GetByAccountNumberAsync(AccountNumber.From(cashAccountNumber));
            if (maybeTransactionalAccount.HasNoValue)
            {
                throw new ResourceNotFoundException($"Cash account with number '{cashAccountNumber}' not found");
            }

            return maybeTransactionalAccount;
        }

        private async Task<TransactionalAccount> AddJobServiceAccountAsync(AddNewJobCommand addNewJobCommand)
        {
            AccountNumber jobServiceSummaryAccountNumber = AccountNumber.From(SummaryAccount.DefaultAccountNumbers.JobServiceAccount);
            AccountNumber newJobServiceAccountNumber = await _getNextAccountNumberService.GetNext(jobServiceSummaryAccountNumber);
            string newJobServiceAccountName = $"{addNewJobCommand.Employee} - {addNewJobCommand.JobHolder}";
            TransactionalAccount newJobServiceAccount = TransactionalAccount.NewRevenueAccount(accountNumber: newJobServiceAccountNumber, name: newJobServiceAccountName, parentAccountNumber: jobServiceSummaryAccountNumber);
            newJobServiceAccount = _transactionalAccountRepository.Add(newJobServiceAccount);
            return newJobServiceAccount;
        }
    }
}
