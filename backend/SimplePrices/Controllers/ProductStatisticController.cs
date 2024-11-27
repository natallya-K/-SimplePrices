using SimplePrices.Models;
using SimplePrices.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SimplePrices.Controllers
{
    [EnableCors("*", "*", "*")]

    public class ProductStatisticController : ApiController
    {
        private ProductStatisticRepository repo = new ProductStatisticRepository();

        public IEnumerable<ProductStatistic> Get()
        {
            return repo.Get();
        }

        [Route("turnover/{fromDate}/{toDate}")]
        public IEnumerable<ProductStatistic> GetTurnover(DateTime fromDate, DateTime toDate)
        {
            return repo.GetTurnover(fromDate, toDate);
        }

    }
}
