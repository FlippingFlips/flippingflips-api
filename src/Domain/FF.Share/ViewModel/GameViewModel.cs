using FF.Domain.Enum;
using System;

namespace FF.Shared.ViewModel
{
    public class GameViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string ScreenUrl { get; set; }
        public string Version { get; set; }
        public int Year { get; set; }
        public string CRC { get; set; }        

        public int ScoreCount { get; set; }

        public int GamesPlayed { get; set; }

        public DateTime Created { get; set; }

        public GameType GameType { get; set; }

        public string Translite { get; set; }

        public byte[] TransliteData { get; set; }
    }
}
