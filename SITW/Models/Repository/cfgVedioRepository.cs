using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SITW.Models.Repository
{
    public class cfgVedioRepository
    {
        private sitwEntities db = new sitwEntities();

        public void add(cfgVedio cv)
        {
            cv.inpdate = DateTime.Now;
            cv.valid = 1;
            db.cfgVedio.Add(cv);

            db.SaveChanges();


        }
        public cfgVedio get(int sn)
        {
            return db.cfgVedio.Where(p => p.sn == sn).FirstOrDefault();
        }

        public List<cfgVedio> getAll()
        {
            return db.cfgVedio.Where(p => p.valid == 1).ToList();
        }
    }
}