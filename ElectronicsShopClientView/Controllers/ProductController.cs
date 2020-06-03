using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using ElectronicsShopBusinessLogic.BindingModels;
using ElectronicsShopBusinessLogic.Enums;
using ElectronicsShopBusinessLogic.Interfaces;
using ElectronicsShopBusinessLogic.ViewModels;
using ElectronicsShopClientView.Models;
using ElectronicsShopDatabase.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsShopClientView.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductLogic _logic;
        private readonly IOrderLogic _orderLogic;

        public ProductController(IProductLogic logic, IOrderLogic orderLogic)
        {
            _logic = logic;
            _orderLogic = orderLogic;
        }

        public IActionResult Index()
        {
            ViewBag.Products = _logic.Read(null);

            if (Program.Client != null)
            {
                ViewBag.Recommended = GetRecommendedProduct();
            }

            return View();
        }

        public IActionResult DeleteProduct(int id)
        {
            _logic.Delete(new ProductBindingModel { Id = id });

            return RedirectToAction("Index");
        }

        public IActionResult EditProduct(int? id)
        {
            if (id != null)
            {
                var product = _logic.Read(new ProductBindingModel { Id = id }).FirstOrDefault();

                return View(new ProductModel
                {
                    Id = id,
                    Name = product.Name,
                    Desc = product.Desc,
                    ProductCategory = product.ProductCategory,
                    Price = product.Price
                });
            } else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult EditProduct(ProductModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.Id == null)
            {
                var foundProduct = _logic.Read(new ProductBindingModel
                {
                    Name = model.Name
                }).FirstOrDefault();

                if (foundProduct != null)
                {
                    ModelState.AddModelError("", "Товар с таким названием уже существует");
                    return View(model);
                }
            }

            _logic.CreateOrUpdate(new ProductBindingModel
            {
                Id = model.Id,
                Name = model.Name,
                Desc = model.Desc,
                Price = model.Price,
                ProductCategory = model.ProductCategory
            });

            return RedirectToAction("Index");
        }

        private ProductViewModel GetRecommendedProduct()
        {
            Dictionary<ProductCategory, int> productCategories = new Dictionary<ProductCategory, int>();

            var categories = Enum.GetValues(typeof(ProductCategory));

            foreach (var order in _orderLogic.Read(new OrderBindingModel { ClientId = Program.Client.Id }))
            {
                foreach (var product in order.Products)
                {
                    var category = product.ProductCategory;

                    if (productCategories.ContainsKey(category))
                    {
                        productCategories[category] += product.Count;
                    } else
                    {
                        productCategories[category] = product.Count;
                    }
                }
            }

            var random = new Random();
            ProductCategory maxCategory = (ProductCategory)categories.GetValue(random.Next(categories.Length));

            foreach (var category in productCategories)
            {
                if (!productCategories.ContainsKey(maxCategory) || productCategories[maxCategory] < category.Value)
                {
                    maxCategory = category.Key;
                }
            }

            var categoryProducts = _logic.Read(new ProductBindingModel
            {
                ProductCategory = maxCategory
            }).ToList();

            if (categoryProducts.Count > 0)
            {
                return categoryProducts[random.Next(categoryProducts.Count)];
            }
            else
            {
                return null;
            }
        }
    }
}