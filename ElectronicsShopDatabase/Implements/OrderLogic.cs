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
    public class OrderLogic : IOrderLogic
    {
        public void CreateOrUpdate(OrderBindingModel model)
        {
            using (var context = new ElectronicsShopDatabase())
            {
                Order tempOrder = model.Id.HasValue ? null : new Order();

                if (model.Id.HasValue)
                {
                    tempOrder = context.Orders.FirstOrDefault(rec => rec.Id == model.Id);
                }

                if (model.Id.HasValue)
                {
                    if (tempOrder == null)
                    {
                        throw new Exception("Элемент не найден");
                    }

                    CreateModel(model, tempOrder);
                }
                else
                {
                    context.Orders.Add(CreateModel(model, tempOrder));
                }

                context.SaveChanges();
            }
        }

        public void Delete(OrderBindingModel model)
        {
            using (var context = new ElectronicsShopDatabase())
            {
                Order element = context.Orders.FirstOrDefault(rec => rec.Id == model.Id.Value);

                if (element != null)
                {
                    context.Orders.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        public List<OrderViewModel> Read(OrderBindingModel model)
        {
            using (var context = new ElectronicsShopDatabase())
            {
                List<OrderViewModel> result = new List<OrderViewModel>();

                if (model != null)
                {
                    result.AddRange(context.Orders
                        .Where(rec => rec.Id == model.Id || rec.ClientId == model.ClientId)
                        .Select(rec => CreateViewModel(rec)));
                }
                else
                {
                    result.AddRange(context.Orders.Select(rec => CreateViewModel(rec)));
                }
                return result;
            }
        }

        private Order CreateModel(OrderBindingModel model, Order order)
        {
            using (var context = new ElectronicsShopDatabase())
            {
                order.ClientId = model.ClientId;
                order.Date = model.Date;
                order.Status = model.Status;
                order.Shipping = model.Shipping;
                order.Address = model.Address;
                order.Sum = model.Sum;

                return order;
            }
        }

        static private OrderViewModel CreateViewModel(Order order)
        {
            using (var context = new ElectronicsShopDatabase())
            {
                return new OrderViewModel
                {
                    Id = order.Id,
                    ClientId = order.ClientId,
                    Date = order.Date,
                    Status = order.Status,
                    Shipping = order.Shipping,
                    Address = order.Address,
                    Sum = order.Sum
                };
            }
        }
    }
}
