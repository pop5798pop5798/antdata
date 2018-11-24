using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SITW.Models;
using SITW.Models.Repository;
using SITW.Models.ViewModel;

namespace SITW.Controllers
{
    public class LeaguesController : Controller
    {
        LeaguesRepository _leagues = new LeaguesRepository();
        cfgPlayGameRepository _cfplay = new cfgPlayGameRepository();

        // GET: Leagues
        public ActionResult Index()
        {
            List<Leagues> leagues = _leagues.getAll().OrderByDescending(x => x.sn).ToList();
            List<ApiTypes> cfplaydata = _cfplay.getAll().ToList();
            List<LeaguesViewModel> leaguesList = new List<LeaguesViewModel>();           
            foreach (var lg in leagues)
            {
                LeaguesViewModel leaguesmodel = new LeaguesViewModel();
                leaguesmodel.LeaguesData = lg;
                leaguesmodel.Playgame = cfplaydata.Where(p => p.sn == lg.playGamesn).FirstOrDefault();
                leaguesList.Add(leaguesmodel);
            }
            
         

            return View(leaguesList);
        }
      

        // GET: Leagues/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Leagues/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Leagues leagues)
        {
            
            if (leagues != null && ModelState.IsValid)
            {
                _leagues.Create(leagues);
                return RedirectToAction("Index", "cfgPlayGames");
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
                return RedirectToAction("Index", "cfgPlayGames");
            }
            else
            {
                var leagues = _leagues.Get(id.Value);
                return View(leagues);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Leagues leagues)
        {
            if (leagues != null && ModelState.IsValid)
            {
                _leagues.Update(leagues);
                return RedirectToAction("Index", "cfgPlayGames");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "cfgPlayGames");
            }
            else
            {
                var category = _leagues.Get(id.Value);
                return View(category);
            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var category = _leagues.Get(id);
                _leagues.Delete(category);
            }
            catch (DataException)
            {
                return RedirectToAction("Delete", new { id = id });
            }
            return RedirectToAction("Index", "cfgPlayGames");
        }

        
    }
}
