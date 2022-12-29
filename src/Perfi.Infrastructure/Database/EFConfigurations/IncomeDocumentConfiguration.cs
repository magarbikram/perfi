using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Perfi.Core.Earnings;
using Perfi.Core.Expenses;
using Perfi.Core.Earnings.IncomeSources;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    internal class IncomeDocumentConfiguration : IEntityTypeConfiguration<IncomeDocument>
    {
        public void Configure(EntityTypeBuilder<IncomeDocument> builder)
        {
            builder.ToTable("IncomeDocument");
            builder.HasKey(x => x.Id);
            builder.OwnsOne(x => x.Source, s =>
            {
                s.Property(x => x.Name).HasMaxLength(IncomeSource.MaxLengths.Name).IsRequired();
                s.Property(x => x.IncomeSourceId).IsRequired();
            });

            builder.Property(x => x.TransactionPeriod)
                  .HasConversion(transactionPeriod => transactionPeriod.Value, value => TransactionPeriod.From(value))
                  .HasMaxLength(TransactionPeriod.MaxLength)
                  .IsRequired();

            builder.OwnsOne(x => x.Amount, la =>
            {
                la.Property(x => x.Value).IsRequired(true);
                la.Property(x => x.Currency).HasMaxLength(5).IsRequired(true);
            });

            builder.HasIndex(x => x.TransactionPeriod).IsUnique(false);
            builder.HasIndex(x => x.TransactionDate).IsUnique(false);

            builder.HasOne(x => x.PaymentDeposition).WithOne().HasForeignKey<PaymentDeposition>();
        }
    }
}
