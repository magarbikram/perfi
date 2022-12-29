using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Earnings.IncomeSources;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    public class IncomeSourceConfiguration : IEntityTypeConfiguration<IncomeSource>
    {
        public void Configure(EntityTypeBuilder<IncomeSource> builder)
        {
            builder.ToTable("IncomeSource");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(IncomeSource.MaxLengths.Name);
            builder.Property(x => x.Type).IsRequired().HasMaxLength(IncomeSource.MaxLengths.Type);
            builder.Property(x => x.AssociatedAccountNumber)
                  .HasConversion(accountNumber => accountNumber.Value, value => AccountNumber.From(value))
                  .IsRequired()
                  .HasMaxLength(AccountNumber.MaxLength);
        }
    }
}
