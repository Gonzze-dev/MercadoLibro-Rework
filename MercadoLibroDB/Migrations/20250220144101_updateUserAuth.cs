using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercadoLibroDB.Migrations
{
    /// <inheritdoc />
    public partial class updateUserAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UserID",
                table: "UserAuth",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "UserAuth",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "AuthMethod",
                table: "UserAuth",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProviderId",
                table: "UserAuth",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthMethod",
                table: "UserAuth");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "UserAuth");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserID",
                table: "UserAuth",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "UserAuth",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
