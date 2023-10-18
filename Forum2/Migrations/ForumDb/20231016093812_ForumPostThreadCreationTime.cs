using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum2.Migrations.ForumDb
{
    /// <inheritdoc />
    public partial class ForumPostThreadCreationTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ForumCategory",
                columns: table => new
                {
                    ForumCategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ForumCategoryName = table.Column<string>(type: "TEXT", nullable: false),
                    ForumCategoryDescription = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumCategory", x => x.ForumCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "ForumThread",
                columns: table => new
                {
                    ForumThreadId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ForumThreadTitle = table.Column<string>(type: "TEXT", nullable: false),
                    ForumCategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    ForumThreadCreatorId = table.Column<string>(type: "TEXT", nullable: false),
                    ForumThreadCreationTimeUnix = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumThread", x => x.ForumThreadId);
                    table.ForeignKey(
                        name: "FK_ForumThread_ForumCategory_ForumCategoryId",
                        column: x => x.ForumCategoryId,
                        principalTable: "ForumCategory",
                        principalColumn: "ForumCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForumPost",
                columns: table => new
                {
                    ForumPostId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ForumThreadId = table.Column<int>(type: "INTEGER", nullable: false),
                    ForumPostCreatorId = table.Column<string>(type: "TEXT", nullable: false),
                    ForumPostContent = table.Column<string>(type: "TEXT", nullable: false),
                    ForumPostCreationTimeUnix = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumPost", x => x.ForumPostId);
                    table.ForeignKey(
                        name: "FK_ForumPost_ForumThread_ForumThreadId",
                        column: x => x.ForumThreadId,
                        principalTable: "ForumThread",
                        principalColumn: "ForumThreadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForumPost_ForumThreadId",
                table: "ForumPost",
                column: "ForumThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumThread_ForumCategoryId",
                table: "ForumThread",
                column: "ForumCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForumPost");

            migrationBuilder.DropTable(
                name: "ForumThread");

            migrationBuilder.DropTable(
                name: "ForumCategory");
        }
    }
}
