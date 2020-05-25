using ElectronicsShopBusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace ElectronicsShopBusinessLogic.BindingModels
{
    [DataContract]
    public class OrderBindingModel
    {
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public int ClientId { get; set; }

        [DataMember]
        public DateTime? Date { get; set; }

        [DataMember]
        public DateTime? DateTo { get; set; }

        [DataMember]
        public OrderStatus Status { get; set; }

        [DataMember]
        public Shipping Shipping { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public int Sum { get; set; }

        [DataMember]
        public List<OrderProductBindingModel> Products { get; set; }
    }
}
