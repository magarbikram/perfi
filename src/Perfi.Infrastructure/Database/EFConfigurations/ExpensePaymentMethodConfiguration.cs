using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perfi.Core.Expenses;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    public class ExpensePaymentMethodConfiguration : IEntityTypeConfiguration<ExpensePaymentMethod>
    {
        public void Configure(EntityTypeBuilder<ExpensePaymentMethod> builder)
        {
            builder.ToTable("ExpensePaymentMethod");
            builder.HasKey(x => x.Id);
        }
    }
}
