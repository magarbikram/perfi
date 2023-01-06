using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perfi.Core.Accounts.AccountAggregate;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    internal class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Account");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Number)
                   .HasConversion(accountNumber => accountNumber.Value, value => AccountNumber.From(value))
                   .HasMaxLength(AccountNumber.MaxLength)
                   .IsRequired();

            builder.HasIndex(x => x.Number).IsUnique();//search by number

            builder.Property(x => x.Name).HasMaxLength(Account.NameMaxLength).IsRequired();
            builder.Property(x => x.AccountCategory).HasConversion<string>().IsRequired().HasMaxLength(20);

            builder.HasIndex(x => x.AccountCategory);//search by category

            builder.Property(x => x.ParentAccountNumber)
                   .HasConversion(accountNumber => accountNumber.Value, value => AccountNumber.From(value))
                   .HasMaxLength(AccountNumber.MaxLength)
                   .IsRequired(false);

            builder.HasDiscriminator<string>("Type")
                   .HasValue<SummaryAccount>("Summary")
                   .HasValue<TransactionalAccount>("Transactional");

            builder.OwnsOne(x => x.BeginingBalance, da =>
            {
                da.Property(x => x.Currency).HasMaxLength(4);
                da.Property(x => x.Value);
            });

            builder.Property("Type").HasMaxLength(13);
        }
    }
}
