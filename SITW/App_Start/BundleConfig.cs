using System;
using System.Web;
using System.Web.Optimization;

namespace SITW
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862        
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.form.min.js",
                        "~/Scripts/jquery-{version}.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryajax").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquerysignalR").Include(
                        "~/Scripts/jquery.signalR-2.2.2.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/GameBetDetail").Include(
                        "~/Scripts/GameBetDetail.js"));

            bundles.Add(new ScriptBundle("~/bundles/FullWebSite").Include(
                        "~/Scripts/UpdateMenu.js"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/bootstrap/js/bootstrap.min.js",
                      "~/Scripts/bootstrap-hover-dropdown.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/themejs").Include(
                      "~/Scripts/jquery-{version}.js",
                      "~/Scripts/jquery.backstretch.min.js",
                      "~/Content/bootstrap/js/bootstrap.min.js",
                      "~/Scripts/bootstrap-hover-dropdown.min.js",
                      "~/Scripts/jquery.backstretch.min.js",
                      "~/Scripts/wow.min.js",
                      "~/Scripts/retina-1.1.0.min.js",
                      "~/Scripts/jquery.magnific-popup.min.js",
                      "~/Content/flexslider/jquery.flexslider-min.js",                      
                      "~/Scripts/jflickrfeed.min.js",
                      "~/Scripts/masonry.pkgd.min.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/animjs").Include(
                        "~/Scripts/antblue/scripts.min.js",
                         "~/Content/antblue/customizer/customizer.min.js"

                      ));
            bundles.Add(new ScriptBundle("~/bundles/adminjs").Include(                    
                      "~/Scripts/modernizr-2.6.2-respond-1.1.0.min.js",
                      "~/Scripts/jquery.js",
                      "~/Scripts/jquery.datetimepicker.full.min.js" ,
                      "~/Scripts/jquery.dcjqaccordion.2.7.js",
                      "~/Scripts/jquery.scrollTo.min.js",
                      "~/Scripts/jquery.nicescroll.js",
                      "~/Scripts/common-scripts.js",
                      "~/Scripts/bootstrap-switch.js",
                      "~/Scripts/jquery.tagsinput.js",
                      "~/Scripts/bootstrap-inputmask.min.js",
                      "~/Scripts/form-component.js"                   
                      ));           
                                         
                 

            //bundles.Add(new StyleBundle("~/Content/styles").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/bootstrap-toggle.min.css",
            //          "~/Content/bootstrap-datetimepicker.min.css",
            //          "~/Content/bootstrap-select.min.css"/*,
            //          "~/Content/site.css"*/));
            bundles.Add(new StyleBundle("~/Content/css_dark_scheme").Include(
                      "~/Content/antblue/css/styles.min.css",
                      "~/Content/antblue/customizer/customizer.min.css"
                      ));
            bundles.Add(new StyleBundle("~/Content/styles").Include(                   
                    "~/Content/assets/css/bootstrap.css",                
                    "~/Content/assets/css/style.css",
                    "~/Content/assets/css/style-responsive.css",
                    "~/Content/vendors//bootstrap/css/bootstrap.min.css"
                     ));
            BundleTable.EnableOptimizations = true;
        }
    }
}
