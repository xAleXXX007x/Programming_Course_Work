using ElectronicsShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShopClientView.Models
{
    public class PayOrderModel
    {
        [Required]
        public string Account { get; set; }

        [Required]
        public int Sum { get; set; }

        public int OrderId { get; set; }
    }
}
