using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Perfi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountCategory = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ParentAccountNumber = table.Column<int>(type: "integer", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_Account_ParentAccountNumber",
                        column: x => x.ParentAccountNumber,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_AccountCategory",
                table: "Account",
                column: "AccountCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Account_Number",
                table: "Account",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_ParentAccountNumber",
                table: "Account",
                column: "ParentAccountNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
