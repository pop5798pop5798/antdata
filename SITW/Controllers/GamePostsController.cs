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
    [Authorize(Roles = "Admin")]
    public class GamePostsController : Controller
    {
        
        private sitwEntities db = new sitwEntities();

        // GET: GamePosts      
        public ActionResult Index()
        {
            return View(db.APIPosts.ToList());
        }

        // GET: GamePosts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APIPosts gamePosts = db.APIPosts.Find(id);
            if (gamePosts == null)
            {
                return HttpNotFound();
            }
            return View(gamePosts);
        }

        // GET: GamePosts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GamePosts/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "sn,GameSn,VedioRecordSn,valid,inpdate")] APIPosts gamePosts)
        {
            if (ModelState.IsValid)
            {
                db.APIPosts.Add(gamePosts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gamePosts);
        }

        // GET: GamePosts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APIPosts gamePosts = db.APIPosts.Find(id);
            if (gamePosts == null)
            {
                return HttpNotFound();
            }
            return View(gamePosts);
        }

        // POST: GamePosts/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "sn,GameSn,VedioRecordSn,valid,inpdate")] APIPosts gamePosts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gamePosts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gamePosts);
        }

        // GET: GamePosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APIPosts gamePosts = db.APIPosts.Find(id);
            if (gamePosts == null)
            {
                return HttpNotFound();
            }
            return View(gamePosts);
        }

        // POST: GamePosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            APIPosts gamePosts = db.APIPosts.Find(id);
            db.APIPosts.Remove(gamePosts);
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
