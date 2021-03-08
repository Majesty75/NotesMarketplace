using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketplace.Models.HomeModels
{
    public class SearchNotesModel
    {
        public IDictionary<string, string> Type { get; set; }

        public IDictionary<string, string> Category { get; set; }

        public IDictionary<string, string> University { get; set; }

        public IDictionary<string, string> Course { get; set; }

        public IDictionary<string, string> Country { get; set; }

        public IDictionary<int, string> Rating { get; set; }
    }
}