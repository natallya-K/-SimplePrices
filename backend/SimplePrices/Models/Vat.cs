using System.Data;

namespace SimplePrices.Models
{
    public class Vat
    {
        public int ID { get; set; }
        public int Code { get; set; }
        public double VatRate { get; set; }

        public Vat From(IDataRecord r)
        {
            ID = (int)r["ID"];
            Code = (int)r["code"];
            VatRate = (double)r["vatRate"];
            return this;
        }
    }
}