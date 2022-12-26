namespace FF.Core.Models.Dto.Games
{
    public class GameQueryDto : PagedBaseDto
    {
        public string Title { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string OrderBy { get; set; }
        public string Author { get; internal set; }
    }
}
