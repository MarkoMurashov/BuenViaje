using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuenViaje.Models;
using BuenViaje.Models.Interfaces;
using BuenViaje.Models.ViewModels.Location;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetStore.Models.ViewModels;

namespace BuenViaje.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class LocationController : Controller
    {
        #region Fields

        private IEFContext<Location> _dbLocation { get; set; }

        private int _pageSize = 6;

        #endregion

        #region Ctors

        public LocationController(IEFContext<Location> location)
        {
            _dbLocation = location;
        }

        #endregion

        public IActionResult Show(string SearchName = "", int page = 1)
        {
            IEnumerable<Location> loc = _dbLocation.Entity;

            if (!String.IsNullOrEmpty(SearchName))
            {
                loc = loc.Where(o => o.Country.ToUpper().Contains(SearchName.ToUpper()) 
                || o.Departure.ToUpper().Contains(SearchName.ToUpper())
                || o.Sity.ToUpper().Contains(SearchName.ToUpper()));
            }

            return View(new LocationShowViewModel()
            {
                Locations = loc.Skip((page - 1) * _pageSize)
                    .Take(_pageSize),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = _pageSize,
                    TotalItems = loc.Count()
                }
            });
        }

        public ViewResult Create()
        {
            return View("Edit", new Location());
        }

        public async Task<IActionResult> Edit(int locationId)
        {
            var location = await _dbLocation.Entity.FirstOrDefaultAsync(p => p.Id == locationId);

            return View(location);
        }

        [HttpPost]
        public IActionResult Edit(Location model)
        {
            if (ModelState.IsValid)
            {
                _dbLocation.SaveEntity(model);
                TempData["message"] = $"Object #{model.Id} has been saved";

                return RedirectToAction("Show");
            }
            
            return View(model);            
        }


        [HttpPost]
        public IActionResult Delete(int locationId)
        {
            var deletedProduct = _dbLocation.DeleteEntity(locationId);
            if (deletedProduct != null)
            {
                TempData["message"] = $"Object #{deletedProduct.Id} was deleted";
            }
            return RedirectToAction("Show");
        }
    }
}