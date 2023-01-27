using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Perfi.Core.Expenses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Perfi.Core.Payments.LoanPayments;
using Perfi.Core.Accounts.AccountAggregate;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    public class LoanPaymentConfiguration : IEntityTypeConfiguration<LoanPayment>
    {
        public void Configure(EntityTypeBuilder<LoanPayment> builder)
        {
            builder.ToTable("LoanPayment");
            builder.HasKey(x => x.Id);

            builder.OwnsOne(x => x.PrincipalAmount, a =>
            {
                a.Property(x => x.Currency).HasMaxLength(4);
            });
            builder.OwnsOne(x => x.EscrowAmount, a =>
            {
                a.Property(x => x.Currency).HasMaxLength(4);
            });
            builder.OwnsOne(x => x.InterestAmount, a =>
            {
                a.Property(x => x.Currency).HasMaxLength(4);
            });
            builder.OwnsOne(x => x.FeeAmount, a =>
            {
                a.Property(x => x.Currency).HasMaxLength(4);
            });


            builder.Property(x => x.TransactionPeriod)
                   .HasConversion(transactionPeriod => transactionPeriod.Value, value => TransactionPeriod.From(value))
                   .HasMaxLength(TransactionPeriod.MaxLength)
                   .IsRequired();

            builder.OwnsOne(x => x.PaymentMethod, pm =>
            {
                pm.Property(x => x.CashAccountNumber)
                  .HasConversion(accountNumber => accountNumber.Value, value => AccountNumber.From(value))
                  .HasMaxLength(AccountNumber.MaxLength);
            });

            builder.HasIndex(x => x.TransactionPeriod).IsUnique(false);
            builder.HasIndex(x => x.TransactionDate).IsUnique(false);
        }
    }
}
