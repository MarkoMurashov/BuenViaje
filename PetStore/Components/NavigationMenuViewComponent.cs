using Microsoft.AspNetCore.Mvc;
using System.Linq;
using PetStore.Models;
using BuenViaje.Models.Interfaces;
using BuenViaje.Models;
using PetStore.Filters.FilterParameters;

namespace PetStore.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        #region fields

        private IEFContext<Categories> _dbCategories;

        private IEFContext<Location> _dbLocation;

        #endregion

        public NavigationMenuViewComponent(IEFContext<Categories> dbCategories,
            IEFContext<Location> dbLocation)
        {
            _dbCategories = dbCategories;
            _dbLocation = dbLocation;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            ViewBag.SelectedCountry = RouteData?.Values["country"];


            var filter = new FilterParametersProducts
            {
                Categories = _dbCategories.Entity.Select(o => o.Name)
                .OrderBy(x => x).ToList(),
                Countries = _dbLocation.Entity.Select(o => o.Country).Distinct()
                .OrderBy(x => x).ToList()

            };

            return View(filter);
        }
    }
}
