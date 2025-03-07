using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercadoLibroDB.Migrations
{
    /// <inheritdoc />
    public partial class MoveRolToUserAndDelProviderId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Admin",
                table: "UserAuth");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "UserAuth");

            migrationBuilder.AddColumn<bool>(
                name: "Admin",
                table: "User",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Admin",
                table: "User");

            migrationBuilder.AddColumn<bool>(
                name: "Admin",
                table: "UserAuth",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ProviderId",
                table: "UserAuth",
                type: "text",
                nullable: true);
        }
    }
}
