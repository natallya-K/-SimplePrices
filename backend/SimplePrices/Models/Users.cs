using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SimplePrices.Models
{
    public class Users
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public string Psw { get; set; }
        public string Phone { get; set; }
        public string EMail { get; set; }
        public string Role { get; set; }

        public Users From(IDataRecord r)
        {
            ID = (int)r["ID"];
            Name = (string)r["name"];
            FirstName = (string)r["firstName"];
            UserName = (string)r["userName"];
            Psw = (string)r["psw"];
            Phone = (string)r["phone"];
            EMail = (string)r["eMail"];
            Role = (string)r["role"];
            return this;
        }
    }
}