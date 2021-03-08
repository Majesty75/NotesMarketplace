using System.ComponentModel.DataAnnotations;
namespace NotesMarketplace.Models.AuthenticationModels
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Old Password is required")]
        [Display(Name ="Old Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "New Password is required")]
        [Display(Name = "New Password")]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=])(?=\S+$).{6,24}$",
        ErrorMessage = "Password must be aleast 6 characters and must contain lowercase, uppercase, special character and a number.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please enter password again to confirm password")]
        [Compare("NewPassword", ErrorMessage = "Passwords does not matches.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}