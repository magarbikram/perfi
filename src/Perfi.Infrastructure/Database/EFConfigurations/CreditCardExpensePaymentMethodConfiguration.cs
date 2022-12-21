using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CreditCardAggregate;
using Perfi.Core.Expenses;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    public class CreditCardExpensePaymentMethodConfiguration : IEntityTypeConfiguration<CreditCardExpensePaymentMethod>
    {
        public void Configure(EntityTypeBuilder<CreditCardExpensePaymentMethod> builder)
        {
            builder.ToTable("CreditCardExpensePaymentMethod");
            builder.Property(x => x.Name).HasMaxLength(CreditCardAccount.MaxLengths.Name).IsRequired();
            builder.Property(x => x.CreditorName).HasMaxLength(CreditCardAccount.MaxLengths.CreditorName).IsRequired();
            builder.Property(x => x.LastFourDigits).HasMaxLength(CreditCardAccount.MaxLengths.LastFourDigits).IsRequired();
            builder.Property(x => x.AssociatedAccountNumber)
                   .HasConversion(accountNumber => accountNumber.Value, value => AccountNumber.From(value))
                   .IsRequired()
                   .HasMaxLength(AccountNumber.MaxLength);
        }
    }
}
