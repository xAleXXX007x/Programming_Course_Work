using ElectronicsShopBusinessLogic.Enums;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsShopDatabase.Models
{
    public class OrderProduct
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        [Required]
        public int Count { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}