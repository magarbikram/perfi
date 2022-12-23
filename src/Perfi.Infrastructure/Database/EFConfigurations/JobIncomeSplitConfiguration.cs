using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.Jobs;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    internal class JobIncomeSplitConfiguration : IEntityTypeConfiguration<JobIncomeSplit>
    {
        public void Configure(EntityTypeBuilder<JobIncomeSplit> builder)
        {
            builder.ToTable("JobIncomeSplit");
            builder.HasKey(x => x.Id);
            builder.OwnsOne(x => x.SplitAmount, la =>
            {
                la.Property(x => x.Value).IsRequired(false);
                la.Property(x => x.Currency).HasMaxLength(5).IsRequired(false);
            });
            builder.Property(x => x.CashAccountNumber)
                   .HasConversion(accountNumber => accountNumber.Value, value => AccountNumber.From(value))
                   .IsRequired()
                   .HasMaxLength(AccountNumber.MaxLength);
        }
    }
}
