using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotesMarketplace.Models.HomeModels
{
    public class ContactUsModel
    {
        [Required]
        [Display(Name="Full Name*")]
        public string FullName { get; set; }
        
        [Required]
        [Display(Name ="Email*")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name="Subject*")]
        public string Subject { get; set; }

        [Required]
        [Display(Name ="Comments/Questions*")]
        public string Comment { get; set; }
    }
}