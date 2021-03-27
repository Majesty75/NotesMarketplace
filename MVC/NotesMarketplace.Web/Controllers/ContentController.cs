using System;
using System.Collections.Generic;
using NotesMarketplace.Data.DownloadsDB;
using System.Web;
using System.IO;
using System.Net;
using System.Web.Mvc;
using NotesMarketplace.Web.RoleManager;
using Ionic.Zip;
using System.Linq;
using NotesMarketplace.Web.Mailer;
using NotesMarketplace.Models.MailModels;

namespace NotesMarketplace.Web.Controllers
{
    [Authorize(Roles = "NormalUser, SuperAdmin, SubAdmin")]
    public class ContentController : Controller
    {
        public ActionResult GetProfilePicture(string MemberId, string UserProfile)
        {
            /* Here Member ID is id of members whose assests we are trying to access i.e. images and notes,
             * so if member id and current userid matches we need not to check if user should have access to member ID's
             * assets. But in other case if we are trying to access notes we first needs to check if user should have access to data,
             * and then proceed accordingly.
             */

            /* here we store file name as imagename$jpg in database and we split path in imagename and 
             * extension jpg. this way we can stop server from treating request as request for static file.
             * without this we need to configure web.config and add http handler. 
             */

            string[] FileAndExtention = UserProfile.Split('$');
            string FileName = FileAndExtention[0] + '.' + FileAndExtention[1];


            var asset = Server.MapPath("~/Members/" + MemberId + @"/" + FileName);

            Boolean IsAccessible = true;

            //If they anonymous user want to access note attachment directly we need to send unauthorized access result.
            if (!Request.IsAuthenticated)
                return new HttpUnauthorizedResult();

            NotesMarketPlaceRoleManager NMPRoles = new NotesMarketPlaceRoleManager();

            /* check if user requesting access to other user's Display Picture via note details page, if so then we need to check
             * weather user should have access to that via session variable ReviewerList.
             */

            if (MimeMapping.GetMimeMapping(FileName).Contains("image"))
            {
                if (Session["ReviewerList"] == null)
                {
                    IsAccessible = false;
                }
                else
                {
                    List<string> ReviewerList = (List<string>)Session["ReviewerList"];
                    if (ReviewerList.Contains(@"/Assets/" + MemberId + @"/" + UserProfile))
                    {
                        ReviewerList.Remove(@"/Assets/" + MemberId + @"/" + UserProfile);
                    }
                    else
                    {
                        IsAccessible = false;
                    }
                }
            }

            //here we are providing current user, super admin and sub admin access to data requested. (current user will only have access to it's own data). 
            if (MemberId == User.Identity.Name || NMPRoles.IsUserInRole(User.Identity.Name,"SuperAdmin") || NMPRoles.IsUserInRole(User.Identity.Name, "SubAdmin"))
            {
                IsAccessible = true;
            }

            else if(!System.IO.File.Exists(asset))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }


            if (!IsAccessible)              
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "You are not authorized to access this file");
            }


            return File(new FileStream(asset, FileMode.Open, FileAccess.Read), MimeMapping.GetMimeMapping(FileName), FileName);
        }

        public ActionResult GetNotesAttachments(string NoteID)
        {

            if(Session["UserID"] == null)
            {
                return Redirect("Authentication/Login?ReturnUrl=%2fAssets%2fNotes%2f"+NoteID);
            }

            string [] NoteInfo = DownloadRepository.NoteAttachments(Convert.ToInt32(NoteID), Convert.ToInt32(User.Identity.Name));
            string path = Server.MapPath("~/Members/");
            if(NoteInfo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            string NoteTitle = NoteInfo[0];
            string AttachmentPath = NoteInfo[1];

            //if buyer request is being submitted first time we send seller an email
            if (AttachmentPath.Contains("BuyerRequestSubmitted"))
            {
                string [] SellerInfo = AttachmentPath.Split(';');
                string SellerEmail = SellerInfo[1];
                //sending email

                ViewBag.SellerName = SellerInfo[2];
                ViewBag.BuyerName = Session["FullName"];
                SendMail.SendEmail(new EmailModel()
                {
                    EmailTo = new string [] { SellerEmail },
                    EmailSubject = Session["FullName"] + " Wants to purchase your notes.",
                    EmailBody = this.getHTMLViewAsString("~/Views/Email/BuyerRequest.cshtml")
                });

                TempData["BuyerRequestSubmitted"] = true;
                return RedirectToAction("NoteDetails/"+NoteID,"Home");
            }
            else if (AttachmentPath == "BuyerRequestAlreadySubmitted")
            {
                TempData["BuyerRequestSubmitted"] = true;
                return RedirectToAction("NoteDetails/" + NoteID, "Home");
            }
            try
            {
                DirectoryInfo NoteDir = new DirectoryInfo(Path.Combine(path, AttachmentPath));
                FileInfo[] Attachments = NoteDir.GetFiles();

                MemoryStream ResponseFile = new MemoryStream();

                if (Attachments.Count() == 1)
                {
                    return File(new FileStream(Attachments[0].FullName, FileMode.Open, FileAccess.Read), "application/pdf", NoteTitle + "-NotesMarketPlace.pdf");
                }
                else if (Attachments.Count() > 1)
                {
                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AddDirectory(Path.Combine(path, AttachmentPath));
                        zip.Save(ResponseFile);
                    }
                    ResponseFile.Position = 0;
                    return File(ResponseFile, "application/zip", NoteTitle + "-NotesMarketPlace.zip");
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }catch(Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }   
    }

}