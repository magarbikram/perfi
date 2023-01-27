using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Perfi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIncomingAndOutgoingPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IncomingPayment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransactionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TransactionPeriod = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    AmountValue = table.Column<decimal>(name: "Amount_Value", type: "numeric", nullable: false),
                    AmountCurrency = table.Column<string>(name: "Amount_Currency", type: "character varying(4)", maxLength: 4, nullable: false),
                    DepositedToAccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingPayment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutgoingPayment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransactionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TransactionPeriod = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    AmountValue = table.Column<decimal>(name: "Amount_Value", type: "numeric", nullable: false),
                    AmountCurrency = table.Column<string>(name: "Amount_Currency", type: "character varying(4)", maxLength: 4, nullable: false),
                    PaidFromAccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutgoingPayment", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncomingPayment_TransactionDate",
                table: "IncomingPayment",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingPayment_TransactionPeriod",
                table: "IncomingPayment",
                column: "TransactionPeriod");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingPayment_TransactionDate",
                table: "OutgoingPayment",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingPayment_TransactionPeriod",
                table: "OutgoingPayment",
                column: "TransactionPeriod");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomingPayment");

            migrationBuilder.DropTable(
                name: "OutgoingPayment");
        }
    }
}
