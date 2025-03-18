using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercadoLibroDB.Migrations
{
    /// <inheritdoc />
    public partial class CreateRelationAuthorBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Author_Book_BookISBN",
                table: "Author");

            migrationBuilder.DropIndex(
                name: "IX_Author_BookISBN",
                table: "Author");

            migrationBuilder.DropColumn(
                name: "BookISBN",
                table: "Author");

            migrationBuilder.CreateTable(
                name: "AuthorBook",
                columns: table => new
                {
                    AuthorsId = table.Column<int>(type: "integer", nullable: false),
                    BooksISBN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorBook", x => new { x.AuthorsId, x.BooksISBN });
                    table.ForeignKey(
                        name: "FK_AuthorBook_Author_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "Author",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorBook_Book_BooksISBN",
                        column: x => x.BooksISBN,
                        principalTable: "Book",
                        principalColumn: "ISBN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorBook_BooksISBN",
                table: "AuthorBook",
                column: "BooksISBN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorBook");

            migrationBuilder.AddColumn<string>(
                name: "BookISBN",
                table: "Author",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Author_BookISBN",
                table: "Author",
                column: "BookISBN");

            migrationBuilder.AddForeignKey(
                name: "FK_Author_Book_BookISBN",
                table: "Author",
                column: "BookISBN",
                principalTable: "Book",
                principalColumn: "ISBN");
        }
    }
}
