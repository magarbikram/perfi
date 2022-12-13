using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using Perfi.Core.Accounts.CreditCardAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    public class CreditCardAccountConfiguration : IEntityTypeConfiguration<CreditCardAccount>
    {
        public void Configure(EntityTypeBuilder<CreditCardAccount> builder)
        {
            builder.ToTable("CreditCardAccount");
            builder.HasKey(x => x.Id);
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
