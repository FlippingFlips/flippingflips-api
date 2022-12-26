using FF.Core.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FF.Core.Features.Players.Cmd
{
    /// <summary>
    /// Deletes players, all of their scores and any games played. Leaves games played intact if more than one player for the game
    /// </summary>
    public class DeletePlayersCmd : IRequest<PlayersDeletedResult>
    {
        public DeletePlayersCmd(DeletePlayersRequest deletePlayers)
        {
            DeletePlayers = deletePlayers;
        }

        public DeletePlayersRequest DeletePlayers { get; }
    }

    internal class DeletePlayersCmdHandler : IRequestHandler<DeletePlayersCmd, PlayersDeletedResult>
    {
        private readonly IRepository repository;
        private readonly ILogger<DeletePlayersCmdHandler> logger;

        public DeletePlayersCmdHandler(IRepository repository, ILogger<DeletePlayersCmdHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<PlayersDeletedResult> Handle(DeletePlayersCmd request, CancellationToken cancellationToken)
        {
            var req = request.DeletePlayers;

            var user = await repository.Users.FirstOrDefaultAsync(c => c.Id == req.UserId);
            if (!user.ApiOn || user.LockoutEnabled)
                throw new UnauthorizedAccessException("This user is not authorized");

            if (req?.PlayerIds?.Count() <= 0)
                throw new ArgumentNullException("No players in request to delete");

            var players = repository.Players
                .Include(x => x.Scores)
                .ThenInclude(gp => gp.GamePlayed)
                //then include scores again to get scores for game played because we don't want to delete if other players are not deleted
                .ThenInclude(s=>s.Scores)
                .Where(p => req.PlayerIds.Contains(p.Id));

            var result = new PlayersDeletedResult();

            //only get this users players games if not in Admin role
            if (!req.IsAdmin)
                players = players.Where(x => x.ApplicationUserId == req.UserId);

            //only remove if some found
            if (await players?.CountAsync(cancellationToken) > 0)
            {
                result.ScoresDeleted = new List<long>();
                result.GamesPlayedDeleted = new List<long>();
                result.PlayersDeleted = new List<int>();

                using (var transaction = await repository.Database.BeginTransactionAsync(cancellationToken))

                    try
                    {
                        foreach (var player in players)
                        {
                            foreach (var score in player.Scores)
                            {
                                //remove the score, we want to keep a count of removed
                                repository.Scores.Remove(score);
                                result.ScoresDeleted.Add(score.Id);
                            }

                            //remove the player
                            repository.Players.Remove(player);
                            result.PlayersDeleted.Add(player.Id);
                        }

                        //save to end up with empty games
                        await repository.SaveChangesAsync(cancellationToken);

                        //remove all the empty games after removing scores
                        var emptyGames = repository.GamesPlayed.Where(x => x.Scores.Count == 0);
                        foreach (var gamePlayed in emptyGames)
                        {
                            repository.GamesPlayed.Remove(gamePlayed);
                            result.GamesPlayedDeleted.Add(gamePlayed.Id);
                        }

                        //save...
                        await repository.SaveChangesAsync(cancellationToken);
                        await transaction.CommitAsync(cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"{ex.Message} {ex.InnerException?.Message}");
                        await transaction.RollbackAsync(cancellationToken);
                        throw;
                    }
            }

            return result;
        }
    }
}
