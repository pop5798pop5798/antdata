using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SITW.Models.Repository
{
    public class VedioRecordRepository
    {
        private sitwEntities db = new sitwEntities();

        public void add(VedioRecord vr)
        {
            vr.inpdate = DateTime.Now;
            vr.valid = 1;
            db.VedioRecord.Add(vr);

            db.SaveChanges();


        }

        public void update(VedioRecord vr)
        {
            VedioRecord vrd = db.VedioRecord.Where(p => p.sn == vr.sn).FirstOrDefault();
            vrd.vediourl = (string.IsNullOrEmpty(vr.vediourl) ? "" : vr.vediourl);
            vrd.cfgVedioSn = vr.cfgVedioSn;
            vrd.comment = vr.comment;
            vrd.title = vr.title;

            db.SaveChanges();
        }


        public VedioRecord get(int sn)
        {

            return db.VedioRecord.Where(p => p.sn == sn).FirstOrDefault();
        }


        public List<VedioRecord> getAll()
        {
            return db.VedioRecord.Where(p => p.valid == 1).ToList();
        }

    }
}