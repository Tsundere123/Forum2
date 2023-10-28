using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum2.Migrations.ForumDb
{
    /// <inheritdoc />
    public partial class ForumPostSoftDeleteTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ForumPostIsSoftDeleted",
                table: "ForumPost",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForumPostIsSoftDeleted",
                table: "ForumPost");
        }
    }
}
