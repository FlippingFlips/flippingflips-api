using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;

namespace FF.Infrastructure.RateLimit
{
    public class ClientQueryStringResolveContributor : IClientResolveContributor
    {
        private IHttpContextAccessor httpContextAccessor;
        private readonly string clientIdHeader;

        public ClientQueryStringResolveContributor(IHttpContextAccessor httpContextAccessor, string clientIdHeader)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.clientIdHeader = clientIdHeader;
        }

        public async Task<string> ResolveClientAsync(HttpContext httpContext)
        {
            var request = httpContextAccessor.HttpContext?.Request;
            var queryDictionary =
                Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(
                    request.QueryString.ToString());
            if (queryDictionary.ContainsKey("api_key")
                && !string.IsNullOrWhiteSpace(queryDictionary["api_key"]))
            {
                return await Task.FromResult(queryDictionary["api_key"]);
            }

            return Guid.NewGuid().ToString();
        }
    }
}
