using System;
using System.Collections.Generic;

namespace FF.Shared.ViewModel.Games
{
    public class GameInformationViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public string FileName { get; set; }
        public string FileNamePatched { get; set; }
        public string FileUrl { get; set; }
        public string ScreenUrl { get; set; }
        public string CRC { get; set; }
        public string CRCPatched { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Created { get; set; }
        public int ScoreCount { get; set; }
        public string Translite { get; set; }
        public int? IPDB { get; internal set; }
        public int TotalGamesPlayed { get; set; }
        public IEnumerable<GamePlayedVm> RecentGamesPlayed { get; set; }
    }
}
