namespace FF.Core.Features.Users
{
    public class UserRequestApiLoginDto
    {
        public string ApiKey { get; set; }
        public string RequestHeader { get; set; }
        public bool IsDevEnvironment { get; set; }
        public bool IncludePlayers { get; set; }
        public bool IncludeGamesInProgress { get; set; }
    }
}
