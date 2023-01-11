using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CreditCardAggregate;
using Perfi.Core.Expenses;
using Perfi.Core.SplitPartners;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    public class SplitPartnerExpensePaymentMethodConfiguration : IEntityTypeConfiguration<SplitPartnerExpensePaymentMethod>
    {
        public void Configure(EntityTypeBuilder<SplitPartnerExpensePaymentMethod> builder)
        {
            builder.ToTable("SplitPartnerExpensePaymentMethod");
            builder.Property(x => x.SplitPartnerName).HasMaxLength(SplitPartner.NameMaxLength).IsRequired();
            builder.Property(x => x.SplitPartnerId).IsRequired();
            builder.Property(x => x.ReceivableAccountNumber)
                   .HasConversion(accountNumber => accountNumber.Value, value => AccountNumber.From(value))
                   .IsRequired()
                   .HasMaxLength(AccountNumber.MaxLength);
        }
    }
}
