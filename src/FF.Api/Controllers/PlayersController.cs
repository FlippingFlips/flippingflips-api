using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using FF.Infrastructure.Data;
using FF.Core.Models;
using FF.Api.Base;
using MediatR;
using FF.Api.Data.Model;
using FF.Core.Features.Players.Cmd;
using FF.Core.Extensions;
using FF.Domain.Models;

namespace FF.Api.Controllers
{
    [Authorize("ApiAndWebPolicy", Roles = $"{Roles.Admin},{Roles.Manager},{Roles.User}")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class PlayersController : FlipsApiControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlayersController(ApplicationDbContext context, IMediator mediator) : base(mediator)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayerDetails(int id)
        {
            //TODO: move to query
            //have to do multiple queries here because SQlite fails.
            //Note: Translating this query requires the SQL APPLY operation, which is not supported on SQLite.
            var player = await _context.Players
                .AsNoTracking().Include(c=> c.ApplicationUser)
                .Select(x=> new
                {
                    x.Id, x.Initials, x.Name, x.MachineDefault, x.ApplicationUser.UserName, x.ApplicationUser.MachName
                })
                .FirstOrDefaultAsync(x => x.Id == id);
            if (player != null)
            {
                var scoreCount = await _context.Scores.CountAsync(x => x.PlayerId == id);

                //get last 10 games played. TODO: Changed to paged games
                var scores = await _context.Scores
                    .AsNoTracking()
                    .Include(g => g.GamePlayed).ThenInclude(gp => gp.Game)
                    .OrderByDescending(s => s.GamePlayed.Ended)
                    .Where(x => x.PlayerId == id)                    
                    .Select(x=> new
                    {
                       x.GamePlayed.Id, x.GamePlayed.GameId, x.GamePlayed.Game.Title, x.Points, x.GamePlayed.Ended,
                    })
                    .Take(10)
                    .ToListAsync();

                return new JsonResult(new { player, TotalGamesPlayed = scoreCount, GamesPlayed = scores });
            }
            else
            {
                return NotFound("player not found");
            }
        }        

        // PUT: Players/5
        //[Authorize("ApiAndWebPolicy", Roles = "Admin")]
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutPlayer(int id, Player player)
        //{
        //    if (id != player.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(player).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PlayerExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromBody] DeletePlayersRequest deletePlayers)
        {
            var userId = User?.Identity?.GetUserId() ?? null;
            if (userId == null)
                return BadRequest("Must be a logged in user");

            deletePlayers.UserId = userId;
            deletePlayers.IsAdmin = User.IsInRole(Roles.Admin);

            try
            {
                var cmd = new DeletePlayersCmd(deletePlayers);
                var result = await mediator.Send(cmd);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} {ex.InnerException?.Message}");
            }
        }

        // POST: Players
        [Authorize("ApiAndWebPolicy", Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(CreatePlayerOption player)
        {
            try
            {
                var cmd = new CreatePlayerCmd(player.UserId, player);
                var result = await mediator.Send(cmd);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} {ex.InnerException?.Message}");
            }
            //return CreatedAtAction("GetPlayer", new { id = player.Id }, player);
        }
    }
}
