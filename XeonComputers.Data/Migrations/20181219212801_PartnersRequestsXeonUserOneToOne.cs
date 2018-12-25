using Microsoft.EntityFrameworkCore.Migrations;

namespace XeonComputers.Data.Migrations
{
    public partial class PartnersRequestsXeonUserOneToOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PartnerRequests_XeonUserId",
                table: "PartnerRequests");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerRequests_XeonUserId",
                table: "PartnerRequests",
                column: "XeonUserId",
                unique: true,
                filter: "[XeonUserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PartnerRequests_XeonUserId",
                table: "PartnerRequests");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerRequests_XeonUserId",
                table: "PartnerRequests",
                column: "XeonUserId");
        }
    }
}
