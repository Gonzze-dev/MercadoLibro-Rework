using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MercadoLibroDB.Migrations
{
    /// <inheritdoc />
    public partial class CreateEntitiesOfSchemeTest1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ISBN",
                table: "CartLine",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coupon",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    Discount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupon", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Publisher",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publisher", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Province",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CountryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Province", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Province_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    ISBN = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    EditionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    EntryDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Discount = table.Column<decimal>(type: "numeric", nullable: false),
                    PublisherId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.ISBN);
                    table.ForeignKey(
                        name: "FK_Book_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Book_Publisher_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Publisher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    PostalCode = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ProvinceId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.PostalCode);
                    table.ForeignKey(
                        name: "FK_City_Province_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Province",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Author",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    BookISBN = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Author", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Author_Book_BookISBN",
                        column: x => x.BookISBN,
                        principalTable: "Book",
                        principalColumn: "ISBN");
                });

            migrationBuilder.CreateTable(
                name: "BookGenre",
                columns: table => new
                {
                    BooksISBN = table.Column<string>(type: "text", nullable: false),
                    GenreId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookGenre", x => new { x.BooksISBN, x.GenreId });
                    table.ForeignKey(
                        name: "FK_BookGenre_Book_BooksISBN",
                        column: x => x.BooksISBN,
                        principalTable: "Book",
                        principalColumn: "ISBN",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookGenre_Genre_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    ISBN = table.Column<string>(type: "text", nullable: false),
                    UserComment = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => new { x.UserID, x.ISBN });
                    table.ForeignKey(
                        name: "FK_Comment_Book_ISBN",
                        column: x => x.ISBN,
                        principalTable: "Book",
                        principalColumn: "ISBN",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comment_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Raiting",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    ISBN = table.Column<string>(type: "text", nullable: false),
                    RaitingBook = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Raiting", x => new { x.UserID, x.ISBN });
                    table.ForeignKey(
                        name: "FK_Raiting_Book_ISBN",
                        column: x => x.ISBN,
                        principalTable: "Book",
                        principalColumn: "ISBN",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Raiting_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Additional = table.Column<string>(type: "text", nullable: false),
                    Dni = table.Column<int>(type: "integer", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PostalCode = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryAddress_City_PostalCode",
                        column: x => x.PostalCode,
                        principalTable: "City",
                        principalColumn: "PostalCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartLine_ISBN",
                table: "CartLine",
                column: "ISBN");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_CouponCode",
                table: "Cart",
                column: "CouponCode");

            migrationBuilder.CreateIndex(
                name: "IX_Author_BookISBN",
                table: "Author",
                column: "BookISBN");

            migrationBuilder.CreateIndex(
                name: "IX_Book_LanguageId",
                table: "Book",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Book_PublisherId",
                table: "Book",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_BookGenre_GenreId",
                table: "BookGenre",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_City_ProvinceId",
                table: "City",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ISBN",
                table: "Comment",
                column: "ISBN");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAddress_PostalCode",
                table: "DeliveryAddress",
                column: "PostalCode");

            migrationBuilder.CreateIndex(
                name: "IX_Province_CountryId",
                table: "Province",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Raiting_ISBN",
                table: "Raiting",
                column: "ISBN");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Coupon_CouponCode",
                table: "Cart",
                column: "CouponCode",
                principalTable: "Coupon",
                principalColumn: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_CartLine_Book_ISBN",
                table: "CartLine",
                column: "ISBN",
                principalTable: "Book",
                principalColumn: "ISBN",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Coupon_CouponCode",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_CartLine_Book_ISBN",
                table: "CartLine");

            migrationBuilder.DropTable(
                name: "Author");

            migrationBuilder.DropTable(
                name: "BookGenre");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Coupon");

            migrationBuilder.DropTable(
                name: "DeliveryAddress");

            migrationBuilder.DropTable(
                name: "Raiting");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "Province");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "Publisher");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropIndex(
                name: "IX_CartLine_ISBN",
                table: "CartLine");

            migrationBuilder.DropIndex(
                name: "IX_Cart_CouponCode",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "ISBN",
                table: "CartLine");
        }
    }
}
