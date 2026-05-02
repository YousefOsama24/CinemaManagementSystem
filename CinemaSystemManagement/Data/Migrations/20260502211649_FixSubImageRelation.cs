using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaSystemManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixSubImageRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieImages_Products_MovieId",
                table: "MovieImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieImages",
                table: "MovieImages");

            migrationBuilder.RenameTable(
                name: "MovieImages",
                newName: "MovieImage");

            migrationBuilder.RenameIndex(
                name: "IX_MovieImages_MovieId",
                table: "MovieImage",
                newName: "IX_MovieImage_MovieId");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "MovieImage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieImage",
                table: "MovieImage",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieImage_Products_MovieId",
                table: "MovieImage",
                column: "MovieId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieImage_Products_MovieId",
                table: "MovieImage");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieImage",
                table: "MovieImage");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "MovieImage");

            migrationBuilder.RenameTable(
                name: "MovieImage",
                newName: "MovieImages");

            migrationBuilder.RenameIndex(
                name: "IX_MovieImage_MovieId",
                table: "MovieImages",
                newName: "IX_MovieImages_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieImages",
                table: "MovieImages",
                column: "Id");

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
