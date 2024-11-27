using System;
using System.Data;

namespace SimplePrices.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public int VatCode { get; set; }
        public string UnitCode { get; set; }
        public decimal UnitPrice { get; set; }

        public Product From(IDataRecord r)
        {
            ID = (int)r["ID"];
            Number = (string)r["Number"];
            Name = (string)r["Name"];
            VatCode = (int)r["VatCode"];
            UnitCode = (string)r["UnitCode"];
            UnitPrice = (r["UnitPrice"] is DBNull) ? 0m : (decimal)r["UnitPrice"];
            return this;
        }

    }
}