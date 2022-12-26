using FF.Core.Interface;
using FF.Core.Models;
using FF.Shared.Model.Games;
using FF.Shared.ViewModel.Games;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FF.Core.Features.Games.Query
{
    public class GetGamesQuery : IRequest<GamesQueryResult>
    {
        public GetGamesQuery(GamesQueryDto gameQuery)
        {
            GameQuery = gameQuery;
        }

        public GamesQueryDto GameQuery { get; }
    }

    internal class GetGamesQueryHandler : IRequestHandler<GetGamesQuery, GamesQueryResult>
    {
        private readonly IRepository repository;
        private readonly IFileService fileService;

        public GetGamesQueryHandler(IRepository repository, IFileService fileService)
        {
            this.repository = repository;
            this.fileService = fileService;
        }

        async Task<GamesQueryResult> IRequestHandler<GetGamesQuery, GamesQueryResult>.Handle(GetGamesQuery request, CancellationToken cancellationToken)
        {
            var dto = request.GameQuery;
            var result = new GamesQueryResult();

            //build query include games played
            Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Game, ICollection<Score>> query = null;
            
            //include other props
            query = repository.Games.AsNoTracking()
                .Include(x => x.GamesPlayed)
                .ThenInclude(s => s.Scores);

            //search by title, todo: Like syntax
            if (!string.IsNullOrWhiteSpace(dto.Title))
            {
                query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Game, ICollection<Score>>)query.Where(x => x.Title.Contains(dto.Title));
            }
            if (!string.IsNullOrWhiteSpace(dto.Author))
            {
                query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Game, ICollection<Score>>)query.Where(x => x.Author.Contains(dto.Title));
            }

            //total games for this query
            result.Total = await query.CountAsync();            
            //total pages
            result.PageTotal = (int)Math.Ceiling((decimal)result.Total / dto.Limit);
            //return page count
            result.Page = dto.Page >= result.PageTotal ? result.PageTotal : dto.Page;
            //skip count
            var skip = result.Page > 1 ? dto.Limit * (result.Page - 1) : 0;

            result.Games = await query.Select(x => new GameResultBaseVm()
            {
                Author = x.Author,
                Title = x.Title,
                Id = x.Id,
                Version = x.Version,
                Year = x.Year,
                GamesPlayed = x.GamesPlayed.Count,
                FileName = x.FileName,
                GameSystem = x.GameSystem,
                FileUrl = x.FileUrl,
                Created = x.Created,
                GameType = x.GameType,
                TotalScores = x.GamesPlayed.Select(s=>s.Scores.Count).Sum()
            }).Take(dto.Limit).ToArrayAsync();

            return result;
        }
    }
}
