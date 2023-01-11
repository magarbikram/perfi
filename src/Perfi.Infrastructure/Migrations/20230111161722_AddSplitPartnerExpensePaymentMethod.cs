using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Perfi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSplitPartnerExpensePaymentMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SplitPartnerExpensePaymentMethod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    SplitPartnerName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    SplitPartnerId = table.Column<int>(type: "integer", nullable: false),
                    ReceivableAccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SplitPartnerExpensePaymentMethod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SplitPartnerExpensePaymentMethod_ExpensePaymentMethod_Id",
                        column: x => x.Id,
                        principalTable: "ExpensePaymentMethod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SplitPartnerExpensePaymentMethod");
        }
    }
}
