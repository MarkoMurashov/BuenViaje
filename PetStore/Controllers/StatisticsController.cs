using Microsoft.AspNetCore.Mvc;
using PetStore.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using PetStore.Models.MongoDb;
using Microsoft.AspNetCore.Http;
using System;
using PetStore.Models.ViewModels;
using System.Collections.Generic;

namespace PetStore.Controllers
{
    public class StatisticsController : Controller
    {
        private IStockRepository _stockRepository;
        private IProductRepository _productRepository;
        private IOrderRepository _orderRepository;

        public StatisticsController(IProductRepository productRepository, 
                                    IStockRepository stockRepository,
                                    IOrderRepository orderRepository)
        {
            _productRepository = productRepository;
            _stockRepository = stockRepository;
            _orderRepository = orderRepository;
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Index(DateTime dateFrom, DateTime dateTo)
        {
            ViewBag.Current = "Statistics";

            TempData["DateFrom"] = dateFrom;
            TempData["DateTo"] = dateTo;

            var listModel = new List<CategoriesChartViewModel>();
            var products = _productRepository.Products;
            var orders = _orderRepository.Orders;
            var categroies = products.Select(p => p.Categories.Name).Distinct();

            foreach (var c in categroies)
            {
                var model = new CategoriesChartViewModel();

                float profit = 0;

                var categoryItems = products.Where(p => p.Categories.Name == c);
                foreach (var p in categoryItems)
                {
                    int quantity = 0;

                    IQueryable<float> sum = null;

                    if(dateFrom > DateTime.MinValue && dateTo > DateTime.MinValue)
                    {
                        quantity = orders
                            .Where(o => o.Lines
                                .FirstOrDefault(i => i.Product == p) != null && o.Date >= dateFrom && o.Date <= dateTo).Count();

                        sum = orders.Where(o => o.Lines.FirstOrDefault(i => i.Product == p) != null
                                     && (o.Date >= dateFrom && o.Date <= dateTo))
                                         .Select(pr => pr.Lines.Sum(o => o.Product.PriceSell - o.Product.PriceBuy));
                    }
                    else
                    {
                        quantity = orders
                            .Where(o => o.Lines
                                .FirstOrDefault(i => i.Product == p) != null).Count();

                        sum = orders.Where(o => o.Lines.FirstOrDefault(i => i.Product == p) != null)
                                         .Select(pr => pr.Lines.Sum(o => o.Product.PriceSell - o.Product.PriceBuy));
                    }
                  
                    model.Charts.Add(new SimpleChartViewModel
                    {
                        DimensionOne = p.Name,
                        Quantity = quantity
                    });
                    
                    foreach(var s in sum)
                    {
                        profit += s;
                    }
                }

                model.Category = c;
                model.Profit = profit;

                listModel.Add(model);
            }

            return View(listModel.OrderBy(i => i.Category).ToList());
        }
    }
}