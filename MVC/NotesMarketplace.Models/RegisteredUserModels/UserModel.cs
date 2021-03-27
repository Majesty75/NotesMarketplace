using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NotesMarketplace.Models.RegisteredUserModels
{
    public class UserModel
    {
        public int UserID { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public int RoleID { get; set; }
        public bool IsEmailVerified { get; set; }
        public System.DateTime? CreatedDate { get; set; }
    }
}