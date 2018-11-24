using SITW.Filter;
using SITW.Models.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Net.Mime;
using System.Web.Routing;
using System.Xml;
using System.Xml.Linq;

namespace SITW.Controllers
{
    
    public class HomeController : Controller
    {
        
        [MissionFilter]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Sitemap()
        {
            Response.ContentType = "application/xml";
            return View();
        }
        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        public ActionResult _Banner()
        {
            DateTime date1 = DateTime.Parse("2018-05-01T00:00:00");
            DateTime dt = DateTime.Now;

            if (DateTime.Compare(date1, dt) < 0)
            {               
                ViewBag.Banner = 1;

            }
            else
            {
                ViewBag.Banner = 0;
            }

            

            return View();
        }

        public ActionResult IndexPage()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult News()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Repair()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
    }
}