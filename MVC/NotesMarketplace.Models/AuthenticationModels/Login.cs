using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotesMarketplace.Models.AuthenticationModels
{
    public class Login
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email Address")]
        public string Email { get; set; }

        [Required]
        //[RegularExpression(@"[a-zA-Z0-9@#$%&*]{6,24}", ErrorMessage = "Password must contain lowercase, uppercase, special character and number.")]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}