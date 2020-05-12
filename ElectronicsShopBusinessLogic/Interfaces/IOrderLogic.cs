using ElectronicsShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicsShopBusinessLogic.Interfaces
{
    public interface IOrderLogic
    {
        List<OrderBindingModel> Read(OrderBindingModel model);

        void CreateOrUpdate(OrderBindingModel model);

        void Delete(OrderBindingModel model);
    }
}
