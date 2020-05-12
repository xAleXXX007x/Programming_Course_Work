﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace ElectronicsShopBusinessLogic.ViewModels
{
    [DataContract]
    public class UserBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [DisplayName("Логин")]
        public string Login { get; set; }

        [DataMember]
        [DisplayName("Пароль")]
        public string Password { get; set; }

        [DataMember]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        [DataMember]
        [DisplayName("Телефон")]
        public int Phone { get; set; }

        [DataMember]
        [DisplayName("Блокировка")]
        public bool Blocked { get; set; }
    }
}
