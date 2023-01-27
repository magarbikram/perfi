using Microsoft.EntityFrameworkCore.Migrations;
using Perfi.Core.Accounts.AccountAggregate;

#nullable disable

namespace Perfi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedIncomeSourceSummaryAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"
INSERT INTO public.""Account""(
	                ""AccountCategory"", ""Number"", ""Name"", ""Type"")
	         VALUES ('{AccountCategory.Revenues}', '{SummaryAccount.DefaultAccountNumbers.IncomeSourcesAccount}', 'Income Sources Accounts', 'Summary');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
