using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.CashAccountAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    public class CashAccountConfiguration : IEntityTypeConfiguration<CashAccount>
    {
        public void Configure(EntityTypeBuilder<CashAccount> builder)
        {
            builder.ToTable("CashAccount");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(CashAccount.MaxLengths.Name).IsRequired();
            builder.Property(x => x.BankName).HasMaxLength(CashAccount.MaxLengths.BankName).IsRequired();
            builder.Property(x => x.AssociatedAccountNumber)
                   .HasConversion(accountNumber => accountNumber.Value, value => AccountNumber.From(value))
                   .IsRequired()
                   .HasMaxLength(AccountNumber.MaxLength);
        }
    }
}
