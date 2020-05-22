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
        [Required]
        public Shipping Shipping { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public Dictionary<int, int> Products { get; set; }
    }
}
