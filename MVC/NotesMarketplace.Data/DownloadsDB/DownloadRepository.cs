using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NotesMarketplace.Models.RegisteredUserModels;

namespace NotesMarketplace.Data.DownloadsDB
{
    public class DownloadRepository
    {

        /* DownloadStatus :
        * 0 : Buyer Requests (where UserID = Seller ID && IsAllowed != true)
        * 1 : My Downloads (where UserID = Buyer ID && IsAllowed = true)
        * 2 : Sold Notes (where UserID = Seller ID && IsAllowed = true)
        */

        public static DownloadsModel GetDownloads(int UserID, int downloadStatus)
        {
            using (NotesMarketPlaceEntities context = new NotesMarketPlaceEntities())
            {
                if (!context.Downloads.Any(d => d.SellerID == UserID || d.BuyerID == UserID))
                    return null;

                DownloadsModel dm = new DownloadsModel();
                List<Download> dls = null;

                if (downloadStatus == 0)
                {
                    dls = context.Downloads.Where(d => d.SellerID == UserID && !d.IsAllowed).ToList();
                }
                else if (downloadStatus == 1)
                {
                    dls  = context.Downloads.Where(d => d.BuyerID == UserID && d.IsAllowed).ToList();
                }
                else if(downloadStatus == 2){
                    dls = context.Downloads.Where(d => d.SellerID == UserID && d.IsAllowed).ToList();
                }


                //Current User is user calling this function and other party is seller/buyer involved in transaction
                User CurrentUser = context.Users.FirstOrDefault(u => u.UserID == UserID);
                User OtherParty;

                UserModel CurrentUserInfo = new UserModel()
                {
                    UserID = UserID,
                    FirstName = CurrentUser.FirstName,
                    LastName = CurrentUser.LastName,
                    Email = CurrentUser.Email
                };

                UserModel OtherPartyInfo;

                foreach (Download d in dls)
                {
                    int OtherPartyID = downloadStatus != 1 ? d.BuyerID : d.SellerID; 

                    OtherParty = context.Users.FirstOrDefault(u => u.UserID == OtherPartyID);

                    if(OtherParty == null)
                    {
                        continue;
                    }

                    OtherPartyInfo = new UserModel()
                    {
                        UserID = OtherPartyID,
                        FirstName = OtherParty.FirstName,
                        LastName = OtherParty.LastName,
                        Email = OtherParty.Email
                    };

                    dm.DownloadProperty.Add(new DownloadsModel.InnerClassDownload()
                    {
                        DownloadID = d.DownloadID,
                        Seller = downloadStatus != 1 ? CurrentUserInfo : OtherPartyInfo,
                        Buyer = downloadStatus != 1 ? OtherPartyInfo : CurrentUserInfo,
                        DownloadDate = d.DownloadDate,
                        IsAllowed = d.IsAllowed,
                        AttachmentsID = d.AttachmentsID,
                        IsDownloaded = d.IsDownloaded,
                        IsPaid = d.IsPaid,
                        PurchasedPrice = d.PurchasedPrice,
                        NoteCategory = d.NoteCategory1.CategoryName,
                        NoteTitle = d.NoteTitle,
                        NoteID = d.NoteID,
                        RequestTime = d.ModifiedDate
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
        public static string[] GetNoteAttachments(int NoteID, int BuyerID)
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

                    //if download entry not found and book is free create new entry only if owner is not downloading note
                    else if (!note.IsPaid && BuyerID != note.SellerID)
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

        public static Tuple<int, decimal, int, int, int> GetUserStats(int UserID)
        {
            using (var context = new NotesMarketPlaceEntities())
            {
                int NotesSold = context.Downloads.Count(dwn => dwn.SellerID == UserID && dwn.IsAllowed);

                var MoneyEarned = context.Downloads.Where(dwn => dwn.SellerID == UserID && dwn.IsAllowed && dwn.IsPaid).Sum(dwn => dwn.PurchasedPrice);

                int Downloads = context.Downloads.Count(dwn => dwn.BuyerID == UserID && dwn.IsAllowed);

                int RejectedNotes = context.Notes.Count(n => n.SellerID == UserID && n.NoteStatus == 4);

                int BuyerRequests = context.Downloads.Count(dwn => dwn.SellerID == UserID && !dwn.IsAllowed);

                return Tuple.Create<int, decimal, int, int, int>(NotesSold, MoneyEarned??0 , Downloads, RejectedNotes, BuyerRequests);
            }
        }

    }
}