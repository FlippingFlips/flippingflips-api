namespace FF.Core.Features.Scores.Cmd
{
    /// <summary>
    /// Return the ids of games played and any scores that were removed from the database
    /// </summary>
    public class GamesPlayedDeletedResult
    {
        public List<long> ScoreIdsRemoved { get; set; } = new List<long>();
        public List<long> GamePlayedIdsRemoved { get; set; } = new List<long>();
    }
}
