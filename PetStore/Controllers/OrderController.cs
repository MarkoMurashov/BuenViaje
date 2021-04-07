using Microsoft.AspNetCore.Mvc;
using PetStore.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace PetStore.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        #region fields

        private IOrderRepository _repository;
        private IStockRepository _stockRepository;
        private Cart _cart;

        #endregion

        public OrderController(IOrderRepository repoService, IStockRepository stockRepository, Cart cartService)
        {
            _repository = repoService;
            _stockRepository = stockRepository;
            _cart = cartService;
        }

        [Authorize(Roles = "Admin, Manager")]
        public ViewResult List()
        {
            ViewBag.Current = "Orders";

            return View(_repository.Orders.Where(o => !o.Shipped));
        }

        [Authorize(Roles = "Admin, Manager")]
        public ViewResult ListShipped()
        {
            ViewBag.Current = "OrdersShipped";

            return View(_repository.Orders.Where(o => o.Shipped));
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult MarkShipped(int orderID)
        {
            Order order = _repository.Orders
                .FirstOrDefault(o => o.OrderID == orderID);

            if (order != null)
            {
                bool flag = true;

                foreach (var l in order.Lines)
                {
                    if (l.Quantity > _stockRepository.StockItems.FirstOrDefault(s => s.Product.ID == l.Product.ID).Quantity)
                    {
                        TempData["message"] = $"{l.Product.Name} недостаточно на складе";
                        flag = false;
                    }
                }
                if (flag)
                {
                    order.Shipped = true;
                    _repository.SaveOrder(order);

                    foreach (var l in order.Lines)
                    {
                        _stockRepository.ReduceQuantity(l.Product.ID, l.Quantity);
                    }
                }
            }

            return RedirectToAction(nameof(List));
        }

        public ViewResult Checkout()
        {
            var email = User.Identity.Name;

            var bonusCount = _repository.Orders.Where(o => o.Email == email).Count();

            if (bonusCount >= 5)
            {
                TempData["bonus"] = true;
            }

            return View(new Order());
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (_cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Ваша корзина пуста!");
            }

            if (ModelState.IsValid)
            {
                order.Lines = _cart.Lines.ToArray();
                order.Email = User.Identity.Name;
                _repository.SaveOrder(order);

                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }
        }

        public ViewResult Completed()
        {
            _cart.Clear();

            return View();
        }
    }
}