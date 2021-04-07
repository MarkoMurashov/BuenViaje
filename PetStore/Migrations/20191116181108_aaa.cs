using Microsoft.EntityFrameworkCore.Migrations;

namespace BuenViaje.Migrations
{
    public partial class aaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transports_TransportOwners_TransportOwnerId",
                table: "Transports");

            migrationBuilder.DropIndex(
                name: "IX_Transports_TransportOwnerId",
                table: "Transports");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transports_TransportOwnerId",
                table: "Transports",
                column: "TransportOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transports_TransportOwners_TransportOwnerId",
                table: "Transports",
                column: "TransportOwnerId",
                principalTable: "TransportOwners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
