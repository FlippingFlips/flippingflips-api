using FF.Shared.ViewModel.Scores;
using System.Collections.Generic;

namespace FF.Shared.Model.Scores
{
    public class ScoreSearchResult
    {
        public List<ScoreResultVm> Scores { get; set; }
        public int Total { get; set; }
        public int Page { get; set; } = 1;
        public int PageTotal { get; set; }
    }
}
