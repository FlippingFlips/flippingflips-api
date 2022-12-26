using FF.Core.Interface;

namespace FF.Core.Models
{
    public class Score : IPublic
    {
        public long Id { get; set; }
        public long GamePlayedId { get; set; }
        public int PlayerId { get; set; }
        public long Points { get; set; }
        public bool ShowPublic { get; set; } = true;
        public Player Player { get; set; }
        public GamePlayed GamePlayed { get; set; }
    }
}
