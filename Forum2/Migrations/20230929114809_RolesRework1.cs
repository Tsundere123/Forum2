using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum2.Migrations
{
    /// <inheritdoc />
    public partial class RolesRework1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountRoles_Accounts_AccountId",
                table: "AccountRoles");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "AccountRoles",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRoles_Accounts_AccountId",
                table: "AccountRoles",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountRoles_Accounts_AccountId",
                table: "AccountRoles");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "AccountRoles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

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
