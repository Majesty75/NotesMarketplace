using System.Web.Mvc;
using NotesMarketplace.Models.HomeModels;
using NotesMarketplace.Data.UserDB;
using NotesMarketplace.Data.NoteDB;
using NotesMarketplace.Web.Mailer;
using NotesMarketplace.Data.SystemConfigDB;
using NotesMarketplace.Models.SystemConfigModels;
using NotesMarketplace.Models.MailModels;
using System;
using System.Collections.Generic;
using NotesMarketplace.Models.RegisteredUserModels;
using System.Linq;

namespace NotesMarketplace.Web.Controllers
{
    [AllowAnonymous]
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated && Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication");
            else if (Request.IsAuthenticated)
            {
                ViewBag.Authorized = true;
            }

            ViewBag.Title = "Home";
            return View();
        }

        public ActionResult Faq()
        {
            if (Request.IsAuthenticated && Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication");
            else if (Request.IsAuthenticated)
            {
                ViewBag.Authorized = true;
            }

            ViewBag.Title = "Faq";
            return View();
        }

        [HttpGet]
        public ActionResult ContactUs()
        {
            if (Request.IsAuthenticated && Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication");

            else if (Request.IsAuthenticated)
            {
                ViewBag.Authorized = true;
            }
                
            ViewBag.Title = "ContactUs";
            return View(new ContactUsModel());
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ContactUs(ContactUsModel cu)
        {

            if (Request.IsAuthenticated && Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication");
            else if (Request.IsAuthenticated)
            {
                ViewBag.Authorized = true;
            }

            ViewBag.Title = "ContactUs";
            if (!ModelState.IsValid)
            {
                return View(cu);
            }

            ViewBag.Name = cu.FullName;
            ViewBag.Email = cu.Email;
            ViewBag.Comment = cu.Comment;
            if (SendMail.SendEmail(new EmailModel()
            {
                EmailTo = SystemConfigData.GetSystemConfigData("AdminEmails").DataValue.Split(';'),
                EmailSubject = cu.Subject,
                EmailBody = this.getHTMLViewAsString("~/Views/Email/ContactUsMail.cshtml")
            }))
            {
                ViewBag.Message = "Thank you for contacting us, we will get back to you.";
                return View(cu);
            }
            else
            {
                ViewBag.Message = "Something went wrong. Please try again.";
                return View(cu);
            }
        }

        [HttpGet]
        public ActionResult SearchNotes()
        {
            ViewBag.Title = "SearchNotes";

            if (Request.IsAuthenticated && Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication");
            else if (Request.IsAuthenticated)
            {
                ViewBag.Authorized = true;
            }


            SearchNotesModel FilterData = new SearchNotesModel();

            FilterData.Type = NotesFilters.Types();
            FilterData.Category = NotesFilters.Categories();
            FilterData.Country = NotesFilters.Countries();
            FilterData.Rating = NotesFilters.Ratings();
            FilterData.University = NotesFilters.Universities();
            FilterData.Course = NotesFilters.Courses();
            ViewBag.FilterData = FilterData;

            ListNotes Ln = NotesRepository.GetAllAvailableNotes(new NoteModel() { IsItSearchAndFilter = false });
            ViewBag.LoadAjaxJS = true;
            ViewBag.NotesData = Ln;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchNotes(NoteModel Nm)
        {
            Nm.IsItSearchAndFilter = true;
            ListNotes Ln = NotesRepository.GetAllAvailableNotes(Nm);
            return PartialView("_ListNotes", Ln);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult NoteDetails(string NoteId)
        {
            if(String.IsNullOrEmpty(NoteId))
                return new HttpNotFoundResult();

            int UserID = 0 ;
            string[] UserRoles = null;

            if (Request.IsAuthenticated)
            {
                if (Session["UserID"] == null)
                    return RedirectToAction("Login","Authentication");
                ViewBag.Authorized = true;

                UserID = Convert.ToInt32(User.Identity.Name);

                UserRoles = new RoleManager.NotesMarketPlaceRoleManager().GetRolesForUser(User.Identity.Name);
            }

            NoteModel Nm = NotesRepository.GetNoteDetailsById(Convert.ToInt32(NoteId));

            if (Nm == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);

            //Only show note details when notes is published or being accessed by owner or admins
            if ( Nm.Status != 3 && (Request.IsAuthenticated && !(Nm.SellerID == UserID || UserRoles.Contains("SuperAdmin") || UserRoles.Contains("SubAdmin")) ))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            }

            List<string> ReviewerList = new List<string>(); 

            foreach (Review r in Nm.Reviews)
            {
                ReviewerList.Add(r.ReviwerProfilePicture);
            }


            /* We will use this list in content controller to give anonymous users, access to those user profiles
             * which are included in notes reviews.
             */

            Session["ReviewerList"] = ReviewerList;

            //Adding Full Name of Seller and contact number of support for popup model

            SystemConfigModel SupportContact = SystemConfigData.GetSystemConfigData("SupportContact");
            if(SupportContact != null)
            {
                ViewBag.SupportContact = SupportContact.DataValue;
            }
            else
            {
                ViewBag.SupportContact = "Not Available";
            }

            //Full Name of Seller
            UserProfileModel Seller  = UserRepository.GetUserData(Nm.SellerID); 
            if(Seller != null) {
                ViewBag.Seller = Seller.User.FirstName + " " + Seller.User.LastName;
            }
            else
            {
                ViewBag.Seller = "Anonymous User";
            }

            //TempData passed by GetNoteAttachments method to confirm buyer request submission
            if(TempData.ContainsKey("BuyerRequestSubmitted") && (bool)TempData["BuyerRequestSubmitted"])
            {
                ViewBag.BuyerRequestSubmitted = true;
            }

            ViewBag.Title = "NotesDetails";
            return View(Nm);

        }

    }
}