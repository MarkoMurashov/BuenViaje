using BuenViaje.Models;
using BuenViaje.Models.Interfaces;
using PetStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuenViaje.Models.Entity
{
    public class EFTransport: IEFContext<Transport>
    {
        #region Fields

        private ApplicationDbContext _context;

        #endregion

        #region Properties

        public IQueryable<Transport> Entity => _context.Transports;

        #endregion

        public EFTransport(ApplicationDbContext ctx)
        {
            _context = ctx;

        }

        public void SaveEntity(Transport transport)
        {
            if (transport.Id == 0)
            {
                _context.Transports.Add(transport);
            }
            else
            {
                var dbEntry = _context.Transports
                    .FirstOrDefault(p => p.Id == transport.Id);
                if (dbEntry != null)
                {
                    dbEntry.Name = transport.Name;
                    dbEntry.TransportOwnerId = transport.TransportOwnerId;
                    dbEntry.ImageId = transport.ImageId;
                }
            }
            _context.SaveChanges();
        }

        public Transport DeleteEntity(int ID)
        {
            var dbEntry = _context.Transports
                .FirstOrDefault(p => p.Id == ID);
            if (dbEntry != null)
            {
                _context.Transports.Remove(dbEntry);
                _context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
