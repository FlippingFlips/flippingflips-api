namespace FF.Core.Models
{
    /// <summary>
    /// This model is just used when seeding database
    /// </summary>
    public class FlipsUser : ApplicationUser
    {
        public string Password { get; set; }
        public string Player { get; set; }
        public string PlayerInitials { get; set; }
        public string RoleName { get; set; }
    }
}
