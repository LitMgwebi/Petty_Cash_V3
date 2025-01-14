using System.ComponentModel.DataAnnotations;

namespace Backend.ViewModel
{
    public class UserRegister
    {
        [Required(ErrorMessage = "Please enter your first name.")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name.")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your phone number.")]
        [DataType(DataType.PhoneNumber)]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 5)]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 5)]
        [Compare(nameof(Password))]
        public required string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please choose your division.")]
        public int DivisionId { get; set; }

        [Required(ErrorMessage = "Please choose your job title.")]
        public int JobTitleId { get; set; }

        [Required(ErrorMessage = "Please choose your office.")]
        public int OfficeId { get; set; }
    }
}
