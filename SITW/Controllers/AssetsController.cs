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
using Microsoft.AspNet.Identity;
using SITW.Filter;
using SITW.Models.ViewModel;

namespace SITW.Controllers
{
    [Authorize]
    [MissionFilter]
    [Authorize(Roles = "Admin")]
    public class AssetsController : Controller
    {
        private sitwEntities db = new sitwEntities();

        // GET: AssetsRecords
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            return View(db.AssetsRecord.Where(p=>p.UserId== userID).ToList());
        }

        // GET: AssetsRecords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetsRecord assetsRecord = db.AssetsRecord.Find(id);
            if (assetsRecord == null)
            {
                return HttpNotFound();
            }
            return View(assetsRecord);
        }

        // GET: AssetsRecords/Create
        public ActionResult Create(string userId,int? unitSn)
        {
            userId = (string.IsNullOrEmpty(userId) ? User.Identity.GetUserId() : userId);
            AssetsRecord ar = new AssetsRecord();
            ar.UserId = userId;
            if (unitSn.HasValue)
                ar.unitSn = unitSn.Value;
            return View(ar);
        }

        // POST: AssetsRecords/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AssetsRecord assetsRecord)
        {
            if (ModelState.IsValid)
            {
                //string userID = User.Identity.GetUserId();
                //assetsRecord.unitSn = 1;
                //assetsRecord.UserId = userID;
                new AssetsRepository().AddAssetsByAssets(assetsRecord);
                return RedirectToAction("Index","game");
            }

            return View(assetsRecord);
        }

        // GET: AssetsRecords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetsRecord assetsRecord = db.AssetsRecord.Find(id);
            if (assetsRecord == null)
            {
                return HttpNotFound();
            }
            return View(assetsRecord);
        }

        // POST: AssetsRecords/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "sn,UserId,unitSn,gameSn,assets,type,inpdate")] AssetsRecord assetsRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assetsRecord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(assetsRecord);
        }

        // GET: AssetsRecords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetsRecord assetsRecord = db.AssetsRecord.Find(id);
            if (assetsRecord == null)
            {
                return HttpNotFound();
            }
            return View(assetsRecord);
        }

        // POST: AssetsRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AssetsRecord assetsRecord = db.AssetsRecord.Find(id);
            db.AssetsRecord.Remove(assetsRecord);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult _getAssetsString()
        {
            List<AssetsViewModel> avList = new List<AssetsViewModel>();
            /* 每次都重新讀取
            if (Session["Assets"]!=null)
            {
                avList = ((List<AssetsViewModel>)Session["Assets"]);
            }
            else
            {
                avList = new AssetsRepository().getAssetsListByUserID(User.Identity.GetUserId());
                Session["Assets"] = avList;
            }
            */
            avList = new AssetsRepository().getAssetsListByUserID(User.Identity.GetUserId());
            Session["Assets"] = avList;

            string returnstring = "";
            foreach(AssetsViewModel avm in avList)
            {
                returnstring += (returnstring == "" ? "" : " ");
                returnstring += avm.Asset;
            }
            return View(avList);
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
