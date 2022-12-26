namespace FF.Core.Features.Players.Cmd
{
    public class DeletePlayersRequest
    {
        public int[] PlayerIds { get; set; }
        public string UserId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
