using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketplace.Models.RegisteredUserModels
{
    public class DashboardModel
    {

        public int NotesSold { get; set; }

        public decimal MoneyEarned { get; set; }

        public int Downloads { get; set; }

        public int Rejecteds { get; set; }

        public int BuyerRequests { get; set; }


        public List<InProgressNotesClass> InProgressNotes { get; set;}

        public List<PublishedNotesClass> PublishedNotes { get; set; }

        public class InProgressNotesClass
        {
            public int NoteID { get; set; }

            public string NoteTitle { get; set; }

            public string NoteCategory { get; set; }

            public string Status { get; set; }

            public Nullable<DateTime> AddedDate { get; set; }
        }

        public class PublishedNotesClass
        {
            public int NoteID { get; set; }

            public string NoteTitle { get; set; }

            public string NoteCategory { get; set; }

            public string SellType { get; set; }

            public decimal Price { get; set; }

            public Nullable<DateTime> AddedDate { get; set; }
        }
    }
}