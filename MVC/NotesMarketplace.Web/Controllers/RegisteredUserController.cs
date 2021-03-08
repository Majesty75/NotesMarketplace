using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NotesMarketplace.Models.HomeModels;
using NotesMarketplace.Data.NoteDB;
using NotesMarketplace.Data.UserDB;
using System.IO;
using NotesMarketplace.Models.RegisteredUserModels;
using NotesMarketplace.Data.DownloadsDB;
using NotesMarketplace.Models.MailModels;
using NotesMarketplace.Web.Mailer;
using NotesMarketplace.Data.SystemConfigDB;

namespace NotesMarketplace.Web.Controllers
{
    public class RegisteredUserController : Controller
    {
        [HttpGet]
        [Authorize(Roles = "NormalUser,SuperAdmin,SubAdmin")]
        public ActionResult Dashboard()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication");

            ViewBag.Title = "DashBoard";
            ViewBag.Authorized = true;
            return View();
        }


        [HttpGet]
        [Authorize(Roles = "UserProfileNotCreated, NormalUser,SuperAdmin,SubAdmin")]
        public ActionResult UserProfile()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication");

            ViewBag.Title = "UserProfile";
            ViewBag.Authorized = true;
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "NormalUser,SuperAdmin,SubAdmin")]
        public ActionResult AddNotes()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication");

            ViewBag.Title = "AddNotes";
            ViewBag.Authorized = true;

            SearchNotesModel FilterData = new SearchNotesModel();

            FilterData.Type = NotesFilters.Types();
            FilterData.Category = NotesFilters.Categories();
            FilterData.Country = NotesFilters.Countries();
            ViewBag.FilterData = FilterData;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "NormalUser,SuperAdmin,SubAdmin")]
        [ValidateAntiForgeryToken]
        public ActionResult AddNotes(NoteModel Nm,string SaveOrPublish)
        {

            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication");

            SearchNotesModel FilterData = new SearchNotesModel();

            FilterData.Type = NotesFilters.Types();
            FilterData.Category = NotesFilters.Categories();
            FilterData.Country = NotesFilters.Countries();
            ViewBag.FilterData = FilterData;


            //check uploaded files and their size
            if (Nm.PreviewFile != null && (Path.GetExtension(Nm.PreviewFile.FileName).ToLower() != ".pdf" || Nm.PreviewFile.ContentLength > 31457280))
            {
                ModelState.AddModelError("PreviewFile", "Only PDF files that are under 30MBs are allowed.");
            }

            foreach (HttpPostedFileBase file in Nm.NotesFiles)
                if(file == null)
                    ModelState.AddModelError("NotesFiles", "Make sure you have uploaded PDF file that are under 30MBs");
                else if (Path.GetExtension(file.FileName).ToLower() != ".pdf" || file.ContentLength > 31457280)
                    ModelState.AddModelError("NotesFiles", "Only PDF file that are under 30MBs are allowed.");

            if (Nm.NotesCoverPage != null && !MimeMapping.GetMimeMapping(Nm.NotesCoverPage.FileName).ToLower().Contains("image"))
                ModelState.AddModelError("NotesCoverPage", "Only Images are allowed.");

            if (!ModelState.IsValid)
                return View(Nm);


            //0 for draft 1 for submit for review
            if (SaveOrPublish == "Save")
                Nm.Status = 0;
            else if (SaveOrPublish == "Publish")
                Nm.Status = 1;
            else
            {
                ViewBag.Message = "Adding / Editing note didnt go as expected please try again.";
                return View(Nm);
            }

            ViewBag.Title = "AddNotes";
            ViewBag.Authorized = true;

            string UID = User.Identity.Name;

            int result = NotesRepository.AddNote(Nm, Convert.ToInt32(UID), Server.MapPath("~"));

            if (result < 1) {
                ViewBag.Message = "Adding/Editing note didnt go as expected please try again.";
                return View(Nm);
            }
            if (SaveOrPublish == "Publish")
            {
                ViewBag.SellerName = Session["FullName"];
                ViewBag.NoteName = Nm.Title;
                SendMail.SendEmail(new EmailModel()
                {
                    EmailTo = SystemConfigData.GetSystemConfigData("AdminEmails").DataValue.Split(';'),
                    EmailSubject = Session["FullName"].ToString() + " sent his note for review.",
                    EmailBody = this.getHTMLViewAsString("~/Views/Email/RequestToReview.cshtml")
                });
                ViewBag.Message = "Note has been submitted for review, we will send you mail when it gets approved.";
            }
            else
                ViewBag.Message = "Note has been successfully saved in Drafts";
            return View(Nm);

        }

        [HttpGet]
        [Authorize(Roles = "NormalUser,SuperAdmin,SubAdmin")]
        public ActionResult BuyerRequests()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication");

            int UID = Convert.ToInt32(User.Identity.Name);
            BuyerRequestsModel br = new BuyerRequestsModel();
            DownloadsModel dm = DownloadRepository.GetDownloads(UID, 1);
            foreach(DownloadsModel.InnerClassDownload d in dm.DownloadProperty)
            {
                UserProfileModel usrp = UserRepository.GetUserData(d.BuyerID);
                br.BRequests.Add(new BuyerRequestsModel.BuyerRequest()
                {
                    BuyerEmail = usrp.User.Email,
                    BuyerPhone = usrp.PhoneNo != null ? usrp.CountryCode + " " + usrp.PhoneNo : "N/A",
                    NoteTitle = d.NoteTitle,
                    NoteID = d.NoteID,
                    NoteCategory = d.NoteCategory,
                    SellType = d.IsPaid ? "Paid" : "Free",
                    DownloadID = d.DownloadID,
                    ReqTime = (System.DateTime)d.RequestTime,
                    Price = (int)d.PurchasedPrice
                });
            }
            return View(br);
        }

        [HttpGet]
        [Authorize(Roles = "NormalUser,SuperAdmin,SubAdmin")]
        public ActionResult AllowDownload(string id)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication");

            int DownloadId = Convert.ToInt32(id);
            int UID = Convert.ToInt32(User.Identity.Name);
            int SuccessBuyerId = DownloadRepository.AllowDownload(DownloadId, UID);
            if (SuccessBuyerId <= 0)
            {
                return new HttpStatusCodeResult(403);
            }
            else 
            {

                UserProfileModel usrp = UserRepository.GetUserData(SuccessBuyerId);
                ViewBag.BuyerName = usrp.User.FirstName + " " + usrp.User.LastName;
                ViewBag.SellerName = Session["FullName"].ToString();
                SendMail.SendEmail(new EmailModel()
                {
                    EmailTo =new string [] { usrp.User.Email },
                    EmailSubject = Session["FullName"].ToString()+ "Allows you to download a note.",
                    EmailBody = this.getHTMLViewAsString("~/Views/Email/AllowDownload.cshtml")
                });
                return RedirectToAction("BuyerRequests","RegisteredUser");
            }
        }
    }
}