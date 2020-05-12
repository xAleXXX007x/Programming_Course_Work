using ElectronicsShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicsShopBusinessLogic.Interfaces
{
    public interface IPaymentLogic
    {
        List<PaymentBindingModel> Read(PaymentBindingModel model);

        void CreateOrUpdate(PaymentBindingModel model);

        void Delete(PaymentBindingModel model);
    }
}
