using System;

namespace FF.Shared.ViewModel.Scores
{
    public class ScoreResultVm
    {
        public long Id { get; set; }
        public string Initials { get; set; }
        public long Points { get; set; }
        public long PlayerId { get; set; }
        public string Name { get; set; }
        public long GamePlayedId { get; set; }
        public string UserName { get; set; }
        public TimeSpan GameTime { get; set; }
        public DateTime Ended { get; set; }
        public int Balls { get; set; }
        public string Title { get; set; }
        public string Machine { get; set; }
        public string CRC { get; set; }
        public string SimVersion { get; set; }
        public bool Desktop { get; set; }
        public string FileName { get; set; }
    }
}
