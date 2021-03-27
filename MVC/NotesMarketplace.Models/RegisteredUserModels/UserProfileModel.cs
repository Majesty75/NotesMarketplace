using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NotesMarketplace.Models.RegisteredUserModels
{
    public class UserProfileModel
    {
        public int UserID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true, NullDisplayText = "dd-MM-yyyy")]
        [Display(Name = "Date Of Birth")]
        public Nullable<System.DateTime> DOB { get; set; }

        public string Gender { get; set; }

        public string SecondaryEmailAddress { get; set; }

        public string CountryCode { get; set; }

        [Display(Name = "Phone Number")]
        [RegularExpression(@"[0-9]{10}", ErrorMessage = "Invalid Phone Number")]
        public string PhoneNo { get; set; }

        [Display(Name = "Profile Picture")]
        public string ProfilePicture { get; set; }

        [Display(Name = "Profile Picture")]
        public HttpPostedFileBase ProfilePictureFile { get; set; }

        [Required(ErrorMessage = "Please Enter Primary Address Line")]
        [Display(Name = "Address Line 1*")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "Please Enter City")]
        [Display(Name = "City*")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please Enter State")]
        [Display(Name = "State*")]
        public string State { get; set; }

        [Required(ErrorMessage = "Please Enter Zip Code")]
        [Display(Name = "ZipCode*")]
        [RegularExpression(@"[0-9]{6}", ErrorMessage = "Invalid Zip Code")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Please Choose Country")]
        public string Country { get; set; }

        public string University { get; set; }

        public string College { get; set; }

        public UserModel User { get; set; }
    }
}