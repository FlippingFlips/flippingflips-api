using System;
using System.Collections.Generic;

namespace FF.Shared.ViewModel.Games
{
    public class GameResultVm : GameResultBaseVm
    {
        public string Description { get; set; }
        public string ScreenUrl { get; set; }
        public string CRC { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? Updated { get; set; }
        public int ScoreCount { get; set; }
        public string Translite { get; set; }
        public int? IPDB { get; set; }
        public int TotalGamesPlayed { get; set; }
        public string FileNamePatched { get; set; }
        public string CRCPatched { get; set; }
        public string FilePatchUrl { get; set; }
    }
}
