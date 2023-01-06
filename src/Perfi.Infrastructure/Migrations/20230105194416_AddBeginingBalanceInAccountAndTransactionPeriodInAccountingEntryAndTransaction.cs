using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Perfi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBeginingBalanceInAccountAndTransactionPeriodInAccountingEntryAndTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionPeriod",
                table: "AccountingTransaction",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TransactionPeriod",
                table: "AccountingEntry",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BeginingBalance_Currency",
                table: "Account",
                type: "character varying(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BeginingBalance_Value",
                table: "Account",
                type: "numeric",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountingTransaction_TransactionPeriod",
                table: "AccountingTransaction",
                column: "TransactionPeriod");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingEntry_TransactionPeriod",
                table: "AccountingEntry",
                column: "TransactionPeriod");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccountingTransaction_TransactionPeriod",
                table: "AccountingTransaction");

            migrationBuilder.DropIndex(
                name: "IX_AccountingEntry_TransactionPeriod",
                table: "AccountingEntry");

            migrationBuilder.DropColumn(
                name: "TransactionPeriod",
                table: "AccountingTransaction");

            migrationBuilder.DropColumn(
                name: "TransactionPeriod",
                table: "AccountingEntry");

            migrationBuilder.DropColumn(
                name: "BeginingBalance_Currency",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "BeginingBalance_Value",
                table: "Account");
        }
    }
}
