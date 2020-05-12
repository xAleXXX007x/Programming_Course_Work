using ElectronicsShopBusinessLogic.BindingModels;
using ElectronicsShopBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace ElectronicsShopBusinessLogic.Interfaces
{
    public interface IProductLogic
    {
        List<ProductViewModel> Read(ProductBindingModel model);

        void CreateOrUpdate(ProductBindingModel model);

        void Delete(ProductBindingModel model);
    }
}
