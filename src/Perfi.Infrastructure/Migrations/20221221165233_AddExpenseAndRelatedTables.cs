using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Perfi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExpenseAndRelatedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountingTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    TransactionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DocumentDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingTransaction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Expense",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    TransactionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TransactionPeriod = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DocumentDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ExpenseCategoryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AmountValue = table.Column<decimal>(name: "Amount_Value", type: "numeric", nullable: false),
                    AmountCurrency = table.Column<string>(name: "Amount_Currency", type: "character varying(4)", maxLength: 4, nullable: false),
                    AccountingTransactionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expense", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountingEntry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DocumentDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TransactionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    AccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DebitAmountValue = table.Column<decimal>(name: "DebitAmount_Value", type: "numeric", nullable: false),
                    DebitAmountCurrency = table.Column<string>(name: "DebitAmount_Currency", type: "character varying(4)", maxLength: 4, nullable: false),
                    CreditAmountValue = table.Column<decimal>(name: "CreditAmount_Value", type: "numeric", nullable: false),
                    CreditAmountCurrency = table.Column<string>(name: "CreditAmount_Currency", type: "character varying(4)", maxLength: 4, nullable: false),
                    AccountingTransactionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountingEntry_AccountingTransaction_AccountingTransaction~",
                        column: x => x.AccountingTransactionId,
                        principalTable: "AccountingTransaction",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExpensePaymentMethod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpensePaymentMethod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpensePaymentMethod_Expense_Id",
                        column: x => x.Id,
                        principalTable: "Expense",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CashAccountExpensePaymentMethod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    BankName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AssociatedAccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashAccountExpensePaymentMethod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashAccountExpensePaymentMethod_ExpensePaymentMethod_Id",
                        column: x => x.Id,
                        principalTable: "ExpensePaymentMethod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreditCardExpensePaymentMethod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    LastFourDigits = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    CreditorName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AssociatedAccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCardExpensePaymentMethod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditCardExpensePaymentMethod_ExpensePaymentMethod_Id",
                        column: x => x.Id,
                        principalTable: "ExpensePaymentMethod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountingEntry_AccountingTransactionId",
                table: "AccountingEntry",
                column: "AccountingTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingEntry_TransactionDate",
                table: "AccountingEntry",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingTransaction_TransactionDate",
                table: "AccountingTransaction",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_TransactionDate",
                table: "Expense",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_TransactionPeriod",
                table: "Expense",
                column: "TransactionPeriod");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountingEntry");

            migrationBuilder.DropTable(
                name: "CashAccountExpensePaymentMethod");

            migrationBuilder.DropTable(
                name: "CreditCardExpensePaymentMethod");

            migrationBuilder.DropTable(
                name: "AccountingTransaction");

            migrationBuilder.DropTable(
                name: "ExpensePaymentMethod");

            migrationBuilder.DropTable(
                name: "Expense");
        }
    }
}
