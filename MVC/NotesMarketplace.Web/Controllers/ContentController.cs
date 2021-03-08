using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Diagnostics;
using NotesMarketplace.Web.RoleManager;


namespace NotesMarketplace.Web.Controllers
{
    public class ContentController : Controller
    {
        [AllowAnonymous]
        public ActionResult GetRestictedData(string MemberId, string Path)
        {
            if (!Request.IsAuthenticated)
                return new HttpUnauthorizedResult();
            if (MemberId == User.Identity.Name || new NotesMarketPlaceRoleManager().IsUserInRole(User.Identity.Name,"SuperAdmin") || new NotesMarketPlaceRoleManager().IsUserInRole(User.Identity.Name, "SubAdmin"))
            {
                /*here we store file name as imagename$jpg in database and we split path in imagename and 
                 * extension jpg. this way we can stop server from treating request as request for static file.
                 * without this we need to configure web.config and add http handler 
                 */

                string[] FileAndExtention = Path.Split('$');
                string FileName = FileAndExtention[0] + '.' + FileAndExtention[1];
                var asset = Server.MapPath("~/Members/" + MemberId + @"/" + FileName);

                MimeMapping.GetMimeMapping(FileName);

                return File(new FileStream(asset, FileMode.Open, FileAccess.Read), MimeMapping.GetMimeMapping(FileName), FileName);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "You are not authorized to access this file");
            }
        }
    }
}