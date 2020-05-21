using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShopClientView.Models
{
    public class OrderProductModel
    {
        public string Name { get; set; }

        public string Desc { get; set; }

        public int Count { get; set; }

        public int Price { get; set; }
    }
}
