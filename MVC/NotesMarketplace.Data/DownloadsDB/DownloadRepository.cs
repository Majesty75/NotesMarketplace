using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NotesMarketplace.Models.RegisteredUserModels;

namespace NotesMarketplace.Data.DownloadsDB
{
    public class DownloadRepository
    {

        // RequestOrDownloads : 1 for Buyer Reuests : 0 for Downloads
        public static DownloadsModel GetDownloads(int UID, int RequestOrDownloads )
        {
            using (NotesMarketPlaceEntities context = new NotesMarketPlaceEntities())
            {
                if (!context.Downloads.Any(d => d.SellerID == UID || d.BuyerID == UID))
                    return null;

                DownloadsModel dm = new DownloadsModel();
                List<Download> dls = null;

                if (RequestOrDownloads == 1)
                {
                    dls = context.Downloads.Where(d => d.SellerID == UID && !d.IsAllowed).ToList();
                }
                else if (RequestOrDownloads == 0)
                {
                    dls  = context.Downloads.Where(d => d.BuyerID == UID).ToList();
                }
                foreach (Download d in dls)
                {
                    dm.DownloadProperty.Add(new DownloadsModel.InnerClassDownload()
                    {
                        DownloadID = d.DownloadID,
                        SellerID = d.SellerID,
                        BuyerID = d.BuyerID,
                        DownloadDate = d.DownloadDate,
                        IsAllowed = d.IsAllowed,
                        AttachmentsID = d.AttachmentsID,
                        IsDownloaded = d.IsDownloaded,
                        IsPaid = d.IsPaid,
                        PurchasedPrice = d.PurchasedPrice,
                        NoteCategory = d.NoteCategory1.CategoryName,
                        NoteTitle = d.NoteTitle,
                        NoteID = d.NoteID,
                        RequestTime = d.CreatedDate
                    });
                }
                return dm;
            }
        }

        public static int AllowDownload(int DownloadId, int UID)
        {
            using(NotesMarketPlaceEntities context = new NotesMarketPlaceEntities())
            {
                if (!context.Downloads.Any(d => d.DownloadID == DownloadId && UID == d.SellerID))
                    return 0;

                Download dwn = context.Downloads.FirstOrDefault(d => d.DownloadID == DownloadId);
                dwn.ModifiedBy = UID;
                dwn.ModifiedDate = System.DateTime.Now;
                dwn.IsAllowed = true;
                context.SaveChanges();
                return dwn.BuyerID;
            }
        }
    }
}