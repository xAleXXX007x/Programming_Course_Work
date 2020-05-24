using ElectronicsShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicsShopBusinessLogic.HelperModels
{
    class OrderProductsInfo
    {
        public string FileName { get; set; }

        public string Title { get; set; }

        public List<ProductViewModel> Products { get; set; }
    }
}
