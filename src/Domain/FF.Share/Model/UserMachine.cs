using System;
using System.Collections.Generic;

namespace FF.Shared.Model
{
    public class UserMachine
    {
        public string UserId { get; set; }
        public string ApiKey { get; set; }
        public DateTime? Birthday { get; set; }
        public string MachineDescription { get; set; }
        public string Country { get; set; }
        public string MachineName { get; set; }
        public string Username { get; set; }
        public IEnumerable<Player> Players { get; set; }
        public DateTime? Created { get; set; }
        public int PlayerCnt { get; set; }
        public int ScoresTotal { get; set; }
    }
}
