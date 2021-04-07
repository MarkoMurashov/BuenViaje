using Microsoft.EntityFrameworkCore.Migrations;

namespace BuenViaje.Migrations
{
    public partial class fixFIO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Patronymic",
                table: "Orders",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "Orders",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Patronymic",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "Orders");
        }
    }
}
