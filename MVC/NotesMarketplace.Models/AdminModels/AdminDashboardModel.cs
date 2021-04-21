using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketplace.Models.AdminModels
{
    public class AdminDashboardModel
    {

        public int NoteInReview { get; set; }

        public int Downloads { get; set; }

        public int NewUsers { get; set; }

        public List<PublishedNote> PublishedNotes { get; set; }

        public class PublishedNote
        {
            public int NoteID { get; set; }

            public string NoteTitle { get; set; }

            public string Category { get; set; }

            public string AttachmentSize { get; set; }

            public string SellType { get; set; }

            public decimal Price { get; set; }

            public string Publisher { get; set; }

            public int PublisherID { get; set; }

            public Nullable<System.DateTime> PublishedDate { get; set; }

            public int NoOfDownloads { get; set; }

            public string ApprovedBy { get; set; }
        }
    }
}