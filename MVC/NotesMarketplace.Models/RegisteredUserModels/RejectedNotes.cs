using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketplace.Models.RegisteredUserModels
{
    public class RejectedNotes
    {

        public RejectedNotes()
        {
            RejectedNotesList = new List<RejectedNote>();
        }

        public List<RejectedNote> RejectedNotesList;

        public class RejectedNote {

            public int NoteID;

            public string NoteTitle;

            public string NoteCategory;

            public string Remarks;
        }
    }
}