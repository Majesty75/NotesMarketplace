using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotesMarketplace.Models.AuthenticationModels
{
    public class SignUp
    {
        [Required(ErrorMessage = "First Name is required")]
        [RegularExpression(@"[a-zA-z ]*", ErrorMessage = "Name must only contain alphabates")]
        [Display(Name = "First Name*")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name*")]
        [RegularExpression(@"[a-zA-z ]*", ErrorMessage = "Name must only contain alphabates")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email*")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password*")]
        [RegularExpression(
            @"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=])(?=\S+$).{6,24}$",
            ErrorMessage = "Password must be aleast 6 characters and must contain lowercase, uppercase, special character and a number.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter password again to confirm password")]
        [Compare("Password", ErrorMessage = "Passwords does not matches.")]
        [Display(Name = "Confirm Password*")]
        public string ConfirmPassword { get; set; }

        public int? RoleID { get; set; }

        public string GUID { get; set; }
    }
}