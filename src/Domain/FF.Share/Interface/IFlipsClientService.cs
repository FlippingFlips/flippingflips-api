using FF.Core.Features.Games.Query;
using FF.Shared.Model;
using FF.Shared.Model.Games;
using FF.Shared.Model.Scores;
using FF.Shared.ViewModel;
using FF.Shared.ViewModel.Games;
using FF.Shared.ViewModel.Menus;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FF.Shared.Interface
{
    /// <summary>
    /// Wrapper for making http requests to api
    /// </summary>
    public interface IFlipsClientService
    {
        Task<IEnumerable<GameViewModel>> GetGamesAsync(GameSearchOption searchOption);
        Task<GamesQueryResult> GetGames(GamesQueryDto gamesQuery);
        Task<GamePlayedVm> GetGamePlayedByIdAsync(long id);
        Task<GamesPlayedResult> GetGamesPlayedAsync(GamesPlayedQueryDto gamesPlayed);
        Task<IEnumerable<RomMenuItemVm>> GetGamePlayedSettingsAsync(long id);
        Task<UserMachinesResult> GetUserMachineAsync();
        Task<UserMachinesResult> GetUserMachinesAsync(UserMachinesQueryDto queryDto);
        Task<UserMachinesResult> GetUserMachineByNameAsync(string name);
        Task<ScoreSearchResult> GetScoresAsync(ScoresQueryDto scoresQueryDto);
    }
}
