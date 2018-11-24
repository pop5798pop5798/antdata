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
    public class NewsMenuRepository
    {
        private sitwEntities Db = new sitwEntities();

        public List<NewsMenu> getAll()
        {
            return Db.NewsMenu.ToList();
        }
        public NewsMenu Get(int menuID)
        {
            return Db.NewsMenu.FirstOrDefault(x => x.Id == menuID);
        }



    }
}