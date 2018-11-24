using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace SITW.Models.Repository
{
    public class SeasonsRepository
    {
        private sitwEntities db = new sitwEntities();
        public SeasonsRepository()
        {

        }

        public List<Seasons> getAll()
        {
            return db.Seasons.ToList();
        }



        public Seasons GetNowSeason()
        {
            List<Seasons> SeasonList = new List<Seasons>();
            Seasons sea = new Seasons();

            using (IDbConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["SQLData"].ConnectionString.ToString()))
            {
                string query = @"
                select *
                from Seasons s
                where getdate() between viewSDate and viewEDate
                ";
                using (var multi = cn.QueryMultiple(query))
                {
                    SeasonList = multi.Read<Seasons>().ToList();
                    if (SeasonList.Count > 0)
                        sea = SeasonList.FirstOrDefault();
                }
            }

            return sea;
        }


    }
}