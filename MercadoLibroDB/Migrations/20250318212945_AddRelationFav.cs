using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercadoLibroDB.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationFav : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookUser",
                columns: table => new
                {
                    FavoritedById = table.Column<Guid>(type: "uuid", nullable: false),
                    FavoritesISBN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookUser", x => new { x.FavoritedById, x.FavoritesISBN });
                    table.ForeignKey(
                        name: "FK_BookUser_Book_FavoritesISBN",
                        column: x => x.FavoritesISBN,
                        principalTable: "Book",
                        principalColumn: "ISBN",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookUser_User_FavoritedById",
                        column: x => x.FavoritedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookUser_FavoritesISBN",
                table: "BookUser",
                column: "FavoritesISBN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookUser");
        }
    }
}
