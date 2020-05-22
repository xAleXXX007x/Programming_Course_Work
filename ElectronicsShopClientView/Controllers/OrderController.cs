using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicsShopBusinessLogic.BindingModels;
using ElectronicsShopBusinessLogic.Enums;
using ElectronicsShopBusinessLogic.Interfaces;
using ElectronicsShopClientView.Models;
using ElectronicsShopDatabase.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsShopClientView.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderLogic _orderLogic;
        private readonly IProductLogic _productLogic;

        public OrderController(IOrderLogic orderLogic, IProductLogic productLogic)
        {
            _orderLogic = orderLogic;
            _productLogic = productLogic;
        }

        public IActionResult Index()
        {
            var orders = _orderLogic.Read(new OrderBindingModel
            {
                ClientId = Program.Client.Id
            });

            var orderModels = new List<OrderModel>();

            foreach (var order in orders)
            {
                var products = new List<OrderProductModel>();

                foreach (var product in order.Products)
                {
                    var productData = _productLogic.Read(new ProductBindingModel
                    {
                        Id = product.ProductId
                    }).FirstOrDefault();

                    if (productData != null)
                    {
                        products.Add(new OrderProductModel
                        {
                            Name = productData.Name,
                            Desc = productData.Desc,
                            Count = product.Count,
                            Price = product.Count * productData.Price
                        });
                    }
                }
 
                orderModels.Add(new OrderModel
                {
                    Id = order.Id,
                    Date = order.Date,
                    Status = order.Status,
                    Shipping = order.Shipping,
                    Address = order.Address,
                    Sum = order.Sum,
                    Products = products
                });
            }

            ViewBag.Orders = orderModels;

            return View();
        }

        public IActionResult CreateOrder()
        {
            ViewBag.Products = _productLogic.Read(null);
            return View();
        }

        [HttpPost]
        public ActionResult CreateOrder(CreateOrderModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Products = _productLogic.Read(null);
                return View(model);
            }

            var orderProducts = new List<OrderProductBindingModel>();

            foreach (var product in model.Products)
            {
                orderProducts.Add(new OrderProductBindingModel
                {
                    ProductId = product.Key,
                    Count = product.Value
                });
            }

            _orderLogic.CreateOrUpdate(new OrderBindingModel
            {
                ClientId = Program.Client.Id,
                Date = DateTime.Now,
                Status = OrderStatus.Принят,
                Shipping = model.Shipping,
                Address = model.Address,
                Sum = CalculateSum(orderProducts),
                Products = orderProducts
            });

            return RedirectToAction("Index");
        }

        private int CalculateSum(List<OrderProductBindingModel> orderProducts)
        {
            int sum = 0;

            foreach(var product in orderProducts)
            {
                var productData = _productLogic.Read(new ProductBindingModel { Id = product.ProductId }).FirstOrDefault();

                if (productData != null)
                {
                    sum += productData.Price;
                }
            }

            return sum;
        }
    }
}