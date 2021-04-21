using System.Web;
using System.Web.Optimization;

namespace NotesMarketplace.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(
                new Bundle("~/bundles/JSDependecies").Include("~/Content/js/jquery/jquery-3.5.1.min.js")
                                                           .Include("~/Content/js/bootstrap/bootstrap.min.js")
                                                           .Include("~/Content/js/tablesorter/jquery.tablesorter.min.js")
                                                           .Include("~/Content/js/script.js")
            );

            bundles.Add(
                new StyleBundle("~/bundles/CSSDependencies").Include("~/Content/css/bootstrap/bootstrap.min.css")
                                                            .Include("~/Content/css/style.css")
                                                            .Include("~/Content/css/responsive.css")
            );

            bundles.Add(
                new ScriptBundle("~/bundles/HomeHeaderScript").Include("~/Content/js/header.js")
            );

            bundles.Add(
                new ScriptBundle("~/bundles/JSValidate").Include("~/Scripts/jquery.validate.js")
                                                        .Include("~/Scripts/jquery.validate.unobtrusive.js")
            );
        }
    }
}
