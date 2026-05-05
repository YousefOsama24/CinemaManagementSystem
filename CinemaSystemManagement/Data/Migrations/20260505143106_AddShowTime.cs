using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaSystemManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddShowTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductsId",
                table: "ShowTimes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShowTime",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_ShowTimes_ProductsId",
                table: "ShowTimes",
                column: "ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowTimes_Products_ProductsId",
                table: "ShowTimes",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "ProductsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowTimes_Products_ProductsId",
                table: "ShowTimes");

            migrationBuilder.DropIndex(
                name: "IX_ShowTimes_ProductsId",
                table: "ShowTimes");

            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "ShowTimes");

            migrationBuilder.DropColumn(
                name: "ShowTime",
                table: "Products");
        }
    }
}
