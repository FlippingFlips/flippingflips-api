using System.Collections.Generic;

namespace FF.Shared.Model
{
    public class RegisterUserVm
    {
        public bool IsSuccessfulRegistration { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}