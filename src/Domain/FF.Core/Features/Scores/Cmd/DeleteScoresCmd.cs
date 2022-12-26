using FF.Core.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FF.Core.Features.Scores.Cmd
{
    /// <summary>
    /// Allows users to delete their scores. If only one score made then the GamePlayed should be removed too.
    /// </summary>
    public class DeleteScoresCmd : IRequest<ScoresDeletedResult>
    {
        public DeleteScoresCmd(DeleteScoreRequest deleteScoreRequest)
        {
            DeleteScoreRequest = deleteScoreRequest;
        }

        public DeleteScoreRequest DeleteScoreRequest { get; }
    }

    internal class DeleteScoresCmdHandler : IRequestHandler<DeleteScoresCmd, ScoresDeletedResult>
    {
        private readonly IRepository repository;
        private readonly ILogger<DeleteScoresCmdHandler> logger;

        public DeleteScoresCmdHandler(IRepository repository, ILogger<DeleteScoresCmdHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<ScoresDeletedResult> Handle(DeleteScoresCmd request, CancellationToken cancellationToken)
        {
            var req = request.DeleteScoreRequest;

            var user = await repository.Users.FirstOrDefaultAsync(c => c.Id == req.UserId);
            if (!user.ApiOn || user.LockoutEnabled)
                throw new UnauthorizedAccessException("This user is not authorized");
            
            if (req?.ScoreIds?.Count() <= 0)
                throw new ArgumentNullException("No scores in request to delete");

            var result = new ScoresDeletedResult();

            //build query with games played
            IQueryable<Models.Score> scores = repository.Scores
                    .Include(g => g.GamePlayed)
                    .Where(x => req.ScoreIds.Contains(x.Id));

            //only get this users scores games if not in Admin role
            if (!req.IsAdmin)
                scores = scores.Where(x => x.GamePlayed.ApplicationUserId == req.UserId);

            //only remove if some found
            if (await scores?.CountAsync(cancellationToken) > 0)
            {
                using (var transaction = await repository.Database.BeginTransactionAsync(cancellationToken))

                    try
                    {
                        foreach (var item in scores)
                        {
                            //remove the score and add the id removed
                            repository.Scores.Remove(item);
                            result.ScoreIdsRemoved.Add(item.Id);

                            //only one score found so also delete the game played
                            if (item.GamePlayed.Scores.Count == 1)
                            {
                                repository.GamesPlayed.Remove(item.GamePlayed);
                                result.GamePlayedIdsRemoved.Add(item.GamePlayed.Id);
                            }
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
            else { logger.LogWarning($" {user?.UserName} : No scores found to delete"); }

            return result;
        }
    }
}
