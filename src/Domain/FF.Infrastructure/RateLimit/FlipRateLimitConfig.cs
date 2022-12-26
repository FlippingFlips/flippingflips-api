using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace FF.Infrastructure.RateLimit
{
    public class FlipRateLimitConfig : RateLimitConfiguration
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public FlipRateLimitConfig(IOptions<IpRateLimitOptions> ipOptions,
            IHttpContextAccessor httpContextAccessor,
            IOptions<ClientRateLimitOptions> clientOptions) : base(ipOptions, clientOptions)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public override void RegisterResolvers()
        {
            base.RegisterResolvers();
            if (!string.IsNullOrEmpty(ClientRateLimitOptions?.ClientIdHeader))
            {
                ClientResolvers.Add(new ClientQueryStringResolveContributor(httpContextAccessor, ClientRateLimitOptions.ClientIdHeader));
            }
        }
    }
}
