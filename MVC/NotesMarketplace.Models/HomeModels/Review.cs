using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketplace.Models.HomeModels
{
    public class Review
    {
        public int ReviewID { get; set; }
        public string ReviwerProfilePicture { get; set; }
        public string BuyerName { get; set; }
        public decimal Rating { get; set; }
        public string Comment { get; set; }
    }
}