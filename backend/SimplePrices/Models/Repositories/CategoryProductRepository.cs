using ReadConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ToolBox.DataAccess.DataBase;

namespace SimplePrices.Models.Repositories
{
    public class CategoryProductRepository
    {
        //charger la configuration
        // -----------------------
        static readonly GetConfig config = new GetConfig();
        static Connection connection = new Connection(config.ConnectionString);

        #region \SELECT ALL
        public IEnumerable<CategoryProduct> Get()
        {
            Command command = new Command($@"SELECT 
                    c.categoryCode as categoryCode, 
                    p.number as productNumber, 
                    p.name as productName 
                        from categoryProduct as c
                        join product as p on c.productNumber = p.number");

            return connection.ExecuteReader(command, r => new CategoryProduct().From(r));
        }
        #endregion

        #region \DELETE
        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region \SELECT BY CategoryCode
        //-----------------------------
        [HttpGet]
        public IEnumerable<CategoryProduct> GetbyCategoryCode(string categoryCode)
        {
            Command command = new Command($@"SELECT 
                    c.categoryCode as categoryCode, 
                    p.number as productNumber, 
                    p.name as productName 
                        from categoryProduct as c
                        join product as p on c.productNumber = p.number where categoryCode = @categoryCode");
            command.AddParameter("categoryCode", categoryCode);

            return connection.ExecuteReader(command, r => new CategoryProduct().From(r));
        }
        #endregion

        #region \POST
        //-----------
        public CategoryProduct Post(CategoryProduct entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region \PUT
        //-----------
        public CategoryProduct Put(CategoryProduct entity)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}