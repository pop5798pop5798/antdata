using SITW.Models.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SITW.Models.Repository
{
    public class RewardTitleRepository : IRewardTitleRepository, IDisposable
    {
        protected sitwEntities Db
        {
            get;
            private set;
        }

        public RewardTitleRepository()
        {
            this.Db = new sitwEntities();
        }

        public void Create(Ranking_title instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                Db.Ranking_title.Add(instance);
                this.SaveChanges();
            }
        }

        public void Update(Ranking_title instance)
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

        public void Delete(Ranking_title instance)
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

        public Ranking_title Get(int RewardID)
        {
            return Db.Ranking_title.FirstOrDefault(x => x.Id == RewardID);
        }

        public IQueryable<Ranking_title> GetAll()
        {           

            return Db.Ranking_title.OrderBy(x => x.Id);
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