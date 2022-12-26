namespace FF.Domain.Models
{
    public class ScoreSearch
    {
        public string? UserId { get; set; }
        public string? GameId { get; set; }
        public bool IncludeOtherMachines { get; set; } = false;
        public bool GetTop { get; set; } = false;
        public int Limit { get; set; } = 10;
    }
}
