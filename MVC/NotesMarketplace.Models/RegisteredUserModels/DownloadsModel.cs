using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketplace.Models.RegisteredUserModels
{
    public class DownloadsModel
    {
        public DownloadsModel()
        {
            DownloadProperty = new List<InnerClassDownload>();
        }
        public class InnerClassDownload
        {
            public int DownloadID { get; set; }
            public int NoteID { get; set; }
            public UserModel Seller { get; set; }
            public UserModel Buyer { get; set; }
            public bool IsAllowed { get; set; }
            public int AttachmentsID { get; set; }
            public bool IsDownloaded { get; set; }
            public Nullable<System.DateTime> DownloadDate { get; set; }
            public bool IsPaid { get; set; }
            public string NoteTitle { get; set; }
            public string NoteCategory { get; set; }
            public Nullable<decimal> PurchasedPrice { get; set; }

            public Nullable<System.DateTime>  RequestTime { get; set; }
        }

        public List<InnerClassDownload> DownloadProperty { get; set; }
    }
}