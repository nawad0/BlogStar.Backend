using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogStar.Backend.Migrations
{
    public partial class profile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "UserImage",
                table: "Users",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserImage",
                table: "Users");
        }
    }
}
