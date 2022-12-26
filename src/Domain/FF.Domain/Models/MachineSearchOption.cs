using System;

namespace FF.Domain.Models
{
    public class MachineSearchOption
    {
        /// <summary>
        /// ApplicationUserId
        /// </summary>
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? GameId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
