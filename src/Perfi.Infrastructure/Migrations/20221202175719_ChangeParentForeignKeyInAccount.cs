using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Perfi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeParentForeignKeyInAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Account_ParentAccountId",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_ParentAccountId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "ParentAccountId",
                table: "Account");

            migrationBuilder.AddColumn<string>(
                name: "ParentAccountNumber",
                table: "Account",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Account_Number",
                table: "Account",
                column: "Number");

            migrationBuilder.CreateIndex(
                name: "IX_Account_ParentAccountNumber",
                table: "Account",
                column: "ParentAccountNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Account_ParentAccountNumber",
                table: "Account",
                column: "ParentAccountNumber",
                principalTable: "Account",
                principalColumn: "Number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Account_ParentAccountNumber",
                table: "Account");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Account_Number",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_ParentAccountNumber",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "ParentAccountNumber",
                table: "Account");

            migrationBuilder.AddColumn<int>(
                name: "ParentAccountId",
                table: "Account",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_ParentAccountId",
                table: "Account",
                column: "ParentAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Account_ParentAccountId",
                table: "Account",
                column: "ParentAccountId",
                principalTable: "Account",
                principalColumn: "Id");
        }
    }
}
