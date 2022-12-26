using FF.Core.Mappings;
using Microsoft.AspNetCore.Identity;

namespace FF.Core.Models
{
    public class ApplicationUser : IdentityUser, IMapFrom<FlipsUser> //, IPublic
    {
        public string Country { get; set; }        
        /// <summary>
        /// Age of the machine
        /// </summary>
        public DateTime? MachBirthday { get; set; }
        public string MachDesc { get; set; }
        public string MachName { get; set; }
        public string ApiKey { get; set; }
        public int PlayersPerCabinet { get; set; } = 8;
        public bool ApiOn { get; set; } = true;
        public int RequestsMade { get; set; }
        public bool ShowPublic { get; set; } = true;
        public DateTime Created { get; set; }
        public ICollection<Player> Players { get; set; }
        public ICollection<GameInProgress> GamesInProgress { get; set; }
        /// <summary>
        /// Friends, blocked, etc
        /// </summary>
        public ICollection<CustomList> CustomLists { get; set; }
    }
}
