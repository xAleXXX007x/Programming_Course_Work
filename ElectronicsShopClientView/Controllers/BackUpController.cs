using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using ElectronicsShopBusinessLogic.BindingModels;
using ElectronicsShopBusinessLogic.BusinessLogics;
using ElectronicsShopBusinessLogic.Enums;
using ElectronicsShopBusinessLogic.Interfaces;
using ElectronicsShopBusinessLogic.ViewModels;
using ElectronicsShopClientView.Models;
using ElectronicsShopDatabase.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsShopClientView.Controllers
{
    public class BackUpController : Controller
    {
        private readonly BackUpAbstractLogic _logic;

        public BackUpController(BackUpAbstractLogic logic)
        {
            _logic = logic;
        }

        public IActionResult Backup()
        {
            _logic.CreateArchive();
            return RedirectToAction("Index", "Product");
        }
    }
}