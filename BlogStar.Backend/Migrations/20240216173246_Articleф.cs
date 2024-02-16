using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogStar.Backend.Migrations
{
    public partial class Articleф : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "ArticleId",
                keyValue: 1,
                columns: new[] { "AuthorImagePath", "AuthorUserId", "AuthorUserName", "BlogId", "Content", "ContentHtml", "PublicationDate", "Title" },
                values: new object[] { null, null, null, null, "Содержание статьи...", null, "2024-02-16", "Пример статьи" });

            migrationBuilder.InsertData(
                table: "Likes",
                columns: new[] { "LikeId", "ArticleId", "UserId" },
                values: new object[] { 3, 1, 3 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Likes",
                keyColumn: "LikeId",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "ArticleId",
                keyValue: 1,
                columns: new[] { "AuthorImagePath", "AuthorUserId", "AuthorUserName", "BlogId", "Content", "ContentHtml", "PublicationDate", "Title" },
                values: new object[] { "author.jpg", 1, "AuthorName", 1, "This is an example article content.", "<p>This is an example article content.</p>", "16.02.2024 20:26:55", "Example Article" });
        }
    }
}
