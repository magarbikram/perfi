using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perfi.Core.Earnings;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    public class PaymentDepositionConfiguration : IEntityTypeConfiguration<PaymentDeposition>
    {
        public void Configure(EntityTypeBuilder<PaymentDeposition> builder)
        {
            builder.ToTable("PaymentDeposition");
            builder.HasKey(x => x.Id);
        }
    }
}
