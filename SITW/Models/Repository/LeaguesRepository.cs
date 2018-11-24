using Dapper;
using SITW.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace SITW.Models.Repository
{
    public class LeaguesRepository
    {
        private sitwEntities Db = new sitwEntities();

        public List<Leagues> getAll()
        {
            return Db.Leagues.ToList();
        }
        public void Create(Leagues instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                Db.Leagues.Add(instance);
                this.SaveChanges();
            }
        }
        public void Update(Leagues instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                Db.Entry(instance).State = EntityState.Modified;
                this.SaveChanges();
            }
        }

        public void Delete(Leagues instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                Db.Entry(instance).State = EntityState.Deleted;
                this.SaveChanges();
            }
        }
        public Leagues Get(int leaguesID)
        {
            return Db.Leagues.FirstOrDefault(x => x.sn == leaguesID);
        }

        public void SaveChanges()
        {
            this.Db.SaveChanges();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.Db != null)
                {
                    this.Db.Dispose();
                    this.Db = null;
                }
            }
        }


    }
}