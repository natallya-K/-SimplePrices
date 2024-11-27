using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SimplePrices.Models
{
    public class ProductStatistic
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string ProductNumber { get; set; }
        public string Name { get; set; }
        public decimal NetPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal Samt { get; set; }
        public decimal Ramt { get; set; }
        public decimal Tamt { get; set; }
        public decimal Damt { get; set; }
        public decimal Sexc { get; set; }
        public decimal Rexc { get; set; }
        public decimal Exc { get; set; }
        public decimal Svat { get; set; }
        public decimal Rvat { get; set; }
        public decimal Vat { get; set; }
        public decimal Reg { get; set; }

        public ProductStatistic From(IDataRecord r)
        {
            ID = (int)r["ID"];
            Date = (DateTime)r["Date"];
            ProductNumber = (string)r["ProductNumber"];
            Name = (string)r["Name"];
            NetPrice = (r["NetPrice"] is DBNull) ? 0m : (decimal)r["NetPrice"];
            Quantity = (r["Quantity"] is DBNull) ? 0m : (decimal)r["Quantity"];
            Samt = (r["Samt"] is DBNull) ? 0m : (decimal)r["Samt"];
            Ramt = (r["Ramt"] is DBNull) ? 0m : (decimal)r["Ramt"];
            Tamt = (r["Tamt"] is DBNull) ? 0m : (decimal)r["Tamt"];
            Damt = (r["Damt"] is DBNull) ? 0m : (decimal)r["Damt"];
            Sexc = (r["Sexc"] is DBNull) ? 0m : (decimal)r["Sexc"];
            Rexc = (r["Rexc"] is DBNull) ? 0m : (decimal)r["Rexc"];
            Exc = (r["Exc"] is DBNull) ? 0m : (decimal)r["Exc"];
            Svat = (r["Svat"] is DBNull) ? 0m : (decimal)r["Svat"];
            Rvat = (r["Rvat"] is DBNull) ? 0m : (decimal)r["Rvat"];
            Vat = (r["Vat"] is DBNull) ? 0m : (decimal)r["Vat"];
            Reg = (r["Reg"] is DBNull) ? 0m : (decimal)r["Reg"];
            return this;
        }

        public ProductStatistic FromTurnover(IDataRecord r)
        {
            Date = (DateTime)r["Date"];
            Quantity = (r["Quantity"] is DBNull) ? 0m : (decimal)r["Quantity"];
            Samt = (r["Samt"] is DBNull) ? 0m : (decimal)r["Samt"];
            Ramt = (r["Ramt"] is DBNull) ? 0m : (decimal)r["Ramt"];
            Tamt = (r["Tamt"] is DBNull) ? 0m : (decimal)r["Tamt"];
            Damt = (r["Damt"] is DBNull) ? 0m : (decimal)r["Damt"];
            Sexc = (r["Sexc"] is DBNull) ? 0m : (decimal)r["Sexc"];
            Rexc = (r["Rexc"] is DBNull) ? 0m : (decimal)r["Rexc"];
            Exc = (r["Exc"] is DBNull) ? 0m : (decimal)r["Exc"];
            Svat = (r["Svat"] is DBNull) ? 0m : (decimal)r["Svat"];
            Rvat = (r["Rvat"] is DBNull) ? 0m : (decimal)r["Rvat"];
            Vat = (r["Vat"] is DBNull) ? 0m : (decimal)r["Vat"];
            Reg = (r["Reg"] is DBNull) ? 0m : (decimal)r["Reg"];
            return this;

        }
    }
}