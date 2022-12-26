namespace FF.Core.Features.Players.Cmd
{
    public class PlayersDeletedResult
    {
        public List<int> PlayersDeleted { get; set; }
        public List<long> ScoresDeleted { get; set; }
        public List<long> GamesPlayedDeleted { get; set; }
    }
}
