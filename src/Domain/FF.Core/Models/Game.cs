using FF.Domain.Enum;
using FF.Domain.Models.Data;

namespace FF.Core.Models
{
    public class Game
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        /// <summary>
        /// Comma separated 
        /// </summary>
        public string PinmameRomId { get; set; }
        /// <summary>
        /// CRC32
        /// </summary>
        public string CRC { get; set; }
        public string FileNamePatched { get; set; }
        public string FilePatchUrl { get; set; }
        /// <summary>
        /// Patched table CRC32
        /// </summary>
        public string CRCPatched { get; set; }
        public string PatchNotes { get; set; }
        public bool IsDeleted { get; set; }
        /// <summary>
        /// Total players for this game
        /// </summary>
        public int Players { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public GameSystem GameSystem { get; set; }
        public GameType GameType { get; set; }
        /// <summary>
        /// Internet Pinball Database ID
        /// </summary>
        public int? IPDB { get; set; }
        public bool IsEnabled { get; set; }
        public string Manufacturer { get; set; }
        public PinmameRom PinmameRom { get; set; }
        public virtual ICollection<GamePlayed> GamesPlayed { get; set; }
    }
}