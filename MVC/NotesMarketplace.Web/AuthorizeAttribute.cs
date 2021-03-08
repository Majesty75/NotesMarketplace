/* Customizing invalid Authorizatioin handling so that user don't get redirected to login,
 * when they don't have permission to access the web page they requested.
 * Here we are basically over-riding the default Authorize Attribute HandleUnauhtorizedRequest Method.*/

using System;
using System.Web.Mvc;
using System.Net;
using NotesMarketplace.Web.RoleManager;

namespace NotesMarketplace.Web
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            if (!filterContext.HttpContext.Request.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else if (new NotesMarketPlaceRoleManager().GetRolesForUser(filterContext.HttpContext.User.Identity.Name)[0] == "UserProfileNotCreated")
            { 
                filterContext.Result = new RedirectResult("/RegisteredUser/UserProfile");
            }
            else
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
        }
    }
}