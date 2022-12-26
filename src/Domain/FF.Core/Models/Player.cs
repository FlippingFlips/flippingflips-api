using FF.Core.Interface;

namespace FF.Core.Models
{
    public class Player : IPlayer
    {
        public int Id { get; set; }
        public string Initials { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// The default player for the machine
        /// </summary>
        public bool MachineDefault { get; set; }
        public DateTime Created { get; set; }
        public string ApplicationUserId { get; set; }
        public ICollection<Score> Scores { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
