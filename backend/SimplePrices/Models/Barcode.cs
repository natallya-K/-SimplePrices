using System.Data;

namespace SimplePrices.Models
{
    public class Barcode
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string ProductNumber { get; set; }
        public Product Product { get; set; }
        

        public Barcode From(IDataRecord r)
        {
            ID = (int)r["ID"];
            Code = (string)r["barcode"];
            ProductNumber = (string)r["productNumber"];
            Product = new Product().From(r);
            return this;
        }

    }
}