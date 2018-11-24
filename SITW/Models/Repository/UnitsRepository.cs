using Dapper;
using SITW.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace SITW.Models.Repository
{
    public class UnitsRepository
    {
        private sitwEntities db = new sitwEntities();

        public List<cfgUnit> getAll()
        {
            return db.cfgUnit.ToList();
        }

        public List<cfgUnit> getAllValid()
        {
            return db.cfgUnit.Where(p => p.valid == 1).ToList();
        }

    }
}