using System.Collections.Generic;

namespace FF.Shared.Model
{
    public class UserMachinesResult
    {
        public int Results { get; set; }
        public IEnumerable<UserMachine> UserMachines { get; set; }
    }
}
