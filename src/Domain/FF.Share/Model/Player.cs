using System;

namespace FF.Shared.Model
{
    public class Player
    {
        public int Id { get; set; }
        public string Initials { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// The default player for the machine
        /// </summary>
        public bool MachineDefault { get; set; }
        public DateTime Created { get; set; }
    }
}
