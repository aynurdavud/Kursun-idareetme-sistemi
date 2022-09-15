using Microsoft.EntityFrameworkCore.Migrations;

namespace Courseeee.Migrations
{
    public partial class neewetyhjhgf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MailMessage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MailMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Başlıq = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kimə = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mətn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MailMessage_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MailMessage_StudentId",
                table: "MailMessage",
                column: "StudentId");
        }
    }
}
