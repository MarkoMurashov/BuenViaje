using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BonVoyage.Models;
using BuenViaje.Models;
using BuenViaje.Models.Interfaces;
using BuenViaje.Models.ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetStore.Models.ViewModels;

namespace BuenViaje.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class CategoriesController : Controller
    {
        #region Fields

        private IEFContext<Categories> _dbCategories { get; set; }

        private int _pageSize = 3;

        #endregion

        #region Ctors

        public CategoriesController(IEFContext<Categories> categories)
        {
            _dbCategories = categories;
        }

        #endregion

        public IActionResult Show(string SearchName="",int page = 1)
        {

            IEnumerable<Categories> categories = _dbCategories.Entity;

            if (!String.IsNullOrEmpty(SearchName))
            {
                categories = categories.Where(o => o.Name.ToUpper().Contains(SearchName.ToUpper()));
            }

            return View(new CategoriesShowViewModel()
            {
                Categories = categories.Skip((page - 1) * _pageSize)
                    .Take(_pageSize),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = _pageSize,
                    TotalItems = categories.Count()
                }
            });
        }

        public ViewResult Create()
        {
            return View("Edit", new Categories());
        }

        public async Task<IActionResult> Edit(int categoriesId)
        {
            var sale = await _dbCategories.Entity.FirstOrDefaultAsync(p => p.Id == categoriesId);

            return View(sale);
        }

        [HttpPost]
        public IActionResult Edit(Categories model)
        {
            if (ModelState.IsValid)
            {
                _dbCategories.SaveEntity(model);
                TempData["message"] = $"Object #{model.Id} has been saved";

                return RedirectToAction("Show");
            }

            return View(model);
        }


        [HttpPost]
        public IActionResult Delete(int categoriesId)
        {
            var deletedProduct = _dbCategories.DeleteEntity(categoriesId);
            if (deletedProduct != null)
            {
                TempData["message"] = $"Object #{deletedProduct.Id} was deleted";
            }
            return RedirectToAction("Show");
        }
    }
}