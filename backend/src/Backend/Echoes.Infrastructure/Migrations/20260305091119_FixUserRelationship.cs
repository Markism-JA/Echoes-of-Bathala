using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Echoes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixUserRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Users_Id",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameIndex(
                name: "IX_Users_NormalizedUserName",
                table: "User",
                newName: "IX_User_NormalizedUserName");

            migrationBuilder.RenameIndex(
                name: "IX_Users_NormalizedEmail",
                table: "User",
                newName: "IX_User_NormalizedEmail");

            migrationBuilder.RenameIndex(
                name: "IX_Users_LinkedWalletAddress",
                table: "User",
                newName: "IX_User_LinkedWalletAddress");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_AspNetUsers_Id",
                table: "User",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_AspNetUsers_Id",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_User_NormalizedUserName",
                table: "Users",
                newName: "IX_Users_NormalizedUserName");

            migrationBuilder.RenameIndex(
                name: "IX_User_NormalizedEmail",
                table: "Users",
                newName: "IX_Users_NormalizedEmail");

            migrationBuilder.RenameIndex(
                name: "IX_User_LinkedWalletAddress",
                table: "Users",
                newName: "IX_Users_LinkedWalletAddress");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Users_Id",
                table: "AspNetUsers",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
