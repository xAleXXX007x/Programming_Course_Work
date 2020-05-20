using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShopClientView.Models
{
    public class User : IdentityUser
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }
    }
}
