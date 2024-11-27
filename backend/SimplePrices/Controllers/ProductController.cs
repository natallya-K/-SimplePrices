using SimplePrices.Models;
using SimplePrices.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SimplePrices.Controllers
{
    [EnableCors("*","*", "*")]
    public class ProductController : ApiController
    {
        private ProductRepository repo = new ProductRepository();

        public IEnumerable<Product> Get()
        {
            return repo.Get();
        }

        public Product Get(int id)
        {
            return repo.Get(id);
        }

        [Route("product/search/{number}")]
        public Product Get(string number)
        {
            return repo.GetbyNumber(number);
        }

        public Product Post(Product prod)
        {
            return repo.Post(prod);
        }

        public Product Put(Product prod)
        {
            return repo.Put(prod);
        }

        public bool Delete(int id)
        {
            return repo.Delete(id);
        }
    }
}