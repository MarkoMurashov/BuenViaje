﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PetStore.Models;

namespace BuenViaje.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20191117101041_fix")]
    partial class fix
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BuenViaje.Models.Categories", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("BuenViaje.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Country")
                        .IsRequired();

                    b.Property<string>("Departure")
                        .IsRequired();

                    b.Property<string>("Sity")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("BuenViaje.Models.LocationVoucher", b =>
                {
                    b.Property<int>("LocationId");

                    b.Property<int>("VoucherId");

                    b.Property<int>("ID");

                    b.HasKey("LocationId", "VoucherId");

                    b.HasIndex("VoucherId");

                    b.ToTable("LocationVoucher");
                });

            modelBuilder.Entity("BuenViaje.Models.Transport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ImageId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("TransportOwnerId");

                    b.HasKey("Id");

                    b.ToTable("Transports");
                });

            modelBuilder.Entity("BuenViaje.Models.TransportOwner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ImageId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Patronymic");

                    b.Property<string>("Phone")
                        .IsRequired();

                    b.Property<string>("Surname")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("TransportOwners");
                });

            modelBuilder.Entity("BuenViaje.Models.Voucher", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateEnd");

                    b.Property<DateTime>("DateStart");

                    b.Property<int>("LocationFromId");

                    b.Property<int>("ProductId");

                    b.Property<int>("Status");

                    b.Property<int>("TranspotId");

                    b.HasKey("ID");

                    b.HasIndex("LocationFromId");

                    b.ToTable("Vouchers");
                });

            modelBuilder.Entity("PetStore.Models.CartLine", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("OrderID");

                    b.Property<int?>("ProductID");

                    b.Property<int>("Quantity");

                    b.HasKey("ID");

                    b.HasIndex("OrderID");

                    b.HasIndex("ProductID");

                    b.ToTable("CartLine");
                });

            modelBuilder.Entity("PetStore.Models.Order", b =>
                {
                    b.Property<int>("OrderID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code");

                    b.Property<DateTime>("Date");

                    b.Property<bool>("HavePassport");

                    b.Property<string>("IssuingAuthority");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<bool>("Shipped");

                    b.HasKey("OrderID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("PetStore.Models.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CategoriesID");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("ImageId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<float>("PriceBuy");

                    b.Property<float>("PriceSell");

                    b.HasKey("ID");

                    b.HasIndex("CategoriesID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("PetStore.Models.Stock", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ProductID");

                    b.Property<int>("Quantity");

                    b.HasKey("ID");

                    b.HasIndex("ProductID");

                    b.ToTable("StockItems");
                });

            modelBuilder.Entity("BuenViaje.Models.LocationVoucher", b =>
                {
                    b.HasOne("BuenViaje.Models.Location", "Location")
                        .WithMany("LocationVouchers")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BuenViaje.Models.Voucher", "Voucher")
                        .WithMany("LocationVouchers")
                        .HasForeignKey("VoucherId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BuenViaje.Models.Voucher", b =>
                {
                    b.HasOne("BuenViaje.Models.Location", "LocationFrom")
                        .WithMany()
                        .HasForeignKey("LocationFromId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("PetStore.Models.CartLine", b =>
                {
                    b.HasOne("PetStore.Models.Order")
                        .WithMany("Lines")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("PetStore.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("PetStore.Models.Product", b =>
                {
                    b.HasOne("BuenViaje.Models.Categories", "Categories")
                        .WithMany()
                        .HasForeignKey("CategoriesID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("PetStore.Models.Stock", b =>
                {
                    b.HasOne("PetStore.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
