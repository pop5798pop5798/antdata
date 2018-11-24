using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SITAPI.Models.Repository
{
    public class ChoiceRepository
    {
        private sitdbEntities db = new sitdbEntities();

        public choice setData(choice c)
        {
            if (c.sn == 0)
                return AddNew(c);
            else
                return Edit(c);
        }

        private choice AddNew(choice c)
        {
            db.choices.Add(c);
            db.SaveChanges();
            return c;
        }

        private choice Edit(choice c)
        {
            c.modiDate = DateTime.Now;
            db.Entry(c).State = EntityState.Modified;
            db.SaveChanges();
            return c;
        }
    }
}