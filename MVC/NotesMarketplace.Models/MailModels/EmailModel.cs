using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace NotesMarketplace.Models.MailModels
{
    public class EmailModel
    {
        [Required]
        [EmailAddress]
        public string Emailfrom { get; set; }
        
        [Required]
        [EmailAddress]
        public string [] EmailTo { get; set; }

        [Required]
        public string EmailSubject { get; set; }

        public string EmailBody { get; set; }

    }
}