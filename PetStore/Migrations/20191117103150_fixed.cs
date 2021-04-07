using Microsoft.EntityFrameworkCore.Migrations;

namespace BuenViaje.Migrations
{
    public partial class @fixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_ProductId",
                table: "Vouchers",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_TranspotId",
                table: "Vouchers",
                column: "TranspotId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_Products_ProductId",
                table: "Vouchers",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_Transports_TranspotId",
                table: "Vouchers",
                column: "TranspotId",
                principalTable: "Transports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transports_TransportOwners_TransportOwnerId",
                table: "Transports");

            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_Products_ProductId",
                table: "Vouchers");

            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_Transports_TranspotId",
                table: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_Vouchers_ProductId",
                table: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_Vouchers_TranspotId",
                table: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_Transports_TransportOwnerId",
                table: "Transports");
        }
    }
}
