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

    public class CategoryProductController : ApiController
    {
        private CategoryProductRepository repo = new CategoryProductRepository();

        public IEnumerable<CategoryProduct> Get()
        {
            return repo.Get();
        }

        [Route("categoryProduct/search/{code}")]
        public IEnumerable<CategoryProduct> Get(string code)
        {
            return repo.GetbyCategoryCode(code);
        }

        public CategoryProduct Post(CategoryProduct catProd)
        {
            return repo.Post(catProd);
        }

        public CategoryProduct Put(CategoryProduct catProd)
        {
            return repo.Put(catProd);
        }

        public bool Delete(int id)
        {
            return repo.Delete(id);
        }
    }
}
