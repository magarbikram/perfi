using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.LoanAggregate;
using Perfi.Core.SplitPartners;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    public class SplitPartnerConfiguration : IEntityTypeConfiguration<SplitPartner>
    {
        public void Configure(EntityTypeBuilder<SplitPartner> builder)
        {
            builder.ToTable("SplitPartner");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(Loan.MaxLengths.Name).IsRequired();
            builder.Property(x => x.ReceivableAccountNumber)
                   .HasConversion(accountNumber => accountNumber.Value, value => AccountNumber.From(value))
                   .IsRequired()
                   .HasMaxLength(AccountNumber.MaxLength);
        }
    }
}
