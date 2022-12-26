using FF.Api.Base;
using FF.Core.Features.Games.Query;
using FF.Core.Features.GamesInProgress.Cmd;
using FF.Core.Features.GamesPlayed.Cmd;
using FF.Core.Features.Players.Cmd;
using FF.Core.Features.Scores.Query;
using FF.Core.Features.Users;
using FF.Core.Features.Users.Query;
using FF.Domain.Models;
using FF.Shared.ViewModel.Games;
using FF.Shared.ViewModel.Scores;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlippingFlips.Blazor.Server.Controllers
{
    /// <summary>
    /// Users with API key can use these methods
    /// </summary>
    [Produces("application/json")]
    [Route("[controller]/[action]")]
    [ApiController]
    public partial class UserMachineController : FlipsApiControllerBase
    {
        private readonly ILogger<UserMachineController> logger;
        protected readonly IWebHostEnvironment hostingEnvironment;

        public UserMachineController(ILogger<UserMachineController> logger,
            IWebHostEnvironment hostingEnvironment, IMediator mediator) : base(mediator)
        {
            this.logger = logger;
            this.hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Submit score for a game in progress
        /// </summary>
        /// <param name="playedOption"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddGamePlayed(string api_key, [FromBody] GamePlayedDto playedOption)
        {
            //user checks
            var userCheckResult = await RunUserCheck(api_key);
            if (userCheckResult.UserCheckResult > 0)
            {
                switch (userCheckResult.UserCheckResult)
                {
                    case UserCheckResult.Bad:
                        return BadRequest(userCheckResult.Message);
                    case UserCheckResult.Forbid:
                        return Forbid(userCheckResult.Message);
                }
            }

            try
            {
                var cmd = new AddGamePlayedCmd(playedOption);
                var result = await mediator.Send(cmd);
                if (result.Errored)
                    return BadRequest(result.Message);
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} {ex.InnerException?.Message}");
            }           
        }

        /// <summary>
        /// Creates a player for the given ApiKeys machine. Initials must be unique for the machine
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreatePlayer(string api_key, [FromBody] CreatePlayerOption player)
        {
            //user checks, include players
            FlipsUserResult userCheckResult = await RunUserCheck(api_key, true);
            if (userCheckResult.UserCheckResult > 0)
            {
                //failed login redirect
                switch (userCheckResult.UserCheckResult)
                {
                    case UserCheckResult.Bad:
                        return BadRequest(userCheckResult.Message);
                    case UserCheckResult.Forbid:
                        return Forbid(userCheckResult.Message);
                }
            }

            //check if this users machine has a player under the same initials
            if (userCheckResult.ApplicationUser.Players.Any(x => x.Initials == player.Initials))
                return BadRequest($"Player already exists for this machine with the initials {player.Initials}");

            //check if machine is above the player limit
            var playerCnt = userCheckResult.ApplicationUser.Players.Count;
            if (playerCnt >= userCheckResult.ApplicationUser.PlayersPerCabinet)
            {
                return BadRequest($"Maximum of {userCheckResult.ApplicationUser.PlayersPerCabinet} players allowed for this machine");
            }

            try
            {
                //create new player under this users id
                player.IsDefault = playerCnt == 0;
                var cmd = new CreatePlayerCmd(userCheckResult.ApplicationUser.Id, player);
                var pId = await mediator.Send(cmd);

                return Ok($"Player created: {pId}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [ProducesResponseType(typeof(GameResultVm), StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> GameInformation(string api_key, [FromBody] GameInfoOption gameInfo)
        {
            //user checks
            FlipsUserResult userCheckResult = await RunUserCheck(api_key);
            if (userCheckResult.UserCheckResult > 0)
            {
                //failed login redirect
                switch (userCheckResult.UserCheckResult)
                {
                    case UserCheckResult.Bad:
                        return BadRequest(userCheckResult.Message);
                    case UserCheckResult.Forbid:
                        return Forbid(userCheckResult.Message);
                }
            }

            try
            {
                var q = new GetGameQuery(new GameDto { Id = gameInfo.GameId });
                var game = await mediator.Send(q);
                if (game != null)
                {
                    return Ok(game);
                }
                else
                {
                    return BadRequest("no game found");
                }
            }
            catch (ArgumentNullException argEx)
            {
                return BadRequest("arg null: " + argEx.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("no game found:" + ex.Message);
            }
        }

        /// <summary>
        /// Used for returning the scores for simulator
        /// </summary>
        /// <param name="scoreSearch"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ScoreResultUserMachineVm), StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> GetScores(string api_key, [FromBody] ScoreSearch scoreSearch)
        {
            try
            {
                //check users api_key
                FlipsUserResult userCheckResult = await RunUserCheck(api_key);
                if (userCheckResult.UserCheckResult > 0)
                {
                    //failed login redirect
                    switch (userCheckResult.UserCheckResult)
                    {
                        case UserCheckResult.Bad:
                            return BadRequest(userCheckResult.Message);
                        case UserCheckResult.Forbid:
                            return Forbid(userCheckResult.Message);
                    }
                }

                //user good, return scores
                scoreSearch.UserId = userCheckResult.ApplicationUser.Id;
                var query = new GetScoresQuery(new FF.Shared.Model.Scores.ScoresQueryDto { UserId = scoreSearch.UserId});
                var results = await mediator.Send(query);
                return Ok(results);
            }
            catch (Exception ex)
            {
                logger.LogError("get score error {}", ex.ToString());
                return BadRequest($"An error occurred getting scores. {ex.Message}");
            }
        }

        /// <summary>
        /// Gets all the players linked to the ApiKey
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Players(string api_key)
        {
            try
            {
                //check users api_key, return players with request
                FlipsUserResult userCheckResult = await RunUserCheck(api_key, true);
                if (userCheckResult.UserCheckResult > 0)
                {
                    //failed login redirect
                    switch (userCheckResult.UserCheckResult)
                    {
                        case UserCheckResult.Bad:
                            return BadRequest(userCheckResult.Message);
                        case UserCheckResult.Forbid:
                            return Forbid(userCheckResult.Message);
                    }
                }

                return Ok(userCheckResult.ApplicationUser.Players?
                    .Select(x => new { x.Id, x.Initials, x.Name }));
            }
            catch (Exception ex)
            {
                logger.LogError("get score error {}", ex.ToString());
                return BadRequest($"An error occurred getting players. {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a game in progress from a players apikey
        /// </summary>
        /// <param name="gameOption"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> StartGame(string api_key, [FromBody] StartGameOption gameOption)
        {
            logger.LogInformation("start game called");

            //user checks, include players and in progress
            var userCheckResult = await RunUserCheck(api_key);
            if (userCheckResult.UserCheckResult > 0)
            {
                switch (userCheckResult.UserCheckResult)
                {
                    case UserCheckResult.Bad:
                        return BadRequest(userCheckResult.Message);
                    case UserCheckResult.Forbid:
                        return BadRequest(userCheckResult.Message);
                }
            }

            //game checks
            if (string.IsNullOrWhiteSpace(gameOption.GameId)) 
                return BadRequest("No game id was supplied with request");

            try
            {
                var cmd = new StartNewGameCmd(userCheckResult.ApplicationUser.Id, gameOption);
                var gamePId = await mediator.Send(cmd);
                return Ok(gamePId);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}-{ex.InnerException?.Message}");
            }
        }

        #region Support Methods
        /// <summary>
        /// Helper to return if the user has access to the API with their key
        /// </summary>
        /// <param name="api_key"></param>
        /// <param name="includePlayers"></param>
        /// <param name="includeGamesInProgress"></param>
        /// <returns></returns>
        private async Task<FlipsUserResult> RunUserCheck(string api_key, bool includePlayers = false, bool includeGamesInProgress = false)
        {
            var userReq = new UserRequestApiLoginDto()
            {
                ApiKey = api_key,
                RequestHeader = Request.Headers["User-Agent"][0],
                IsDevEnvironment = hostingEnvironment.IsDevelopment(),
                IncludePlayers = includePlayers,
                IncludeGamesInProgress = includeGamesInProgress
            };
            var userQuery = new UserApiKeyLoginQuery(userReq);
            var userCheckResult = await mediator.Send(userQuery);
            return userCheckResult;
        }
        #endregion
    }
}
