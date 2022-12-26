using System;

namespace FF.Core.Models.ViewModel.Scores
{
    public class ScoreResultVm
    {
        public int Id { get; set; }
        public string GameId { get; set; }
        public int PlayerId { get; set; }
        public long Points { get; set; }
        public int BallsPerGame { get; set; }
        public DateTime Created { get; set; }
        public bool ShowPublic { get; set; }
        public TimeSpan GameTime { get; set; }
        public string Title { get; set; }
        public string Initials { get; set; }
    }
}
