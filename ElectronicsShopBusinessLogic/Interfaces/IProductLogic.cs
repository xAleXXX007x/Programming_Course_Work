using ElectronicsShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicsShopBusinessLogic.Interfaces
{
    public interface IProductLogic
    {
        List<ProductBindingModel> Read(ProductBindingModel model);

        void CreateOrUpdate(ProductBindingModel model);

        void Delete(ProductBindingModel model);
    }
}
