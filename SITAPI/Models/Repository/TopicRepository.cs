using SITDto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SITAPI.Models.Repository
{
    public class TopicRepository
    {
        public topic setData(topic t)
        {
            if (t.sn == 0)
                return AddNew(t);
            else
                return Edit(t);
        }

        public topic setStop(int sn)
        {
            topic t = db.topics.Where(p => p.sn == sn).FirstOrDefault();
            db.SaveChanges();
            return t;
        }

        public topic setReopen(int sn)
        {
            topic t = db.topics.Where(p => p.sn == sn).FirstOrDefault();
            gameDto g = new GameRepository().GetGame(t.apiSn.Value);
            db.SaveChanges();
            return t;
        }

        private sitdbEntities db = new sitdbEntities();
        private topic AddNew(topic t)
        {
            db.topics.Add(t);
            db.SaveChanges();
            return t;
        }

        private topic Edit(topic t)
        {
            t.modiDate = DateTime.Now;
            db.Entry(t).State = EntityState.Modified;
            db.SaveChanges();
            return t;
        }

    }
}