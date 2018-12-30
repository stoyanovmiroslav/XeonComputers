using Microsoft.EntityFrameworkCore.Migrations;

namespace XeonComputers.Data.Migrations
{
    public partial class AddUserRequestSeen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Seen",
                table: "UserRequests",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Seen",
                table: "UserRequests");
        }
    }
}
