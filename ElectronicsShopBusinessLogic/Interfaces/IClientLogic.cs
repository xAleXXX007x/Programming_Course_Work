using ElectronicsShopBusinessLogic.BindingModels;
using System.Collections.Generic;

namespace ElectronicsShopBusinessLogic.Interfaces
{
    public interface IClientLogic
    {
        List<ClientBindingModel> Read(ClientBindingModel model);

        void CreateOrUpdate(ClientBindingModel model);

        void Delete(ClientBindingModel model);
    }
}
