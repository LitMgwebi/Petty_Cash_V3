using System.ComponentModel.DataAnnotations;

namespace Backend.ViewModel
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 5)]
        public required string Password { get; set; }
    }
}
