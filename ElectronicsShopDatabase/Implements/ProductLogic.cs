using ElectronicsShopBusinessLogic.BindingModels;
using ElectronicsShopBusinessLogic.Interfaces;
using ElectronicsShopBusinessLogic.ViewModels;
using ElectronicsShopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronicsShopDatabase.Implements
{
    public class ProductLogic : IProductLogic
    {
        public void CreateOrUpdate(ProductBindingModel model)
        {
            using (var context = new ElectronicsShopDatabase())
            {
                Product tempProduct = model.Id.HasValue ? null : new Product();

                if (model.Id.HasValue)
                {
                    tempProduct = context.Products.FirstOrDefault(rec => rec.Id == model.Id);
                }

                if (model.Id.HasValue)
                {
                    if (tempProduct == null)
                    {
                        throw new Exception("Элемент не найден");
                    }

                    CreateModel(model, tempProduct);
                }
                else
                {
                    context.Products.Add(CreateModel(model, tempProduct));
                }

                context.SaveChanges();
            }
        }

        public void Delete(ProductBindingModel model)
        {
            using (var context = new ElectronicsShopDatabase())
            {
                Product element = context.Products.FirstOrDefault(rec => rec.Id == model.Id.Value);

                if (element != null)
                {
                    context.Products.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        public List<ProductViewModel> Read(ProductBindingModel model)
        {
            using (var context = new ElectronicsShopDatabase())
            {
                List<ProductViewModel> result = new List<ProductViewModel>();

                if (model != null)
                {
                    result.AddRange(context.Products
                        .Where(rec => rec.Id == model.Id || rec.Name == model.Name)
                        .Select(rec => CreateViewModel(rec)));
                }
                else
                {
                    result.AddRange(context.Products.Select(rec => CreateViewModel(rec)));
                }
                return result;
            }
        }

        private Product CreateModel(ProductBindingModel model, Product product)
        {
            using (var context = new ElectronicsShopDatabase())
            {
                product.Name = model.Name;
                product.Desc = model.Desc;
                product.Price = model.Price;

                return product;
            }
        }

        static private ProductViewModel CreateViewModel(Product product)
        {
            using (var context = new ElectronicsShopDatabase())
            {
                return new ProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Desc = product.Desc,
                    Price = product.Price
                };
            }
        }
    }
}
