
using ElectronicsShopBusinessLogic.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronicsShopDatabase.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Desc { get; set; }

        public int Price { get; set; }

        public ProductCategory ProductCategory { get; set; }

        [ForeignKey("PartId")]
        public virtual List<OrderProduct> OrderProducts { get; set; }
    }
}