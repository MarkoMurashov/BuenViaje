using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Net.Http;
using BuenViaje.Models.Interfaces;
using BuenViaje.Models;
using PetStore.Models.MongoDb;
using BuenViaje.Models.ViewModels;
using PetStore.Models.ViewModels;
using BuenViaje.Models.ViewModels.TransportOwner;

namespace BuenViaje.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class TransportOwnerController : Controller
    {
        #region Fields

        private IEFContext<TransportOwner> _dbOwners { get; set; }
        private readonly ImagesDbContext _imagesDb;

        private int _pageSize = 6;

        #endregion

        #region Ctors

        public TransportOwnerController(IEFContext<TransportOwner> own,
            ImagesDbContext imagesDb)
        {
            _dbOwners = own;
            _imagesDb = imagesDb;
        }

        #endregion

        public IActionResult Show(string SearchName = "",int page = 1)
        {
            IEnumerable<TransportOwner> owners = _dbOwners.Entity;

            if (!String.IsNullOrEmpty(SearchName))
            {
                owners = owners.Where(o => o.Name.ToUpper().Contains(SearchName.ToUpper()) 
                || o.Surname.ToUpper().Contains(SearchName.ToUpper()) 
                || o.Patronymic.ToUpper().Contains(SearchName.ToUpper()));
            }

            return View(new TransportOwnerShowViewModel()
            {
                Owners = owners.Skip((page - 1) * _pageSize)
                    .Take(_pageSize),
                PagingInfo  = new PagingInfo()
            {
                CurrentPage = page,
                ItemsPerPage = _pageSize,
                TotalItems = owners.Count()
            }
            });
        }

        public async Task<IActionResult> Detail(int id)
        {
            var owner = await _dbOwners.Entity.FirstOrDefaultAsync(p => p.Id == id);

            return View(owner);
        }

        public ViewResult Create() => View("Edit", new TransportOwnerEditViewModel()
        {
            Owner = new TransportOwner()
        } );

        public async Task<IActionResult> Edit(int transportOwnerId)
        {
            var owner = await _dbOwners.Entity.FirstOrDefaultAsync(p => p.Id == transportOwnerId);

            var viewModel = new TransportOwnerEditViewModel()
            {
                Owner = owner
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TransportOwnerEditViewModel model)
        {
            //TODO: valid phone
            if (!ModelState.IsValid)
            {
                return View(model.Owner);
            }

            if (model.Photo != null)
            {
                var imageName = DateTime.Now.ToString() + model.Owner.Name + model.Owner.Surname;
                var image = await _imagesDb.StoreImage(model.Photo.OpenReadStream(),
                                                        imageName);

                model.Owner.ImageId = image;
            }
          
            _dbOwners.SaveEntity(model.Owner);
            TempData["message"] = $"{model.Owner.Name}" +
                $" {model.Owner.Surname} сохраненно";

            return RedirectToAction("Show");

        }


        [HttpPost]
        public IActionResult Delete(int transportOwnerId)
        {
            var deletedProduct = _dbOwners.DeleteEntity(transportOwnerId);
            if (deletedProduct != null)
            {
                TempData["message"] = $"{deletedProduct.Name} удалено";
            }
            return RedirectToAction("Show");
        }

        public async Task<ActionResult> GetImage(string id)
        {
            var image = await _imagesDb.GetImage(id);

            if (image == null)
            {
                return NotFound();
            }

            return File(image, "image/png");
        }
    }
}