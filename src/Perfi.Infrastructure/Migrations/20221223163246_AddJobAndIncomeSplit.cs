using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Perfi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddJobAndIncomeSplit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Job",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Employee = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    JobHolder = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    AssociatedAccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
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
                    CashAccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SplitAmountValue = table.Column<decimal>(name: "SplitAmount_Value", type: "numeric", nullable: true),
                    SplitAmountCurrency = table.Column<string>(name: "SplitAmount_Currency", type: "character varying(5)", maxLength: 5, nullable: true),
                    SplitRemainderAmount = table.Column<bool>(type: "boolean", nullable: false),
                    JobId = table.Column<int>(type: "integer", nullable: true)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobIncomeSplit");

            migrationBuilder.DropTable(
                name: "Job");
        }
    }
}
