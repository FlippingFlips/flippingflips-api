using FF.Core.Interface;
using FF.Shared.ViewModel.Games;
using FF.Shared.ViewModel.Scores;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FF.Core.Features.Games.Query
{
    public class GetGameQuery : IRequest<GameResultVm>
    {
        public GameDto GameDto { get; }
        public GetGameQuery(GameDto gameDto)
        {
            GameDto = gameDto;
        }
    }

    public class GameDto
    {
        public string Id { get; set; }
    }

    class GetGameQueryHandler : IRequestHandler<GetGameQuery, GameResultVm>
    {
        private readonly IRepository repository;
        private readonly IFileService fileService;

        public GetGameQueryHandler(IRepository repository, IFileService fileService)
        {
            this.repository = repository;
            this.fileService = fileService;
        }

        public async Task<GameResultVm> Handle(GetGameQuery request, CancellationToken cancellationToken)
        {
            //ScoreCount = game.Scores.Count,
            //Translite = fileService.GetFileUrl($"games/{game.Id}/translite.jpg"), //TODO games translite
            var result = new GameResultVm();
            var gameReq = request.GameDto;

            //monster query
            var gameQuery = repository?.Games?.AsNoTracking()
            .Include(g => g.GamesPlayed).ThenInclude(u => u.ApplicationUser)
            .Include(g => g.GamesPlayed).ThenInclude(u => u.Scores).ThenInclude(p => p.Player)
            .Select(x => new GameResultVm()
            {
                Author = x.Author,
                CRC = x.CRC,
                CRCPatched = x.CRCPatched,
                Created = x.Created,
                Description = x.Description,
                FileUrl = x.FileUrl,
                FileName = x.FileName,
                FileNamePatched = x.FileNamePatched,
                FilePatchUrl = x.FilePatchUrl,
                Id = x.Id,
                IsDeleted = x.IsDeleted,
                IPDB = x.IPDB,
                Title = x.Title,
                Version = x.Version,
                Year = x.Year,
                Translite = fileService.GetFileUrl($"games/{gameReq.Id}/translite.jpg"),
                TotalGamesPlayed = x.GamesPlayed.Count,
                ScoreCount = x.GamesPlayed.Select(x => x.Scores.Count).Sum(),
            });

            var gResult = await gameQuery.FirstOrDefaultAsync(x => x.Id == gameReq.Id, cancellationToken: cancellationToken);

            return gResult;
        }
    }
}
