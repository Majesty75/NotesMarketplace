using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using NotesMarketplace.Models.HomeModels;
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
                            Approved = (System.DateTime)n.PublishedDate,
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
                    Institution = note.University,
                    Country = context.Countries.FirstOrDefault(c => c.CountryID == note.Country).CountryName,
                    CourseCode = note.CourseCode,
                    CourseName = note.Course,
                    Professor = note.Professor,
                    Pages = note.NumberOfPages,
                    Approved = (System.DateTime)note.PublishedDate,
                    rating = note.NotesReviews.Count != 0 ? (int)note.NotesReviews.Average(r => r.Rating) : 0,
                    Reports = note.NotesReports.Count,
                    Reviews = GetReviews(note.NoteID),
                    Preview = note.Preview
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
                    Price = Nm.Price,
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
                        file.SaveAs(Serverpath + "Attachment-" + i.ToString() + ".pdf");
                        filenames += "Attachment-" + i.ToString() + "$pdf;";
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
    }
}