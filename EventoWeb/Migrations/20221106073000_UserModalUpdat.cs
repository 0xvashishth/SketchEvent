using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventoWeb.Migrations
{
    public partial class UserModalUpdat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_CreatedById",
                table: "Events");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Events",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_CreatedById",
                table: "Events",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_CreatedById",
                table: "Events");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_CreatedById",
                table: "Events",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
