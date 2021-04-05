using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NotesMarketplace.Models.RegisteredUserModels
{
    public class ReportNoteModel
    {
        [Required]
        public int NoteID { set; get; }

        public string NoteTitle { set; get; }

        [Required(ErrorMessage = "Please Provide Reason For Reporting This Note.")]
        [Display(Name = "Remarks*")]
        public string Remarks { set; get; }

        public string SellerName { get; set; }
    }
}