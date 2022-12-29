using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.Core.Earnings;
using Perfi.Core.Expenses;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    public class PaymentDepositionToCashAccountConfiguration : IEntityTypeConfiguration<PaymenDepositionToCashAccount>
    {
        public void Configure(EntityTypeBuilder<PaymenDepositionToCashAccount> builder)
        {
            builder.ToTable("PaymenDepositionToCashAccount");
            builder.Property(x => x.Name).HasMaxLength(CashAccount.MaxLengths.Name).IsRequired();
            builder.Property(x => x.BankName).HasMaxLength(CashAccount.MaxLengths.BankName).IsRequired();
            builder.Property(x => x.CashAccountNumber)
                   .HasConversion(accountNumber => accountNumber.Value, value => AccountNumber.From(value))
                   .IsRequired()
                   .HasMaxLength(AccountNumber.MaxLength);
        }
    }
}
