using System;

namespace FF.Core.Models
{
    public class GamePlayed
    {
        public long Id { get; set; }
        public string GameId { get; set; }
        public string ApplicationUserId { get; set; }
        public int BallsPerGame { get; set; }
        public int? TiltWarnings { get; set; }
        public int? MaxExtraBall { get; set; }
        public int? BallSave { get; set; }
        public bool Desktop { get; set; }
        public string CRC { get; set; }
        public string SystemVersion { get; set; }
        public string NvRam { get; set; }
        /// <summary>
        /// Server time created
        /// </summary>
        public DateTime Created { get; set; }
        public DateTime Ended { get; set; }
        public TimeSpan GameTime { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Game Game { get; set; }
        public ICollection<Score> Scores { get; set; }
    }
}