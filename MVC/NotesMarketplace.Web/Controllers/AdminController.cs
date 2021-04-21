using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using NotesMarketplace.Models.AdminModels;
using NotesMarketplace.Data.DownloadsDB;
using NotesMarketplace.Data.NoteDB;
using NotesMarketplace.Data.UserDB;
using NotesMarketplace.Data.SystemConfigDB;
using NotesMarketplace.Models.HomeModels;
using NotesMarketplace.Models.SystemConfigModels;
using NotesMarketplace.Models.RegisteredUserModels;
using NotesMarketplace.Web.Mailer;
using NotesMarketplace.Models.MailModels;

namespace NotesMarketplace.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin, SubAdmin")]
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("AdminDashboard");
        }

        [Route("Admin/AdminDashboard")]
        public ActionResult AdminDashboard()
        {

            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/Admin/AdminDashboard" });

            ViewBag.Title = "AdminDashboard";
            ViewBag.Authorized = true;
            ViewBag.LoadAjaxJS = true;
            ViewBag.LoadValidationScript = true;

            AdminDashboardModel Dashboard = new AdminDashboardModel();

            Dashboard.NoteInReview = AdminNoteRepository.GetCountNotesInReview();

            Dashboard.Downloads = AdminDownloadRepository.CountNewDownloads();

            Dashboard.NewUsers = AdminUserRepository.CountNewUsers();

            Dashboard.PublishedNotes = AdminNoteRepository.GetPublishedNotes();

            return View(Dashboard);
        }

        public ActionResult Unpublish(int NoteID)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/Admin/NoteActions/" + NoteID + "/Approve" });

            int UserID = Convert.ToInt32(User.Identity.Name);

            NoteModel Note = NotesRepository.GetNoteDetailsById(NoteID);

            if (Note == null ? true : Note.Status != 3)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            }

            RejectNoteModel Rn = new RejectNoteModel()
            {
                NoteID = NoteID,
                NoteCategory = Note.Category,
                NoteTitle = Note.Title
            };


            return PartialView("~/Views/Admin/NoteViews/_UnpublishNotePopup.cshtml", Rn);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Unpublish(int NoteID, RejectNoteModel Rn)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/Admin/AdminDashBoard" });
            }

            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Something went wrong";
                return RedirectToAction("AdminDashBoard", "Admin");
            }

            int UserID = Convert.ToInt32(User.Identity.Name);

            if (AdminNoteRepository.UnpublishNote(NoteID, UserID, Rn.Remarks))
            {
                TempData["Message"] = "Note Unpublished";

                NoteModel Note = NotesRepository.GetNoteDetailsById(NoteID);

                UserProfileModel Seller = UserRepository.GetUserData(Note.SellerID);

                ViewBag.SellerName = Seller.User.FirstName + " " + Seller.User.LastName;
                ViewBag.NoteTitle = Note.Title;
                ViewBag.Remarks = Rn.Remarks;

                SendMail.SendEmail(new EmailModel()
                {
                    EmailTo = new string[] { Seller.User.Email },
                    EmailSubject = "Sorry! We need to remove your notes from our portal",
                    EmailBody = this.getHTMLViewAsString("~/Views/Email/NoteUnpublished.cshtml")
                });

                return RedirectToAction("AdminDashBoard", "Admin");
            }
            else
            {
                TempData["Message"] = "Unpublication of Note failed";
                return RedirectToAction("AdminDashBoard", "Admin");
            }
        }

        public ActionResult NoteDetails(String id)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/Admin/NoteDetails/" + id });

            string NoteId = id;
            ViewBag.Title = "NotesDetails";
            ViewBag.Authorized = true;

            if (String.IsNullOrEmpty(NoteId))
                return new HttpNotFoundResult();

            int UserID = 0;
            string[] UserRoles = null;

            if (Request.IsAuthenticated)
            {
                if (Session["UserID"] == null)
                    return RedirectToAction("Login", "Authentication");

                ViewBag.Authorized = true;

                UserID = Convert.ToInt32(User.Identity.Name);

                UserRoles = new RoleManager.NotesMarketPlaceRoleManager().GetRolesForUser(User.Identity.Name);
            }

            NoteModel Nm = NotesRepository.GetNoteDetailsById(Convert.ToInt32(NoteId));

            if (Nm == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);

            //Only show note details when notes is published or being accessed by owner or admins
            if (Nm.Status != 3 && (Request.IsAuthenticated && !(Nm.SellerID == UserID || UserRoles.Contains("SuperAdmin") || UserRoles.Contains("SubAdmin"))))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            }
            
            return View("~/Views/Admin/NoteViews/NoteDetails.cshtml", Nm);
        }

        public ActionResult DeleteReview(int NoteID, int ID)
        {
            int ReviewID = ID;

            if (NoteID == 0 || ID == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            bool Result = AdminDownloadRepository.DeleteReview(ReviewID);
            if (Result)
            {
                TempData["Message"] = "Review Deleted Successfully";
            }
            else
            {
                TempData["Message"] = "Review Deletion encountered some problem!";
            }

            return RedirectToAction(@"NoteDetails/"+NoteID ,"Admin");
        }

        [Route("Admin/NotesInReview")]
        public ActionResult NotesInReview()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/Admin/NotesInReview" });

            ViewBag.Title = "NotesInReview";
            ViewBag.Authorized = true;
            ViewBag.LoadAjaxJS = true;
            ViewBag.LoadValidationScript = true;

            AdminNotesInReviewModel Notes = new AdminNotesInReviewModel();

            Notes.NotesInReview = AdminNoteRepository.GetNotesInReviews();

            return View("~/Views/Admin/NoteViews/NotesInReview.cshtml", Notes);

        }

        public ActionResult Approve(int NoteID)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/Admin/NoteActions/" + NoteID + "/Approve" });

            int UserID = Convert.ToInt32(User.Identity.Name);

            if(AdminNoteRepository.ApproveNote(NoteID, UserID))
            {
                TempData["Message"] = "Note Approved.";
            }
            else
            {
                TempData["Message"] = "Note Approval Failed";
            }

            return RedirectToAction("NotesInReview", "Admin");
        }

        public ActionResult InReview(int NoteID)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/Admin/NoteActions/" + NoteID + "/InReview" });

            int UserID = Convert.ToInt32(User.Identity.Name);

            int result = AdminNoteRepository.MakeNoteInReview(NoteID, UserID);
            if (result == 1)
            {
                TempData["Message"] = "Note Status Changed To In Review";
            }
            else if(result == -1)
            {
                TempData["Message"] = "Note Is Already In Review";
            }
            else
            {
                TempData["Message"] = "Note Status Change failed";
            }

            return RedirectToAction("NotesInReview", "Admin");
        }


        [HttpGet]
        public ActionResult Reject(int NoteID)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/Admin/NotesInReview" });
            }

            int UserID = Convert.ToInt32(User.Identity.Name);

            NoteModel Note = NotesRepository.GetNoteDetailsById(NoteID);

            if (Note == null ? true : (Note.Status != 1 && Note.Status != 2))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            }

            RejectNoteModel Rn = new RejectNoteModel()
            {
                NoteID = NoteID,
                NoteCategory = Note.Category,
                NoteTitle = Note.Title
            };

            return PartialView("~/Views/Admin/NoteViews/_RejectPopupModal.cshtml", Rn);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reject(int NoteID,RejectNoteModel Rn)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/Admin/NotesInReview" });
            }

            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Something went wrong";
                return RedirectToAction("NotesInReview", "Admin");
            }

            int UserID = Convert.ToInt32(User.Identity.Name);

            if (AdminNoteRepository.RejectNote(NoteID, UserID, Rn.Remarks))
            {
                TempData["Message"] = "Note Rejected";
                return RedirectToAction("NotesInReview" ,"Admin");
            }
            else
            {
                TempData["Message"] = "Rejection of Note failed";
                return RedirectToAction("NotesInReview", "Admin");
            }
        }

        public ActionResult DownloadedNotes()
        {
            TempData["Message"] = " Work in Progress, Yet to be completed";
            return RedirectToAction("AdminDashBoard", "Admin");
        }

        public ActionResult Members()
        {
            TempData["Message"] = " Work in Progress, Yet to be completed";
            return RedirectToAction("AdminDashBoard", "Admin");
        }

        public ActionResult RejectedNotes()
        {
            TempData["Message"] = " Work in Progress, Yet to be completed";
            return RedirectToAction("AdminDashBoard", "Admin");
        }

        [Route("Admin/PublishedNotes")]
        public ActionResult PublishedNotes()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/Admin/PublishedNotes" });

            ViewBag.Title = "PublishedNotes";
            ViewBag.Authorized = true;
            ViewBag.LoadAjaxJS = true;
            ViewBag.LoadValidationScript = true;

            PublishedNotesModel publishedNotes = new PublishedNotesModel();

            publishedNotes.PublishedNotes = AdminNoteRepository.GetPublishedNotes();

            return View("~/Views/Admin/NoteViews/PublishedNotes.cshtml", publishedNotes);
        }

        [Route("Admin/SpamReports")]
        public ActionResult SpamReports()
        {
            TempData["Message"] = " Work in Progress, Yet to be completed";
            return RedirectToAction("AdminDashBoard", "Admin");
        }

        public ActionResult SystemConfig()
        {
            TempData["Message"] = " Work in Progress, Yet to be completed";
            return RedirectToAction("AdminDashBoard", "Admin");
        }
        public ActionResult Admins()
        {
            TempData["Message"] = " Work in Progress, Yet to be completed";
            return RedirectToAction("AdminDashBoard", "Admin");
        }
        public ActionResult Types()
        {
            TempData["Message"] = " Work in Progress, Yet to be completed";
            return RedirectToAction("AdminDashBoard", "Admin");
        }
        public ActionResult Categories()
        {
            TempData["Message"] = " Work in Progress, Yet to be completed";
            return RedirectToAction("AdminDashBoard", "Admin");
        }

        public ActionResult Countries()
        {
            TempData["Message"] = " Work in Progress, Yet to be completed";
            return RedirectToAction("AdminDashBoard", "Admin");
        }
        public ActionResult AdminProfile()
        {
            TempData["Message"] = " Work in Progress, Yet to be completed";
            return RedirectToAction("AdminDashBoard", "Admin");
        }

    }
}