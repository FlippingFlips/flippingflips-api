namespace FF.Domain.Models.Data
{
    public class PinmameRom
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? ParentRom { get; set; }
        /// <summary>
        /// Only store this on the parent rom
        /// </summary>
        public string? NvMapJson { get; set; }
        /// <summary>
        /// Some rom games don't have last scores
        /// </summary>
        public bool NoLastScores { get; set; }
    }
}