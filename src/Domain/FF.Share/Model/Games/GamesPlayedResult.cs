using FF.Shared.ViewModel.Games;
using System.Collections.Generic;

namespace FF.Shared.Model.Games
{
    public class GamesPlayedResult
    {
        public List<GamePlayedSlimVm> GamesPlayed { get; set; }
        public int TotalGamesPlayed { get; set; }
        public int Page { get; set; } = 1;
        public int PageTotal { get; set; }
    }
}
