using SITW.Models.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SITW.Models.Repository
{
    public class RnakRewardRepository : IRankRewardRepository, IDisposable
    {
        protected sitwEntities Db
        {
            get;
            private set;
        }

        public RnakRewardRepository()
        {
            this.Db = new sitwEntities();
        }

        public void Create(Ranking_content instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                Db.Ranking_content.Add(instance);
                this.SaveChanges();
            }
        }

        public void Update(Ranking_content instance)
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

        public void Delete(Ranking_content instance)
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

        public Ranking_content Get(int RewardID)
        {
            return Db.Ranking_content.FirstOrDefault(x => x.Id == RewardID);
        }

        public IQueryable<Ranking_content> GetAll()
        {
            return Db.Ranking_content.OrderBy(x => x.Id);
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