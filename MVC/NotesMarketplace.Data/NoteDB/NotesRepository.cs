using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using NotesMarketplace.Models.HomeModels;
using NotesMarketplace.Models.RegisteredUserModels;
using System.IO;
using System.Diagnostics;

namespace NotesMarketplace.Data.NoteDB
{
    /*
    *  ------ NoteStatus ------
    *  0: Draft
    *  1: Submitted
    *  2: In Review
    *  3: Published
    *  4: Rejected
    *  5: Unpublished
    */

    public class NotesRepository
    {
        public static ListNotes GetAllAvailableNotes(NoteModel Nm,int Status = 3)
        {
            ListNotes AvailableNotes = new ListNotes();
            AvailableNotes.Notes = new List<NoteModel>();  
            using (NotesMarketPlaceEntities context = new NotesMarketPlaceEntities())
            {
                var NotesInDB = context.Notes.Where(n => n.NoteStatus == Status && n.IsActive == true).ToList();
                if(NotesInDB != null && Nm.IsItSearchAndFilter)
                {
                    if(Nm.Title != null)
                    {
                        NotesInDB = NotesInDB.Where(n => {return new CultureInfo("en-IN").CompareInfo.IndexOf(n.Title, Nm.Title, CompareOptions.IgnoreCase) >= 0; }).ToList();
                    }
                    if (Nm.Type != null)
                        NotesInDB = NotesInDB.Where(n => n.NoteType1.TypeName == Nm.Type).ToList();
                    if (Nm.Category != null)
                        NotesInDB = NotesInDB.Where(n => n.NoteCategory.CategoryName == Nm.Category).ToList();
                    if (Nm.Institution != null)
                        NotesInDB = NotesInDB.Where(n => n.University == Nm.Institution).ToList();
                    if (Nm.CourseName != null)
                        NotesInDB = NotesInDB.Where(n => n.Course == Nm.CourseName).ToList();
                    if (Nm.Country != null)
                        NotesInDB = NotesInDB.Where(n => n.Country1.CountryName == Nm.Country).ToList();
                    if (Nm.rating != null)
                        NotesInDB = NotesInDB.Where(n => n.NotesReviews.Count != 0 ? (int)n.NotesReviews.Average(r => r.Rating) >= Nm.rating : false).ToList();
                }

                foreach (Note n in NotesInDB)
                {
                    AvailableNotes.Notes.Add(
                        new NoteModel()
                        {
                            NoteId = n.NoteID,
                            Title = n.Title,
                            Category = n.NoteCategory.CategoryName,
                            Type = n.NoteType1.TypeName,
                            Description = n.NoteDescription,
                            DisplayPicture = n.DisplayPicture,
                            Price = n.Price,
                            Institution = n.University,
                            Country = context.Countries.FirstOrDefault(c => c.CountryID == n.Country).CountryName,
                            CourseCode = n.CourseCode,
                            CourseName = n.Course,
                            Professor = n.Professor,
                            Pages = n.NumberOfPages,
                            Approved = n.PublishedDate != null ? (System.DateTime)n.PublishedDate : System.DateTime.MinValue,
                            rating = n.NotesReviews.Count != 0 ? (int)n.NotesReviews.Average(r => r.Rating) : 0,
                            Reports = n.NotesReports.Count,
                            Reviews = GetReviews(n.NoteID),
                            Preview = n.Preview
                        }
                    );
                }
            }
            return AvailableNotes;
        }

