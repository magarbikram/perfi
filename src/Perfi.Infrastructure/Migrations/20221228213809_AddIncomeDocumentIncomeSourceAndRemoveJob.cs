using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Perfi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIncomeDocumentIncomeSourceAndRemoveJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobIncomeSplit");

            migrationBuilder.DropTable(
                name: "Job");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount_Value",
                table: "Expense",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "IncomeDocument",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransactionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TransactionPeriod = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DocumentDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    SourceName = table.Column<string>(name: "Source_Name", type: "character varying(150)", maxLength: 150, nullable: false),
                    SourceIncomeSourceId = table.Column<int>(name: "Source_IncomeSourceId", type: "integer", nullable: false),
                    AmountValue = table.Column<decimal>(name: "Amount_Value", type: "numeric", nullable: false),
                    AmountCurrency = table.Column<string>(name: "Amount_Currency", type: "character varying(5)", maxLength: 5, nullable: false),
                    TransactionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeDocument", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IncomeSource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AssociatedAccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeSource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IncomePaymentMethod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomePaymentMethod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomePaymentMethod_IncomeDocument_Id",
                        column: x => x.Id,
                        principalTable: "IncomeDocument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CashAccountIncomePaymentMethod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    BankName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CashAccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CashAccountId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashAccountIncomePaymentMethod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashAccountIncomePaymentMethod_IncomePaymentMethod_Id",
                        column: x => x.Id,
                        principalTable: "IncomePaymentMethod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncomeDocument_TransactionDate",
                table: "IncomeDocument",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeDocument_TransactionPeriod",
                table: "IncomeDocument",
                column: "TransactionPeriod");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashAccountIncomePaymentMethod");

            migrationBuilder.DropTable(
                name: "IncomeSource");

            migrationBuilder.DropTable(
                name: "IncomePaymentMethod");

            migrationBuilder.DropTable(
                name: "IncomeDocument");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount_Value",
                table: "Expense",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.CreateTable(
                name: "Job",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssociatedAccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Employee = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    JobHolder = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobIncomeSplit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SplitAmountCurrency = table.Column<string>(name: "SplitAmount_Currency", type: "character varying(5)", maxLength: 5, nullable: true),
                    SplitAmountValue = table.Column<decimal>(name: "SplitAmount_Value", type: "numeric", nullable: true),
                    CashAccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    JobId = table.Column<int>(type: "integer", nullable: true),
                    SplitRemainderAmount = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobIncomeSplit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobIncomeSplit_Job_JobId",
                        column: x => x.JobId,
                        principalTable: "Job",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobIncomeSplit_JobId",
                table: "JobIncomeSplit",
                column: "JobId");
        }
    }
}
