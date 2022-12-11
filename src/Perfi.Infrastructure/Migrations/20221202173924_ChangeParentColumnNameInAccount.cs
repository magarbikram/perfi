using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Perfi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeParentColumnNameInAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Account_ParentAccountNumber",
                table: "Account");

            migrationBuilder.RenameColumn(
                name: "ParentAccountNumber",
                table: "Account",
                newName: "ParentAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Account_ParentAccountNumber",
                table: "Account",
                newName: "IX_Account_ParentAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Account_ParentAccountId",
                table: "Account",
                column: "ParentAccountId",
                principalTable: "Account",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Account_ParentAccountId",
                table: "Account");

            migrationBuilder.RenameColumn(
                name: "ParentAccountId",
                table: "Account",
                newName: "ParentAccountNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Account_ParentAccountId",
                table: "Account",
                newName: "IX_Account_ParentAccountNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Account_ParentAccountNumber",
                table: "Account",
                column: "ParentAccountNumber",
                principalTable: "Account",
                principalColumn: "Id");
        }
    }
}
