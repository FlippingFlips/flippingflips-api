namespace FF.Core.Features.Scores.Cmd
{
    /// <summary>
    /// Return the ids of the scores and games played that were removed from the database
    /// </summary>
    public class ScoresDeletedResult
    {
        public List<long> ScoreIdsRemoved { get; set; } = new List<long>();
        public List<long> GamePlayedIdsRemoved { get; set; } = new List<long>();
    }
}
