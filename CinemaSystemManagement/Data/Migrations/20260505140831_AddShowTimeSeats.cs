using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaSystemManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddShowTimeSeats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieImages_Products_MovieId",
                table: "MovieImages");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Tickets",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Tickets",
                newName: "PurchaseDate");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "Seats",
                newName: "ShowTimeId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "ProductsId");

            migrationBuilder.AddColumn<int>(
                name: "SeatId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShowTimeId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductsId",
                table: "Seats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ShowTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CinemaId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShowTimes_Cinemas_CinemaId",
                        column: x => x.CinemaId,
                        principalTable: "Cinemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShowTimes_Products_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Products",
                        principalColumn: "ProductsId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_SeatId",
                table: "Tickets",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ShowTimeId",
                table: "Tickets",
                column: "ShowTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_ProductsId",
                table: "Seats",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_ShowTimeId",
                table: "Seats",
                column: "ShowTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShowTimes_CinemaId",
                table: "ShowTimes",
                column: "CinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_ShowTimes_MovieId",
                table: "ShowTimes",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieImages_Products_MovieId",
                table: "MovieImages",
                column: "MovieId",
                principalTable: "Products",
                principalColumn: "ProductsId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Products_ProductsId",
                table: "Seats",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "ProductsId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_ShowTimes_ShowTimeId",
                table: "Seats",
                column: "ShowTimeId",
                principalTable: "ShowTimes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Seats_SeatId",
                table: "Tickets",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_ShowTimes_ShowTimeId",
                table: "Tickets",
                column: "ShowTimeId",
                principalTable: "ShowTimes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieImages_Products_MovieId",
                table: "MovieImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Products_ProductsId",
                table: "Seats");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_ShowTimes_ShowTimeId",
                table: "Seats");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Seats_SeatId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_ShowTimes_ShowTimeId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "ShowTimes");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_SeatId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ShowTimeId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Seats_ProductsId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_ShowTimeId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "SeatId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ShowTimeId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "Seats");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Tickets",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "PurchaseDate",
                table: "Tickets",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "ShowTimeId",
                table: "Seats",
                newName: "MovieId");

            migrationBuilder.RenameColumn(
                name: "ProductsId",
                table: "Products",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieImages_Products_MovieId",
                table: "MovieImages",
                column: "MovieId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
