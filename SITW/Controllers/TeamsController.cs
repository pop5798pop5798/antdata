using AutoMapper;
using Google.Apis.Auth.OAuth2;
using GoogleCloudSamples.Services;
using Microsoft.AspNet.Identity;
using SITW.Filter;
using SITW.Helper;
using SITW.Models;
using SITW.Models.Repository;
using SITW.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SITW.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TeamsController : Controller
    {
        private TeamsRepository teamsRepository;     
        private LeaguesRepository leaguesRepository;
        private cfgPlayGameRepository playgameRepository;
        private ImageUploader _imageUploader;

        public TeamsController()
        {
            this.teamsRepository = new TeamsRepository();
            this.leaguesRepository = new LeaguesRepository();
            this.playgameRepository = new cfgPlayGameRepository();
        }

        // GET: Teams
        public ActionResult Index()
        {
            // var teamdata = this.teamsRepository.getAll().ToList();
            // var leaguedata = this.leaguesRepository.getAll().ToList();
            List<Teams> teamdata = this.teamsRepository.getAll();
            List<Leagues> leaguedata = this.leaguesRepository.getAll();
            List<ApiTypes> gamedata = this.playgameRepository.getAll();
            List<TeamsViewModel> teamsList = new List<TeamsViewModel>();
            foreach (var td in teamdata)
            {
                TeamsViewModel tdvm = new TeamsViewModel();
                tdvm.teamspost = td;
                tdvm.leagues = leaguedata.Where(p => p.sn == td.leagueSn).FirstOrDefault();
                tdvm.cfplaygame = gamedata.Where(p => p.sn == tdvm.leagues.playGamesn).FirstOrDefault();

                teamsList.Add(tdvm);


            }


            /* TeamsViewModel teams = new TeamsViewModel
             {
                 Teams = teamdata,
                 LeaguesData = leaguedata
             };*/
            return View(teamsList);
        }

       

        // GET: Teams/Create
        public ActionResult Create()
        {
            var teamdata = this.teamsRepository.getAll().ToList();


            TeamsViewModel teams = new TeamsViewModel
            {
                Teams = teamdata
            };

            return View(teams);
        }

        [HttpPost]     
        public async System.Threading.Tasks.Task<ActionResult> Create(Teams teams, HttpPostedFileBase image)
        {

            if (teams != null && ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(teams.imageURL) && image != null)
                {
                    string filename = "";
                    filename = DateTime.Now.ToString("yyyyMMddTHHmmssfff");
                    Google.Apis.Auth.OAuth2.GoogleCredential credential = await Google.Apis.Auth.OAuth2.GoogleCredential.GetApplicationDefaultAsync();
                    _imageUploader = new ImageUploader(System.Web.Configuration.WebConfigurationManager.AppSettings["GoogleCloud:BucketName"]);
                    var imageUrl = await _imageUploader.UploadImage(image, filename, "Teams");
                    teams.imageURL = imageUrl;
                }


                teams.valid = 1;
                this.teamsRepository.Create(teams);
                return RedirectToAction("Index");
            }
            else
            {
                return View(teams);
            }

            /* try
             {
                 Mapper.Initialize(cfg => {
                     cfg.CreateMap<TeamsViewModel, Teams>();
                 });
                 Teams teams = Mapper.Map<Teams>(mvm);


                 if (string.IsNullOrEmpty(teams.imageURL) && image != null)
                 {
                     string filename = "";
                     filename = DateTime.Now.ToString("yyyyMMddTHHmmssfff");
                     Google.Apis.Auth.OAuth2.GoogleCredential credential = await Google.Apis.Auth.OAuth2.GoogleCredential.GetApplicationDefaultAsync();
                     _imageUploader = new ImageUploader(System.Web.Configuration.WebConfigurationManager.AppSettings["GoogleCloud:BucketName"]);
                     var imageUrl = await _imageUploader.UploadImage(image, filename, "Teams");
                     teams.imageURL = imageUrl;
                 }


                 TeamsRepository mr = new TeamsRepository();
                 teams.valid = 1;
                 mr.AddTeams(teams);


                 return RedirectToAction("Index");
             }
             catch (Exception e)
             {
                 return View(mvm);
             }*/




        }

        public ActionResult CreateLeague()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateLeague(Leagues leagues)
        {
            if (leagues != null && ModelState.IsValid)
            {
                this.leaguesRepository.Create(leagues);
                return RedirectToAction("Create");
            }
            else
            {
                return View(leagues);
            }
        }



        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var teamget= this.teamsRepository.Get(id.Value);

                TeamsViewModel teams = new TeamsViewModel
                {
                    sn = teamget.sn,
                    name = teamget.name,
                    shortName = teamget.shortName,
                    imageURL = teamget.imageURL,
                    leagueSn = teamget.leagueSn,
                    valid = teamget.valid
                };
             
                return View(teams);
            }
        }

        [HttpPost]   
        public async System.Threading.Tasks.Task<ActionResult> Edit(Teams teams, HttpPostedFileBase image)
        {
           if (teams != null && ModelState.IsValid)
            {
                if (image != null)
                {
                    string filename = "";
                    filename = DateTime.Now.ToString("yyyyMMddTHHmmssfff");
                    Google.Apis.Auth.OAuth2.GoogleCredential credential = await Google.Apis.Auth.OAuth2.GoogleCredential.GetApplicationDefaultAsync();
                    _imageUploader = new ImageUploader(System.Web.Configuration.WebConfigurationManager.AppSettings["GoogleCloud:BucketName"]);
                    var imageUrl = await _imageUploader.UploadImage(image, filename, "Teams");
                    teams.imageURL = imageUrl;
                }


                teams.valid = 1;
                this.teamsRepository.Update(teams);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }


             /*try
             {
                 Mapper.Initialize(cfg => {
                     cfg.CreateMap<TeamsViewModel, Teams>();
                 });
                 Teams teams = Mapper.Map<Teams>(mvm);


                 if (string.IsNullOrEmpty(teams.imageURL) && image != null)
                 {
                     string filename = "";
                     filename = DateTime.Now.ToString("yyyyMMddTHHmmssfff");
                     Google.Apis.Auth.OAuth2.GoogleCredential credential = await Google.Apis.Auth.OAuth2.GoogleCredential.GetApplicationDefaultAsync();
                     _imageUploader = new ImageUploader(System.Web.Configuration.WebConfigurationManager.AppSettings["GoogleCloud:BucketName"]);
                     var imageUrl = await _imageUploader.UploadImage(image, filename, "Teams");
                     teams.imageURL = imageUrl;
                 }


                 TeamsRepository mr = new TeamsRepository();
                 teams.valid = 1;
                 mr.UpdateTeams(teams);


                 return RedirectToAction("Index");
             }
             catch (Exception e)
             {
                 return View(mvm);
             }*/

        }

        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var category = this.teamsRepository.Get(id.Value);
                return View(category);
            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var category = this.teamsRepository.Get(id);
                this.teamsRepository.Delete(category);
            }
            catch (DataException)
            {
                return RedirectToAction("Delete", new { id = id });
            }
            return RedirectToAction("Index");
        }
      
    }
}
