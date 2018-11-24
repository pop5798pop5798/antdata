using SITW.Models.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SITW.Models.Repository
{
    public class NewsRepository : INewsRepository, IDisposable
    {
        protected sitwEntities Db
        {
            get;
            private set;
        }

        public NewsRepository()
        {
            this.Db = new sitwEntities();
        }

        public void Create(placard instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                Db.placard.Add(instance);
                this.SaveChanges();
            }
        }

        public void Update(placard instance)
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

        public void Delete(placard instance)
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

        public placard Get(int placardID)
        {
            return Db.placard.FirstOrDefault(x => x.Id == placardID);
        }

        public IQueryable<placard> GetAll()
        {
            return Db.placard.OrderBy(x => x.Id);
        }
        public IQueryable<placard> GetAllaction()
        {
            return Db.placard.Where(x => x.p_class == 0);
        }

        public IQueryable<placard> GetAllstop()
        {
            return Db.placard.Where(x => x.p_class == 1);
        }
        public IQueryable<placard> GetAllnews()
        {
            return Db.placard.Where(x => x.p_class == 2);
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