namespace FF.Shared.Model.Games
{

    public class GamesPlayedQueryDto : PageQueryBaseDto
    {        
        public string UserId { get; set; }
        /// <summary>
        /// limit results to the userId
        /// </summary>
        public bool GetUsersGames { get; set; }
        public string UserName { get; set; }
        public string GameId { get; set; }        
        public string Title { get; set; }
    }
}
