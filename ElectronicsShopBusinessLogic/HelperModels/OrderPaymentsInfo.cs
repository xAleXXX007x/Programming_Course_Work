using ElectronicsShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicsShopBusinessLogic.HelperModels
{
    class OrderPaymentsInfo
    {
        public string FileName { get; set; }

        public string Title { get; set; }

        public List<OrderViewModel> Orders { get; set; }

        public Dictionary<int, List<PaymentViewModel>> Payments { get; set; }
    }
}
