using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Perfi.Core.Expenses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Perfi.Core.MoneyTransfers;
using Perfi.Core.Accounts.AccountAggregate;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    internal class MoneyTransferConfiguration : IEntityTypeConfiguration<MoneyTransfer>
    {
        public void Configure(EntityTypeBuilder<MoneyTransfer> builder)
        {
            builder.ToTable("MoneyTransfer");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Remarks).HasMaxLength(MoneyTransfer.MaxLengths.Remarks).IsRequired();

            builder.OwnsOne(x => x.Amount, a =>
            {
                a.Property(x => x.Currency).HasMaxLength(4);
            });

            builder.Property(x => x.FromAccountNumber)
                   .HasConversion(accountNumber => accountNumber.Value, value => AccountNumber.From(value))
                   .IsRequired()
                   .HasMaxLength(AccountNumber.MaxLength);
            builder.Property(x => x.From).HasMaxLength(MoneyTransfer.MaxLengths.From).IsRequired();

            builder.Property(x => x.ToAccountNumber)
                   .HasConversion(accountNumber => accountNumber.Value, value => AccountNumber.From(value))
                   .IsRequired()
                   .HasMaxLength(AccountNumber.MaxLength);
            builder.Property(x => x.To).HasMaxLength(MoneyTransfer.MaxLengths.To).IsRequired();

            builder.Property(x => x.TransactionPeriod)
                   .HasConversion(transactionPeriod => transactionPeriod.Value, value => TransactionPeriod.From(value))
                   .HasMaxLength(TransactionPeriod.MaxLength)
                   .IsRequired();


            builder.HasIndex(x => x.TransactionPeriod).IsUnique(false);
            builder.HasIndex(x => x.TransactionDate).IsUnique(false);
        }
    }
}
