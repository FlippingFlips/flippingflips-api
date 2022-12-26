using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using FF.Core.Features.Games.Query;
using FF.Core.Models.Dto.Games;
using FF.Api.Data.Model;
using FF.Core.Features.Games.Cmd;
using FF.Shared.Model;
using AutoMapper;
using FF.Api.Base;
using FF.Shared.ViewModel.Games;
using FF.Shared.Model.Games;

namespace FF.Api.Controllers
{
    [Authorize("ApiAndWebPolicy")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class GamesController : FlipsApiControllerBase
    {
        private readonly IMapper mapper;

        public GamesController(IMediator mediator = null, IMapper mapper = null) : base(mediator)
        {
            this.mapper = mapper;
        }

        // GET: /Games/byid
        [HttpGet]
        public async Task<IActionResult> ById(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var gQuery = new GetGameQuery(new Core.Features.Games.Query.GameDto { Id = id });
                GameResultVm game = await mediator.Send(gQuery, cancellationToken);
                return new JsonResult(game);
            }
            catch
            {
                return NotFound($"Couldn't find game for : {id}");
            }
        }

        // Post: /Games/CreateGame
        [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody] CreateGameVm game, CancellationToken cancellationToken = default)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var mapped = (CreateGameDto)mapper.Map(game, typeof(CreateGameVm), typeof(CreateGameDto));
                    var gameCmd = new CreateGameCmd(mapped);
                    var id = await mediator.Send(gameCmd, cancellationToken);
                    return Ok(id);
                }
                catch(Exception ex)
                {
                    return BadRequest($"{ex.Message}-{ex.InnerException?.Message}");
                }
            }

            return BadRequest("Invalid model");
        }

        // Post: /Games/SearchGames
        [HttpPost]
        public async Task<IActionResult> SearchGames([FromBody] GamesQueryDto gameSearch)
        {
            try
            {
                var gamesQuery = new GetGamesQuery(gameSearch);
                GamesQueryResult games = await mediator.Send(gamesQuery);
                return new JsonResult(games);
            }
            catch
            {
                return NotFound($"Error fetching games.");
            }
        }
    }
}
