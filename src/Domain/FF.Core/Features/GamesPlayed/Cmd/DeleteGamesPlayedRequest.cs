namespace FF.Core.Features.Scores.Cmd
{
    public class DeleteGamesPlayedRequest
    {
        public bool IsAdmin { get; set; }
        public List<long> GamesPlayedIds { get; set; }
        public string UserId { get; set; }
    }
}
