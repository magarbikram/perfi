using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Perfi.Core.Expenses;
using Perfi.Core.Payments.IncomingPayments;
using Perfi.Core.Accounts.AccountAggregate;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    public class IncomingPaymentConfiguration : IEntityTypeConfiguration<IncomingPayment>
    {
        public void Configure(EntityTypeBuilder<IncomingPayment> builder)
        {
            builder.ToTable("IncomingPayment");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Description).HasMaxLength(Expense.DescriptionMaxLength).IsRequired();

            builder.OwnsOne(x => x.Amount, a =>
            {
                a.Property(x => x.Currency).HasMaxLength(4);
            });

            builder.Property(x => x.DepositedToAccountNumber)
                   .HasConversion(accountNumber => accountNumber.Value, value => AccountNumber.From(value))
                   .HasMaxLength(AccountNumber.MaxLength)
                   .IsRequired();

            builder.Property(x => x.TransactionPeriod)
                   .HasConversion(transactionPeriod => transactionPeriod.Value, value => TransactionPeriod.From(value))
                   .HasMaxLength(TransactionPeriod.MaxLength)
                   .IsRequired();

            builder.HasIndex(x => x.TransactionPeriod).IsUnique(false);
            builder.HasIndex(x => x.TransactionDate).IsUnique(false);
        }
    }
}
