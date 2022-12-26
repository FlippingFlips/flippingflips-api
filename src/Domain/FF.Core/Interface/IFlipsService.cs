using FF.Core.Models.ViewModel.Scores;
using FF.Domain.Models;
using FF.Shared.ViewModel.Games;

namespace FF.Core.Interface
{
    public interface IFlipsService
    {
        /// <summary>
        /// Get all machine scores for a users id
        /// </summary>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        Task<IEnumerable<ScoreResultVm>> GetMachineScoresAsync(MachineSearchOption searchOption);

        /// <summary>
        /// Page games and include score count
        /// </summary>
        /// <param name="gameSearch"></param>
        /// <returns></returns>
        //Task<IEnumerable<GameResultVm>> GetGames(GameSearchOption gameSearch);
        //Task<IEnumerable<GamePlayed>> GetGamesPlayed(MachineSearchOption gameSearch); //TODO
        Task<IEnumerable<GamePlayedVm>> GetRecentGamesPlayed(RecentGamesPlayedDto recentGamesDto);
    }
}
