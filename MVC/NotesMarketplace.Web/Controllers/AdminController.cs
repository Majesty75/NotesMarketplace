using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace NotesMarketplace.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin, SubAdmin")]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View("AdminDashBoard");
        }

        public ActionResult AdminDashBoard()
        {
            return View();
        }
    }
}