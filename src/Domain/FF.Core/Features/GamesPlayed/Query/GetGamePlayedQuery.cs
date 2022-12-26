using FF.Core.Interface;
using FF.Core.Models;
using FF.Shared.Model.Games;
using FF.Shared.ViewModel.Games;
using FF.Shared.ViewModel.Scores;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FF.Core.Features.GamesPlayed.Query
{
    public class GetGamePlayedByIdQuery : IRequest<GamePlayedVm>
    {
        public GetGamePlayedByIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    internal class GetGamePlayedByIdQueryHandler : IRequestHandler<GetGamePlayedByIdQuery, GamePlayedVm>
    {
        private readonly IRepository repository;
        private readonly IFileService fileService;

        public GetGamePlayedByIdQueryHandler(IRepository repository, IFileService fileService)
        {
            this.repository = repository;
            this.fileService = fileService;
        }

        public async Task<GamePlayedVm> Handle(GetGamePlayedByIdQuery request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            IQueryable<GamePlayed> scoreQuery = repository.GamesPlayed
                .AsNoTracking()
                .Include(u => u.ApplicationUser)
                .Include(x => x.Game).ThenInclude(r => r.PinmameRom)
                .Include(s => s.Scores)
                .ThenInclude(p => p.Player);

            var result = new GamesPlayedResult();
            var gamePlayed = await scoreQuery
                .Select(x => new GamePlayedVm
                {
                    Id = x.Id,
                    GameId = x.GameId,
                    Title = x.Game.Title,
                    FileName = x.Game.FileNamePatched,
                    CRC = x.Game.CRC,
                    GamePlayedCRC = x.CRC,
                    SystemVersion = x.SystemVersion,
                    Desktop = x.Desktop,
                    Players = x.Scores.Count,
                    BallsPerGame = x.BallsPerGame,
                    SystemType = x.Game.GameSystem,
                    GameType = x.Game.GameType,
                    GameTime = x.GameTime,
                    Posted = x.Ended,
                    GameVersion = x.Game.Version,
                    Author = x.Game.Author,
                    Rom = x.Game.PinmameRom.Id,
                    User = x.ApplicationUser.UserName,
                    MachineName = x.ApplicationUser.MachName,
                    Scores = x.Scores.OrderByDescending(p => p.Points)
                        .Select(s => new ScoreResultSlimVm
                        {
                            Id = s.Id,
                            PlayerId = s.PlayerId,
                            Points = s.Points,
                            Name = s.Player.Name,
                            Initials = s.Player.Initials
                        })
                }).FirstOrDefaultAsync(x => x.Id == id);

            if (gamePlayed != null)
            {
                var gameId = gamePlayed.GameId;
                var topscoresQuery = repository.Scores
                    .Include(p => p.Player)
                    .Include(g => g.GamePlayed).ThenInclude(gp => gp.Game);

                var topscores = await topscoresQuery
                    .Where(x => x.GamePlayed.GameId == gameId)
                    .OrderByDescending(p => p.Points)
                    .Select(x => new ScoreResultSlimVm
                    {
                        Id = x.Id,
                        PlayerId = x.PlayerId,
                        Points = x.Points,
                        Name = x.Player.Name,
                        Initials = x.Player.Initials
                    }).Take(5).ToListAsync();

                gamePlayed.Translite = fileService.GetFileUrl($"/games/{gameId}/translite.jpg");
                gamePlayed.TopScores = topscores;
            }

            return gamePlayed;
        }
    }
}
