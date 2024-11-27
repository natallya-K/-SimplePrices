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
    public class CategoryRepository : IRepository<Category, int>
    {
        static readonly GetConfig config = new GetConfig();
        static Connection connection = new Connection(config.ConnectionString);

        #region \SELECT ALL
        public IEnumerable<Category> Get()
        {
            Command command = new Command("SELECT * FROM category");

            return connection.ExecuteReader(command, r => new Category().From(r));
        }
        #endregion

        #region \SELECT BY ID
        public Category Get(int id)
        {
            Command command = new Command("SELECT * FROM category where ID = @ID");
            command.AddParameter("ID", id);
            return connection.ExecuteReader(command, r => new Category().From(r)).FirstOrDefault();
        }
        #endregion

        #region \SELECT BY CODE
        [HttpGet]
        public Category GetbyCode(string code)
        {
            Command command = new Command("SELECT * FROM category where code = @code");
            command.AddParameter("code", code);
            return connection.ExecuteReader(command, r => new Category().From(r)).FirstOrDefault();
        }
        #endregion

        #region \POST
        public Category Post(Category entity)
        {
            Command command = new Command($@"INSERT INTO category
                    (code, name)
                    OUTPUT Inserted.Id
                    values (@code, @name)
                    ");
            command.AddParameter("code", entity.Code);
            command.AddParameter("name", entity.Name);
            int newId = (int)connection.ExecuteScalar(command);
            entity.ID = newId;
            return entity;
        }
        #endregion

        #region \PUT
        public Category Put(Category entity)
        {
            Command command = new Command($@"update category
                    set name = @name, code = @code
                    where ID = @ID
                    ");
            command.AddParameter("code", entity.Code);
            command.AddParameter("name", entity.Name);
            command.AddParameter("ID", entity.ID);
            connection.ExecuteNonQuery(command);
            return Get(entity.ID);
        }
        #endregion

        #region \DELETE
        public bool Delete(int id)
        {
            Command command = new Command("DELETE FROM category WHERE ID=@ID");
            command.AddParameter("ID", id);
            return (connection.ExecuteNonQuery(command) > 0) ? true : false;
        }
        #endregion
    }
}