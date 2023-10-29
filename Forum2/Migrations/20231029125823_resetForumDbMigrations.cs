using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum2.Migrations
{
    /// <inheritdoc />
    public partial class resetForumDbMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ForumCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WallPost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AuthorId = table.Column<string>(type: "TEXT", nullable: false),
                    ProfileId = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WallPost", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ForumThread",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatorId = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EditedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EditedBy = table.Column<string>(type: "TEXT", nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPinned = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsLocked = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumThread", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumThread_ForumCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ForumCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WallPostReply",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AuthorId = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    WallPostId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WallPostReply", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WallPostReply_WallPost_WallPostId",
                        column: x => x.WallPostId,
                        principalTable: "WallPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForumPost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ThreadId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatorId = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EditedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    EditedBy = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumPost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumPost_ForumThread_ThreadId",
                        column: x => x.ThreadId,
                        principalTable: "ForumThread",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForumPost_ThreadId",
                table: "ForumPost",
                column: "ThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumThread_CategoryId",
                table: "ForumThread",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WallPostReply_WallPostId",
                table: "WallPostReply",
                column: "WallPostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForumPost");

            migrationBuilder.DropTable(
                name: "WallPostReply");

            migrationBuilder.DropTable(
                name: "ForumThread");

            migrationBuilder.DropTable(
                name: "WallPost");

            migrationBuilder.DropTable(
                name: "ForumCategory");
        }
    }
}
