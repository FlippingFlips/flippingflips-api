using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FF.Core.Extensions;
using FF.Core.Features.GamesPlayed.Query;
using FF.Api.Base;
using MediatR;
using FF.Core.Features.Scores.Cmd;
using FF.Api.Data.Model;
using FF.Core.Features.GamesPlayed.Cmd;
using FF.Shared.Model.Games;

namespace FlippingFlips.Blazor.Server.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize("ApiAndWebPolicy")]
    [ApiController]
    public class GamesPlayedController : FlipsApiControllerBase
    {        
        public GamesPlayedController(IMediator mediator = null) : base(mediator) { }

        /// <summary>
        /// Gets recent games played for a logged in user id
        /// </summary>
        /// <returns></returns>
        // GET: /GetAll
        [HttpPost]
        public async Task<IActionResult> GetAll(GamesPlayedQueryDto gamesPlayed)
        {
            try
            {
                gamesPlayed.UserId = User.Identity.GetUserId();                
                var query = new GetGamesPlayedQuery(gamesPlayed);
                var gamesPlayedResult = await mediator.Send(query);
                return Ok(gamesPlayedResult);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} {ex.InnerException?.Message}");
            }
        }

        /// <summary>
        /// Gets recent games played from a user name
        /// </summary>
        /// <returns></returns>
        // GET: /GamesPlayed/userName
        [HttpGet]
        public async Task<IActionResult> RecentByUserName(string userName)
        {
            try
            {
                var id = User.Identity.GetUserId();
                var query = new GetGamesPlayedQuery(new GamesPlayedQueryDto { UserName = userName});
                var gamesPlayedResult = await mediator.Send(query);
                return Ok(gamesPlayedResult);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} {ex.InnerException?.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ByIdAsync(long id)
        {
            try
            {
                var q = new GetGamePlayedByIdQuery(id);
                return Ok(await mediator.Send(q));
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} {ex.InnerException?.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetGamePlayedSettingsAsync(long gamePlayedId)
        {
            var q = new GetGamePlayedSettingsQuery(gamePlayedId);
            return Ok(await mediator.Send(q));
        }

        [Authorize(Roles = $"{Roles.Admin},{Roles.Manager},{Roles.User}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteGamesPlayedRequest deleteGames)
        {
            var userId = User?.Identity?.GetUserId() ?? null;
            if (userId == null)
                return BadRequest("Must be a logged in user");

            deleteGames.UserId = userId;
            deleteGames.IsAdmin = User.IsInRole(Roles.Admin);

            var cmd = new DeleteGamesPlayedCmd(deleteGames);
            var result = await mediator.Send(cmd);

            return Ok(result);
        }
    }
}
