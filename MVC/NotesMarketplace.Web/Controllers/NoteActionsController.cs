using NotesMarketplace.Data.NoteDB;
using NotesMarketplace.Data.SystemConfigDB;
using NotesMarketplace.Models.MailModels;
using NotesMarketplace.Models.RegisteredUserModels;
using NotesMarketplace.Models.HomeModels;
using NotesMarketplace.Web.Mailer;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.IO;
using System.Web;

namespace NotesMarketplace.Web.Controllers
{
    [Authorize(Roles = "NormalUser,SubAdmin,SuperAdmin")]
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
    public class NoteActionsController : Controller
    {
        [HttpGet]
        public ActionResult Rate(int NoteID)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/RegisteredUser/Downloads" });
            }

            int UserID = Convert.ToInt32(User.Identity.Name);

            RatingModel Rating = NotesRepository.GetNoteRating(NoteID, UserID);

            if (Rating == null)
            {
                return PartialView("~/Views/RegisteredUser/_RatingsPopupModal.cshtml", new RatingModel() { NoteID = NoteID });
            }
            else if (Rating.NoteID == -1)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }
            else
            {
                return PartialView("~/Views/RegisteredUser/_RatingsPopupModal.cshtml", Rating);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rate(RatingModel Rm)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/RegisteredUser/Downloads" });
            }

            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            int UserID = Convert.ToInt32(User.Identity.Name);

            if (NotesRepository.SubmitNoteRating(Rm, UserID))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            else
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }
        }

        [HttpGet]
        public ActionResult Report(int NoteID)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/RegisteredUser/Downloads" });
            }

            int UserID = Convert.ToInt32(User.Identity.Name);

            ReportNoteModel Report = NotesRepository.GetNoteReport(NoteID, UserID);

            if (Report.NoteID == -1)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }
            else
            {
                if (Report.Remarks != null)
                {
                    ModelState.AddModelError("Remarks", "You already reported this note once.");
                }
                return PartialView("~/Views/RegisteredUser/_ReportNotePopupModal.cshtml", Report);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Report(ReportNoteModel Rm)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/RegisteredUser/Downloads" });
            }

            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            int UserID = Convert.ToInt32(User.Identity.Name);

            if (NotesRepository.SubmitNoteReport(Rm, UserID))
            {
                ViewBag.SellerName = Rm.SellerName;
                ViewBag.NoteName = Rm.NoteTitle;
                ViewBag.BuyerName = Session["FullName"];
                SendMail.SendEmail(new EmailModel()
                {
                    EmailTo = SystemConfigData.GetSystemConfigData("AdminEmails").DataValue.Split(';'),
                    EmailSubject = Session["FullName"].ToString() + " reported an issue for "+ Rm.NoteTitle,
                    EmailBody = this.getHTMLViewAsString("~/Views/Email/ReportedNote.cshtml")
                });

                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            else
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }
        }


        /// <summary>
        /// This Action handles view note url only for note that are submitted for review,
        /// or are in review for only the owners
        /// </summary>
        /// <param name="NoteID">Id no of note passed in url</param>
        /// <returns>View with readonly note model and Bad Request if not allowed</returns>
        public ActionResult View(int NoteID)
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/NoteActions/View/" + NoteID });
            }

            int UserID = Convert.ToInt32(User.Identity.Name);

            NoteModel Nm = NotesRepository.GetNoteDetailsById(NoteID);

            if((Nm.Status != 1 && Nm.Status != 2) || Nm.SellerID != UserID)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "You are not allowed to view this note.");
            }

            SearchNotesModel FilterData = new SearchNotesModel();
            FilterData.Type = new Dictionary<string, string>() { { Nm.Type, Nm.Type } };
            FilterData.Category = new Dictionary<string, string>() { { Nm.Category, Nm.Category } };
            FilterData.Country = new Dictionary<string, string>() { { Nm.Country, Nm.Country } };
            ViewBag.FilterData = FilterData;

            ViewBag.Title = "AddNotes";
            ViewBag.Authorized = true;
            ViewBag.LoadValidationScript = true;
            ViewBag.Readonly = true;
            //ViewBag.Edit = true;
            return View("~/Views/RegisteredUser/AddNotes.cshtml", Nm);
        }

        /// <summary>
        /// This Action Handles reuest for edit note. 
        /// </summary>
        /// <param name="NoteID">Note ID of note to be edited</param>
        /// <returns>AddNotes page with preloaded note data or bad request result if note is not draft or user is not allowed to edit the note.</returns>
        [HttpGet]
        public ActionResult Edit(int NoteID)
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/NoteActions/Edit/" + NoteID });
            }

            int UserID = Convert.ToInt32(User.Identity.Name);

            NoteModel Nm = NotesRepository.GetNoteDetailsById(NoteID);

            if ((Nm.Status != 0) || Nm.SellerID != UserID)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "You are not allowed to edit this note.");
            }

            SearchNotesModel FilterData = new SearchNotesModel();
            FilterData.Type = NotesFilters.Types();
            FilterData.Category = NotesFilters.Categories();
            FilterData.Country = NotesFilters.Countries();
            ViewBag.FilterData = FilterData;


            ViewBag.Title = "AddNotes";
            ViewBag.Authorized = true;
            ViewBag.LoadValidationScript = true;
            ViewBag.Edit = true;
            return View("~/Views/RegisteredUser/AddNotes.cshtml", Nm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int NoteID, NoteModel Nm, string SaveOrPublish)
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/NoteActions/Edit/" + NoteID });
            }

            SearchNotesModel FilterData = new SearchNotesModel();

            FilterData.Type = NotesFilters.Types();
            FilterData.Category = NotesFilters.Categories();
            FilterData.Country = NotesFilters.Countries();
            ViewBag.FilterData = FilterData;

            ViewBag.Title = "AddNotes";
            ViewBag.Authorized = true;
            ViewBag.LoadValidationScript = true;
            ViewBag.Edit = true;

            int UserID = Convert.ToInt32(User.Identity.Name);

            //check uploaded files and their size
            if (Nm.PreviewFile != null && (Path.GetExtension(Nm.PreviewFile.FileName).ToLower() != ".pdf" || Nm.PreviewFile.ContentLength > 31457280))
            {
                ModelState.AddModelError("PreviewFile", "Only PDF files that are under 30MBs are allowed.");
            }

            long NoteSize = 0;

            foreach (HttpPostedFileBase file in Nm.NotesFiles)
            {
                if (file == null)
                    break;
                else if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                {
                    ModelState.AddModelError("NotesFiles", "Only PDF file(s) are allowed.");
                }
                if(file != null)
                    NoteSize += file.ContentLength;
            }
            if (Nm.NotesFiles[0] != null && NoteSize > 31457280)
            {
                ModelState.AddModelError("NotesFiles", "Combine PDF(s) size should not exceeds 30MBs");
            }

            if (Nm.NotesCoverPage != null && !MimeMapping.GetMimeMapping(Nm.NotesCoverPage.FileName).ToLower().Contains("image"))
                ModelState.AddModelError("NotesCoverPage", "Only Images are allowed.");

            if (!ModelState.IsValid)
                return View("~/Views/RegisteredUser/AddNotes.cshtml", Nm);


            //0 for draft 1 for submit for review
            if (SaveOrPublish == "Save")
                Nm.Status = 0;
            else if (SaveOrPublish == "Publish")
                Nm.Status = 1;
            else
            {
                ViewBag.Message = "Adding / Editing note didnt go as expected please try again.";
                return View("~/Views/RegisteredUser/AddNotes.cshtml", Nm);
            }

            Nm.NoteId = NoteID;

            bool result = NotesRepository.EditNote(UserID, Nm,Server.MapPath("~"));

            if (!result)
            {
                ViewBag.Message = "Adding/Editing note didnt go as expected please try again.";
                return View("~/Views/RegisteredUser/AddNotes.cshtml", Nm);
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

        public ActionResult Delete(int NoteID)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/NoteActions/Delete/" + NoteID });
            }

            int UserID = Convert.ToInt32(User.Identity.Name);

            if(NotesRepository.DeleteNote(NoteID, UserID, Server.MapPath("~")))
            {
                TempData["Message"] = "Draft Deleted";
            }
            else
            {
                TempData["Message"] = "Something Went Wrong, Deletion Unsuccessful";
            }

            return RedirectToAction("Dashboard", "RegisteredUser");
        }

    }
}