using ElectronicsShopBusinessLogic.BindingModels;
using System.Collections.Generic;

namespace ElectronicsShopBusinessLogic.Interfaces
{
    public interface IOrderLogic
    {
        List<OrderBindingModel> Read(OrderBindingModel model);

        void CreateOrUpdate(OrderBindingModel model);

        void Delete(OrderBindingModel model);
    }
}
