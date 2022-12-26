using FF.Infrastructure.Data;
using FF.Core.Interface;
using FF.Domain.Models;
using FF.Core.Models.ViewModel.Scores;
using FF.Shared.ViewModel.Games;
using FF.Core.Features.Scores.Query;

namespace FF.Infrastructure
{
    public class FlipsService : IFlipsService
    {
        private readonly ApplicationDbContext context;
        private readonly IFileService fileService;

        public FlipsService(ApplicationDbContext applicationDbContext, IFileService fileService)
        {
            this.context = applicationDbContext;
            this.fileService = fileService;
        }

        public Task<IEnumerable<ScoreResultVm>> GetMachineScoresAsync(MachineSearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GamePlayedVm>> GetRecentGamesPlayed(RecentGamesPlayedDto recentGamesDto)
        {
            throw new NotImplementedException();
        }

        //public async Task<IEnumerable<GamePlayed>> GetGamesPlayed(MachineSearchOption gameSearch)
        //{
        //    IQueryable<GamePlayed> scoreQuery = context.GamesPlayed
        //        .AsNoTracking()
        //        .Include(s => s.Scores).ThenInclude(p => p.Player);


        //    var results = await scoreQuery.ToListAsync();

        //    return results;
        //}
    }
}
