using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Perfi.Core.Expenses;
using Perfi.Core.MoneyTransfers;
using Perfi.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfi.Infrastructure.Database.Repositories
{
    public class MoneyTransferRepository : IMoneyTransferRepository
    {
        private readonly AppDbContext _appDbContext;

        public IUnitOfWork UnitOfWork => _appDbContext;
        public MoneyTransferRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public MoneyTransfer Add(MoneyTransfer moneyTransfer)
        {
            return _appDbContext.MoneyTransfers.Add(moneyTransfer).Entity;
        }

        public async Task<IEnumerable<MoneyTransfer>> GetAllAsync(TransactionPeriod transactionPeriod)
        {
            return await _appDbContext.MoneyTransfers.Where(mt => mt.TransactionPeriod == transactionPeriod).OrderByDescending(mt => mt.TransactionDate).ToListAsync();
        }

        public async Task<IEnumerable<MoneyTransfer>> GetLimitedTransfersAsync(TransactionPeriod transactionPeriod, int count)
        {
            Guard.Against.OutOfRange(count, nameof(count), rangeFrom: 1, rangeTo: int.MaxValue);
            return await _appDbContext.MoneyTransfers.Where(mt => mt.TransactionPeriod == transactionPeriod)
                                                     .OrderByDescending(mt => mt.TransactionDate)
                                                     .Take(count).ToListAsync();
        }

        public void Update(MoneyTransfer moneyTransfer)
        {
            _appDbContext.MoneyTransfers.Update(moneyTransfer);
        }
    }


}
