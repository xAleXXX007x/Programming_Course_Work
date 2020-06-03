using ElectronicsShopBusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShopClientView.Models
{
    public class ProductModel
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Desc { get; set; }

        [Required]
        public int Price { get; set; }

        public ProductCategory ProductCategory { get; set; }
    }
}
