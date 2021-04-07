using Microsoft.EntityFrameworkCore.Migrations;

namespace BuenViaje.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_Products_ProductID",
                table: "Vouchers");

            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_Transports_TranspotID",
                table: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_Vouchers_ProductID",
                table: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_Vouchers_TranspotID",
                table: "Vouchers");

            migrationBuilder.RenameColumn(
                name: "TranspotID",
                table: "Vouchers",
                newName: "TranspotId");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "Vouchers",
                newName: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TranspotId",
                table: "Vouchers",
                newName: "TranspotID");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Vouchers",
                newName: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_ProductID",
                table: "Vouchers",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_TranspotID",
                table: "Vouchers",
                column: "TranspotID");

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_Products_ProductID",
                table: "Vouchers",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_Transports_TranspotID",
                table: "Vouchers",
                column: "TranspotID",
                principalTable: "Transports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
