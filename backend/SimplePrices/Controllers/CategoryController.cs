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
    public class CategoryController : ApiController
    {
        private CategoryRepository repo = new CategoryRepository();

        public IEnumerable<Category> Get()
        {
            return repo.Get();
        }

        public Category Get(int id)
        {
            return repo.Get(id);
        }

        [Route("category/search/{code}")]
        public Category Get(string code)
        {
            return repo.GetbyCode(code);
        }

        public Category Post(Category cat)
        {
            return repo.Post(cat);
        }

        public Category Put(Category cat)
        {
            return repo.Put(cat);
        }

        public bool Delete(int id)
        {
            return repo.Delete(id);
        }
    }
}
