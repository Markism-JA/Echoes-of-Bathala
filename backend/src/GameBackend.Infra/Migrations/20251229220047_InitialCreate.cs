using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameBackend.Infra.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "character(36)", fixedLength: true, maxLength: 36, nullable: false),
                    Username = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    HashedPassword = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    LinkedWalletAddress = table.Column<string>(type: "character varying(42)", maxLength: 42, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.AccountId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_Email",
                table: "Players",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_LinkedWalletAddress",
                table: "Players",
                column: "LinkedWalletAddress",
                unique: true,
                filter: "\"LinkedWalletAddress\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Players_Username",
                table: "Players",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
