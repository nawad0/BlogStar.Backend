using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogStar.Backend.Migrations
{
    public partial class comment1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorImagePath",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorImagePath",
                table: "Articles");
        }
    }
}
