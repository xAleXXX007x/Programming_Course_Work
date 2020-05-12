using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace ElectronicsShopBusinessLogic.BindingModels
{
    [DataContract]
    public class PaymentBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int OrderId { get; set; }

        [DataMember]
        public int ClientId { get; set; }

        [DataMember]
        public int Account { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public int Sum { get; set; }
    }
}
