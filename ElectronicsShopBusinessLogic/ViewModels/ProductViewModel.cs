using System.ComponentModel;
using System.Runtime.Serialization;

namespace ElectronicsShopBusinessLogic.ViewModels
{
    [DataContract]
    public class ProductBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [DisplayName("Название")]
        public string Name { get; set; }

        [DataMember]
        [DisplayName("Описание")]
        public string Desc { get; set; }

        [DataMember]
        [DisplayName("Цена")]
        public int Price { get; set; }
    }
}