using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perfi.Core.Accounting.AccountingTransactionAggregate;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Expenses;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    internal class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.ToTable("Expense");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Description).HasMaxLength(Expense.DescriptionMaxLength).IsRequired();

            builder.OwnsOne(x => x.Amount, a =>
            {
                a.Property(x => x.Currency).HasMaxLength(4);
            });

            builder.Property(x => x.ExpenseCategoryCode)
                   .HasConversion(expenseCategoryCode => expenseCategoryCode.Value, value => ExpenseCategoryCode.From(value))
                   .HasMaxLength(ExpenseCategoryCode.MaxLength)
                   .IsRequired();


            builder.Property(x => x.TransactionPeriod)
                   .HasConversion(transactionPeriod => transactionPeriod.Value, value => TransactionPeriod.From(value))
                   .HasMaxLength(TransactionPeriod.MaxLength)
                   .IsRequired();

            builder.HasOne(x => x.PaymentMethod).WithOne().HasForeignKey<ExpensePaymentMethod>();

            builder.HasIndex(x => x.TransactionPeriod).IsUnique(false);
            builder.HasIndex(x => x.TransactionDate).IsUnique(false);
        }
    }
}
