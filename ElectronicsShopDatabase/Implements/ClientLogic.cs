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
    public class ClientLogic : IClientLogic
    {
        public void CreateOrUpdate(ClientBindingModel model)
        {
            using (var context = new ElectronicsShopDatabase())
            {
                Client tempClient = model.Id.HasValue ? null : new Client();

                if (model.Id.HasValue)
                {
                    tempClient = context.Clients.FirstOrDefault(rec => rec.Id == model.Id);
                }

                if (model.Id.HasValue)
                {
                    if (tempClient == null)
                    {
                        throw new Exception("Элемент не найден");
                    }

                    CreateModel(model, tempClient);
                }
                else
                {
                    context.Clients.Add(CreateModel(model, tempClient));
                }

                context.SaveChanges();
            }
        }

        public void Delete(ClientBindingModel model)
        {
            using (var context = new ElectronicsShopDatabase())
            {
                Client element = context.Clients.FirstOrDefault(rec => rec.Id == model.Id.Value);

                if (element != null)
                {
                    context.Clients.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        public List<ClientViewModel> Read(ClientBindingModel model)
        {
            using (var context = new ElectronicsShopDatabase())
            {
                List<ClientViewModel> result = new List<ClientViewModel>();

                if (model != null)
                {
                    result.AddRange(context.Clients
                        .Where(rec => (rec.Id == model.Id) || ((rec.Login == model.Login || rec.Email == model.Email)
                        && (model.Password == null || rec.Password == model.Password)))
                        .Select(rec => CreateViewModel(rec)));
                }
                else
                {
                    result.AddRange(context.Clients.Select(rec => CreateViewModel(rec)));
                }
                return result;
            }
        }

        private Client CreateModel(ClientBindingModel model, Client client)
        {
            using (var context = new ElectronicsShopDatabase())
            {
                client.Login = model.Login;
                client.Password = model.Password;
                client.Email = model.Email;
                client.Phone = model.Phone;
                client.Blocked = model.Blocked;

                return client;
            }
        }

        static private ClientViewModel CreateViewModel(Client client)
        {
            using (var context = new ElectronicsShopDatabase())
            {
                return new ClientViewModel
                {
                    Id = client.Id,
                    Login = client.Login,
                    Password = client.Password,
                    Email = client.Email,
                    Phone = client.Phone,
                    Blocked = client.Blocked
                };
            }
        }
    }
}
