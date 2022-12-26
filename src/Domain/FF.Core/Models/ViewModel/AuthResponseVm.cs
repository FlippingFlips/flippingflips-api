namespace FF.Domain.Models.ViewModel
{
    public class AuthResponseVm
    {
        public bool IsAuthSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }
    }
}
