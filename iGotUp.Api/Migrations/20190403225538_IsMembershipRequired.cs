using Microsoft.EntityFrameworkCore.Migrations;

namespace iGotUp.Api.Migrations
{
    public partial class IsMembershipRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_membership_required",
                table: "Runs",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_membership_required",
                table: "Runs");
        }
    }
}
