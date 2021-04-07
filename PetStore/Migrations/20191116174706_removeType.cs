using Microsoft.EntityFrameworkCore.Migrations;

namespace BuenViaje.Migrations
{
    public partial class removeType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInStock",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Transports");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInStock",
                table: "Vouchers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Transports",
                nullable: false,
                defaultValue: "");
        }
    }
}
