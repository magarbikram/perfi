using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Perfi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addloanpayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoanPayment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LoanId = table.Column<int>(type: "integer", nullable: false),
                    TransactionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TransactionPeriod = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DocumentDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PaymentMethodCashAccountId = table.Column<int>(name: "PaymentMethod_CashAccountId", type: "integer", nullable: false),
                    PaymentMethodCashAccountNumber = table.Column<string>(name: "PaymentMethod_CashAccountNumber", type: "character varying(20)", maxLength: 20, nullable: false),
                    PrincipalAmountValue = table.Column<decimal>(name: "PrincipalAmount_Value", type: "numeric", nullable: false),
                    PrincipalAmountCurrency = table.Column<string>(name: "PrincipalAmount_Currency", type: "character varying(4)", maxLength: 4, nullable: false),
                    InterestAmountValue = table.Column<decimal>(name: "InterestAmount_Value", type: "numeric", nullable: true),
                    InterestAmountCurrency = table.Column<string>(name: "InterestAmount_Currency", type: "character varying(4)", maxLength: 4, nullable: true),
                    FeeAmountValue = table.Column<decimal>(name: "FeeAmount_Value", type: "numeric", nullable: true),
                    FeeAmountCurrency = table.Column<string>(name: "FeeAmount_Currency", type: "character varying(4)", maxLength: 4, nullable: true),
                    EscrowAmountValue = table.Column<decimal>(name: "EscrowAmount_Value", type: "numeric", nullable: true),
                    EscrowAmountCurrency = table.Column<string>(name: "EscrowAmount_Currency", type: "character varying(4)", maxLength: 4, nullable: true),
                    TransactionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanPayment", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoanPayment_TransactionDate",
                table: "LoanPayment",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_LoanPayment_TransactionPeriod",
                table: "LoanPayment",
                column: "TransactionPeriod");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanPayment");
        }
    }
}
