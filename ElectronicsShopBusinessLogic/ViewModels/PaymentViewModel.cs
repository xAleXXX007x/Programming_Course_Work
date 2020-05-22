using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace ElectronicsShopBusinessLogic.ViewModels
{
    [DataContract]
    public class PaymentViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int OrderId { get; set; }

        [DataMember]
        public int ClientId { get; set; }

        [DataMember]
        [DisplayName("Счет")]
        public string Account { get; set; }

        [DataMember]
        [DisplayName("Дата оплаты")]
        public DateTime Date { get; set; }

        [DataMember]
        [DisplayName("Сумма")]
        public int Sum { get; set; }
    }
}
