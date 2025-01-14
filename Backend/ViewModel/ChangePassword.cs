using System.ComponentModel.DataAnnotations;

namespace Backend.ViewModel
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Please enter your current password.")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 5)]
        public string CurrentPassword { get; set; } = null!;


        [Required(ErrorMessage = "Please enter your new password.")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 5)]
        public string NewPassword { get; set; } = null!;


        [Required(ErrorMessage = "Please enter your confirmation of new password.")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "The new password and confirmation password do not match.")]
        [StringLength(50, MinimumLength = 5)]
        public string ConfirmPassword { get; set; } = null!;
    }
}
