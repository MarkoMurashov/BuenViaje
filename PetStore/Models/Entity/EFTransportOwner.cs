using BuenViaje.Models;
using BuenViaje.Models.Interfaces;
using PetStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuenViaje.Models.Entity
{
    public class EFTransportOwner: IEFContext<TransportOwner>
    {
        #region Fields

        private ApplicationDbContext _context;

        #endregion

        #region Properties

        public IQueryable<TransportOwner> Entity => _context.TransportOwners;

        #endregion

        public EFTransportOwner(ApplicationDbContext ctx)
        {
            _context = ctx;

        }

        public void SaveEntity(TransportOwner owner)
        {
            if (owner.Id == 0)
            {
                _context.TransportOwners.Add(owner);
            }
            else
            {
                var dbEntry = _context.TransportOwners
                    .FirstOrDefault(p => p.Id == owner.Id);
                if (dbEntry != null)
                {
                    dbEntry.Name = owner.Name;
                    dbEntry.Surname = owner.Surname;
                    dbEntry.Patronymic = owner.Patronymic;
                    dbEntry.Phone = owner.Phone;
                    dbEntry.ImageId = owner.ImageId;
                }
            }
            _context.SaveChanges();
        }

        public TransportOwner DeleteEntity(int ID)
        {
            var dbEntry = _context.TransportOwners
                .FirstOrDefault(p => p.Id == ID);
            if (dbEntry != null)
            {
                _context.TransportOwners.Remove(dbEntry);
                _context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
