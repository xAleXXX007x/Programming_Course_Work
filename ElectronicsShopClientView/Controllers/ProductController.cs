using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicsShopBusinessLogic.BindingModels;
using ElectronicsShopBusinessLogic.Interfaces;
using ElectronicsShopClientView.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsShopClientView.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductLogic _logic;

        public ProductController(IProductLogic logic)
        {
            _logic = logic;
        }

        public IActionResult Index()
        {
            ViewBag.Products = _logic.Read(null);

            return View();
        }

        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateProduct(CreateProductModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var foundProduct = _logic.Read(new ProductBindingModel
            {
                Name = model.Name
            }).FirstOrDefault();

            if (foundProduct != null)
            {
                ModelState.AddModelError("", "Товар с таким названием уже существует");
                return View(model);
            }

            _logic.CreateOrUpdate(new ProductBindingModel
            {
                Name = model.Name,
                Desc = model.Desc,
                Price = model.Price
            });

            return RedirectToAction("Index");
        }
    }
}