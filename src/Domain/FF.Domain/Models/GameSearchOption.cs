using System;

namespace FF.Domain.Models
{
    public class GameSearchOption
    {
        /// <summary>
        /// Specify scores from a machine id
        /// </summary>
        public string? Title { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int Limit { get; set; } = 10;
        public string? OrderBy { get; set; }
    }
}
