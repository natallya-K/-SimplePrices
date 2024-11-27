using ReadConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ToolBox.DataAccess.DataBase;

namespace SimplePrices.Models.Repositories
{
    public class ProductStatisticRepository
    {
        //charger la configuration
        // -----------------------
        static readonly GetConfig config = new GetConfig();
        static Connection connection = new Connection(config.ConnectionString);

        #region \SELECT ALL
        //-----------------
        public IEnumerable<ProductStatistic> Get()
        {
            Command command = new Command("SELECT * FROM productStatistic");

            return connection.ExecuteReader(command, r => new ProductStatistic().From(r));
        }
        #endregion

        #region \SELECT Turnover
        //-----------------------------

        [HttpGet]
        public IEnumerable<ProductStatistic> GetTurnover(DateTime fromDate, DateTime toDate)
        {
            Command command = new Command($@"SELECT date, 
                    SUM(quantity) as quantity, SUM(samt) as samt, SUM(ramt) as ramt, 
                    SUM(tamt) as tamt, SUM(damt) as damt, SUM(sexc) as sexc, SUM(rexc) as rexc, 
                    SUM(exc) as exc, SUM(svat) as svat, SUM(rvat) as rvat, SUM(vat) as vat, SUM(reg) as reg 
                    FROM productStatistic 
                    WHERE date BETWEEN @fromDate AND @toDate 
                    GROUP BY date 
                    ORDER BY date ASC");

            command.AddParameter("fromDate", fromDate);
            command.AddParameter("toDate", toDate);

            return connection.ExecuteReader(command, r => new ProductStatistic().FromTurnover(r));
        }
        #endregion
    }
}