        public static NoteModel GetNoteDetailsById(int Noteid)
        {
            using (NotesMarketPlaceEntities context = new NotesMarketPlaceEntities())
            {
                Note note = context.Notes.FirstOrDefault(n => n.NoteID == Noteid);
                if (note == null)
                    return null;
                NoteModel Nm = new NoteModel()
                {
                    NoteId = note.NoteID,
                    Title = note.Title,
                    Category = note.NoteCategory.CategoryName,
                    Type = note.NoteType1.TypeName,
                    Description = note.NoteDescription,
                    DisplayPicture = note.DisplayPicture,
                    Price = note.Price,
                    Institution = note.University != null ? note.University : "N/A",
                    Country = context.Countries.FirstOrDefault(c => c.CountryID == note.Country).CountryName,
                    CourseCode = note.CourseCode != null ? note.CourseCode : "N/A" ,
                    CourseName = note.Course != null ? note.Course : "N/A",
                    Professor = note.Professor != null ? note.Professor : "N/A",
                    Pages = note.NumberOfPages != null ? note.NumberOfPages : 0,
                    Approved = note.PublishedDate != null ? (System.DateTime)note.PublishedDate : System.DateTime.MinValue,
                    rating = note.NotesReviews.Count != 0 ? (int)note.NotesReviews.Average(r => r.Rating) : 0,
                    Reports = note.NotesReports.Count,
                    Reviews = GetReviews(note.NoteID),
                    Preview = note.Preview,
                    SellerID = note.SellerID,
                    Status = note.NoteStatus,
                    SellFor = note.IsPaid
                };
                return Nm;
            }
        }

        public static List<Review> GetReviews(int NoteID)
        {
            using (NotesMarketPlaceEntities context = new NotesMarketPlaceEntities())
            {
                var Reviews = context.NotesReviews.Include("User")
                    .Where(r => r.NoteID == NoteID);
                List<Review> ReturnObject = new List<Review>();
                if (Reviews != null)
                {
                    foreach(NotesReview nr in Reviews)
                    {
                        ReturnObject.Add(new Review()
                        {
                            ReviewID = nr.ReviewID,
                            BuyerName = nr.User.FirstName + " " + nr.User.LastName,
                            Rating = nr.Rating,
                            Comment = nr.Comment,
                            ReviwerProfilePicture = nr.User.UserProfiles.FirstOrDefault().ProfilePicture != null ?  nr.User.UserProfiles.FirstOrDefault().ProfilePicture : "/Content/SystemConfig/DefaultUserProfile.png"
                        });
                    }
                }
                return ReturnObject;
            }
        }


        //Adds New Note to Database
        public static int AddNote(NoteModel Nm, int UID, string AppRoot)
        {
            using(NotesMarketPlaceEntities context = new NotesMarketPlaceEntities())
            {
                Note note = new Note()
                {
                    Title = Nm.Title,
                    Category = context.NoteCategories.FirstOrDefault(n => n.CategoryName == Nm.Category).CategoryID,
                    NoteType = context.NoteTypes.FirstOrDefault(n => n.TypeName == Nm.Type).TypeID,
                    NoteDescription = Nm.Description,
                    DisplayPicture = Nm.DisplayPicture,
                    Price = Nm.SellFor ? Nm.Price : 0,
                    University = Nm.Institution,
                    Country = context.Countries.FirstOrDefault(c => c.CountryName == Nm.Country).CountryID,
                    CourseCode = Nm.CourseCode,
                    Course = Nm.CourseName,
                    Professor = Nm.Professor,
                    NumberOfPages = Nm.Pages,
                    SellerID = UID,
                    NoteStatus = Nm.Status,
                    IsActive = true,
                    IsPaid = Nm.SellFor,
                    CreatedBy = UID,
                    ModifiedBy = UID,
                    CreatedDate = System.DateTime.Now,
                    ModifiedDate = System.DateTime.Now
                };

                context.Notes.Add(note);

                try
                {
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    return 0;
                }

                string filenames = "";
                int i = 1;

                //Saving Preview And Coverpage
                if (Nm.NotesCoverPage != null || Nm.PreviewFile != null)
                {
                    string ContentPath = AppRoot + "\\Content\\NotesData\\" + note.NoteID.ToString() + "\\";
                    Directory.CreateDirectory(ContentPath);
                    if (Nm.NotesCoverPage != null)
                    {
                        note.DisplayPicture = @"/Content/NotesData/" + note.NoteID.ToString() + @"/CoverPage-" + note.NoteID.ToString() + Path.GetExtension(Nm.NotesCoverPage.FileName);
                        Nm.NotesCoverPage.SaveAs(ContentPath + "CoverPage-" + note.NoteID.ToString() + Path.GetExtension(Nm.NotesCoverPage.FileName));
                    }

                    if (Nm.PreviewFile != null)
                    {
                        note.Preview = @"/Content/NotesData/" + note.NoteID.ToString() + @"/Preview-" + note.NoteID.ToString() + Path.GetExtension(Nm.PreviewFile.FileName);
                        Nm.PreviewFile.SaveAs(ContentPath + "Preview-" + note.NoteID.ToString() + Path.GetExtension(Nm.PreviewFile.FileName));
                    }
                }


                string Serverpath = AppRoot + "\\Members\\" + UID + "\\Notes\\" + note.NoteID + "\\";
                Directory.CreateDirectory(Serverpath);

                foreach (HttpPostedFileBase file in Nm.NotesFiles)
                {
                    if (file != null)
                    {
                        file.SaveAs(Serverpath + "Attachment-" + note.NoteID + "-" + i.ToString() + ".pdf");
                        filenames += "Attachment-" + note.NoteID + "-" + i.ToString() + ".pdf;";
                        i++;
                    }
                }

                NotesAttachment Na = new NotesAttachment()
                {
                    NoteID = note.NoteID,
                    FilePath = UID.ToString() + @"/Notes/" + note.NoteID + @"/",
                    FilesName = filenames,
                    CreatedBy = UID,
                    ModifiedBy = UID,
                    IsActive = true,
                    ModifiedDate = System.DateTime.Now,
                    CreatedDate = System.DateTime.Now
                };

                context.NotesAttachments.Add(Na);

                try
                {
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    return 0;
                }

                return note.NoteID;
            }
        }

