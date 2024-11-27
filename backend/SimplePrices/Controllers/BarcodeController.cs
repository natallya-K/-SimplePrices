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
    public class BarcodeController : ApiController
    {
        private BarcodeRepository repo = new BarcodeRepository();

        [AllowAnonymous]
        public IEnumerable<Barcode> Get()
        {
            return repo.Get();
        }

        public Barcode Get(int id)
        {
            return repo.Get(id);
        }

        [Route("barcode/search/{code}")]
        public Barcode Get(string code)
        {
            return repo.GetbyBarcode(code);
        }

        [Route("barcode/searchByPN/{productNumber}")]
        public IEnumerable<Barcode> GetByPN(string productNumber)
        {
            return repo.GetbyProductNumber(productNumber);
        }

        public Barcode Post(Barcode prod)
        {
            return repo.Post(prod);
        }

        public Barcode Put(Barcode prod)
        {
            return repo.Put(prod);
        }

        public bool Delete(int id)
        {
            return repo.Delete(id);
        }

    }
}
