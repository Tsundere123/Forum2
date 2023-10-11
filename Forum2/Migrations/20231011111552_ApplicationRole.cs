using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum2.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "AspNetRoles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFixed",
                table: "AspNetRoles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "IsFixed",
                table: "AspNetRoles");
        }
    }
}