        public static List<DashboardModel.InProgressNotesClass> GetInProgressNotes(int UserID)
        {
            using(var context = new NotesMarketPlaceEntities())
            {

                var NotesInDB = context.Notes.Where(n => n.SellerID == UserID && (n.NoteStatus <= 2) && n.IsActive);

                if (NotesInDB.Count() == 0)
                {
                    return null;
                }

                var InProgressNotes = new List<DashboardModel.InProgressNotesClass>();

                foreach(var Note in NotesInDB)
                {

                    string NoteStatus = null;
                    if (Note.NoteStatus == 0)
                        NoteStatus = "Draft";
                    else if (Note.NoteStatus == 1)
                        NoteStatus = "Submitted";
                    else
                        NoteStatus = "In Review";

                    InProgressNotes.Add(
                        new DashboardModel.InProgressNotesClass() { 
                            NoteID = Note.NoteID,
                            NoteTitle = Note.Title,
                            NoteCategory = context.NoteCategories.FirstOrDefault(nc => nc.CategoryID == Note.Category).CategoryName,
                            Status = NoteStatus,
                            AddedDate = Note.ModifiedDate
                    });
                }

                return InProgressNotes;
            }
        }

        public static List<DashboardModel.PublishedNotesClass> GetPublishedNotes(int UserID)
        {
            using (var context = new NotesMarketPlaceEntities())
            {

                var NotesInDB = context.Notes.Where(n => n.SellerID == UserID && (n.NoteStatus == 3) && n.IsActive);

                if (NotesInDB.Count() == 0)
                {
                    return null;
                }

                var PublishedNotes = new List<DashboardModel.PublishedNotesClass>();

                foreach (var Note in NotesInDB)
                {
                    PublishedNotes.Add(
                        new DashboardModel.PublishedNotesClass()
                        {
                            NoteID = Note.NoteID,
                            NoteTitle = Note.Title,
                            NoteCategory = context.NoteCategories.FirstOrDefault(nc => nc.CategoryID == Note.Category).CategoryName,
                            AddedDate = Note.ModifiedDate,
                            Price = Note.Price??0,
                            SellType = Note.IsPaid ? "Paid" : "Free"
                        });
                }

                return PublishedNotes;
            }
        }

