using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FF.Core.Extensions;
using FF.Core.Interface;
using FF.Core.Features.Scores.Cmd;
using MediatR;
using FF.Api.Base;
using FF.Api.Data.Model;
using FF.Core.Features.Scores.Query;
using FF.Shared.Model.Scores;

namespace FlippingFlips.Blazor.Server.Controllers
{
    /// <summary>
    /// Need to explicitly set user roles here otherwise no roles come through when trying to use IsInRole (for JWT anyway) 
    /// </summary>
    [Authorize("ApiAndWebPolicy", Roles = $"{Roles.Admin},{Roles.Manager},{Roles.User}")]
    [Route("[controller]")]
    [ApiController]
    public class ScoresController : FlipsApiControllerBase
    {
        public ScoresController(IMediator mediator) : base(mediator) { }

        // POST: /Scores
        [HttpPost]
        public async Task<IActionResult> PostScores(ScoresQueryDto scoresQuery)
        {
            var id = User.Identity.GetUserId();
            scoresQuery.UserId = id;
            try
            {
                var q = new GetScoresQuery(scoresQuery);
                return Ok(await mediator.Send(q));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteScores([FromBody] DeleteScoreRequest deleteScore)
        {            
            var userId = User?.Identity?.GetUserId() ?? null;
            if (userId == null)
                return BadRequest("Must be a logged in user");

            deleteScore.UserId = userId;
            deleteScore.IsAdmin = User.IsInRole(Roles.Admin);

            var cmd = new DeleteScoresCmd(deleteScore);
            var result = await mediator.Send(cmd);

            return Ok(result);
        }
    }
}
