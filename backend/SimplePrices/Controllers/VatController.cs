using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SimplePrices.Models.Repositories;
using SimplePrices.Models;
using System.Web.Http.Cors;
using SimplePrices.Helpers;

namespace SimplePrices.Controllers
{
    [Authorize(Roles ="ADMIN")]
    [EnableCors("*", "*", "*")]
    public class VatController : ApiController
    {
        private VatRepository repo = new VatRepository();

        [AllowAnonymous]
        //[BasicAuthenticator(realm: "SimplePrices")]
        public IEnumerable<Vat> Get()
        {
            return repo.Get();
        }


        [AllowAnonymous]
        //[BasicAuthenticator(realm: "SimplePrices")]
        public Vat Get(int id)
        {
            return repo.Get(id);
        }


        [AllowAnonymous]
        //[BasicAuthenticator(realm: "SimplePrices")]
        [Route("vat/search/{code}")]
        public Vat Get(string code)
        {
            return repo.GetbyCode(code);
        }

        [AllowAnonymous]
        //[BasicAuthenticator(realm:"SimplePrices")]
        public Vat Post(Vat prod)
        {
            return repo.Post(prod);
        }

        [AllowAnonymous]
        //[BasicAuthenticator(realm: "SimplePrices")]
        public Vat Put(Vat prod)
        {
            return repo.Put(prod);
        }

        [AllowAnonymous]
        //[BasicAuthenticator(realm: "SimplePrices")]
        public bool Delete(int id)
        {
            return repo.Delete(id);
        }
    }
}