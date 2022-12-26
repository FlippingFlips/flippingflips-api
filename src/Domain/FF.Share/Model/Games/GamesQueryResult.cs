using FF.Shared.ViewModel.Games;
using System.Collections.Generic;

namespace FF.Core.Features.Games.Query
{
    public class GamesQueryResult
    {
        public IEnumerable<GameResultBaseVm> Games { get; set; }
        public int Total { get; set; }
        public int PageTotal { get; set; }
        public int Page { get; set; }
    }
}
