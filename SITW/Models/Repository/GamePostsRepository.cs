using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SITW.Models.Repository
{
    public class GamePostsRepository
    {
        private sitwEntities db = new sitwEntities();

        public void add(APIPosts gp)
        {
            gp.inpdate = DateTime.Now;
            gp.valid = 1;
            db.APIPosts.Add(gp);

            db.SaveChanges();


        }

        public void update(APIPosts gp)
        {
            APIPosts gpd = db.APIPosts.Where(p => p.sn == gp.sn).FirstOrDefault();
            gpd.ApiSn = gp.ApiSn;;
            gpd.sdate = gp.sdate;
            gpd.edate = gp.edate;
            gpd.categorySn = gp.categorySn;
            db.SaveChanges();


        }

        public APIPosts get(int sn)
        {
            return db.APIPosts.Where(p => p.sn == sn).FirstOrDefault();
        }

        public APIPosts getgame(int sn)
        {
            return db.APIPosts.Where(p => p.ApiSn == sn).FirstOrDefault();
        }

        public List<APIPosts> getAll()
        {
            return db.APIPosts.Where(p => p.valid == 1).ToList();
        }

        public List<APIPosts> getValidAll()
        {
            DateTime dt = DateTime.Now.AddHours(-2);
            return db.APIPosts.Where(p => p.valid == 1 && p.edate >= dt).ToList();
        }
    }
}