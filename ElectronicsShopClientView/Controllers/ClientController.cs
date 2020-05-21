﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ElectronicsShopClientView.Models;
using ElectronicsShopBusinessLogic.Interfaces;
using ElectronicsShopBusinessLogic.BindingModels;
using Microsoft.AspNetCore.Identity;

namespace ElectronicsShopClientView.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientLogic _client;

        public ClientController(IClientLogic client)
        {
            _client = client;
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

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ViewResult Registration(RegistrationModel client)
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

                return View("Login");
            }

            return View(client);
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
}