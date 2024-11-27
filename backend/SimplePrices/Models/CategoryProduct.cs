using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SimplePrices.Models
{
    public class CategoryProduct
    {
        public string CategoryCode { get; set; }
        public string ProductNumber { get; set; }
        public string ProductName { get; set; }

        public CategoryProduct From(IDataRecord r)
        {
            CategoryCode = (string)r["categoryCode"];
            ProductNumber = (string)r["productNumber"];
            ProductName = (string)r["productName"];
            return this;
        }
    }
}