using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuenViaje.Models;
using BuenViaje.Models.Interfaces;
using BuenViaje.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetStore.Models.MongoDb;
using PetStore.Models.ViewModels;

namespace BuenViaje.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class TransportController : Controller
    {
        #region Fields

        private IEFContext<Transport> _dbTransport { get; set; }

        private IEFContext<TransportOwner> _dbTransportOwners { get; set; }

        private readonly ImagesDbContext _imagesDb;

        private int _pageSize = 6;

        #endregion

        #region Ctors

        public TransportController(IEFContext<Transport> transport, IEFContext<TransportOwner> owners,
            ImagesDbContext imagesDb)
        {
            _dbTransport = transport;
            _dbTransportOwners = owners;
            _imagesDb = imagesDb;

        }

        #endregion

        public async Task<IActionResult> Show(string SearchName = "", int page = 1)
        {
            IEnumerable<Transport> transport = _dbTransport.Entity;

            if (!String.IsNullOrEmpty(SearchName))
            {
                transport = transport.Where(o => o.Name.ToUpper().Contains(SearchName.ToUpper()));
            }
            
            foreach(var tr in transport.Skip((page - 1) * _pageSize)
                    .Take(_pageSize))
            {
                tr.TransportOwner = await GetOwner(tr.TransportOwnerId);
            }

            return View(new TransportShowViewModel()
            {
                Transports = transport.Skip((page - 1) * _pageSize)
                    .Take(_pageSize),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = _pageSize,
                    TotalItems = transport.Count()
                }
            });
        }

        public async Task<IActionResult> Detail(int id)
        {
            var transport = await _dbTransport.Entity.FirstOrDefaultAsync(p => p.Id == id);

            transport.TransportOwner = await GetOwner(transport.TransportOwnerId);

            return View(transport);
        }

        public ViewResult Create()
        {
            return View("Edit", new TransportEditViewModel()
            {
                Transports = new Transport(), 
                TransportOwners = _dbTransportOwners.Entity
            });
        }

        public async Task<IActionResult> Edit(int transportId)
        {
            var transport = await _dbTransport.Entity.FirstOrDefaultAsync(p => p.Id == transportId);

            var model = new TransportEditViewModel()
            {
                Transports = transport,
                TransportOwners = _dbTransportOwners.Entity
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TransportEditViewModel transport)
        {
            if(!String.IsNullOrEmpty(transport.FIOOwner)|| !String.IsNullOrEmpty(transport.Transports.Name))
            {
                transport.Transports.TransportOwnerId = Convert.ToInt32(transport.FIOOwner
                    .Split(new char[] { ' ' })[0]);

                if (transport.Photo != null)
                {
                    var imageName = DateTime.Now.ToString() + transport.Transports.Name;
                    var image = await _imagesDb.StoreImage(transport.Photo.OpenReadStream(),
                                                            imageName);

                    transport.Transports.ImageId = image;
                }

                transport.Transports.TransportOwner = _dbTransportOwners.Entity.FirstOrDefault(a => a.Id == transport.Transports.TransportOwnerId);


                _dbTransport.SaveEntity(transport.Transports);
                TempData["message"] = $"{transport.Transports.Name} сохраненно";

                return RedirectToAction("Show");
            }
            else
            {
                // there is something wrong with the data values
                transport.TransportOwners = _dbTransportOwners.Entity;

                return View(transport );
            }
        }


        [HttpPost]
        public IActionResult Delete(int transportId)
        {
            var deletedProduct = _dbTransport.DeleteEntity(transportId);
            if (deletedProduct != null)
            {
                TempData["message"] = $"{deletedProduct.Name} удаленно";
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

        private async Task<TransportOwner> GetOwner(int id)
        {
           var owner = await _dbTransportOwners.Entity.FirstOrDefaultAsync(p => p.Id == id);

            return owner;
        }
    }
}