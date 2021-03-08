using System.Web.Mvc;
using NotesMarketplace.Models.HomeModels;
using NotesMarketplace.Data.UserDB;
using NotesMarketplace.Data.NoteDB;
using NotesMarketplace.Web.Mailer;
using NotesMarketplace.Data.SystemConfigDB;
using NotesMarketplace.Models.SystemConfigModels;
using NotesMarketplace.Models.MailModels;
using System;

namespace NotesMarketplace.Web.Controllers
{
    [AllowAnonymous]
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
            if (Request.IsAuthenticated)
                ViewBag.Authorized = true;
            NoteModel Nm = NotesRepository.GetNoteDetailsById(Convert.ToInt32(NoteId));
            if (Nm != null)
                return View(Nm);
            return new HttpNotFoundResult();
        }

    }
}