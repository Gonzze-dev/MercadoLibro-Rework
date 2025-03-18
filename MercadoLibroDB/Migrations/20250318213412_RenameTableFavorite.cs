using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercadoLibroDB.Migrations
{
    /// <inheritdoc />
    public partial class RenameTableFavorite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookUser_Book_FavoritesISBN",
                table: "BookUser");

            migrationBuilder.DropForeignKey(
                name: "FK_BookUser_User_FavoritedById",
                table: "BookUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookUser",
                table: "BookUser");

            migrationBuilder.DropIndex(
                name: "IX_BookUser_FavoritesISBN",
                table: "BookUser");

            migrationBuilder.RenameTable(
                name: "BookUser",
                newName: "Favorite");

            migrationBuilder.RenameColumn(
                name: "FavoritesISBN",
                table: "Favorite",
                newName: "FavoriteISBN");

            migrationBuilder.RenameColumn(
                name: "FavoritedById",
                table: "Favorite",
                newName: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Favorite",
                table: "Favorite",
                columns: new[] { "FavoriteISBN", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Favorite_UserId",
                table: "Favorite",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorite_Book_FavoriteISBN",
                table: "Favorite",
                column: "FavoriteISBN",
                principalTable: "Book",
                principalColumn: "ISBN",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorite_User_UserId",
                table: "Favorite",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorite_Book_FavoriteISBN",
                table: "Favorite");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorite_User_UserId",
                table: "Favorite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Favorite",
                table: "Favorite");

            migrationBuilder.DropIndex(
                name: "IX_Favorite_UserId",
                table: "Favorite");

            migrationBuilder.RenameTable(
                name: "Favorite",
                newName: "BookUser");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "BookUser",
                newName: "FavoritedById");

            migrationBuilder.RenameColumn(
                name: "FavoriteISBN",
                table: "BookUser",
                newName: "FavoritesISBN");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookUser",
                table: "BookUser",
                columns: new[] { "FavoritedById", "FavoritesISBN" });

            migrationBuilder.CreateIndex(
                name: "IX_BookUser_FavoritesISBN",
                table: "BookUser",
                column: "FavoritesISBN");

            migrationBuilder.AddForeignKey(
                name: "FK_BookUser_Book_FavoritesISBN",
                table: "BookUser",
                column: "FavoritesISBN",
                principalTable: "Book",
                principalColumn: "ISBN",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookUser_User_FavoritedById",
                table: "BookUser",
                column: "FavoritedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
