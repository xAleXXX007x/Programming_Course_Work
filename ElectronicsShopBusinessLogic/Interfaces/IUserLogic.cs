using ElectronicsShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicsShopBusinessLogic.Interfaces
{
    public interface IUserLogic
    {
        List<UserBindingModel> Read(UserBindingModel model);

        void CreateOrUpdate(UserBindingModel model);

        void Delete(UserBindingModel model);
    }
}
