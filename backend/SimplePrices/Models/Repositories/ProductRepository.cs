using ReadConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ToolBox.DataAccess.DataBase;

namespace SimplePrices.Models.Repositories
{
    public class ProductRepository : IRepository<Product, int>
    {
        //charger la configuration
        // -----------------------
        static readonly GetConfig config = new GetConfig();
        static Connection connection = new Connection(config.ConnectionString);

        #region \SELECT ALL
        //-----------------
        public IEnumerable<Product> Get()
        {
            Command command = new Command("Select * from product");

            return connection.ExecuteReader(command, r => new Product().From(r));
        }
        #endregion

        #region \SELECT BY ID
        //-------------------
        public Product Get(int ID)
        {
            Command command = new Command("Select * from product where ID = @ID");
            command.AddParameter("ID", ID);
            return connection.ExecuteReader(command, r => new Product().From(r)).FirstOrDefault();
        }
        #endregion

        #region \SELECT BY NUMBER
        //-----------------------
        [HttpGet]
        public Product GetbyNumber(string number)
        {
            Command command = new Command("Select * from product where number = @number");
            command.AddParameter("number", number);

            return connection.ExecuteReader(command, r => new Product()
            {
                ID = (int)r["ID"],
                Name = (string)r["Name"],
                Number = (string)r["Number"],
                VatCode = (int)r["VatCode"],
                UnitCode = (string)r["UnitCode"],
                UnitPrice = (r["UnitPrice"] is DBNull) ? 0M : (decimal)r["UnitPrice"]
            }).FirstOrDefault();
        }
        #endregion

        #region \POST
        //-----------
        public Product Post(Product entity)
        {
            Command command = new Command($@"insert into product
                    (number, name, vatCode, unitCode, unitPrice)
                    OUTPUT Inserted.Id
                    values (@number, @name, @vatCode, @unitCode,@unitPrice)
                    ");
            command.AddParameter("number", entity.Number);
            command.AddParameter("name", entity.Name);
            command.AddParameter("vatCode", entity.VatCode);
            command.AddParameter("unitCode", entity.UnitCode);
            command.AddParameter("unitPrice", entity.UnitPrice);
            int nouvelleId = (int)connection.ExecuteScalar(command);
            entity.ID = nouvelleId;
            return entity;
        }
        #endregion

        #region \PUT
        //----------
        public Product Put(Product entity)
        {
            Command command = new Command($@"update product
                    set number = @number, name = @name, vatCode = @vatCode, unitCode = @unitCode, unitPrice = @unitPrice
                    where ID = @ID
                    ");
            command.AddParameter("number", entity.Number);
            command.AddParameter("name", entity.Name);
            command.AddParameter("vatCode", entity.VatCode);
            command.AddParameter("unitCode", entity.UnitCode);
            command.AddParameter("unitPrice", entity.UnitPrice);
            command.AddParameter("ID", entity.ID);
            connection.ExecuteNonQuery(command);
            return Get(entity.ID);
        }
        #endregion

        #region \DELETE
        //------------
        public bool Delete(int ID)
        {
            Command command = new Command("delete from product where ID=@ID");
            command.AddParameter("ID", ID);
            return (connection.ExecuteNonQuery(command) > 0) ? true : false;
        }
        #endregion
    }
}