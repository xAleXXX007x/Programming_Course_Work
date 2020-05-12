using System.ComponentModel;
using System.Runtime.Serialization;

namespace ElectronicsShopBusinessLogic.ViewModels
{
    [DataContract]
    public class OrderProductBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int OrderId { get; set; }

        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        [DisplayName("Количество")]
        public int Count { get; set; }
    }
}