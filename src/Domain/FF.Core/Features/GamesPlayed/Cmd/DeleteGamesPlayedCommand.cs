using FF.Core.Features.Scores.Cmd;
using FF.Core.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FF.Core.Features.GamesPlayed.Cmd
{
    public class DeleteGamesPlayedCmd : IRequest<GamesPlayedDeletedResult>
    {
        public DeleteGamesPlayedCmd(DeleteGamesPlayedRequest deleteGamesPlayed)
        {
            DeleteGamesPlayed = deleteGamesPlayed;
        }

        public DeleteGamesPlayedRequest DeleteGamesPlayed { get; }
    }

    internal class DeleteGamesPlayedCmdHandler : IRequestHandler<DeleteGamesPlayedCmd, GamesPlayedDeletedResult>
    {
        private readonly IRepository repository;

        private readonly ILogger<DeleteGamesPlayedCmdHandler> logger;

        public DeleteGamesPlayedCmdHandler(IRepository repository, ILogger<DeleteGamesPlayedCmdHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<GamesPlayedDeletedResult> Handle(DeleteGamesPlayedCmd request, CancellationToken cancellationToken)
        {
            var req = request.DeleteGamesPlayed;

            var user = await repository.Users.FirstOrDefaultAsync(c => c.Id == req.UserId);
            if (!user.ApiOn || user.LockoutEnabled)
                throw new UnauthorizedAccessException("This user is not authorized");

            if (req?.GamesPlayedIds?.Count() <= 0)
                throw new ArgumentNullException("No games in request to delete");

            //build query with games played including the scores
            IQueryable<Models.GamePlayed> gamesP = repository.GamesPlayed
                    .Include(g => g.Scores)
                    .Where(x => req.GamesPlayedIds.Contains(x.Id));

            //only get this users games played if not in Admin role
            if (!req.IsAdmin)
                gamesP = gamesP.Where(x => x.ApplicationUserId == req.UserId);

            var result = new GamesPlayedDeletedResult();

            //only remove if some found
            if (await gamesP?.CountAsync(cancellationToken) > 0)
            {
                using (var transaction = await repository.Database.BeginTransactionAsync(cancellationToken))

                    try
                    {
                        foreach (var item in gamesP)
                        {
                            //remove the score and add the id removed
                            repository.Scores.RemoveRange(item.Scores);
                            result.ScoreIdsRemoved.AddRange(item.Scores.Select(x=>x.Id));

                            //remove the game played
                            repository.GamesPlayed.RemoveRange(item);
                            result.GamePlayedIdsRemoved.Add(item.Id);
                        }

                        //save and commit
                        await repository.SaveChangesAsync(cancellationToken);
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"{ex.Message} {ex.InnerException?.Message}");
                        await transaction.RollbackAsync(cancellationToken);
                        throw;
                    }
            }
            else { logger.LogWarning($" {user?.UserName} : No games played found to delete"); }

            return result;            
        }
    }
}
