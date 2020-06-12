using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Charts;
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
            ViewBag.Orders = _orderLogic.Read(new OrderBindingModel
            {
                ClientId = Program.Client.Id
            });

            return View();
        }

        [HttpPost]
        public IActionResult Index(PeriodOrderModel model)
        {
            ViewBag.Orders = _orderLogic.Read(new OrderBindingModel
            {
                ClientId = Program.Client.Id,
                Date = model.From,
                DateTo = model.To
            });

            if (model.SendMail)
            {
                _reportLogic.SendOrdersReport(new OrderBindingModel
                {
                    ClientId = Program.Client.Id,
                    Date = model.From,
                    DateTo = model.To
                }, Program.Client.Email);
            }

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
            return View();
        }

        [HttpPost]
        public ActionResult PayOrder(PayOrderModel model)
        {
            OrderViewModel order = _orderLogic.Read(new OrderBindingModel
            {
                Id = model.OrderId
            }).FirstOrDefault();

            if (!ModelState.IsValid)
            {
                ViewBag.Order = order;
                return View(model);
            }

            if (order.SumPaid > model.Sum)
            {
                ViewBag.Order = order;
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

            _orderLogic.CreateOrUpdate(new OrderBindingModel
            {
                Id = order.Id,
                ClientId = order.ClientId,
                Date = order.Date,
                Status = order.SumPaid + model.Sum < order.Sum ? OrderStatus.Оплачивается : OrderStatus.Оплачен,
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
    }
}