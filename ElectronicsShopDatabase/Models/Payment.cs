using System;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsShopDatabase.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ClientId { get; set; }

        [Required]
        public int Account { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Sum { get; set; }

        public virtual Order Order { get; set; }

        public virtual Client Client { get; set; }
    }
}
