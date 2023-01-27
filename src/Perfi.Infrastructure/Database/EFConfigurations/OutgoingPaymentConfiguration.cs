using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Perfi.Core.Expenses;
using Perfi.Core.Payments.OutgoingPayments;
using Perfi.Core.Accounts.AccountAggregate;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    public class OutgoingPaymentConfiguration : IEntityTypeConfiguration<OutgoingPayment>
    {
        public void Configure(EntityTypeBuilder<OutgoingPayment> builder)
        {
            builder.ToTable("OutgoingPayment");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Description).HasMaxLength(Expense.DescriptionMaxLength).IsRequired();

            builder.OwnsOne(x => x.Amount, a =>
            {
                a.Property(x => x.Currency).HasMaxLength(4);
            });

            builder.Property(x => x.PaidFromAccountNumber)
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
