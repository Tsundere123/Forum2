using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum2.Migrations.ForumDb
{
    /// <inheritdoc />
    public partial class ForumPostLastEditedByWhenNameFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ForumPostLastEdited",
                table: "ForumPost",
                newName: "ForumPostLastEditedTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ForumPostLastEditedTime",
                table: "ForumPost",
                newName: "ForumPostLastEdited");
        }
    }
}
