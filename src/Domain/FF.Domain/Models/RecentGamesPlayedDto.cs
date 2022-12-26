namespace FF.Domain.Models
{
    public class RecentGamesPlayedDto
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public int Limit { get; set; } = 10;
    }
}
