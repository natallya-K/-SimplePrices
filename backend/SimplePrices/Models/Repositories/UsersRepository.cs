using ReadConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToolBox.DataAccess.DataBase;

namespace SimplePrices.Models.Repositories
{
    public class UsersRepository
    {
        //charger la configuration
        // -----------------------
        static readonly GetConfig config = new GetConfig();
        static Connection connection = new Connection(config.ConnectionString);

        #region \POST
        public bool Post(Users entity)
        {
            Command command = new Command($@"select * from users where userName = @userName and psw = @psw");
            command.AddParameter("userName", entity.UserName);
            command.AddParameter("psw", entity.Psw);
            Users u = connection.ExecuteReader(command, r => new Users().From(r)).FirstOrDefault();
            return u == null ? false : true;
        }
        #endregion
    }
}