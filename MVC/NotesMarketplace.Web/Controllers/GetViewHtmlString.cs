using System.Web.Mvc;
using System.IO;
namespace NotesMarketplace.Web.Controllers
{
    public static class GetViewHtmlString
    {
        //method to convert html views into string to embed it in emails
        public static string getHTMLViewAsString(this Controller controller, string ViewName)
        {
            using (var stringWriter = new StringWriter())
            {

                //Get View in Viewresult object
                var vr = ViewEngines.Engines.FindView(controller.ControllerContext, ViewName, "~/Views/Email/_Email.cshtml");

                //create viewcontext with tempdata, viewbag, viewdata etc
                var viewContext = new ViewContext(controller.ControllerContext, vr.View,
                                 controller.ViewData, controller.TempData, stringWriter);
                //render it in string builder
                vr.View.Render(viewContext, stringWriter);

                //return html rendered string
                string ReturnString = stringWriter.GetStringBuilder().ToString();
                return ReturnString;
            }
        }
    }
}