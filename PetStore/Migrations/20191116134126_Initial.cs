using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BuenViaje.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Country = table.Column<string>(nullable: false),
                    Sity = table.Column<string>(nullable: false),
                    Departure = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Shipped = table.Column<bool>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    IssuingAuthority = table.Column<string>(nullable: true),
                    HavePassport = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                });

            migrationBuilder.CreateTable(
                name: "TransportOwners",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Surname = table.Column<string>(nullable: false),
                    Patronymic = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: false),
                    ImageId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportOwners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    PriceBuy = table.Column<float>(nullable: false),
                    PriceSell = table.Column<float>(nullable: false),
                    CategoriesID = table.Column<int>(nullable: true),
                    ImageId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoriesID",
                        column: x => x.CategoriesID,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    TransportOwnerId = table.Column<int>(nullable: false),
                    ImageId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transports_TransportOwners_TransportOwnerId",
                        column: x => x.TransportOwnerId,
                        principalTable: "TransportOwners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CartLine",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductID = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    OrderID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartLine", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CartLine_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CartLine_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockItems",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductID = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_StockItems_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TranspotID = table.Column<int>(nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    DateStart = table.Column<DateTime>(nullable: false),
                    DateEnd = table.Column<DateTime>(nullable: false),
                    LocationFromId = table.Column<int>(nullable: false),
                    IsInStock = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Vouchers_Location_LocationFromId",
                        column: x => x.LocationFromId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vouchers_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vouchers_Transports_TranspotID",
                        column: x => x.TranspotID,
                        principalTable: "Transports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LocationVoucher",
                columns: table => new
                {
                    LocationId = table.Column<int>(nullable: false),
                    VoucherId = table.Column<int>(nullable: false),
                    ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationVoucher", x => new { x.LocationId, x.VoucherId });
                    table.ForeignKey(
                        name: "FK_LocationVoucher_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LocationVoucher_Vouchers_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Vouchers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartLine_OrderID",
                table: "CartLine",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_CartLine_ProductID",
                table: "CartLine",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationVoucher_VoucherId",
                table: "LocationVoucher",
                column: "VoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoriesID",
                table: "Products",
                column: "CategoriesID");

            migrationBuilder.CreateIndex(
                name: "IX_StockItems_ProductID",
                table: "StockItems",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Transports_TransportOwnerId",
                table: "Transports",
                column: "TransportOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_LocationFromId",
                table: "Vouchers",
                column: "LocationFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_ProductID",
                table: "Vouchers",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_TranspotID",
                table: "Vouchers",
                column: "TranspotID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartLine");

            migrationBuilder.DropTable(
                name: "LocationVoucher");

            migrationBuilder.DropTable(
                name: "StockItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Transports");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "TransportOwners");
        }
    }
}
