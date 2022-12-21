using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.Core.Expenses;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    public class CashAccountExpensePaymentMethodConfiguration : IEntityTypeConfiguration<CashAccountExpensePaymentMethod>
    {
        public void Configure(EntityTypeBuilder<CashAccountExpensePaymentMethod> builder)
        {
            builder.ToTable("CashAccountExpensePaymentMethod");
            builder.Property(x => x.Name).HasMaxLength(CashAccount.MaxLengths.Name).IsRequired();
            builder.Property(x => x.BankName).HasMaxLength(CashAccount.MaxLengths.BankName).IsRequired();
            builder.Property(x => x.AssociatedAccountNumber)
                   .HasConversion(accountNumber => accountNumber.Value, value => AccountNumber.From(value))
                   .IsRequired()
                   .HasMaxLength(AccountNumber.MaxLength);
        }
    }
}
