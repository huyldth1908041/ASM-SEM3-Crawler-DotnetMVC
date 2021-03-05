using System.Web;
using System.Web.Optimization;

namespace ReadNewsWebClient
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));




            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-3.5.0.min.js"));

   

            bundles.Add(new StyleBundle("~/bundles/app_styles").Include(
                      "~/Content/Css/animate.css", "~/Content/Css/owl.carousel.css",
                      "~/Content/Css/owl.theme.default.css", "~/Content/Css/style_1.css", "~/Content/Css/indexArticle.css", "~/Content/Css/adminPending.css"));


            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                       "~/Content/Js/core/jquery.min.js",
                       "~/Content/Js/core/popper.min.js",
                       "~/Content/Js/core/bootstrap-material-design.min.js",
                       "~/Content/Js/plugins/perfect-scrollbar.jquery.min.js",
                       "~/Content/Js/plugins/moment.min.js",
                       "~/Content/Js/plugins/sweetalert2.js",
                       "~/Content/Js/plugins/jquery.validate.min.js",
                       "~/Content/Js/plugins/jquery.bootstrap-wizard.js",
                       "~/Content/Js/plugins/bootstrap-selectpicker.js",
                       "~/Content/Js/plugins/bootstrap-datetimepicker.min.js",
                       "~/Content/Js/plugins/jquery.dataTables.min.js",
                       "~/Content/Js/plugins/bootstrap-tagsinput.js",
                       "~/Content/Js/plugins/jasny-bootstrap.min.js",
                       "~/Content/Js/plugins/fullcalendar.min.js",
                       "~/Content/Js/plugins/jquery-jvectormap.js",
                       "~/Content/Js/plugins/nouislider.min.js",
                       "~/Content/Js/plugins/chartist.min.js",
                       "~/Content/Js/plugins/bootstrap-notify.js",
                       "~/Content/Js/material-dashboard.js?v=2.1.2"));
        }
    }
}