        public static RejectedNotes GetRejectedNotes(int UserID)
        {
            using (var context = new NotesMarketPlaceEntities())
            {
                var RejectedNotesInDB = context.Notes.Where(n => n.SellerID == UserID && n.NoteStatus == 4);
                if (RejectedNotesInDB.Count() == 0)
                {
                    return new RejectedNotes();
                }

                var rejectedNotes = new RejectedNotes(); 

                foreach(var note in RejectedNotesInDB)
                {
                    rejectedNotes.RejectedNotesList.Add(new RejectedNotes.RejectedNote()
                    {
                        NoteID = note.NoteID,
                        NoteTitle = note.Title,
                        Remarks = note.AdminRemarks ?? "N/A",
                        NoteCategory = note.NoteCategory.CategoryName
                    });
                }

                return rejectedNotes;
            }
        }

        /* Get Rating variables if already rated and check if allowed to rate note, it retuns rate model with note id = -1
         * if not allowed and return null if reviews not exists
         *  */
        public static RatingModel GetNoteRating(int NoteID, int UserID)
        {
            using (var context = new NotesMarketPlaceEntities())
            {
                var IsAllowedToRate = context.Downloads.Any(dwn => dwn.NoteID == NoteID && dwn.BuyerID == UserID && dwn.IsAllowed && dwn.IsDownloaded);
                if (!IsAllowedToRate)
                {
                    return new RatingModel(){ NoteID = -1 }; //Not allowed return RatingModel with note id = -1
                }
                else
                {
                    var Rating = context.NotesReviews.FirstOrDefault(r => r.NoteID == NoteID && r.BuyerID == UserID);
                    if(Rating == null)
                    {
                        return null; //rating not found
                    }
                    else
                    {
                        return new RatingModel()
                        {
                            NoteID = Rating.NoteID,
                            Stars = (int)Rating.Rating,
                            Comment = Rating.Comment
                        };
                    }
                }
                
            }
        }

        /* Submits Note Rating 
         * Returns false if fails or not allowed to rate
         * Return True if successfully submitted or updated
         * */
        public static bool SubmitNoteRating(RatingModel Rm, int UserID)
        {
            using (var context = new NotesMarketPlaceEntities())
            {
                var IsAllowedToRate = context.Downloads.FirstOrDefault(dwn => dwn.NoteID == Rm.NoteID && dwn.BuyerID == UserID && dwn.IsAllowed && dwn.IsDownloaded);
                if (IsAllowedToRate == null)
                {
                    return false; //Not allowed
                }
                else
                {
                    NotesReview NR = context.NotesReviews.FirstOrDefault(nr => nr.NoteID == Rm.NoteID && nr.BuyerID == UserID);
                    if (NR != null)
                    {
                        NR.Comment = Rm.Comment;
                        NR.Rating = Rm.Stars;
                        NR.ModifiedBy = UserID;
                        NR.ModifiedDate = System.DateTime.Now;
                    }
                    else
                    {
                        context.NotesReviews.Add(new NotesReview()
                        {
                            NoteID = Rm.NoteID,
                            BuyerID = UserID,
                            Rating = Rm.Stars,
                            Comment = Rm.Comment,
                            CreatedBy = UserID,
                            CreatedDate = System.DateTime.Now,
                            ModifiedBy = UserID,
                            ModifiedDate = System.DateTime.Now,
                            DownloadID = IsAllowedToRate.DownloadID
                        });
                    }
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        return false;
                    }
                    return true;
                }
            }
        }

