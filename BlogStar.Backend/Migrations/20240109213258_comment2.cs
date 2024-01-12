using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogStar.Backend.Migrations
{
    public partial class comment2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorImagePath",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorImagePath",
                table: "Blogs");
        }
    }
}
