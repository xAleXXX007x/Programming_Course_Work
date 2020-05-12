using ElectronicsShopBusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace ElectronicsShopDatabase.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        public Shipping Shipping { get; set; }

        public string Address { get; set; }

        [Required]
        public int Sum { get; set; }

        [ForeignKey("OrderId")]
        public virtual List<OrderProduct> Products { get; set; }

        public virtual Client Client { get; set; }
    }
}
