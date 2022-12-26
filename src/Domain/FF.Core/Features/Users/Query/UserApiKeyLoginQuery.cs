using FF.Core.Interface;
using FF.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FF.Core.Features.Users.Query
{
    /// <summary>
    /// Requests a user login from an API Key. Some requests can include their players or games in progress
    /// </summary>
    public class UserApiKeyLoginQuery : IRequest<FlipsUserResult>
    {
        public UserApiKeyLoginQuery(UserRequestApiLoginDto userRequestApi)
        {
            UserRequestApi = userRequestApi;
        }

        public UserRequestApiLoginDto UserRequestApi { get; }
    }

    internal class UserApiKeyLoginQueryHandler : IRequestHandler<UserApiKeyLoginQuery, FlipsUserResult>
    {
        private readonly IRepository repository;
        private readonly ILogger<UserApiKeyLoginQueryHandler> logger;

        public UserApiKeyLoginQueryHandler(IRepository repository, ILogger<UserApiKeyLoginQueryHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<FlipsUserResult> Handle(UserApiKeyLoginQuery request, CancellationToken cancellationToken)
        {
            var userResult = new FlipsUserResult();
            var userAgent = request.UserRequestApi.RequestHeader;
            var key = request.UserRequestApi.ApiKey;

            logger.LogInformation("user agent: " + userAgent);
            if (!request.UserRequestApi.IsDevEnvironment && userAgent != "FlippingFlips (compatible; HorsePinInc/3.6.9)")
            {
                userResult.Message = "Must be posted from COM controller";
                userResult.UserCheckResult = UserCheckResult.Bad;
                return userResult;
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                userResult.Message = "Please supply your api_key in the request";
                userResult.UserCheckResult = UserCheckResult.Bad;
                return userResult;
            }

            logger.LogInformation("Getting user from the ApiKey");
            IQueryable<ApplicationUser> query = repository.Users.AsQueryable();

            if (request.UserRequestApi.IncludeGamesInProgress) query = query.Include(x => x.GamesInProgress);
            if (request.UserRequestApi.IncludePlayers) query = query.Include(x => x.Players);

            var userMachine = await query.FirstOrDefaultAsync(x => x.ApiKey == key);
            if (userMachine == null)
            {
                userResult.Message = "This API key or user doesn't exist. Create an account to generate a key.";
                userResult.UserCheckResult = UserCheckResult.Bad;
                return userResult;
            }

            if (!userMachine.ApiOn)
            {
                userResult.Message = "API is disabled for this user";
                userResult.UserCheckResult = UserCheckResult.Bad;
                return userResult;
            }

            userResult.ApplicationUser = userMachine;
            return userResult;
        }
    }
}
