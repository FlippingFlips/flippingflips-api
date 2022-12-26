namespace FF.Core.Features.Scores.Cmd
{
    public class DeleteScoreRequest
    {
        public bool IsAdmin { get; set; }
        public List<long> ScoreIds { get; set; }
        public string UserId { get; set; }
    }
}
