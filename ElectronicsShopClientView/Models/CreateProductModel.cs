using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShopClientView.Models
{
    public class CreateProductModel
    {
        [Required]
        public string Name { get; set; }

        public string Desc { get; set; }

        [Required]
        public int Price { get; set; }
    }
}
