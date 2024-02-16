using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogStar.Backend.Migrations
{
    public partial class likesU : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Articles_ArticleId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_ArticleId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "LikeId",
                table: "Articles");

            migrationBuilder.AddColumn<int>(
                name: "ArtilcleId",
                table: "Likes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Likes_ArtilcleId",
                table: "Likes",
                column: "ArtilcleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Articles_ArtilcleId",
                table: "Likes",
                column: "ArtilcleId",
                principalTable: "Articles",
                principalColumn: "ArticleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Articles_ArtilcleId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_ArtilcleId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "ArtilcleId",
                table: "Likes");

            migrationBuilder.AddColumn<int>(
                name: "ArticleId",
                table: "Likes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LikeId",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Likes_ArticleId",
                table: "Likes",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Articles_ArticleId",
                table: "Likes",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "ArticleId");
        }
    }
}
