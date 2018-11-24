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
    public class TeamsRepository
    {
        private sitwEntities Db = new sitwEntities();

        public List<Teams> getAll()
        {
            return Db.Teams.ToList();
        }

        public List<Teams> getAllValid()
        {
            return Db.Teams.Where(p => p.valid == 1).ToList();
        }
        public void Create(Teams instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                Db.Teams.Add(instance);
                this.SaveChanges();
            }
        }
        public void Update(Teams instance)
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

        public void Delete(Teams instance)
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
        public Teams Get(int teamsID)
        {
            return Db.Teams.FirstOrDefault(x => x.sn == teamsID);
        }
        public Teams AddTeams(Teams teams)
        {
            Db.Teams.Add(teams);
            Db.SaveChanges();
            return teams;
        }

        public Teams UpdateTeams(Teams teams)
        {
            Db.Entry(teams).State = EntityState.Modified;
            Db.SaveChanges();
            return teams;
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