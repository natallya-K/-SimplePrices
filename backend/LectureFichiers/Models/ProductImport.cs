using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireFichiers.Models
{
    public struct ProductImport
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public int VatCode { get; set; }
        public string UnitCode { get; set; }
        public string Price { get; set; }

        public ProductImport(int vatCode, string price) : this()
            {
            VatCode = vatCode;
            Price = price;
            }
    }
}
