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
using NotesMarketplace.Web.RoleManager;

namespace NotesMarketplace.Web.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
    public class RegisteredUserController : Controller
    {
        [HttpGet]
        [Authorize(Roles = "NormalUser,SuperAdmin,SubAdmin")]
        public ActionResult Dashboard()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = "/RegisteredUser/Dashboard" });

            //Check if Admin
            string[] roles = new NotesMarketPlaceRoleManager().GetRolesForUser(User.Identity.Name);
            if (roles.Contains("SuperAdmin") || roles.Contains("SubAdmin"))
                return RedirectToAction("AdminDashBoard", "Admin");

            int UserID = Convert.ToInt32(User.Identity.Name);

            var Stats = DownloadRepository.GetUserStats(UserID);

            DashboardModel DM = new DashboardModel()
            {
                NotesSold = Stats.Item1,
                MoneyEarned = Stats.Item2,
                Downloads = Stats.Item3,
                Rejecteds = Stats.Item4,
                BuyerRequests = Stats.Item5,

                InProgressNotes = NotesRepository.GetInProgressNotes(UserID),
                PublishedNotes = NotesRepository.GetPublishedNotes(UserID)
            };

            ViewBag.Title = "Dashboard";
            ViewBag.Authorized = true;
            return View(DM);
        }


        [HttpGet]
        [Authorize(Roles = "UserProfileNotCreated, NormalUser,SuperAdmin,SubAdmin")]
        public ActionResult UserProfile()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = "/RegisteredUser/UserProfile" });

            UserProfileModel userProfile = UserRepository.GetUserData(Convert.ToInt32(User.Identity.Name));

            ViewBag.Genders = DropdownItems.Genders();
            ViewBag.CountryCodes = DropdownItems.CountryCodes();
            ViewBag.Countries = NotesFilters.Countries();
            ViewBag.LoadValidationScript = true;
            ViewBag.Title = "UserProfile";
            ViewBag.Authorized = true;
            return View(userProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "UserProfileNotCreated, NormalUser,SuperAdmin,SubAdmin")]
        public ActionResult UserProfile(UserProfileModel up)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = "/RegisteredUser/UserProfile" });

            int UserID = Convert.ToInt32(User.Identity.Name);
            if (ModelState.IsValid) {

                DateTime Birthdate = up.DOB == null ? DateTime.MinValue : (System.DateTime)up.DOB; 

                if (up.ProfilePictureFile != null && !MimeMapping.GetMimeMapping(up.ProfilePictureFile.FileName).Contains("image"))
                {
                    ModelState.AddModelError("ProfilePictureFile", "Only Images Are Allowed");
                    up.ProfilePicture = UserRepository.GetUserData(Convert.ToInt32(User.Identity.Name)).ProfilePicture;
                }   
                else if (up.DOB != null &&  (DateTime.Compare(Birthdate,DateTime.Now.AddYears(-130)) < 0 || DateTime.Compare(Birthdate, DateTime.Now) > 0))
                {
                    ModelState.AddModelError("DOB", "Please Enter Valid Date");
                    up.ProfilePicture = UserRepository.GetUserData(Convert.ToInt32(User.Identity.Name)).ProfilePicture;
                }
                else
                {
                    UserProfileModel updated = UserRepository.addUserProfile(up, UserID, Server.MapPath(@"~/"));

                    if (updated != null)
                    {
                        ViewBag.Message = "User Profile Updated";
                        up = updated;
                        return RedirectToAction("Login", "Authentication",new { ReturnURL = @"/RegisteredUser/UserProfile" });
                    }
                    else
                    {
                        ViewBag.Message = "Something went wrong please try again";
                    }
                }
            }


            ViewBag.Genders = DropdownItems.Genders();
            ViewBag.CountryCodes = DropdownItems.CountryCodes();
            ViewBag.Countries = NotesFilters.Countries();
            ViewBag.LoadValidationScript = true;
            ViewBag.Title = "UserProfile";
            ViewBag.Authorized = true;
            return View(up);
        }

        [HttpGet]
        [Authorize(Roles = "NormalUser,SuperAdmin,SubAdmin")]
        public ActionResult AddNotes()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = "/RegisteredUser/AddNotes" });

            ViewBag.Title = "AddNotes";
            ViewBag.Authorized = true;
            ViewBag.LoadValidationScript = true;
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

            ViewBag.Title = "AddNotes";
            ViewBag.Authorized = true;
            ViewBag.LoadValidationScript = true;

            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = "/RegisteredUser/AddNotes" });

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

            long NoteSize = 0;

            foreach (HttpPostedFileBase file in Nm.NotesFiles)
            {
                if (file == null)
                    ModelState.AddModelError("NotesFiles", "Make sure you have uploaded PDF file(s) that are under 30MBs");
                else if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                {

                    ModelState.AddModelError("NotesFiles", "Only PDF file(s) are allowed.");
                }
                if (file != null)
                    NoteSize += file.ContentLength;
            }
            if(NoteSize > 31457280)
            {
                ModelState.AddModelError("NotesFiles", "Combine PDF(s) size should not exceeds 30MBs");
            }
                

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
            ViewBag.LoadValidationScript = true;
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
                TempData["Message"] = "Note has been submitted for review, we will send you mail when it gets approved.";
            }
            else
                TempData["Message"] = "Note has been successfully saved in Drafts";

            return RedirectToAction("Dashboard", "RegisteredUser");

        }

        [HttpGet]
        [Authorize(Roles = "NormalUser,SuperAdmin,SubAdmin")]
        public ActionResult BuyerRequests()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/RegisteredUser/BuyerRequests" });

            int UID = Convert.ToInt32(User.Identity.Name);
            BuyerRequestsModel br = new BuyerRequestsModel();
            DownloadsModel dm = DownloadRepository.GetDownloads(UID, 0);
            foreach(DownloadsModel.InnerClassDownload d in dm.DownloadProperty)
            {
                UserProfileModel usrp = UserRepository.GetUserData(d.Buyer.UserID);
                br.BRequests.Add(new BuyerRequestsModel.BuyerRequest()
                {
                    BuyerEmail = d.Buyer.Email,
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
            ViewBag.Authorized = true;
            ViewBag.Title = "BuyerRequests";
            return View(br);
        }

        [HttpGet]
        [Authorize(Roles = "NormalUser,SuperAdmin,SubAdmin")]
        public ActionResult AllowDownload(string id)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/RegisteredUser/AllowDownload/"+id });

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

        [HttpGet]
        [Authorize(Roles = "NormalUser,SuperAdmin,SubAdmin")]
        public ActionResult SoldNotes()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/RegisteredUser/SoldNotes" });

            int UserID = Convert.ToInt32(User.Identity.Name);

            DownloadsModel soldNotes = DownloadRepository.GetDownloads(UserID, 2); // DownnloadStatus : 2 for sold notes

            ViewBag.Authorized = true;
            ViewBag.Title = "SoldNotes";

            return View(soldNotes);

        }

        [HttpGet]
        [Authorize(Roles = "NormalUser,SuperAdmin,SubAdmin")]
        public ActionResult RejectedNotes()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/RegisteredUser/RejectedNotes" });

            int UserID = Convert.ToInt32(User.Identity.Name);

            RejectedNotes RejectedNotesObj = NotesRepository.GetRejectedNotes(UserID);

            ViewBag.Authorized = true;
            ViewBag.Title = "RejectedNotes";
            return View(RejectedNotesObj);

        }

        [HttpGet]
        [Authorize(Roles = "NormalUser,SuperAdmin,SubAdmin")]
        public ActionResult Downloads()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/RegisteredUser/Downloads" });

            int UserID = Convert.ToInt32(User.Identity.Name);

            DownloadsModel DownloadedNotes = DownloadRepository.GetDownloads(UserID, 1); // DownnloadStatus : 1 for downloads
            ViewBag.LoadAjaxJS = true;
            ViewBag.LoadValidationScript = true;
            ViewBag.Authorized = true;
            ViewBag.Title = "Downloads";
            return View(DownloadedNotes);

        }

    }
}