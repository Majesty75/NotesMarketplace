using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketplace.Models.RegisteredUserModels
{
    public class BuyerRequestsModel
    {

        public BuyerRequestsModel()
        {
            BRequests = new List<BuyerRequest>();
        }

        public class BuyerRequest
        {
            public int DownloadID { get; set; }
            public string NoteTitle { get; set; }
            public string NoteCategory { get; set; }
            public string BuyerEmail { get; set; }
            public string BuyerPhone { get; set; }
            public string SellType { get; set; }
            public int Price { get; set; }
            public System.DateTime ReqTime { get; set; }
            public int NoteID { get; set; }
        }

        public List<BuyerRequest> BRequests { get; set; }
    }
}