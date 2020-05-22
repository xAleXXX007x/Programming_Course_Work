using ElectronicsShopBusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShopClientView.Models
{
    public class OrderModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public OrderStatus Status { get; set; }

        public Shipping Shipping { get; set; }

        public string Address { get; set; }

        public int Sum { get; set; }

        public int LeftSum { get; set; }

        public List<OrderProductModel> Products { get; set; }
    }
}
