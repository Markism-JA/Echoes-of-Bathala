using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameBackend.Infra.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePlayerToAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Players",
                table: "Players");

            migrationBuilder.RenameTable(
                name: "Players",
                newName: "Accounts");

            migrationBuilder.RenameIndex(
                name: "IX_Players_Username",
                table: "Accounts",
                newName: "IX_Accounts_Username");

            migrationBuilder.RenameIndex(
                name: "IX_Players_LinkedWalletAddress",
                table: "Accounts",
                newName: "IX_Accounts_LinkedWalletAddress");

            migrationBuilder.RenameIndex(
                name: "IX_Players_Email",
                table: "Accounts",
                newName: "IX_Accounts_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "Players");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_Username",
                table: "Players",
                newName: "IX_Players_Username");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_LinkedWalletAddress",
                table: "Players",
                newName: "IX_Players_LinkedWalletAddress");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_Email",
                table: "Players",
                newName: "IX_Players_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Players",
                table: "Players",
                column: "AccountId");
        }
    }
}
