using System.IO;
using System.Text.Json;

namespace FF.Blazer.Pages.MyMachine
{
    public class FlipsSettings
    {
        public string ApiKey { get; set; }
        public string ServerUrl { get; set; }
        public LatestScoreSettings LatestScoreSettings { get; set; } = new();
    }

    public class LatestScoreSettings
    {
        /// <summary>
        /// Include other user machines in the query
        /// </summary>
        public bool IncludeOtherMachines { get; set; }

        /// <summary>
        /// Latest or Top scores
        /// </summary>
        public bool GetLatestOrTopScores { get; set; }
    }
}
