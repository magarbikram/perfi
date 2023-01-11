using Microsoft.EntityFrameworkCore.Migrations;
using Perfi.Core.Accounts.AccountAggregate;

#nullable disable

namespace Perfi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountReceivableSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"
INSERT INTO public.""Account""(
	                ""AccountCategory"", ""Number"", ""Name"", ""Type"")
	         VALUES ('{AccountCategory.Assets}', '{SummaryAccount.DefaultAccountNumbers.ReceivableAccount}', 'Receivable Accounts', 'Summary');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
