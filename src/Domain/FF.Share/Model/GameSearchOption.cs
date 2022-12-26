using System;

namespace FF.Shared.Model
{
    public class GameSearchOption
    {
        /// <summary>
        /// Specify scores from a machine id
        /// </summary>
        public string Id { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}