using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perfi.Core.Accounting.AccountingTransactionAggregate;
using Perfi.Core.Accounts.AccountAggregate;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    internal class AccountingEntryConfiguration : IEntityTypeConfiguration<AccountingEntry>
    {
        public void Configure(EntityTypeBuilder<AccountingEntry> builder)
        {
            builder.ToTable("AccountingEntry");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.AccountNumber)
                   .HasConversion(accountNumber => accountNumber.Value, value => AccountNumber.From(value))
                   .HasMaxLength(AccountNumber.MaxLength)
                   .IsRequired();

            builder.OwnsOne(x => x.DebitAmount, da =>
            {
                da.Property(x => x.Currency).HasMaxLength(4).IsRequired(false);
                da.Property(x => x.Value).IsRequired(false);
            });

            builder.OwnsOne(x => x.CreditAmount, da =>
            {
                da.Property(x => x.Currency).HasMaxLength(4).IsRequired(false);
                da.Property(x => x.Value).IsRequired(false);
            });

            builder.HasIndex(x => x.TransactionDate).IsUnique(false);
        }
    }
}
