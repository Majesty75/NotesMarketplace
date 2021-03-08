using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NotesMarketplace.Models.HomeModels
{

    /*
     *  ------ NoteStatus ------
     *  0: Draft
     *  1: Submitted
     *  2: In Review
     *  3: Published
     *  4: Rejected
     *  5: Unpublished
    */
    public class NoteModel
    {
        public int? NoteId { get; set;}

        [Display(Name = "Title*")]
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Display(Name = "Category*")]
        [Required(ErrorMessage = "Please Select Category")]
        public string Category { get; set; }

        [Display(Name = "Type*")]
        [Required(ErrorMessage = "Please Select Type")]
        public string Type { get; set; }

        [Display(Name = "Description*")]
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Display(Name = "Price*")]
        [Range(0, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        [Required(ErrorMessage = "Please fill up price, if free enter 0.")]
        public decimal? Price { get; set; }

        [Display(Name = "Institution Name")]
        public string Institution { get; set; }

        [Display(Name = "Country*")]
        [Required(ErrorMessage = "Please Select Country")]
        public string Country { get; set; }

        [Display(Name = "Course Name")]
        public string CourseName { get; set; }

        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }

        [Display(Name = "Professor/Lecturer")]
        public string Professor { get; set; }

        [Display(Name = "Number Of Pages")]
        [Range(0, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int? Pages { get; set; }

        public System.DateTime Approved { get; set; }

        public int? rating {get; set;}

        public int Reports { get; set; }

        public string DisplayPicture { get; set; }

        public ICollection<Review> Reviews { get; set; } 
        
        public bool IsItSearchAndFilter { get; set; }

        public int SellerID { get; set; }

        public Nullable<int> ActionBy { get; set; }

        public string AdminRemarks { get; set; }

        public string Preview { get; set; }

        // True: Paid, False: Free
        [Required]
        [Display(Name = "Sell For*")]
        public bool SellFor { get; set; }

        [Required(ErrorMessage = "Notes File(s) are required.")]
        [Display(Name = "Upload Notes*")]
        public HttpPostedFileBase [] NotesFiles { get; set; }

        [Display(Name = "Note Preview")]
        public HttpPostedFileBase PreviewFile { get; set; }

        [Display(Name = "Display Picture")]
        public HttpPostedFileBase NotesCoverPage { get; set; }

        public int Status { get; set; }

        public string [] Attachments { get; set; }
    }
}