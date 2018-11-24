using GoogleCloudSamples.Services;
using SITW.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SITW.Controllers
{
    public class ToolsController : Controller
    {
        // GET: Tools
        [Authorize(Roles = "Admin")]
        public ActionResult ImageList(string path)
        {
            ImageListViewModel Obj = new ImageListViewModel();
            Obj.path = path;
            ImageUploader imageClient = new ImageUploader(System.Web.Configuration.WebConfigurationManager.AppSettings["GoogleCloud:BucketName"]);
            Obj.ImageList = imageClient.ListOfObjects(path);
            return View(Obj);
        }
    }
}