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
    public class PaymentLogic : IPaymentLogic
    {
        public void CreateOrUpdate(PaymentBindingModel model)
        {
            using (var context = new ElectronicsShopDatabase())
            {
                Payment tempPayment = model.Id.HasValue ? null : new Payment();

                if (model.Id.HasValue)
                {
                    tempPayment = context.Payments.FirstOrDefault(rec => rec.Id == model.Id);
                }

                if (model.Id.HasValue)
                {
                    if (tempPayment == null)
                    {
                        throw new Exception("Элемент не найден");
                    }

                    CreateModel(model, tempPayment);
                }
                else
                {
                    context.Payments.Add(CreateModel(model, tempPayment));
                }

                context.SaveChanges();
            }
        }

        public void Delete(PaymentBindingModel model)
        {
            using (var context = new ElectronicsShopDatabase())
            {
                Payment element = context.Payments.FirstOrDefault(rec => rec.Id == model.Id.Value);

                if (element != null)
                {
                    context.Payments.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        public List<PaymentViewModel> Read(PaymentBindingModel model)
        {
            using (var context = new ElectronicsShopDatabase())
            {
                List<PaymentViewModel> result = new List<PaymentViewModel>();

                if (model != null)
                {
                    result.AddRange(context.Payments
                        .Where(rec => rec.Id == model.Id || rec.OrderId.Equals(model.OrderId))
                        .Select(rec => CreateViewModel(rec)));
                }
                else
                {
                    result.AddRange(context.Payments.Select(rec => CreateViewModel(rec)));
                }
                return result;
            }
        }

        private Payment CreateModel(PaymentBindingModel model, Payment payment)
        {
            using (var context = new ElectronicsShopDatabase())
            {
                payment.OrderId = model.OrderId;
                payment.ClientId = model.ClientId;
                payment.Account = model.Account;
                payment.Date = model.Date;
                payment.Sum = model.Sum;

                return payment;
            }
        }

        static private PaymentViewModel CreateViewModel(Payment payment)
        {
            using (var context = new ElectronicsShopDatabase())
            {
                return new PaymentViewModel
                {
                    Id = payment.Id,
                    OrderId = payment.OrderId,
                    ClientId = payment.ClientId,
                    Account = payment.Account,
                    Date = payment.Date,
                    Sum = payment.Sum
                };
            }
        }
    }
}
