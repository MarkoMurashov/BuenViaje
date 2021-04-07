using Microsoft.AspNetCore.Mvc;
using PetStore.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using PetStore.Models.MongoDb;
using Microsoft.AspNetCore.Http;
using System;
using BuenViaje.Models.Interfaces;
using BuenViaje.Models;
using BuenViaje.Models.ViewModels.Voucher;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using BuenViaje.Models.ViewModels;
using PetStore.Models.ViewModels;

namespace PetStore.Controllers
{
    public class AdminController : Controller
    {
        #region fields

        private readonly ImagesDbContext _imagesDb;
        private IStockRepository _stockRepository;
        private IProductRepository _repository;
        private IEFContext<Transport> _dbTransport { get; set; }
        private IVoucher _dbVoucher { get; set; }
        private IEFContext<Location> _dbLocation { get; set; }
        private IEFContext<Categories> _dbCategories { get; set; }

        private int _pageSize = 4;

        #endregion

        #region ctor

        public AdminController(IProductRepository repo, IStockRepository stockRepo, ImagesDbContext context,
            IEFContext<Transport> dbTransport,
                                IVoucher dbVoucher,
                                IEFContext<Location> dbLocation,
                                IEFContext<Categories> dbCategories)
        {
            _repository = repo;
            _stockRepository = stockRepo;
            _imagesDb = context;
            _dbTransport = dbTransport;
            _dbVoucher = dbVoucher;
            _dbLocation = dbLocation;
            _dbCategories = dbCategories;
        }

        #endregion

        [Authorize(Roles = "Admin, Manager")]
        public ViewResult Index(string SearchName = "", int page = 1)
        {
            ViewBag.Current = "Products";
            TempData["SearchName"] = SearchName;

            IEnumerable<Stock> product = _stockRepository.StockItems;

            if (!String.IsNullOrEmpty(SearchName))
            {
                product = product.Where(o => o.Product.Name.ToUpper().Contains(SearchName.ToUpper()));
            }

            var viewModel = new ShowViewModel<Stock>
            {
                Items = product.Skip((page - 1) * _pageSize)
                    .Take(_pageSize),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = _pageSize,
                    TotalItems = product.Count()
                }
            };

            return View(viewModel);
        }

        public async Task<ViewResult> Detail(int id)
        {
            var voucher = await _dbVoucher.Entity.FirstOrDefaultAsync(p => p.ProductId == id);

            var transport = await _dbTransport.Entity.FirstOrDefaultAsync(p => p.Id == voucher.TranspotId);

            voucher.Product = await _repository.Products.FirstOrDefaultAsync(p => p.ID == voucher.ProductId);

            var categories = await _dbCategories.Entity.FirstOrDefaultAsync(p => p.Id == voucher.Product.CategoriesID);

            var locationFrom = await _dbLocation.Entity.FirstOrDefaultAsync(p => p.Id == voucher.LocationFromId);

            voucher.Transport = transport;
            voucher.LocationFrom = locationFrom;

            voucher.LocationVouchers = _dbVoucher.GetVoucherLocation(voucher.ID);
            foreach (var loc in voucher.LocationVouchers)
            {
                var locationTo = await _dbLocation.Entity.FirstOrDefaultAsync(p => p.Id == loc.LocationId);
                loc.Location = locationTo;
            }

            return View(voucher);
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<ViewResult> Edit(int productId)
        {

            var voucher = await _dbVoucher.Entity.FirstOrDefaultAsync(p => p.ProductId == productId);

            var model = new VoucherEditViewModel()
            {
                Voucher = voucher,
                Transports = _dbTransport.Entity,
                Product = _repository.Products.FirstOrDefault(p => p.ID == productId),
                Categories = _dbCategories.Entity,
                LocationsTO = _dbLocation.Entity,
                LocationsFROM = _dbLocation.Entity
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Edit(VoucherEditViewModel model)
        {
            if (!String.IsNullOrEmpty(model.Product.Name))
            {
                model.Product.CategoriesID = Convert.ToInt32(model.CategoriesInfo
                  .Split(new char[] { ' ' })[0]);

                if (model.Photo != null)
                {
                    var imageName = DateTime.Now.ToString() + model.Product.Name;
                    var image = await _imagesDb.StoreImage(model.Photo.OpenReadStream(),
                                                            imageName);

                    model.Product.ImageId = image;
                }

                _repository.SaveProduct(model.Product);

                model.Voucher.TranspotId = Convert.ToInt32(model.TransportInfo
                        .Split(new char[] { ' ' })[0]);

                model.Voucher.Transport = _dbTransport.Entity.FirstOrDefault(tr => tr.Id == model.Voucher.TranspotId);

                model.Voucher.LocationFromId = Convert.ToInt32(model.LocationFROMInfo
                   .Split(new char[] { ' ' })[0]);

                model.Voucher.LocationFrom = _dbLocation.Entity.FirstOrDefault(tr => tr.Id == model.Voucher.LocationFromId);

                model.Voucher.ProductId = model.Product.ID;

                model.Voucher.Product = _repository.Products.FirstOrDefault(tr => tr.ID == model.Voucher.ProductId);

                _dbVoucher.SaveEntity(model.Voucher);

                _dbVoucher.AddVoucherLocation(model.LocationTOInfo, model.Voucher.ID);


                TempData["message"] = $"{model.Product.Name} сохранен";




                if (_stockRepository.StockItems.FirstOrDefault(s => s.Product.ID == model.Product.ID) == null)
                {
                    _stockRepository.SaveStockItem(new Stock { Product = model.Product, Quantity = 0 });
                }

                return RedirectToAction("Index");
            }

                // there is something wrong with the data values
                model.Transports = _dbTransport.Entity;
                model.Categories = _dbCategories.Entity;
                model.LocationsTO = _dbLocation.Entity;
                model.LocationsFROM = _dbLocation.Entity;

                return View(model);
        }

        [Authorize(Roles = "Admin, Manager")]
        public ViewResult Create() => View("Edit", new VoucherEditViewModel()
        {
            Voucher = new Voucher(),
            Transports = _dbTransport.Entity,
            Product = new Product(),
            Categories = _dbCategories.Entity,
            LocationsTO = _dbLocation.Entity,
            LocationsFROM = _dbLocation.Entity
        });

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int productId)
        {
            Stock deletedStock = _stockRepository.DeleteStockItem(productId);
            Product deletedProduct = _repository.DeleteProduct(productId);

            if (deletedProduct != null && deletedStock != null)
            {
                TempData["message"] = $"{deletedProduct.Name} was deleted";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult AddToStock(int stockId, int quantity)
        {
            var stock = _stockRepository.StockItems.FirstOrDefault(s => s.ID == stockId);

            stock.Quantity += quantity;
            _stockRepository.SaveStockItem(stock);

            return RedirectToAction("Index");
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

        [HttpPost]
        public async Task<ActionResult> AttachImage(int id, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                var image = await _imagesDb.StoreImage(uploadedFile.OpenReadStream(), uploadedFile.FileName);

                Product product = _repository.Products.FirstOrDefault(p => p.ID == id);
                product.ImageId = image;

                _repository.SaveProduct(product);
                TempData["message"] = $"{product.Name} был сохранен";
            }

            return RedirectToAction("Index");
        }
    }
}