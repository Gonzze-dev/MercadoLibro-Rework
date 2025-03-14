using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercadoLibroDB.Migrations
{
    /// <inheritdoc />
    public partial class userAuthTest2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAuth_User_UserID",
                table: "UserAuth");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAuth_User_UserID",
                table: "UserAuth",
                column: "UserID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAuth_User_UserID",
                table: "UserAuth");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAuth_User_UserID",
                table: "UserAuth",
                column: "UserID",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
