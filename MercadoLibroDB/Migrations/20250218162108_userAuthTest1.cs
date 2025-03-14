﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercadoLibroDB.Migrations
{
    /// <inheritdoc />
    public partial class userAuthTest1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAuth_User_UserID",
                table: "UserAuth");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserID",
                table: "UserAuth",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAuth_User_UserID",
                table: "UserAuth",
                column: "UserID",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAuth_User_UserID",
                table: "UserAuth");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserID",
                table: "UserAuth",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAuth_User_UserID",
                table: "UserAuth",
                column: "UserID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
