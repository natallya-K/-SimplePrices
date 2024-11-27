using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace SimplePrices.Helpers
{
    internal class AuthenticationFailureResult : IHttpActionResult
    {
        private string v;
        private HttpRequestMessage req;

        public AuthenticationFailureResult(string v, HttpRequestMessage req)
        {
            this.v = v;
            this.req = req;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(codeReturn());
        }

        private HttpResponseMessage codeReturn()
        {
            HttpResponseMessage reponse = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            reponse.RequestMessage = req;
            reponse.ReasonPhrase = v;
            return reponse;
        }
    }
}