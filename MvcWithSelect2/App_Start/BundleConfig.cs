using System.Web;
using System.Web.Optimization;

namespace MvcWithSelect2
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/stylesheets/bootstrap.min.css",
                "~/stylesheets/site.css",
                "~/stylesheets/select2.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                  "~/Scripts/jquery-{version}.js",
                  "~/Scripts/jquery.validate*",
                  "~/Scripts/jquery.validate.unobtrusive*",
                  "~/Scripts/select2*"
                  ));

            bundles.Add(new ScriptBundle("~/bundles/App").Include(
                  "~/Scripts/App.js"
                  ));
        }
    }
}