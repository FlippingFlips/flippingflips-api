using FF.Core.Interface;
using FF.Core.Models;
using FF.Shared.Model.Games;
using FF.Shared.ViewModel.Games;
using FF.Shared.ViewModel.Scores;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FF.Core.Features.GamesPlayed.Query
{
    public class GetGamesPlayedQuery : IRequest<GamesPlayedResult>
    {
        public GetGamesPlayedQuery(GamesPlayedQueryDto playedDto)
        {
            PlayedDto = playedDto;
        }

        public GamesPlayedQueryDto PlayedDto { get; }
    }

    internal class GetGamesPlayedQueryHandler : IRequestHandler<GetGamesPlayedQuery, GamesPlayedResult>
    {
        private readonly IRepository repository;

        public GetGamesPlayedQueryHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<GamesPlayedResult> Handle(GetGamesPlayedQuery request, CancellationToken cancellationToken)
        {
            var dto = request.PlayedDto;
            var result = new GamesPlayedResult();

            IQueryable<GamePlayed> scoreQuery = repository.GamesPlayed
                .AsNoTracking()
                .Include(u => u.ApplicationUser)
                .Include(x => x.Game)
                .Include(s => s.Scores).ThenInclude(p => p.Player);

            //by user id
            if (!string.IsNullOrWhiteSpace(dto.UserId) && dto.GetUsersGames)
            {
                scoreQuery = scoreQuery.Where(x => x.ApplicationUserId == dto.UserId);
            }
            //by user name
            else if (!string.IsNullOrWhiteSpace(dto.UserName))
            {
                scoreQuery = scoreQuery.Where(x => EF.Functions.Like(x.ApplicationUser.UserName, $"%{dto.UserName}%"));
            }

            if (!string.IsNullOrWhiteSpace(dto.GameId))
                scoreQuery = scoreQuery.Where(x => x.GameId == dto.GameId);

            if (!string.IsNullOrWhiteSpace(dto.Title))
                scoreQuery = scoreQuery.Where(x => EF.Functions.Like(x.Game.Title, $"%{dto.Title}%"));

            //total games for this query
            result.TotalGamesPlayed = await scoreQuery.CountAsync();
            //total pages
            result.PageTotal = (int)Math.Ceiling((decimal)result.TotalGamesPlayed / dto.Limit);
            //return page count
            result.Page = dto.Page >= result.PageTotal ? result.PageTotal : dto.Page;
            //skip count
            var skip = result.Page > 1 ? dto.Limit * (result.Page-1) : 0;

            result.GamesPlayed = await scoreQuery
                .OrderByDescending(x => x.Ended)
                .Skip(skip)
                .Take(dto.Limit)
                .Select(x => new GamePlayedSlimVm
                {
                    Id = x.Id,
                    GameId = x.GameId,
                    Title = x.Game.Title,
                    FileName = x.Game.FileNamePatched,
                    BallsPerGame = x.BallsPerGame,
                    CRC = x.Game.CRC,
                    GamePlayedCRC = x.CRC,
                    SystemType = x.Game.GameSystem,
                    GameType = x.Game.GameType,
                    GameTime = x.GameTime,
                    Desktop = x.Desktop,
                    GameVersion = x.Game.Version,
                    Posted = x.Ended,
                    Players = x.Scores.Count,
                    Scores = x.Scores.OrderByDescending(p => p.Points)
                        .Select(s => new ScoreResultSlimVm
                        {
                            Id = s.Id,
                            Points = s.Points,
                            Initials = s.Player.Initials
                        }),
                    User = x.ApplicationUser.UserName
                }).ToListAsync();

            return result;
        }
    }
}
