using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ElectronicsShopBusinessLogic.BindingModels;
using ElectronicsShopBusinessLogic.BusinessLogics;
using ElectronicsShopBusinessLogic.Enums;
using ElectronicsShopBusinessLogic.Interfaces;
using ElectronicsShopBusinessLogic.ViewModels;
using ElectronicsShopClientView.Models;
using ElectronicsShopDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Protocols;

namespace ElectronicsShopClientView.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderLogic _orderLogic;
        private readonly IProductLogic _productLogic;
        private readonly IPaymentLogic _paymentLogic;
        private readonly ReportLogic _reportLogic;

        public OrderController(IOrderLogic orderLogic, IProductLogic productLogic, IPaymentLogic paymentLogic, ReportLogic reportLogic)
        {
            _orderLogic = orderLogic;
            _productLogic = productLogic;
            _paymentLogic = paymentLogic;
            _reportLogic = reportLogic;
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
                            Price = productData.Price
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
                    LeftSum = CalculateLeftSum(order),
                    Products = products
                });
            }

            ViewBag.Orders = orderModels;

            return View();
        }

        public IActionResult SendWordReport(int id)
        {
            var order = _orderLogic.Read(new OrderBindingModel { Id = id }).FirstOrDefault();
            _reportLogic.SendOrderProducts(order, Program.Client.Email, FileExtension.Word);
            return RedirectToAction("Index");
        }

        public IActionResult SendExcelReport(int id)
        {
            var order = _orderLogic.Read(new OrderBindingModel { Id = id }).FirstOrDefault();
            _reportLogic.SendOrderProducts(order, Program.Client.Email, FileExtension.Excel);
            return RedirectToAction("Index");
        }

        public IActionResult CreateOrder()
        {
            ViewBag.Products = _productLogic.Read(null);
            return View();
        }

        [HttpPost]
        public ActionResult CreateOrder(CreateOrderModel model)
        {
            if (model.Shipping == Shipping.Самовывоз)
            {
                model.Address = "Адрес магазина";
            } else
            {
                ViewBag.Products = _productLogic.Read(null);
                ModelState.AddModelError("", "Не введён адрес доставки");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Products = _productLogic.Read(null);
                return View(model);
            }

            var orderProducts = new List<OrderProductBindingModel>();

            foreach (var product in model.Products)
            {
                if (product.Value > 0)
                {
                    orderProducts.Add(new OrderProductBindingModel
                    {
                        ProductId = product.Key,
                        Count = product.Value
                    });
                }
            }

            if (orderProducts.Count == 0)
            {
                ViewBag.Products = _productLogic.Read(null);
                ModelState.AddModelError("", "Ни один товар не выбран");
                return View(model);
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

            foreach (var product in orderProducts)
            {
                var productData = _productLogic.Read(new ProductBindingModel { Id = product.ProductId }).FirstOrDefault();

                if (productData != null)
                {
                    sum += productData.Price * product.Count;
                }
            }

            return sum;
        }

        public IActionResult PayOrder(int id)
        {
            var order = _orderLogic.Read(new OrderBindingModel
            {
                Id = id
            }).FirstOrDefault();
            ViewBag.Order = order;
            ViewBag.LeftSum = CalculateLeftSum(order);
            return View();
        }

        [HttpPost]
        public ActionResult PayOrder(PayOrderModel model)
        {
            OrderViewModel order = _orderLogic.Read(new OrderBindingModel
            {
                Id = model.OrderId
            }).FirstOrDefault();

            int leftSum = CalculateLeftSum(order);

            if (!ModelState.IsValid)
            {
                ViewBag.Order = order;
                ViewBag.LeftSum = leftSum;
                return View(model);
            }

            if (leftSum < model.Sum)
            {
                ViewBag.Order = order;
                ViewBag.LeftSum = leftSum;
                return View(model);
            }

            _paymentLogic.CreateOrUpdate(new PaymentBindingModel
            {
                OrderId = order.Id,
                ClientId = Program.Client.Id,
                Account = model.Account,
                Date = DateTime.Now,
                Sum = model.Sum
            });

            leftSum -= model.Sum;

            _orderLogic.CreateOrUpdate(new OrderBindingModel
            {
                Id = order.Id,
                ClientId = order.ClientId,
                Date = order.Date,
                Status = leftSum > 0 ? OrderStatus.Оплачивается : OrderStatus.Оплачен,
                Sum = order.Sum,
                Products = order.Products.Select(rec => new OrderProductBindingModel
                {
                    Id = rec.Id,
                    OrderId = rec.OrderId,
                    ProductId = rec.ProductId,
                    Count = rec.Count
                }).ToList()
            });

            return RedirectToAction("Index");
        }

        private int CalculateLeftSum(OrderViewModel order)
        {
            int sum = order.Sum;
            int paidSum = _paymentLogic.Read(new PaymentBindingModel {
                OrderId = order.Id
            }).Select(rec => rec.Sum).Sum();

            return sum - paidSum;
        }
    }
}