using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Perfi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSplitPaymentInExpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SplitPayment_OwnerShare_Currency",
                table: "Expense",
                type: "character varying(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SplitPayment_OwnerShare_Value",
                table: "Expense",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SplitPayment_SplitPartnerId",
                table: "Expense",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SplitPayment_SplitPartnerReceivableAccountNumber",
                table: "Expense",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SplitPayment_SplitPartnerShare_Currency",
                table: "Expense",
                type: "character varying(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SplitPayment_SplitPartnerShare_Value",
                table: "Expense",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SplitPayment_OwnerShare_Currency",
                table: "Expense");

            migrationBuilder.DropColumn(
                name: "SplitPayment_OwnerShare_Value",
                table: "Expense");

            migrationBuilder.DropColumn(
                name: "SplitPayment_SplitPartnerId",
                table: "Expense");

            migrationBuilder.DropColumn(
                name: "SplitPayment_SplitPartnerReceivableAccountNumber",
                table: "Expense");

            migrationBuilder.DropColumn(
                name: "SplitPayment_SplitPartnerShare_Currency",
                table: "Expense");

            migrationBuilder.DropColumn(
                name: "SplitPayment_SplitPartnerShare_Value",
                table: "Expense");
        }
    }
}
