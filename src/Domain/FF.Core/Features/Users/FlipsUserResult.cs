using FF.Core.Models;

namespace FF.Core.Features.Users
{
    public class FlipsUserResult
    {
        public UserCheckResult UserCheckResult { get; set; }
        public string Message { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
