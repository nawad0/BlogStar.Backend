using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogStar.Backend.Migrations
{
    public partial class image : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserImage",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "UserImagePath",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserImagePath",
                table: "Users");

            migrationBuilder.AddColumn<byte[]>(
                name: "UserImage",
                table: "Users",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
