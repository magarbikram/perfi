using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Perfi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMoneyTransfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MoneyTransfer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Remarks = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    FromAccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    From = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ToAccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    To = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    AmountValue = table.Column<decimal>(name: "Amount_Value", type: "numeric", nullable: false),
                    AmountCurrency = table.Column<string>(name: "Amount_Currency", type: "character varying(4)", maxLength: 4, nullable: false),
                    TransactionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TransactionPeriod = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DocumentDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    AccountingTransactionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoneyTransfer", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransfer_TransactionDate",
                table: "MoneyTransfer",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransfer_TransactionPeriod",
                table: "MoneyTransfer",
                column: "TransactionPeriod");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoneyTransfer");
        }
    }
}
