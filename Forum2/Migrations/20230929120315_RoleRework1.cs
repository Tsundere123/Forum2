using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum2.Migrations
{
    /// <inheritdoc />
    public partial class RoleRework1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountRoles_Accounts_AccountId",
                table: "AccountRoles");

            migrationBuilder.DropIndex(
                name: "IX_AccountRoles_AccountId",
                table: "AccountRoles");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "AccountRoles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "AccountRoles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AccountRoles_AccountId",
                table: "AccountRoles",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRoles_Accounts_AccountId",
                table: "AccountRoles",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
