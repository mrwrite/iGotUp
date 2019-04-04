using Microsoft.EntityFrameworkCore.Migrations;

namespace iGotUp.Api.Migrations
{
    public partial class AuthenticationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "initialLogin",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "initialLogin",
                table: "Users");
        }
    }
}
