using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ElectronicsShopClientView.Models;
using ElectronicsShopBusinessLogic.Interfaces;
using ElectronicsShopBusinessLogic.BindingModels;
using Microsoft.AspNetCore.Identity;
using ElectronicsShopBusinessLogic.Enums;
using ElectronicsShopDatabase.Models;

namespace ElectronicsShopClientView.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientLogic _client;
        private readonly IOrderLogic _order;

        public ClientController(IClientLogic client, IOrderLogic order)
        {
            _client = client;
            _order = order;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel client)
        {
            var clients = _client.Read(null);
            var clientView = _client.Read(new ClientBindingModel
            {
                Login = client.Login,
                Password = client.Password
            }).FirstOrDefault();

            if (clientView == null)
            {
                ModelState.AddModelError("", "Вы ввели неверный пароль, либо пользователь не найден");
                return View(client);
            }

            Program.Client = clientView;

            return RedirectToAction("Index", "Product");
        }

        public IActionResult Logout()
        {
            Program.Client = null;

            return RedirectToAction("Index", "Product");
        }

        public IActionResult Admin()
        {
            Program.AdminMode = !Program.AdminMode;

            return RedirectToAction("Index", "Product");
        }

        public IActionResult Clients()
        {
            ViewBag.Clients = _client.Read(null);
            return View();
        }

        public IActionResult BlockClient(int id)
        {
            var client = _client.Read(new ClientBindingModel { Id = id }).FirstOrDefault();

            var blocked = !client.Blocked;

            if (client.Id == Program.Client.Id)
            {
                Program.Client.Blocked = blocked;
            }

            _client.CreateOrUpdate(new ClientBindingModel
            {
                Id = client.Id,
                Login = client.Login,
                Password = client.Password,
                Email = client.Email,
                Phone = client.Phone,
                Blocked = blocked
            });

            return RedirectToAction("Clients");
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(RegistrationModel client)
        {
            if (ModelState.IsValid)
            {
                var existClient = _client.Read(new ClientBindingModel
                {
                    Login = client.Login
                }).FirstOrDefault();

                if (existClient != null)
                {
                    ModelState.AddModelError("", "Данный логин уже занят");
                    return View(client);
                }

                existClient = _client.Read(new ClientBindingModel
                {
                    Email = client.Email
                }).FirstOrDefault();

                if (existClient != null)
                {
                    ModelState.AddModelError("", "Данный E-Mail уже занят");
                    return View(client);
                }

                _client.CreateOrUpdate(new ClientBindingModel
                {
                    Login = client.Login,
                    Password = client.Password,
                    Email = client.Email,
                    Phone = client.Phone
                });

                return RedirectToAction("Login");
            }

            return View(client);
        }

        public IActionResult Profile()
        {
            ViewBag.Favorites = GetFavoriteCategories();

            return View();
        }

        private List<ProductCategory> GetFavoriteCategories()
        {
            Dictionary<ProductCategory, int> productCategories = new Dictionary<ProductCategory, int>();

            foreach (var order in _order.Read(new OrderBindingModel { ClientId = Program.Client.Id }))
            {
                foreach (var product in order.Products)
                {
                    var category = product.ProductCategory;

                    if (productCategories.ContainsKey(category))
                    {
                        productCategories[category] += product.Count;
                    }
                    else
                    {
                        productCategories[category] = product.Count;
                    }
                }
            }

            var categories = productCategories.ToList();
            categories.Sort((p1, p2) => p2.Value.CompareTo(p1.Value));

            var result = new List<ProductCategory>();

            foreach (var cat in categories)
            {
                result.Add(cat.Key);
            }

            return result;
        }
    }
}
