using FF.Domain.Enum;
using System;

namespace FF.Shared.ViewModel.Games
{
    public class GameResultBaseVm
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string Id { get; set; }
        public string Version { get; set; }
        public int GamesPlayed { get; set; }
        public string FileUrl { get; set; }
        public int Year { get; set; }
        public DateTime Created { get; set; }
        public GameType GameType { get; set; }
        public string FileName { get; set; }
        public GameSystem GameSystem { get; set; }
        public int TotalScores { get; set; }
    }
}
