using FF.Api.Data.Model;
using FF.Core.Models;

namespace FF.Api.ViewModel
{
    public class UserMachine
    {
        public string ApiKey { get; set; }
        public DateTime? Birthday { get; set; }
        public string MachineDescription { get; set; }
        public string Country { get; set; }
        public string MachineName { get; set; }
        public string Username { get; set; }
        public List<Player> Players { get; set; }
        public DateTime? Created { get; set; }
        public int PlayerCnt { get; set; }
        public int ScoresTotal { get; set; }
    }
}
