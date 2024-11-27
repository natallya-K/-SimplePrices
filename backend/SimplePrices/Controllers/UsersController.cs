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

    public class UsersController : ApiController
    {
        private UsersRepository repo = new UsersRepository();

        [Route("dashboard")]
        [EnableCors("*", "*", "*")]
        public bool PostUserExist(Users user)
        {
            return this.repo.Post(user);
        }
    }
}