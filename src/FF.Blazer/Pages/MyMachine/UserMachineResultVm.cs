using FF.Shared.Model;

namespace FF.Blazer.Pages.MyMachine
{
    public class UserMachineResultVm
    {
        public int Results { get; set; }
        public IEnumerable<UserMachine> UserMachines { get; set; }
    }
}