        /* Get Report variables if already reported and check if allowed to report note, it retuns rate model with note id = -1
         * if not allowed and return reportmodel with note title if report not exists
         *  */
        public static ReportNoteModel GetNoteReport(int NoteID, int UserID)
        {
            using (var context = new NotesMarketPlaceEntities())
            {
                var IsAllowedToRate = context.Downloads.Any(dwn => dwn.NoteID == NoteID && dwn.BuyerID == UserID && dwn.IsAllowed && dwn.IsDownloaded);
                if (!IsAllowedToRate)
                {
                    return new ReportNoteModel() { NoteID = -1 }; //Not allowed return RatingModel with note id = -1
                }
                else
                {
                    var Report = context.NotesReports.FirstOrDefault(r => r.NoteID == NoteID && r.BuyerID == UserID);
                    if (Report == null)
                    {
                        return new ReportNoteModel()
                        {
                            NoteTitle = context.Notes.FirstOrDefault(n => n.NoteID == NoteID).Title
                        }; //report not found
                    }
                    else
                    {
                        return new ReportNoteModel()
                        {
                            NoteID = Report.NoteID,
                            Remarks = Report.Remarks,
                            NoteTitle = context.Notes.FirstOrDefault(n => n.NoteID == NoteID).Title
                        };
                    }
                }

            }
        }

