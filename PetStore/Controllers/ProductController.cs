using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuenViaje.Models;
using BuenViaje.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PetStore.Filters;
using PetStore.Filters.FilterParameters;
using PetStore.Models;
using PetStore.Models.MongoDb;
using PetStore.Models.ViewModels;

namespace PetStore.Controllers
{
    public class ProductController : Controller
    {
        #region fields

        private readonly ImagesDbContext _imagesDb;

        private IProductRepository _repository;

        private IStockRepository _stockRepository;

        private IFilterConditionsProducts _filterConditions;

        private IEFContext<Transport> _dbTransport { get; set; }

        private IVoucher _dbVoucher { get; set; }


        private IEFContext<Location> _dbLocation { get; set; }

        private IEFContext<Categories> _dbCategories { get; set; }

        public int PageSize = 4;

        #endregion

        #region ctor

        public ProductController(IProductRepository repository,
                                IStockRepository stockRepository,
                                ImagesDbContext context,
                                IFilterConditionsProducts filterConditions,
                                IEFContext<Transport> dbTransport,
                                IVoucher dbVoucher,
                                IEFContext<Location> dbLocation,
                                IEFContext<Categories> dbCategories)
        {
            _repository = repository;
            _stockRepository = stockRepository;
            _imagesDb = context;
            _filterConditions = filterConditions;
            _dbTransport = dbTransport;
            _dbVoucher = dbVoucher;
            _dbLocation = dbLocation;
            _dbCategories = dbCategories;
        }

        #endregion

        public  ViewResult List(FilterParametersProducts filter, int productPage = 1, bool IsHot = false)
        {
            var products = _repository.Products;

            foreach(var p in products)
            {
                p.Categories = _dbCategories.Entity.FirstOrDefault(o => o.Id == p.CategoriesID);
            }

            if (filter.Categories != null)
            {
                TempData["SelectedCategory"] = filter.Categories[0];
            }
            if (filter.Countries != null)
            {
                TempData["SelectedCountry"] = String.Join("", filter.Countries);
            }

            filter.IsHot = IsHot;

            TempData["MaxPrice"] = filter.MaxPrice;
            TempData["MinPrice"] = filter.MinPrice;
            TempData["MinDate"] = filter.MinDate;
            TempData["MaxDate"] = filter.MaxDate;
            TempData["Hot"] = filter.IsHot;

            var productsList = _filterConditions.GetProducts(products, filter);
          
            foreach (var p in productsList)
            {
                if (_stockRepository.StockItems.FirstOrDefault(pr => pr.Product == p && pr.Quantity > 0) != null)
                {
                    p.IsInStock = true;
                }
            }

            var paging = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = filter.Categories == null ?
                        productsList.Count() :
                        productsList.Where(e =>
                             filter.Categories.Contains(e.Categories.Name)).Count()
            };

            return View(new ProductsListViewModel
            {
                Products = productsList
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = paging,
                CurrentFilter = filter
            });
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
