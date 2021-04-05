using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NotesMarketplace.Models.RegisteredUserModels
{
    public class RatingModel
    {
        [Required]
        public int NoteID { get; set; }

        [Required(ErrorMessage = "Please write your views on note, here.")]
        [Display(Name = "Comments*")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "Please provide ratings out of 5")]
        [Display(Name = "Stars")]
        public int Stars { get; set; }
    }
}