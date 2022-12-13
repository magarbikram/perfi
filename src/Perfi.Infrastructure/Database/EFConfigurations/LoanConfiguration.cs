using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perfi.Core.Accounting;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.LoanAggregate;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.ToTable("Loan");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(Loan.MaxLengths.Name).IsRequired();
            builder.Property(x => x.LoanProvider).HasMaxLength(Loan.MaxLengths.LoanProvider).IsRequired();
            builder.Property(x => x.InterestRate).HasConversion(interestRate => interestRate.Value, value => InterestRate.From(value));
            builder.OwnsOne(x => x.LoanAmount, la =>
            {
                la.Property(x => x.Value).IsRequired();
                la.Property(x => x.Currency).HasMaxLength(5).IsRequired();
            });
            builder.Property(x => x.AssociatedAccountNumber)
                   .HasConversion(accountNumber => accountNumber.Value, value => AccountNumber.From(value))
                   .IsRequired()
                   .HasMaxLength(AccountNumber.MaxLength);
        }
    }
}
