using ReadConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using ToolBox.DataAccess.Crypto;
using ToolBox.DataAccess.DataBase;

namespace SimplePrices.Helpers
{
    public class BasicAuthenticator : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple => false;
        private readonly string realm;

        static readonly GetConfig config = new GetConfig();
        static Connection connection = new Connection(config.ConnectionString);

        #region
        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            // context : requete venant du front
            // but ici = définir un objet "principale" qui représente l'utilisateur connecté

            HttpRequestMessage req = context.Request;

            if (req.Headers.Authorization != null)
            {
                if (req.Headers.Authorization.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
                {
                    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                    string credentials = encoding.GetString(Convert.FromBase64String(req.Headers.Authorization.Parameter));
                    string[] parts = credentials.Split(':');
                    string userName = parts[0].Trim();
                    string password = parts[1].Trim();
                    //string password = CryptingPassword.HashSHA1Decryption(parts[1].Trim());

                    Command command = new Command("select role from  users where userName = @userName and psw = @psw");
                    command.AddParameter("userName", userName);
                    command.AddParameter("psw", password);

                    string role = (string)connection.ExecuteScalar(command);

                    if (role == "ADMIN")
                    {
                        var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name, userName),
                            new Claim(ClaimTypes.Role, role)
                        };

                        var identity = new ClaimsIdentity(claims, "Basic");

                        var principal = new ClaimsPrincipal(new[] { identity });

                        // définition de l'identité de l'utilisateur dans le système
                        // ---------------------------------------------------------

                        context.Principal = principal;
                    }
                    else
                    {
                        context.ErrorResult = new AuthenticationFailureResult("You cannot access this resource !", req);
                    }

                }
                else
                {
                    context.ErrorResult = new AuthenticationFailureResult("Your Authentication Scheme is not Basic !", req);
                }
            }
            else
            {
                context.ErrorResult = new AuthenticationFailureResult("You must authenticate first !", req);
            }

            return Task.FromResult(0); // indiquer que la requete est terminé

        }
        #endregion

        #region

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            context.Result = new ResultChallenge(context.Result, realm);
            return Task.FromResult(0);
        }
        #endregion

        public BasicAuthenticator(string realm)
        {
            this.realm = "realm=" + realm;
        }
    }
}