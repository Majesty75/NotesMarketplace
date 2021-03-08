using System.Collections.Generic;

namespace NotesMarketplace.Models.HomeModels
{
    public class ListNotes
    {
        public ICollection<NoteModel> Notes { get; set; }
    }
}