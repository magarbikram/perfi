using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perfi.Core.Expenses;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    public class SummaryExpenseCategoryConfiguration : IEntityTypeConfiguration<SummaryExpenseCategory>
    {
        public void Configure(EntityTypeBuilder<SummaryExpenseCategory> builder)
        {
            builder.ToTable("ExpenseCategory");
            builder.HasMany(x => x.Categories).WithOne()
                   .HasForeignKey(x => x.SummaryExpenseCategoryCode)
                   .HasPrincipalKey(x => x.Code);
        }
    }
}
