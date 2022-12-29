using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Perfi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameIncomePaymentMethodToPaymentDeposition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashAccountIncomePaymentMethod");

            migrationBuilder.DropTable(
                name: "IncomePaymentMethod");

            migrationBuilder.CreateTable(
                name: "PaymentDeposition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDeposition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentDeposition_IncomeDocument_Id",
                        column: x => x.Id,
                        principalTable: "IncomeDocument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymenDepositionToCashAccount",
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
                    table.PrimaryKey("PK_PaymenDepositionToCashAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymenDepositionToCashAccount_PaymentDeposition_Id",
                        column: x => x.Id,
                        principalTable: "PaymentDeposition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymenDepositionToCashAccount");

            migrationBuilder.DropTable(
                name: "PaymentDeposition");

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
                    BankName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CashAccountId = table.Column<int>(type: "integer", nullable: false),
                    CashAccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
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
        }
    }
}
