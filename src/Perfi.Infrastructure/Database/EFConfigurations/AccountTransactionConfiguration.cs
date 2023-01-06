using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perfi.Core.Accounting.AccountingTransactionAggregate;
using Perfi.Core.Expenses;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    internal class AccountTransactionConfiguration : IEntityTypeConfiguration<AccountingTransaction>
    {
        public void Configure(EntityTypeBuilder<AccountingTransaction> builder)
        {
            builder.ToTable("AccountingTransaction");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Description).HasMaxLength(AccountingTransaction.DescriptionMaxLength).IsRequired();
            builder.HasMany(x => x.AccountingEntries).WithOne();
            var navigation = builder.Metadata.FindNavigation(nameof(AccountingTransaction.AccountingEntries));
            if (navigation != null)
            {
                navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
            }

            builder.Property(x => x.TransactionPeriod)
                   .HasConversion(transactionPeriod => transactionPeriod.Value, value => TransactionPeriod.From(value))
                   .HasMaxLength(TransactionPeriod.MaxLength)
                   .IsRequired();

            builder.HasIndex(x => x.TransactionPeriod).IsUnique(false);
            builder.HasIndex(x => x.TransactionDate).IsUnique(false);
        }
    }
}
