using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum2.Migrations.ForumDb
{
    /// <inheritdoc />
    public partial class ForumPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ForumThread_ForumCategoryId",
                table: "ForumThread");

            migrationBuilder.CreateTable(
                name: "ForumPost",
                columns: table => new
                {
                    ForumPostId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ForumThreadId = table.Column<string>(type: "TEXT", nullable: false),
                    ForumPostCreatorId = table.Column<string>(type: "TEXT", nullable: false),
                    ForumPostContent = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumPost", x => x.ForumPostId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForumThread_ForumCategoryId",
                table: "ForumThread",
                column: "ForumCategoryId",
                unique: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForumPost");

            migrationBuilder.DropIndex(
                name: "IX_ForumThread_ForumCategoryId",
                table: "ForumThread");

            migrationBuilder.CreateIndex(
                name: "IX_ForumThread_ForumCategoryId",
                table: "ForumThread",
                column: "ForumCategoryId");
        }
    }
}
