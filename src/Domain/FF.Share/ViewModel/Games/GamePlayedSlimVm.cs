using FF.Domain.Enum;
using FF.Shared.ViewModel.Menus;
using FF.Shared.ViewModel.Scores;
using System;
using System.Collections.Generic;

namespace FF.Shared.ViewModel.Games
{
    public class GamePlayedSlimVm
    {
        public long Id { get; set; }
        public string GameId { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// Patched filename
        /// </summary>
        public string FileName { get; set; }
        public int BallsPerGame { get; set; }
        public TimeSpan GameTime { get; set; }
        public DateTime Posted { get; set; }
        public IEnumerable<ScoreResultSlimVm> Scores { get; set; }
        public GameSystem SystemType { get; set; }
        public GameType GameType { get; set; }
        public string User { get; set; }
        public bool Desktop { get; set; }
        public string SystemVersion { get; set; }
        public int Players { get; set; }
        public ScoreResultSlimVm TopScore { get; set; }
        public string CRC { get; set; }
        public string GamePlayedCRC { get; set; }
        public string GameVersion { get; set; }
        public string MachineName { get; set; }
        public string Translite { get; set; }
    }
}
