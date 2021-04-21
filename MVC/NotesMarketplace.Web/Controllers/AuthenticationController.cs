using System;
using System.Linq;
using System.Web.Mvc;
using NotesMarketplace.Models.AuthenticationModels;
using NotesMarketplace.Data.UserDB;
using System.Web.Security;
using NotesMarketplace.Models.RegisteredUserModels;
using NotesMarketplace.Models.MailModels;
using NotesMarketplace.Web.Mailer;
using NotesMarketplace.Web.RoleManager;
using System.IO;

namespace NotesMarketplace.Web.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
    public class AuthenticationController : Controller
    {

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            //If already login redirect to user dashboard
            if (Request.IsAuthenticated)
            {
                Session["UserID"] = User.Identity.Name;

                UserProfileModel userProfile = UserRepository.GetUserData(Convert.ToInt32(Session["UserID"]));

                if (!String.IsNullOrEmpty(userProfile.ProfilePicture))
                    Session["UserProfile"] = userProfile.ProfilePicture;
                else
                    Session["UserProfile"] = "/Content/SystemConfig/DefaultUserProfile.png";

                Session["FullName"] = userProfile.User.FirstName + " " + userProfile.User.LastName;

                Session["Email"] = userProfile.User.Email;


                //Check if Admin
                string[] roles = new NotesMarketPlaceRoleManager().GetRolesForUser(User.Identity.Name);
                if (roles.Contains("SuperAdmin") || roles.Contains("SubAdmin"))
                    return RedirectToAction("AdminDashBoard", "Admin");

                return RedirectToAction("Dashboard", "RegisteredUser");
            }

            if (TempData["EmailVerified"] != null) {
                ViewBag.EmailVerificationMsg = TempData["EmailVerifiedMsg"].ToString();
                ViewBag.EmailVerified = (bool)TempData["EmailVerified"];
            }
            return View();
        }

        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login Client)
        {
            if (ModelState.IsValid)
            {
                /* authenticate user return 0 when it finds wrong credentials and UserID when it's successfully authenticate user */
                int AuthResult = UserRepository.AuthenticateUser(Client);
                if (AuthResult != 0)
                {
                    if (Client.RememberMe == true)
                        FormsAuthentication.SetAuthCookie(AuthResult.ToString(), true);
                    else
                        FormsAuthentication.SetAuthCookie(AuthResult.ToString(), false);

                    //saving it in session to use it somewhere
                    Session["UserID"] = AuthResult;

                    UserProfileModel userProfile = UserRepository.GetUserData(AuthResult);

                    //if email is not verified redirect to login with verify email message 
                    if (!userProfile.User.IsEmailVerified)
                    {
                        FormsAuthentication.SignOut();
                        TempData["EmailVerified"] = false;
                        TempData["EmailVerifiedMsg"] = "Please Verify Email Address Via Mail We Have Sent You.";
                        return RedirectToAction("Login", "Authentication");
                    }

                    if (!String.IsNullOrEmpty(userProfile.ProfilePicture))
                        Session["UserProfile"] = userProfile.ProfilePicture;
                    else
                        Session["UserProfile"] = "/Content/SystemConfig/DefaultUserProfile.png";

                    Session["FullName"] = userProfile.User.FirstName + " " + userProfile.User.LastName;

                    Session["Email"] = userProfile.User.Email;


                    //if not entered user profile data redirect to user profile
                    if (userProfile.Country == null)
                        return RedirectToAction("UserProfile", "RegisteredUser");
                    else
                    {

                        //Check if Admin
                        string[] roles = new NotesMarketPlaceRoleManager().GetRolesForUser(AuthResult.ToString());
                        if (roles.Contains("SuperAdmin") | roles.Contains("SubAdmin"))
                            return RedirectToAction("AdminDashBoard", "Admin");

                        return RedirectToAction("Dashboard", "RegisteredUser");
                    }
                }
                else
                {
                    ViewBag.Success = false;
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(SignUp User)
        {
            //if not valid details return view with data and validation error
            if (!ModelState.IsValid)
            {
                return View(User);
            }

            //else create new user and send email with generated GUID
            else
            {
                //set roleID = 4 = UserProfileNotCreated
                User.RoleID = 4;

                //set GUID for email verification
                User.GUID = Guid.NewGuid().ToString();

                //send email
                int UserID = UserRepository.AddUser(User);
                if (UserID > 0)
                {
                    ViewBag.Link = "https://localhost:44374/Authentication/VerifyEmailAddress/"+ UserID + "/"+User.GUID;
                    ViewBag.Name = User.FirstName +" "+ User.LastName;
                    SendMail.SendEmail(
                        new EmailModel (){
                            Emailfrom = "",
                            EmailTo = new string [] { User.Email },
                            EmailSubject = "Notes Marketplace - Email Verification",
                            EmailBody = this.getHTMLViewAsString("~/Views/Email/EmailVerification.cshtml")                            
                        }
                    );
                    ViewBag.Success = true;
                    return View(User);
                }

                //if fails send message user already exists
                else
                {
                    ViewBag.Success = false;
                    return View(User);
                }
            }
        }

        [AllowAnonymous]
        public ActionResult VerifyEmailAddress(string UiD, string strGuid)
        {
            if (UiD == null || strGuid == null)
                return new HttpNotFoundResult();

            int UID = Convert.ToInt32(UiD);
            int EmailVerification = UserRepository.VerifyEmail(UID, strGuid);

            //if verified successfully redirect to login with success message
            if (EmailVerification == 1)
            {
                TempData["EmailVerified"] = true;
                TempData["EmailVerifiedMsg"] = "Email Verified Successfully. Please Login.";
                return RedirectToAction("Login", "Authentication");
            }

            //if verified already redirect to login with that messasge
            else if(EmailVerification == -1)
            {
                TempData["EmailVerified"] = true;
                TempData["EmailVerifiedMsg"] = "Email Already Verified.";
                return RedirectToAction("Login", "Authentication");
            }

            //if unsuccessful redirect to login with unsuccessful message
            else
            {
                TempData["EmailVerified"] = false;
                TempData["EmailVerifiedMsg"] = "Email Verification Parameters Didn't Match.";
                return RedirectToAction("Login", "Authentication");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult ForgotPassword(ForgotPassword EmailAddress)
        {
            if (!ModelState.IsValid)
            {
                return View(EmailAddress);
            }
            else
            {
                string tempPass = UserRepository.ForgotPassword(EmailAddress.Email);
                if (String.IsNullOrEmpty(tempPass))
                {
                    ViewBag.Success = false;
                    return View(EmailAddress);
                }
                ViewBag.Password = tempPass;
                if(!SendMail.SendEmail(
                    new EmailModel(){
                        EmailTo = new string[] { EmailAddress.Email },
                        EmailSubject = "Notes Marketplace - New temporary password has been created for you.",
                        EmailBody = this.getHTMLViewAsString("~/Views/Email/TempPassword.cshtml")
                }))
                {
                    ViewBag.Success = false;
                    return View(EmailAddress);
                }
                ViewBag.Success = true;
                return View(EmailAddress);
            }
        }

        [HttpGet]
        [Authorize(Roles = "NormalUser,SuperAdmin,SubAdmin")]
        public ActionResult ChangePassword()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login","Authentication",new { ReturnUrl = @"/Authentication/ChangePassword" });

            ViewBag.Title = "ChangePassword";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "NormalUser,SuperAdmin,SubAdmin")]
        public ActionResult ChangePassword(ChangePasswordModel cp)
        {

            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Authentication", new { ReturnUrl = @"/Authentication/ChangePassword" });

            ViewBag.Title = "ChangePassword";
            if (!ModelState.IsValid)
                return View(cp);
            if(UserRepository.ChangePassword(cp, Convert.ToInt32(User.Identity.Name)) == 1)
            {
                ViewBag.Success = true;
                return View(cp);
            }
            else
            {
                ModelState.AddModelError("Password", "Password you have entered is incorrect");
                return View(cp);
            }
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            //clear authcookie param and redirect to login
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            //After logout, prevent back browser button action 
            //System.Web.HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
            //System.Web.HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
            //System.Web.HttpContext.Current.Response.AddHeader("Expires", "0");

            return RedirectToAction("Login");
        }
    }
}