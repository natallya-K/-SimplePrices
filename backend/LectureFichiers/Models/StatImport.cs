using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireFichiers.Models
{
    public struct StatImport
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public int VatCode { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public string Samt { get; set; }
        public string Ramt { get; set; }
        public string Tamt { get; set; }
        public string Sdamt { get; set; }
        public string Rdamt { get; set; }
        public string Damt { get; set; }
        public string Sexc { get; set; }
        public string Rexc { get; set; }
        public string Exc { get; set; }
        public string Svat { get; set; }
        public string Rvat { get; set; }
        public string Vat { get; set; }
        public string Sreg { get; set; }
        public string Rreg { get; set; }
        public string Reg { get; set; }

        public StatImport(int vatCode, 
                             string price,
                             string quant,
                             string samt,
                             string ramt,
                             string tamt,
                             string sdamt,
                             string rdamt,
                             string damt,
                             string sexc,
                             string rexc,
                             string exc,
                             string svat,
                             string rvat,
                             string vat,
                             string sreg,
                             string rreg,
                             string reg
                            ) : this()
            {
            VatCode = vatCode;
            Price = price;
            Quantity = quant;
            Samt = samt;
            Ramt = ramt;
            Tamt = tamt;
            Sdamt = sdamt;
            Rdamt = rdamt;
            Damt = damt;
            Sexc = sexc;
            Rexc = rexc;
            Exc = exc;
            Svat = svat;
            Rvat = rvat;
            Vat = vat;
            Sreg = sreg;
            Rreg = rreg;
            Reg = reg;
        }
    }
}
