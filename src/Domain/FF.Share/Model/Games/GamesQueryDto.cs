namespace FF.Shared.Model.Games
{
    public class GamesQueryDto : PageQueryBaseDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string SystemType { get; set; }
        public string GameType { get; set; }
    }
}