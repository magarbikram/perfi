using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Perfi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExpenseCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpenseCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    AssociatedExpenseAccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Type = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    SummaryExpenseCategoryCode = table.Column<string>(type: "character varying(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseCategory", x => x.Id);
                    table.UniqueConstraint("AK_ExpenseCategory_Code", x => x.Code);
                    table.ForeignKey(
                        name: "FK_ExpenseCategory_ExpenseCategory_SummaryExpenseCategoryCode",
                        column: x => x.SummaryExpenseCategoryCode,
                        principalTable: "ExpenseCategory",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseCategory_Code",
                table: "ExpenseCategory",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseCategory_SummaryExpenseCategoryCode",
                table: "ExpenseCategory",
                column: "SummaryExpenseCategoryCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpenseCategory");
        }
    }
}
