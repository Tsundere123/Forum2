using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum2.Migrations.ForumDb
{
    /// <inheritdoc />
    public partial class test1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ForumCategoryId",
                table: "ForumThread",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ForumThread_ForumCategoryId",
                table: "ForumThread",
                column: "ForumCategoryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumThread_ForumCategory_ForumCategoryId",
                table: "ForumThread",
                column: "ForumCategoryId",
                principalTable: "ForumCategory",
                principalColumn: "ForumCategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumThread_ForumCategory_ForumCategoryId",
                table: "ForumThread");

            migrationBuilder.DropIndex(
                name: "IX_ForumThread_ForumCategoryId",
                table: "ForumThread");

            migrationBuilder.DropColumn(
                name: "ForumCategoryId",
                table: "ForumThread");
        }
    }
}
