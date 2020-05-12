using ElectronicsShopBusinessLogic.BindingModels;
using System.Collections.Generic;

namespace ElectronicsShopBusinessLogic.Interfaces
{
    public interface IProductLogic
    {
        List<ProductBindingModel> Read(ProductBindingModel model);

        void CreateOrUpdate(ProductBindingModel model);

        void Delete(ProductBindingModel model);
    }
}
