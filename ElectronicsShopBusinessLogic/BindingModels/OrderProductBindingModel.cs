using System.ComponentModel;
using System.Runtime.Serialization;

namespace ElectronicsShopBusinessLogic.BindingModels
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
        public int Count { get; set; }
    }
}