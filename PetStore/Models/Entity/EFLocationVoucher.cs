using BuenViaje.Models;
using BuenViaje.Models.Interfaces;
using PetStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuenViaje.Models.Entity
{
    public class EFLocationVoucher : IEFContext<LocationVoucher>
    {
        #region Fields

        private ApplicationDbContext _context;

        #endregion

        #region Properties

        public IQueryable<LocationVoucher> Entity => _context.LocationVoucher;

        #endregion

        public EFLocationVoucher(ApplicationDbContext ctx)
        {
            _context = ctx;

        }


        public LocationVoucher DeleteEntity(int ID)
        {
            throw new NotImplementedException();
        }

        public void SaveEntity(LocationVoucher product)
        {
            throw new NotImplementedException();
        }
    }
}
