using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketplace.Models.AdminModels
{
    public class AdminNotesInReviewModel
    {

        public List<NoteInReview> NotesInReview { get; set; }

        public class NoteInReview
        {
            public int NoteID { get; set; }

            public string NoteTitle { get; set; }

            public string NoteCategory { get; set; }

            public string Seller { get; set; }

            public int SellerID { get; set; }

            public Nullable<DateTime> DateAdded { get; set; }

            public string status { get; set; }

        }
    }
}