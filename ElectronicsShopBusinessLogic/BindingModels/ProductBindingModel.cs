using System.ComponentModel;
using System.Runtime.Serialization;

namespace ElectronicsShopBusinessLogic.BindingModels
{
    [DataContract]
    public class ProductBindingModel
    {
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Desc { get; set; }

        [DataMember]
        public int Price { get; set; }
    }
}