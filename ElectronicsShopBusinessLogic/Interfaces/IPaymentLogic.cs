using ElectronicsShopBusinessLogic.BindingModels;
using ElectronicsShopBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace ElectronicsShopBusinessLogic.Interfaces
{
    public interface IPaymentLogic
    {
        List<PaymentViewModel> Read(PaymentBindingModel model);

        void CreateOrUpdate(PaymentBindingModel model);

        void Delete(PaymentBindingModel model);
    }
}
