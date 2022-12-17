using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Expenses;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    public class ExpenseCategoryConfiguration : IEntityTypeConfiguration<ExpenseCategory>
    {
        public void Configure(EntityTypeBuilder<ExpenseCategory> builder)
        {
            builder.ToTable("ExpenseCategory");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Code)
                   .HasConversion(expenseCategoryCode => expenseCategoryCode.Value, value => ExpenseCategoryCode.From(value))
                   .HasMaxLength(ExpenseCategoryCode.MaxLength)
                   .IsRequired();

            builder.HasIndex(x => x.Code).IsUnique();//search by code
            builder.Property(x => x.Name).HasMaxLength(ExpenseCategory.NameMaxLength).IsRequired();
            builder.HasDiscriminator<string>("Type")
                   .HasValue<SummaryExpenseCategory>("Summary")
                   .HasValue<TransactionalExpenseCategory>("Transactional");

            builder.Property("Type").HasMaxLength(13);

            builder.Property(x => x.AssociatedExpenseAccountNumber)
                   .HasConversion(accountNumber => accountNumber.Value, value => AccountNumber.From(value))
                   .HasMaxLength(AccountNumber.MaxLength)
                   .IsRequired(true);
        }
    }
}