        /* Submits Note Report
         * Returns false if fails or not allowed to report or already reported
         * Return True if successfully submitted 
         * */
        public static bool SubmitNoteReport(ReportNoteModel Rm, int UserID)
        {
            using (var context = new NotesMarketPlaceEntities())
            {
                var IsAllowedToRate = context.Downloads.FirstOrDefault(dwn => dwn.NoteID == Rm.NoteID && dwn.BuyerID == UserID && dwn.IsAllowed && dwn.IsDownloaded);
                if (IsAllowedToRate == null)
                {
                    return false; //Not allowed
                }
                else
                {
                    NotesReport NR = context.NotesReports.FirstOrDefault(nr => nr.NoteID == Rm.NoteID && nr.BuyerID == UserID);
                    if (NR != null) 
                    {
                        return false; // cause one can not change report
                    }
                    else
                    {
                        context.NotesReports.Add(new NotesReport()
                        {
                            NoteID = Rm.NoteID,
                            BuyerID = UserID,
                            Remarks = Rm.Remarks,
                            CreatedBy = UserID,
                            CreatedDate = System.DateTime.Now,
                            ModifiedBy = UserID,
                            ModifiedDate = System.DateTime.Now,
                            DownloadID = IsAllowedToRate.DownloadID
                        });

                        var Seller = context.Users.FirstOrDefault(u => u.UserID == IsAllowedToRate.SellerID);

                        Rm.SellerName = Seller.FirstName + " " + Seller.LastName;

                        Rm.NoteTitle = IsAllowedToRate.NoteTitle;
                    }
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        return false;
                    }
                    return true;
                }
            }
        }


        public static bool EditNote(int UserID, NoteModel Nm, string AppRoot)
        {
            using (var context = new NotesMarketPlaceEntities())
            {
                var NoteInDB = context.Notes.FirstOrDefault(n => n.NoteID == Nm.NoteId && n.SellerID == UserID);
                if(NoteInDB == null)
                {
                    return false;
                }
                else if(NoteInDB.NoteStatus != 0)
                {
                    return false;
                }

                NoteInDB.Title = Nm.Title;
                NoteInDB.Category = context.NoteCategories.FirstOrDefault(n => n.CategoryName == Nm.Category).CategoryID;
                NoteInDB.NoteType = context.NoteTypes.FirstOrDefault(n => n.TypeName == Nm.Type).TypeID;
                NoteInDB.NoteDescription = Nm.Description;
                NoteInDB.Price = Nm.SellFor ? Nm.Price : 0;
                NoteInDB.University = Nm.Institution;
                NoteInDB.Country = context.Countries.FirstOrDefault(c => c.CountryName == Nm.Country).CountryID;
                NoteInDB.CourseCode = Nm.CourseCode;
                NoteInDB.Course = Nm.CourseName;
                NoteInDB.Professor = Nm.Professor;
                NoteInDB.NumberOfPages = Nm.Pages;
                NoteInDB.NoteStatus = Nm.Status;
                NoteInDB.IsPaid = Nm.SellFor;
                NoteInDB.ModifiedBy = UserID;
                NoteInDB.ModifiedDate = System.DateTime.Now;


                string ContentPath = AppRoot + "\\Content\\NotesData\\" + NoteInDB.NoteID.ToString() + "\\";
                Directory.CreateDirectory(ContentPath);

                //Saving Preview And Coverpage
                if (Nm.NotesCoverPage != null || Nm.PreviewFile != null)
                {
                    if (Nm.NotesCoverPage != null)
                    {
                        NoteInDB.DisplayPicture = @"/Content/NotesData/" + NoteInDB.NoteID.ToString() + @"/CoverPage-" + NoteInDB.NoteID.ToString() + Path.GetExtension(Nm.NotesCoverPage.FileName);
                        Nm.NotesCoverPage.SaveAs(ContentPath + "CoverPage-" + NoteInDB.NoteID.ToString() + Path.GetExtension(Nm.NotesCoverPage.FileName));
                    }

                    if (Nm.PreviewFile != null)
                    {
                        Nm.PreviewFile.SaveAs(ContentPath + "Preview-" + NoteInDB.NoteID.ToString() + ".pdf");
                    }
                }

                if (Nm.NotesFiles != null && Nm.NotesFiles[0] != null)
                {
                    string filenames = "";
                    int i = 1;

                    string Serverpath = AppRoot + "\\Members\\" + UserID + "\\Notes\\" + NoteInDB.NoteID + "\\";
                    Directory.CreateDirectory(Serverpath);

                    DirectoryInfo Dir = new DirectoryInfo(Serverpath);
                    foreach (FileInfo file in Dir.EnumerateFiles())
                        file.Delete();

                    foreach (HttpPostedFileBase file in Nm.NotesFiles)
                    {
                        if (file != null)
                        {
                            file.SaveAs(Serverpath + "Attachment-" + NoteInDB.NoteID + "-" + i.ToString() + ".pdf");
                            filenames += "Attachment-" + NoteInDB.NoteID + "-" + i.ToString() + ".pdf;";
                            i++;
                        }
                    }

                    NotesAttachment Na = context.NotesAttachments.FirstOrDefault(n => n.NoteID == NoteInDB.NoteID);

                    Na.FilesName = filenames;
                    Na.ModifiedBy = UserID;
                    Na.ModifiedDate = System.DateTime.Now;
                }

                try
                {
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    return false;
                }
                return true;
            }  
        }

        /// <summary>
        /// Deletes Note from database and file from file system only if owner is requesting and is a draft
        /// </summary>
        /// <param name="NoteID">Noteif of note to be deleted</param>
        /// <param name="UserID">User id of user requesting deletion</param>
        /// <param name="AppRoot">Actual Root of web app on file system</param>
        /// <returns>True if succeed in deletion, false otherwise</returns>
        public static bool DeleteNote(int NoteID, int UserID, string AppRoot)
        {
            using (var context = new NotesMarketPlaceEntities())
            {
                var note = context.Notes.FirstOrDefault(n => n.NoteID == NoteID && n.SellerID == UserID && n.NoteStatus == 0);
                if (note == null)
                {
                    return false;
                }

                else
                {
                    context.NotesAttachments.Remove(context.NotesAttachments.FirstOrDefault(n => n.NoteID == NoteID));
                    context.Notes.Remove(note);
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        return false;
                    }


                    //Deleting Note Preview, Note Cover, Note pdfs.

                    string Serverpath = AppRoot + "\\Members\\" + UserID + "\\Notes\\" + NoteID;

                    string ContentPath = AppRoot + "\\Content\\NotesData\\" + NoteID;

                    try
                    {
                        Directory.Delete(Serverpath, true);
                        if(Directory.Exists(ContentPath))
                            Directory.Delete(ContentPath, true);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Deleting Failed : {0}", e.Message);
                    }
                }

                return true;
            }
        }
    }

}