using Microsoft.EntityFrameworkCore.Migrations;

namespace iGotUp.Api.Migrations
{
    public partial class PlusCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "plus_code",
                table: "Locations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "plus_code",
                table: "Locations");
        }
    }
}
