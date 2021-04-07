using BuenViaje.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace PetStore.Models
{
    public class ApplicationDbContext : DbContext
    {
        #region properties

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Stock> StockItems { get; set; }
        public DbSet<Transport> Transports { get; set; }
        public DbSet<TransportOwner> TransportOwners { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<LocationVoucher> LocationVoucher { get; set; }
        public DbSet<Categories> Categories { get; set; }

        #endregion

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            //many to many
            modelBuilder.Entity<LocationVoucher>()
          .HasKey(bc => new { bc.LocationId, bc.VoucherId });

            modelBuilder.Entity<LocationVoucher>()
                .HasOne(bc => bc.Location)
                .WithMany(b => b.LocationVouchers)
                .HasForeignKey(bc => bc.LocationId);

            modelBuilder.Entity<LocationVoucher>()
                .HasOne(bc => bc.Voucher)
                .WithMany(c => c.LocationVouchers)
                .HasForeignKey(bc => bc.VoucherId);

            
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            }
        }


    }
}