using Microsoft.EntityFrameworkCore.Migrations;

namespace Courseeee.Migrations
{
    public partial class NewColumnAdding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeactive",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeactive",
                table: "Students");
        }
    }
}
