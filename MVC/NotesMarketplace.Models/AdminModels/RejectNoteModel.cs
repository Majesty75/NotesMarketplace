using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NotesMarketplace.Models.AdminModels
{
    public class RejectNoteModel
    {
        [Required(ErrorMessage = "Please Provide Reason.")]
        [Display(Name ="Remarks*")]
        public string Remarks { get; set; }

        public string NoteTitle { get; set; }

        public string NoteCategory { get; set; }

        public int NoteID { get; set; }
    }
}