using ReadConfig;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ToolBox.DataAccess.DataBase;

namespace SimplePrices.Models.Repositories
{
    public class VatRepository : IRepository<Vat, int>
    {
        //charger la configuration
        // -----------------------
        static readonly GetConfig config = new GetConfig();
        static Connection connection = new Connection(config.ConnectionString);

        #region \DELETE
        public bool Delete(int id)
        {
            Command command = new Command("DELETE FROM vat WHERE ID=@ID");
            command.AddParameter("ID", id);
            return (connection.ExecuteNonQuery(command) > 0) ? true : false;
        }
        #endregion

        #region \SELECT ALL
        public IEnumerable<Vat> Get()
        {
            Command command = new Command("SELECT * FROM vat");

            return connection.ExecuteReader(command, r => new Vat().From(r));
        }
        #endregion

        #region \SELECT BY ID
        public Vat Get(int id)
        {
            Command command = new Command("SELECT * FROM vat where ID = @ID");
            command.AddParameter("ID", id);

            return connection.ExecuteReader(command, r => new Vat()
            {
                ID = (int)r["ID"],
                Code = (int)r["code"],
                VatRate = (double)r["vatRate"]
            }).FirstOrDefault();
        }
        #endregion

        #region \SELECT BY CODE
        [HttpGet]
        public Vat GetbyCode(string code)
        {
            Command command = new Command("SELECT * FROM vat where code = @code");
            command.AddParameter("code", code);
            return connection.ExecuteReader(command, r => new Vat().From(r)).FirstOrDefault();
        }
        #endregion

        #region \POST
        public Vat Post(Vat entity)
        {
            Command command = new Command($@"INSERT INTO vat
                    (code, vatRate)
                    OUTPUT Inserted.Id
                    values (@code, @vatRate)
                    ");
            command.AddParameter("code", entity.Code);
            command.AddParameter("vatRate", entity.VatRate);
            int newId = (int)connection.ExecuteScalar(command);
            entity.ID = newId;
            return entity;
        }
        #endregion

        #region \PUT
        public Vat Put(Vat entity)
        {
            Command command = new Command($@"UPDATE vat
                    set code = code, vatRate = @vatRate
                    where ID = @ID
                    ");
            command.AddParameter("code", entity.Code);
            command.AddParameter("vatRate", entity.VatRate);
            command.AddParameter("ID", entity.ID);
            connection.ExecuteNonQuery(command);
            return Get(entity.ID);
        }
        #endregion
    }
}
