using System.Web;
using System.Web.Optimization;

namespace HSM
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.ui.touch-punch.min.js", "~/Scripts/modernizr-2.7.1.js",
                        "~/Scripts/bootstrap.min.js", "~/Scripts/jquery.slimscroll.min.js",
                        "~/Scripts/common.js", "~/Scripts/holder.js", "~/Scripts/jquery.uniform.min.js",
                        "~/Scripts/jquery.prettyPhoto.js", "~/Scripts/bootstrap-select.js",
                        "~/Scripts/twitter-bootstrap-hover-dropdown.min.js","~/Scripts/jquery.toggle.buttons.js", 
                        "~/Scripts/jasny-bootstrap.min.js", "~/Scripts/bootstrap-fileupload.js",
                        "~/Scripts/bootbox.js","~/Scripts/wysihtml5-0.3.0_rc2.min.js",
                        "~/Scripts/prettify.js", "~/Scripts/jquery.gritter.min.js",
                        "~/Scripts/jquery.notyfy.js", "~/Scripts/jquery.miniColors.js",
                        "~/Scripts/bootstrap-datetimepicker.min.js", "~/Scripts/jquery.cookie.js",
                        "~/Scripts/themer.js", "~/Scripts/twitter.js", "~/Scripts/jquery.easy-pie-chart.js",
                        "~/Scripts/jquery.sparkline.min.js", "~/Scripts/jquery.ba-resize.js",
                        "~/Scripts/index.js", "~/Scripts/jquery.flot.*",
                        "~/Scripts/resizable.js", "~/Scripts/load-image.min.js", "~/Scripts/bootstrap-image-gallery.min.js"
                        ));
      
            
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jfirst").Include(
                        "~/Scripts/jquery-2.1.1.min.js", "~/Scripts/jquery-ui.min.1.11.1.js"
                        ));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/bundlecss").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/responsive.css",
                      "~/Content/glyphicons.css",
                      "~/Content/uniform.default.css",
                      "~/Content/prettyPhoto.css",
                      "~/Content/excanvas.css",
                      "~/Content/jasny-bootstrap.min.css",
                      "~/Content/jasny-bootstrap-responsive.min.css",
                      "~/Content/bootstrap-wysihtml5-0.0.2.css",
                      "~/Content/bootstrap-select.css",
                      "~/Content/bootstrap-toggle-buttons.css",
                      "~/Content/select2.css",
                      "~/Content/bootstrap-datepicker.css",
                      "~/Content/jquery-ui-1.9.2.custom.min.css",
                      "~/Content/jquery.miniColors.css",
                      "~/Content/jquery.notyfy.css",
                      "~/Content/default.css",
                      "~/Content/jquery.gritter.css",
                      "~/Content/jquery.easy-pie-chart.css",
                      "~/Content/prettify.css",
                      "~/Content/bootstrap-image-gallery.min.css",
                      "~/Content/style-dark.css","~/live.less.css"));

            bundles.Add(new StyleBundle("~/Content/bracket").Include(
                "~/Content/bracketcss.css"));

            bundles.Add(new ScriptBundle("~/bundles/bracket").Include(
                "~/Scripts/jquery.hotkeys.js",
             "~/Scripts/jquery-migrate-1.2.1.min.js",
             "~/Scripts/bootstrap.min.js",
             "~/Scripts/bootstrap-datepicker.js",
             "~/Scripts/jquery.sparkline.min.js",
             "~/Scripts/toggles.min.js",
             "~/Scripts/retina.min.js",
             "~/Scripts/jquery.cookies.js",
             "~/Scripts/bracketcustom.js",
             "~/Scripts/tao.ui.min.js",
             "~/Scripts/jsoncalls.js"));
        }
    }
}
