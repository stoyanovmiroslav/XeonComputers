using Microsoft.EntityFrameworkCore.Migrations;

namespace XeonComputers.Data.Migrations
{
    public partial class MapXeonUserOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "XeonUserId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_XeonUserId",
                table: "Orders",
                column: "XeonUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_XeonUserId",
                table: "Orders",
                column: "XeonUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_XeonUserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_XeonUserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "XeonUserId",
                table: "Orders");
        }
    }
}
