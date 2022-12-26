using System.ComponentModel.DataAnnotations;

namespace FF.Api.Dto
{
    public class RegisterModel
    {
        [Required]
        [StringLength(25, MinimumLength = 3)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 1)]
        [Display(Name = "Player initals")]
        public string Initials { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        [Display(Name = "Player Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Machine Name")]
        [StringLength(32, MinimumLength = 3)]
        public string MachineName { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "User Role", Description = "Will only applied by an admin user")]
        public string UserRole { get; set; }
    }
}
