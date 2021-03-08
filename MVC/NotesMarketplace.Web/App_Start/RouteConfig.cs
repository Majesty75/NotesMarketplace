using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NotesMarketplace.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "EmailVerification",
                url: "Authentication/VerifyEmailAddress/{UiD}/{strGuid}",
                defaults: new { Controller = "Authentication", Action = "VerifyEmailAddress"}
            );

            routes.MapRoute(
                name: "NoteDetails",
                url: "Home/NoteDetails/{NoteId}",
                defaults: new { Controller = "Home", Action = "NoteDetails" }
            );

            routes.MapRoute(
                name: "RestrictedAccess",
                url: "Assets/{MemberId}/{*Path}",
                defaults: new { Controller = "Content", Action = "GetRestictedData" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
