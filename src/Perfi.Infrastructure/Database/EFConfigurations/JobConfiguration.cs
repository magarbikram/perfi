using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Perfi.Core.Accounts.AccountAggregate;
using Perfi.Core.Accounts.LoanAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Perfi.Core.Accounts.Jobs;

namespace Perfi.Infrastructure.Database.EFConfigurations
{
    internal class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.ToTable("Job");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.JobHolder).HasMaxLength(Job.MaxLengths.JobHolder).IsRequired();
            builder.Property(x => x.Employee).HasMaxLength(Job.MaxLengths.Employee).IsRequired();

            builder.Property(x => x.AssociatedAccountNumber)
                   .HasConversion(accountNumber => accountNumber.Value, value => AccountNumber.From(value))
                   .IsRequired()
                   .HasMaxLength(AccountNumber.MaxLength);

            builder.HasMany(j => j.IncomeSplits).WithOne();
        }
    }
}
