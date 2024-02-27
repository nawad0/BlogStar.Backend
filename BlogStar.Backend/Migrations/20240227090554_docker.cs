using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogStar.Backend.Migrations
{
    public partial class docker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "ArticleId",
                keyValue: 1,
                column: "PublicationDate",
                value: "2024-02-27");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "ArticleId",
                keyValue: 1,
                column: "PublicationDate",
                value: "2024-02-16");
        }
    }
}
