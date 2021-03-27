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

        /* Get Note Attachments folder location as string if user authenticated to download note.
         * if it submitted buyer request it return string "BuyerRequestSubmitted"
         * otherwise return null if note not found,
         * else if book is free or user has purchased the book it return attachment path
         * It also return notetitle as first element of array.
         */
        public static string[] NoteAttachments(int NoteID, int BuyerID)
        {
            string ReturnPath = null;
            string NoteTitle = null;
            using (NotesMarketPlaceEntities context = new NotesMarketPlaceEntities())
            {

                Note note = context.Notes.FirstOrDefault(n => n.NoteID == NoteID);

                /* if note not found return null */
                if (note == null)
                {
                    return null;
                }

                /* if note is free or user is allowed to download note or user is book owner or user is admin then return attachment paths*/
                else if (
                    !note.IsPaid
                    || (context.Downloads.Any(d => d.NoteID == NoteID && d.BuyerID == BuyerID && d.IsAllowed))
                    || BuyerID == note.SellerID
                    || context.Users.FirstOrDefault(u => u.UserID == BuyerID).RoleID < 3
                )
                {
                    NotesAttachment notesAttachmentDB = context.NotesAttachments.FirstOrDefault(na => na.NoteID == NoteID);
                    if (notesAttachmentDB == null)
                    {
                        return null;
                    }
                    else
                    {
                        ReturnPath = notesAttachmentDB.FilePath;
                    }

                    //update download entry
                    if (context.Downloads.Any(d => d.NoteID == NoteID && d.BuyerID == BuyerID && d.IsAllowed))
                    {
                        Download dwn = context.Downloads.FirstOrDefault(d => d.NoteID == NoteID && d.BuyerID == BuyerID && d.IsAllowed);
                        dwn.IsDownloaded = true;
                        dwn.DownloadDate = System.DateTime.Now;
                        dwn.ModifiedBy = BuyerID;
                        dwn.ModifiedDate = System.DateTime.Now;
                    }

                    //if download entry not found and book is free create new entry
                    else if (!note.IsPaid)
                    {
                        Download dwn = new Download()
                        {
                            NoteID = NoteID,
                            BuyerID = BuyerID,
                            SellerID = note.SellerID,
                            IsAllowed = true,
                            IsDownloaded = true,
                            DownloadDate = System.DateTime.Now,
                            ModifiedBy = BuyerID,
                            ModifiedDate = System.DateTime.Now,
                            CreatedBy = BuyerID,
                            CreatedDate = System.DateTime.Now,
                            IsPaid = note.IsPaid,
                            NoteCategory = note.Category,
                            NoteTitle = note.Title,
                            PurchasedPrice = note.Price,
                            AttachmentsID = context.NotesAttachments.FirstOrDefault(a => a.NoteID == NoteID).AttachementID
                        };

                        context.Downloads.Add(dwn);
                    }
                }

                // if there is already a buyer request that seller has not accepted
                else if (context.Downloads.Any(d => d.NoteID == NoteID && d.BuyerID == BuyerID && !d.IsAllowed))
                {
                    ReturnPath = "BuyerRequestAlreadySubmitted";
                }

                //if note is paid and there are no buyer requests create new one
                else if (note.IsPaid)
                {
                    Download dwn = new Download()
                    {
                        NoteID = NoteID,
                        BuyerID = BuyerID,
                        SellerID = note.SellerID,
                        IsAllowed = false,
                        IsDownloaded = false,
                        DownloadDate = null,
                        ModifiedBy = BuyerID,
                        ModifiedDate = System.DateTime.Now,
                        CreatedBy = BuyerID,
                        CreatedDate = System.DateTime.Now,
                        IsPaid = note.IsPaid,
                        NoteCategory = note.Category,
                        NoteTitle = note.Title,
                        PurchasedPrice = note.Price,
                        AttachmentsID = context.NotesAttachments.FirstOrDefault(a => a.NoteID == NoteID).AttachementID
                    };
                    context.Downloads.Add(dwn);

                    //return confirmation that buyer request is submitted; seller email; seller full name

                    User seller = context.Users.FirstOrDefault(u => u.UserID == note.SellerID);

                    ReturnPath = "BuyerRequestSubmitted;"+ seller.Email + ';' + seller.FirstName + ' ' + seller.LastName;
                }
                NoteTitle = context.Notes.FirstOrDefault(n => n.NoteID == NoteID).Title;
                context.SaveChanges();
            }
            return new string[] { NoteTitle, ReturnPath };
        }

    }
}