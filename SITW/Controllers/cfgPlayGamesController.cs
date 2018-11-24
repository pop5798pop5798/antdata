using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SITW.Models;

namespace SITW.Controllers
{
    public class cfgPlayGamesController : Controller
    {
        private sitwEntities db = new sitwEntities();

        // GET: cfgPlayGames
        public ActionResult Index()
        {           
            return View(db.ApiTypes.ToList());
        }

        // GET: cfgPlayGames/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApiTypes cfgPlayGame = db.ApiTypes.Find(id);
            if (cfgPlayGame == null)
            {
                return HttpNotFound();
            }
            return View(cfgPlayGame);
        }

        // GET: cfgPlayGames/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: cfgPlayGames/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "sn,cName,eName,shortName,gamelogo,valid")] ApiTypes cfgPlayGame)
        {
            if (ModelState.IsValid)
            {
                db.ApiTypes.Add(cfgPlayGame);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cfgPlayGame);
        }

        // GET: cfgPlayGames/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApiTypes cfgPlayGame = db.ApiTypes.Find(id);
            if (cfgPlayGame == null)
            {
                return HttpNotFound();
            }
            return View(cfgPlayGame);
        }

        // POST: cfgPlayGames/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "sn,cName,eName,shortName,gamelogo,valid")] ApiTypes cfgPlayGame)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cfgPlayGame).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cfgPlayGame);
        }

        // GET: cfgPlayGames/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApiTypes cfgPlayGame = db.ApiTypes.Find(id);
            if (cfgPlayGame == null)
            {
                return HttpNotFound();
            }
            return View(cfgPlayGame);
        }

        // POST: cfgPlayGames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApiTypes cfgPlayGame = db.ApiTypes.Find(id);
            db.ApiTypes.Remove(cfgPlayGame);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
