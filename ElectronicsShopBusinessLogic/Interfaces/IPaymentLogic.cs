using ElectronicsShopBusinessLogic.BindingModels;
using System.Collections.Generic;

namespace ElectronicsShopBusinessLogic.Interfaces
{
    public interface IPaymentLogic
    {
        List<PaymentBindingModel> Read(PaymentBindingModel model);

        void CreateOrUpdate(PaymentBindingModel model);

        void Delete(PaymentBindingModel model);
    }
}
