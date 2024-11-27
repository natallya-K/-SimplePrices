using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SimplePrices.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public Category From(IDataRecord r)
        {
            ID = (int)r["ID"];
            Code = (string)r["code"];
            Name = (string)r["name"];
            return this;
        }
    }
}