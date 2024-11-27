using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace SimplePrices.Helpers
{
    internal class ResultChallenge : IHttpActionResult
    {
        private IHttpActionResult result;
        private string realm;

        public ResultChallenge(IHttpActionResult result, string realm)
        {
            this.result = result;
            this.realm = realm;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var res = await result.ExecuteAsync(cancellationToken);
            if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                res.Headers.WwwAuthenticate.Add(new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", realm));
            }
            return res;
        }
    }
}