using BuenViaje.Models;
using BuenViaje.Models.Interfaces;
using PetStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuenViaje.Models.Entity
{
    public class EFCategories : IEFContext<Categories>
    {
        #region Fields

        private ApplicationDbContext _context;

        #endregion

        #region Properties

        public IQueryable<Categories> Entity => _context.Categories;

        #endregion

        public EFCategories(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        public void SaveEntity(Categories categories)
        {
            if (categories.Id == 0)
            {
                _context.Categories.Add(categories);
            }
            else
            {
                var dbEntry = _context.Categories
                    .FirstOrDefault(p => p.Id == categories.Id);
                if (dbEntry != null)
                {
                    dbEntry.Name = categories.Name;
                    dbEntry.Description = categories.Description;
                }
            }
            _context.SaveChanges();
        }

        public Categories DeleteEntity(int ID)
        {
            var dbEntry = _context.Categories
                .FirstOrDefault(p => p.Id == ID);
            if (dbEntry != null)
            {
                _context.Categories.Remove(dbEntry);
                _context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
