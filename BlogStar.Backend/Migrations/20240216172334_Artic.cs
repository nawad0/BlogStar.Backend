using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogStar.Backend.Migrations
{
    public partial class Artic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Articles_ArtilcleId",
                table: "Likes");

            migrationBuilder.RenameColumn(
                name: "ArtilcleId",
                table: "Likes",
                newName: "ArticleId");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_ArtilcleId",
                table: "Likes",
                newName: "IX_Likes_ArticleId");

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "ArticleId", "AuthorImagePath", "AuthorUserId", "AuthorUserName", "BlogId", "Content", "ContentHtml", "LikesJson", "PublicationDate", "Title" },
                values: new object[] { 1, "author.jpg", 1, "AuthorName", 1, "This is an example article content.", "<p>This is an example article content.</p>", null, "16.02.2024 20:23:34", "Example Article" });

            migrationBuilder.InsertData(
                table: "Likes",
                columns: new[] { "LikeId", "ArticleId", "UserId" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "Likes",
                columns: new[] { "LikeId", "ArticleId", "UserId" },
                values: new object[] { 2, 1, 2 });

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Articles_ArticleId",
                table: "Likes",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "ArticleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Articles_ArticleId",
                table: "Likes");

            migrationBuilder.DeleteData(
                table: "Likes",
                keyColumn: "LikeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Likes",
                keyColumn: "LikeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "ArticleId",
                keyValue: 1);

            migrationBuilder.RenameColumn(
                name: "ArticleId",
                table: "Likes",
                newName: "ArtilcleId");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_ArticleId",
                table: "Likes",
                newName: "IX_Likes_ArtilcleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Articles_ArtilcleId",
                table: "Likes",
                column: "ArtilcleId",
                principalTable: "Articles",
                principalColumn: "ArticleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
