using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicsShopBusinessLogic.HelperModels
{
    class ExcelMergeParameters
    {
        public Worksheet Worksheet { get; set; }

        public string CellFromName { get; set; }

        public string CellToName { get; set; }

        public string Merge => $"{CellFromName}:{CellToName}";

    }
}
