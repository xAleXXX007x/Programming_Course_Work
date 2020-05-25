using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShopClientView.Models
{
    public class PeriodOrderModel
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public bool SendMail { get; set; }
    }
}
