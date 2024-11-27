using Newtonsoft.Json.Linq;
using ReadConfig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using ToolBox.DataAccess.DataBase;

namespace SimplePrices.Models.Repositories
{
    public class BarcodeRepository : IRepository<Barcode, int>
    {
        static readonly GetConfig config = new GetConfig();
        static Connection connection = new Connection(config.ConnectionString);

        #region \DELETE
        public bool Delete(int id)
        {
            Command command = new Command("DELETE FROM barcode WHERE ID=@ID");
            command.AddParameter("ID", id);
            return (connection.ExecuteNonQuery(command) > 0) ? true : false;
        }
        #endregion

        #region \SELECT ALL
        public IEnumerable<Barcode> Get()
        {
            Command command = new Command("select * from barcode as b join product as p on b.productNumber = p.number");
            return connection.ExecuteReader(command, r => new Barcode().From(r));
        }
        #endregion

        #region \SELECT BY ID
        public Barcode Get(int id)
        {
            Command command = new Command("SELECT * FROM barcode as b join product as p on b.productNumber = p.number where b.ID = @ID");
            command.AddParameter("ID", id);
            return connection.ExecuteReader(command, r => new Barcode().From(r)).FirstOrDefault();
        }
        #endregion

        #region \SELECT BY BARCODE
        [HttpGet]
        public Barcode GetbyBarcode(string barcode)
        {
            Command command = new Command("SELECT * FROM barcode as b join product as p on b.productNumber = p.number where barcode = @barcode");
            command.AddParameter("barcode", barcode);
            return connection.ExecuteReader(command, r => new Barcode().From(r)).FirstOrDefault();
        }
        #endregion

        #region \SELECT BY PRODUCTNUMBER
        [HttpGet]
        public IEnumerable<Barcode> GetbyProductNumber(string productNumber)
        {
            Command command = new Command("SELECT * FROM barcode as b join product as p on b.productNumber = p.number where productNumber = @productNumber");
            command.AddParameter("productNumber", productNumber);

            return connection.ExecuteReader(command, r => new Barcode().From(r));
        }

        #endregion

        #region \POST
        public Barcode Post(Barcode entity)
        {
            Command command = new Command($@"INSERT INTO barcode
                    (barcode, productNumber)
                    OUTPUT Inserted.Id
                    values (@barcode, @productNumber)
                    ");
            command.AddParameter("barcode", entity.Code);
            command.AddParameter("productNumber", entity.ProductNumber);
            int newId = (int)connection.ExecuteScalar(command);
            entity.ID = newId;
            return entity;
        }
        #endregion

        #region \PUT
        public Barcode Put(Barcode entity)
        {
            Command command = new Command($@"UPDATE barcode
                    set barcode = @barcode, productNumber = @productNumber
                    where ID = @ID
                    ");
            command.AddParameter("barcode", entity.Code);
            command.AddParameter("productNumber", entity.ProductNumber);
            command.AddParameter("ID", entity.ID);
            connection.ExecuteNonQuery(command);
            return Get(entity.ID);
        }
        #endregion

    }
}