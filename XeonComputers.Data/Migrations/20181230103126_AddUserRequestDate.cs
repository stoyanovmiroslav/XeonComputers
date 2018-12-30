using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace XeonComputers.Data.Migrations
{
    public partial class AddUserRequestDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "UserRequests",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "UserRequests");
        }
    }
}
