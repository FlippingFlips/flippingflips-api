using Microsoft.AspNetCore.Identity;

namespace FF.Core.Models
{
    /// <summary>
    /// Seed data model used by AppSettings.json
    /// </summary>
    public class DatabaseModelCreationOptions
    {
        public IEnumerable<FlipsUser> Users { get; set; }
        public IEnumerable<IdentityRole> UserRoles { get; set; }
    }
}
