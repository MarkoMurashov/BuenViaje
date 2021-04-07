using BuenViaje.Models;
using BuenViaje.Models.Interfaces;
using PetStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuenViaje.Models.Entity
{
    public class EFVoucher : IVoucher
    {
        #region Fields

        private ApplicationDbContext _context;

        #endregion

        #region Properties

        public IQueryable<Voucher> Entity => _context.Vouchers;

        public IQueryable<LocationVoucher> VoucherLocation => _context.LocationVoucher;

        #endregion

        public EFVoucher(ApplicationDbContext ctx)
        {
            _context = ctx;

        }

        public void SaveEntity(Voucher voucher)
        {
            if (voucher.ID == 0)
            {
                _context.Vouchers.Add(voucher);
                _context.SaveChanges();
            }
            else
            {
                var dbEntry = _context.Vouchers
                    .FirstOrDefault(p => p.ID == voucher.ID);
                if (dbEntry != null)
                {
                    dbEntry.ProductId = voucher.ProductId;
                    dbEntry.TranspotId = voucher.TranspotId;
                    dbEntry.DateEnd = voucher.DateEnd;
                    dbEntry.DateStart = voucher.DateStart;
                    dbEntry.Status = voucher.Status;
                    dbEntry.LocationFromId = voucher.LocationFromId;
                }
                _context.SaveChanges();
                voucher.ID = dbEntry.ID;
            }

        }

        public Voucher DeleteEntity(int id)
        {
            var dbEntry = _context.Vouchers
                .FirstOrDefault(p => p.ID == id);
            if (dbEntry != null)
            {
                _context.Vouchers.Remove(dbEntry);
                _context.SaveChanges();
            }
            return dbEntry;
        }

        public void AddVoucherLocation(string[] locID, int voucherID)
        {
            foreach(var loc in locID)
            {
                var dbEntry = new LocationVoucher
                {
                    LocationId = Convert.ToInt32(loc.Split(new char[] { ' ' })[0]),
                    VoucherId = voucherID
                };

                try
                {
                    _context.LocationVoucher.Add(dbEntry);
                    _context.SaveChanges();
                }
                catch { }
            }
        }

        public List<LocationVoucher> GetVoucherLocation(int voucherID)
        {
            var locations = new List<LocationVoucher>();

            var dbEntry = _context.LocationVoucher
                       .Where(p => p.VoucherId == voucherID).ToList();

            return dbEntry;
        }
    }
}
