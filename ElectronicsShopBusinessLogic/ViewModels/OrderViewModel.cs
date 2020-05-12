using ElectronicsShopBusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace ElectronicsShopBusinessLogic.ViewModels
{
    [DataContract]
    public class OrderViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ClientId { get; set; }

        [DataMember]
        [DisplayName("Дата создания")]
        public DateTime Date { get; set; }

        [DataMember]
        [DisplayName("Статус")]
        public OrderStatus Status { get; set; }

        [DataMember]
        [DisplayName("Доставка")]
        public Shipping Shipping { get; set; }

        [DataMember]
        [DisplayName("Адрес доставки")]
        public string Address { get; set; }

        [DataMember]
        [DisplayName("Сумма")]
        public int Sum { get; set; }

        [DataMember]
        public List<OrderProductViewModel> Products { get; set; }
    }
}
