using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum2.Migrations.ForumDb
{
    /// <inheritdoc />
    public partial class ForumPostLastEditedByWhen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ForumPostLastEdited",
                table: "ForumPost",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ForumPostLastEditedBy",
                table: "ForumPost",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForumPostLastEdited",
                table: "ForumPost");

            migrationBuilder.DropColumn(
                name: "ForumPostLastEditedBy",
                table: "ForumPost");
        }
    }
}
