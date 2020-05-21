using ElectronicsShopBusinessLogic.Enums;
using ElectronicsShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShopClientView.Models
{
    public class CreateOrderModel
    {
        public Shipping Shipping { get; set; }

        public string Address { get; set; }

        public List<OrderProductViewModel> Products { get; set; }

        public List<ProductViewModel> AllProducts { get; set; }
    }
}
