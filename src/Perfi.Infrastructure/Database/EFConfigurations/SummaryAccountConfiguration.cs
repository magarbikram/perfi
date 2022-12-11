using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perfi.Core.Accounts.AccountAggregate;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    public class SummaryAccountConfiguration : IEntityTypeConfiguration<SummaryAccount>
    {
        public void Configure(EntityTypeBuilder<SummaryAccount> builder)
        {
            builder.ToTable("Account");
            builder.HasMany(x => x.Components).WithOne()
                   .HasForeignKey(x => x.ParentAccountNumber)
                   .HasPrincipalKey(x => x.Number);
        }
    }
}
