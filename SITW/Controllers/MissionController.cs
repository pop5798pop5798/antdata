using AutoMapper;
using GoogleCloudSamples.Services;
using Microsoft.AspNet.Identity;
using SITW.Filter;
using SITW.Helper;
using SITW.Models;
using SITW.Models.Repository;
using SITW.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SITW.Controllers
{
    [Authorize]
    [MissionFilter]
    public class MissionController : Controller
    {
        private ImageUploader _imageUploader;

        // GET: Mission
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<Missions> ml = new List<Missions>();
            ml = new MissionsRepository().getAll();
            return View(ml);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            MissionViewModel mission = new MissionViewModel
            {
                MissionEndList = new List<MissionEnd>(),
                MissionStartList = new List<MissionStart>()
            };

            
            return View(mission);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]        
        public async System.Threading.Tasks.Task<ActionResult> Create(MissionViewModel mvm, HttpPostedFileBase image)
        {
           
            try
            {
                Mapper.Initialize(cfg => {
                    cfg.CreateMap<MissionViewModel, Missions>();
                });
                Missions mission = Mapper.Map<Missions>(mvm);
                List<MissionStart> MissionStartList = mvm.MissionStartList;
                List<MissionEnd> MissionEndList = mvm.MissionEndList;
                List<MissionAssets> MissionAssetsList = mvm.MissionAssetList;

                if (string.IsNullOrEmpty(mission.imgURL) && image != null)
                {
                    string filename = "";
                    filename = DateTime.Now.ToString("yyyyMMddTHHmmssfff");
                    Google.Apis.Auth.OAuth2.GoogleCredential credential = await Google.Apis.Auth.OAuth2.GoogleCredential.GetApplicationDefaultAsync();
                    _imageUploader = new ImageUploader(System.Web.Configuration.WebConfigurationManager.AppSettings["GoogleCloud:BucketName"]);
                    var imageUrl = await _imageUploader.UploadImage(image, filename, "MissionImg");
                    mission.imgURL = imageUrl;
                }


                MissionsRepository mr = new MissionsRepository();
                mission.valid = 1;
                mr.AddMission(mission);
                foreach (var m in MissionStartList)
                {
                    m.missionSn = mission.sn;
                    mr.AddMissionStart(m);
                }
                foreach (var m in MissionEndList)
                {
                    m.missionSn = mission.sn;
                    mr.AddMissionEnd(m);
                }
                foreach (var m in MissionAssetsList)
                {
                    m.missionSn = mission.sn;
                    mr.AddMissionAsset(m);
                }

                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                return View(mvm);
            }
        }


        [Authorize(Roles = "Admin")]
        public ActionResult _MissionStart(MissionViewModel model, int? index)
        {
            index = index ?? 0;
            ViewBag.Index = index;
            model.MissionStartList = new List<MissionStart>();
            for (int i = 0; i <= index; i++)
            {
                model.MissionStartList.Add(new MissionStart());
            }
            return PartialView(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult _MissionEnd(MissionViewModel model, int? index)
        {
            index = index ?? 0;
            ViewBag.Index = index;
            model.MissionEndList = new List<MissionEnd>();
            for (int i = 0; i <= index; i++)
            {
                model.MissionEndList.Add(new MissionEnd());
            }
            return PartialView(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult _MissionAsset(MissionViewModel model, int? index)
        {
            index = index ?? 0;
            ViewBag.Index = index;
            model.MissionAssetList = new List<MissionAssets>();
            for (int i = 0; i <= index; i++)
            {
                model.MissionAssetList.Add(new MissionAssets());
            }
            return PartialView(model);
        }

        public ActionResult MissionNote()
        {
            IEnumerable<MissionNoteModel> IEMNM = new MissionsRepository().GetMissionCode(User.Identity.GetUserId());
            MissionNoteViewModel mnvm = new MissionNoteViewModel();
            mnvm.MissionNoteList = IEMNM.ToList();
            return View(mnvm);
        }

        [HttpPost]
        public bool SetMissionFinsh(int userMissionSn)
        {
            return new MissionsRepository().SetMissionFinsh(User.Identity.GetUserId(), userMissionSn);
        }

        public ActionResult getMissionCountString()
        {
            IEnumerable<MissionNoteModel> IEMNM = new MissionsRepository().GetMissionCode(User.Identity.GetUserId());
            MissionNoteViewModel mnvm = new MissionNoteViewModel();
            mnvm.MissionNoteList = IEMNM.ToList();


            return Json(new { MissionFinshNumber = mnvm.MissionFinshNumber, MissionNumber = mnvm.MissionNumber }, JsonRequestBehavior.AllowGet);
        }
    }
}