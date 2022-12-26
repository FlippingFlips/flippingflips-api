using FF.Core.Interface;
using FF.Core.Models;
using FF.Shared.Model.Scores;
using FF.Shared.ViewModel.Scores;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FF.Core.Features.Scores.Query
{
    /// <summary>
    /// Gets scores for a given user. Used for returning to simulator
    /// </summary>
    public class GetScoresQuery : IRequest<ScoreSearchResult>
    {
        public GetScoresQuery(ScoresQueryDto scoresQueryDto)
        {
            ScoresQueryDto = scoresQueryDto;
        }

        public ScoresQueryDto ScoresQueryDto { get; }
    }

    internal class GetScoresQueryHandler : IRequestHandler<GetScoresQuery, ScoreSearchResult>
    {
        private readonly IRepository repository;

        public GetScoresQueryHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ScoreSearchResult> Handle(GetScoresQuery request, CancellationToken cancellationToken)
        {
            var scoreSearch = request.ScoresQueryDto;

            var result = new ScoreSearchResult();

            var scoreQuery = repository.Scores
                    .AsNoTracking()
                    .Include(p => p.Player)
                    .Include(gp => gp.GamePlayed).ThenInclude(p => p.Game)
                    .Include(gp => gp.GamePlayed.ApplicationUser);

            //build the actual query
            IQueryable<Score> query = null;
            if (!string.IsNullOrWhiteSpace(scoreSearch.GameId))
            {
                query = scoreQuery.Where(x => x.GamePlayed.GameId == scoreSearch.GameId);
            }
            //only include scores from this users machine ?
            if (scoreSearch.GetUsersScores)
                query = scoreQuery.Where(gp => gp.GamePlayed.ApplicationUserId == scoreSearch.UserId);

            if(scoreSearch.PlayerId > 0)
                query = scoreQuery.Where(gp => gp.PlayerId == scoreSearch.PlayerId);

            //PAGING
            //get totals for search
            result.Total = await query.CountAsync();
            //total pages
            result.PageTotal = (int)Math.Ceiling((decimal)result.Total / scoreSearch.Limit);
            //return page count
            result.Page = scoreSearch.Page >= result.PageTotal ? result.PageTotal : scoreSearch.Page;
            //skip count
            var skip = result.Page > 1 ? scoreSearch.Limit * (result.Page - 1) : 0;

            //sort ordering
            switch (scoreSearch.OrderBy)
            {
                case ScoresOrderBy.Date:
                    if (scoreSearch.SortOrder == Shared.Model.SortDirection.Descending)
                        query = query.OrderByDescending(x => x.GamePlayed.Ended);
                    else
                        query = query.OrderBy(x => x.GamePlayed.Ended);
                    break;
                case ScoresOrderBy.Points:
                    if (scoreSearch.SortOrder == Shared.Model.SortDirection.Descending)
                        query = query.OrderByDescending(x => x.Points);
                    else
                        query = query.OrderBy(x => x.Points);
                    break;
                default:
                    query = query.OrderByDescending(x => x.Points);
                    break;
            }

            //set limit to 10 scores
            scoreSearch.Limit = scoreSearch.Limit > 10 ? 10 : scoreSearch.Limit;

            result.Scores = await query
                .Skip(skip)
                .Take(scoreSearch.Limit)
                .Select(s => new ScoreResultVm()
                {
                    Id = s.Id,
                    Points = s.Points,
                    Initials = s.Player.Initials,
                    Name = s.Player.Name,
                    GameTime = s.GamePlayed.GameTime,
                    Ended = s.GamePlayed.Ended,
                    Balls = s.GamePlayed.BallsPerGame,
                    Title = s.GamePlayed.Game.Title,
                    FileName = s.GamePlayed.Game.FileName,
                    CRC = s.GamePlayed.CRC,
                    SimVersion = s.GamePlayed.SystemVersion,
                    Desktop = s.GamePlayed.Desktop,
                    Machine = s.GamePlayed.ApplicationUser.MachName,
                    UserName = s.GamePlayed.ApplicationUser.UserName,
                    GamePlayedId = s.GamePlayedId
                }).ToListAsync();

            return result;
        }

    }
}
