using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketplace.Models.AdminModels
{
    public class PublishedNotesModel
    {
        public List<AdminDashboardModel.PublishedNote> PublishedNotes { get; set; }
    }
}