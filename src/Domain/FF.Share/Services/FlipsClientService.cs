using FF.Core.Features.Games.Query;
using FF.Shared.Interface;
using FF.Shared.Model;
using FF.Shared.Model.Games;
using FF.Shared.Model.Scores;
using FF.Shared.ViewModel;
using FF.Shared.ViewModel.Games;
using FF.Shared.ViewModel.Menus;
using FF.Shared.ViewModel.Scores;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FF.Shared.Services
{
    public class FlipsClientService : IFlipsClientService
    {
        public HttpClient Http { get; }
        public FlipsClientService(HttpClient Http)
        {
            this.Http = Http;
        }

        public async Task<IEnumerable<GameViewModel>> GetGamesAsync(GameSearchOption searchOption)
        {
            var response = await Http.PostAsJsonAsync("games/searchgames", searchOption);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<GameViewModel>>();
            }

            return null;
        }

        /// <summary>
        /// Gets the currents users machine
        /// </summary>
        /// <returns></returns>
        public async Task<UserMachinesResult> GetUserMachineAsync()
        {
            return await Http.GetFromJsonAsync<UserMachinesResult>("User/GetUserMachine");
        }

        /// <summary>
        /// Gets the currents users machine: TODO: paging
        /// </summary>
        /// <returns></returns>
        public async Task<UserMachinesResult> GetUserMachinesAsync(UserMachinesQueryDto queryDto)
        {
            var response = await Http.PostAsJsonAsync("User/GetUserMachines", queryDto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserMachinesResult>();
            }
            else { return null; }
        }

        /// <summary>
        /// Gets a machine by username
        /// </summary>
        /// <returns></returns>
        public async Task<UserMachinesResult> GetUserMachineByNameAsync(string name)
        {
            return await Http.GetFromJsonAsync<UserMachinesResult>("User/GetUserMachineByUsername?userName=" + name);
        }

        /// <summary>
        /// Gets the currents users machine scores
        /// </summary>
        /// <returns></returns>
        public async Task<ScoreSearchResult> GetScoresAsync(ScoresQueryDto scoresQueryDto)
        {
            var response = await Http.PostAsJsonAsync("Scores", scoresQueryDto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ScoreSearchResult>();
            }
            else { return null; }
        }

        public async Task<GamePlayedVm> GetGamePlayedByIdAsync(long id)
        {
            return await Http.GetFromJsonAsync<GamePlayedVm>("GamesPlayed/ById?id="+id);
        }

        public async Task<IEnumerable<RomMenuItemVm>> GetGamePlayedSettingsAsync(long id)
        {
            return await Http.GetFromJsonAsync<IEnumerable<RomMenuItemVm>>("GamesPlayed/GetGamePlayedSettings?gamePlayedId=" + id);
        }

        public async Task<GamesPlayedResult> GetGamesPlayedAsync(GamesPlayedQueryDto gamesPlayed)
        {
            var response = await Http.PostAsJsonAsync("GamesPlayed/GetAll", gamesPlayed);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<GamesPlayedResult>();
            }
            else { return null; }
        }

        public async Task<GamesQueryResult> GetGames(GamesQueryDto gamesQuery)
        {
            var response = await Http.PostAsJsonAsync("games/searchgames", gamesQuery);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<GamesQueryResult>();
            }
            else { return null; }
        }
    }
}
