using BuenViaje.Models;
using BuenViaje.Models.Interfaces;
using PetStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BonVoyage.Models.Entity
{
    public class EFLocation : IEFContext<Location>
    {
        #region Fields

        private ApplicationDbContext _context;

        #endregion

        #region Properties

        public IQueryable<Location> Entity => _context.Location;

        #endregion

        public EFLocation(ApplicationDbContext ctx)
        {
            _context = ctx;

        }

        public void SaveEntity(Location location)
        {
            if (location.Id == 0)
            {
                _context.Location.Add(location);
            }
            else
            {
                var dbEntry = _context.Location
                    .FirstOrDefault(p => p.Id == location.Id);
                if (dbEntry != null)
                {
                    dbEntry.Country = location.Country;
                    dbEntry.Sity = location.Sity;
                    dbEntry.Departure = location.Departure;
                    // dbEntry.VoucherId = location.VoucherId;
                }
            }
            _context.SaveChanges();
        }

        public Location DeleteEntity(int ID)
        {
            var dbEntry = _context.Location
                .FirstOrDefault(p => p.Id == ID);
            if (dbEntry != null)
            {
                _context.Location.Remove(dbEntry);
                _context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
