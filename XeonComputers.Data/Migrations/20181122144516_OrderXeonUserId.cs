using Microsoft.EntityFrameworkCore.Migrations;

namespace XeonComputers.Data.Migrations
{
    public partial class OrderXeonUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_XeonUserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_XeonUserId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "XeonUserId",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XeonUserId1",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_XeonUserId1",
                table: "Orders",
                column: "XeonUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_XeonUserId1",
                table: "Orders",
                column: "XeonUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_XeonUserId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_XeonUserId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "XeonUserId1",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "XeonUserId",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(int));

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
    }
}
