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
    public class RankingRepository
    {
        public RankingViewModel GetRankingOfWin(int? cycle, int num, string userID, bool isAdmin)
        {
            int maxRanking = 500;
            RankingViewModel vr = new RankingViewModel();
            List<Ranking> rankinglist = new List<Ranking>();

            cycle = (cycle.HasValue ? cycle : 1);
            DateTime sdate, edate;

            sdate = DateTime.Today;
            edate = DateTime.Today.AddDays(1);

            switch (cycle)
            {
                case 1:
                    sdate = DateTime.Today;
                    edate = DateTime.Today.AddDays(1);
                    break;
                case 2:
                    sdate = DateTime.Now.AddDays(((int)DateTime.Now.DayOfWeek - 1) * -1);
                    edate = DateTime.Today.AddDays(1);
                    break;
                case 3:
                    sdate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/") + "01");
                    edate = DateTime.Today.AddDays(1);
                    break;
                case 4:
                    string fmonth = "";
                    switch(DateTime.Today.Month)
                    {
                        case 1:
                        case 2:
                        case 3:
                            fmonth = "01";
                            break;
                        case 4:
                        case 5:
                        case 6:
                            fmonth = "04";
                            break;
                        case 7:
                        case 8:
                        case 9:
                            fmonth = "07";
                            break;
                        case 10:
                        case 11:
                        case 12:
                            fmonth = "10";
                            break;
                    }
                    sdate = DateTime.Parse(DateTime.Now.ToString("yyyy/") + fmonth + "/01");
                    edate = DateTime.Today.AddDays(1);
                    break;
                case 10:
                    Seasons s = new SeasonsRepository().GetNowSeason();
                    sdate = s.sdate;
                    edate = s.edate;
                    break;
            }

            //舊
            string sqlquery = @"
select rank() over(order by ars.assets desc) ord,u.Id, u.UserName, u.Email, u.Name,ars.assets
from (
	SELECT top {num} ar.UserId, sum(ar.assets) as assets
	FROM AssetsRecord ar
	where ar.type in (1,-4) 
	and ar.UserId <>{userID}
	and ar.inpdate between {sdate} and {edate}
	and exists(
		select *
		from Users
		where Id=ar.UserId
		and (isnull(LockoutEndDateUtc,'19110101')<GETUTCDATE() or LockoutEnabled=0)
	)
	group by ar.UserId
	having sum(ar.assets)>0
	order by sum(ar.assets) desc
	union
	SELECT u.Id as UserId, isnull(sum(ar.assets),0) as assets
	FROM Users u
	left join AssetsRecord ar on u.Id=ar.UserId
	and ar.type in (1,-4) 
	and ar.inpdate between {sdate} and {edate}
	where u.Id={userID}
	and (isnull(LockoutEndDateUtc,'19110101')<GETUTCDATE() or LockoutEnabled=0)
	group by u.Id
) as ars
join Users u on u.Id=ars.UserId
order by ars.assets desc
";
           /* string sqlquery = @"
select rank() over(order by ars.assets desc) ord,u.Id, u.UserName, u.Email, u.Name,ars.assets
from (
	SELECT top {num} ar.UserId, sum(ar.assets) as assets
	FROM AssetsRecord ar
	where ar.type = 1
	and ar.UserId <>{userID}
	and ar.inpdate between {sdate} and {edate}
	and exists(
		select *
		from Users
		where Id=ar.UserId
		and (isnull(LockoutEndDateUtc,'19110101')<GETUTCDATE() or LockoutEnabled=0)
	)
	group by ar.UserId
	having sum(ar.assets)>0
	order by sum(ar.assets) desc
	union
	SELECT u.Id as UserId, isnull(sum(ar.assets),0) as assets
	FROM Users u
	left join AssetsRecord ar on u.Id=ar.UserId
	and ar.type in (1,-4) 
	and ar.inpdate between {sdate} and {edate}
	where u.Id={userID}
	and (isnull(LockoutEndDateUtc,'19110101')<GETUTCDATE() or LockoutEnabled=0)
	group by u.Id
) as ars
join Users u on u.Id=ars.UserId
order by ars.assets desc
";*/


            vr.rankinglist = new List<Ranking>();
            using (IDbConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["SQLData"].ConnectionString.ToString()))
            {
                sqlquery = sqlquery.Replace("{sdate}", "'" + sdate.ToString("yyyy-MM-dd HH:mm:ss") + "'");
                sqlquery = sqlquery.Replace("{edate}", "'" + edate.ToString("yyyy-MM-dd HH:mm:ss") + "'");
                sqlquery = sqlquery.Replace("{num}", maxRanking.ToString());
                sqlquery = sqlquery.Replace("{userID}", "'" + userID + "'");
                using (var multi = cn.QueryMultiple(sqlquery))
                {
                    rankinglist = multi.Read<Ranking>().ToList();
                }

                int userBeforOrd = 0;
                Ranking rk = rankinglist.Where(p => p.ID == userID).FirstOrDefault();
                if (rk!=null)
                {
                    userBeforOrd = int.Parse(rk.ord);

                    userBeforOrd = (int)Math.Round(((double)userBeforOrd - 1) / 10) * 10;
                }

                //隱藏username
                foreach(var r in rankinglist)
                {
                    if(int.Parse(r.ord) <= num || r.ID==userID || int.Parse(r.ord) == userBeforOrd)
                    {

                        if (!r.ID.Equals(userID) && !isAdmin)
                        {
                            int subl = (r.UserName.Length / 2);
                            subl = (subl >= 3 ? 3 : subl);
                            r.UserName = r.UserName.Substring(0, subl) + "*****";
                            r.isUser = false;
                        }
                        else
                        {
                            if (r.ID.Equals(userID))
                                r.isUser = true;
                        }
                        int iord;
                        if (int.TryParse(r.ord, out iord) && iord > maxRanking)
                        {
                            r.ord = maxRanking + "+";
                        }


                        vr.rankinglist.Add(r);
                    }
                }

            }
            vr.sdate = sdate;
            vr.edate = edate;
            return vr;
        }

        public RankingViewModel GetRankingOfBet(int? cycle, int num, string userID, bool isAdmin)
        {
            RankingViewModel vr = new RankingViewModel();
            List<Ranking> rankinglist = new List<Ranking>();

            cycle = (cycle.HasValue ? cycle : 1);
            DateTime sdate, edate;

            sdate = DateTime.Today;
            edate = DateTime.Today.AddDays(1);

            switch (cycle)
            {
                case 1:
                    sdate = DateTime.Today;
                    edate = DateTime.Today.AddDays(1);
                    break;
                case 2:
                    sdate = DateTime.Now.AddDays(((int)DateTime.Now.DayOfWeek - 1) * -1);
                    edate = DateTime.Today.AddDays(1);
                    break;
                case 3:
                    sdate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/") + "01");
                    edate = DateTime.Today.AddDays(1);
                    break;
                case 4:
                    string fmonth = "";
                    switch(DateTime.Today.Month)
                    {
                        case 1:
                        case 2:
                        case 3:
                            fmonth = "01";
                            break;
                        case 4:
                        case 5:
                        case 6:
                            fmonth = "04";
                            break;
                        case 7:
                        case 8:
                        case 9:
                            fmonth = "07";
                            break;
                        case 10:
                        case 11:
                        case 12:
                            fmonth = "10";
                            break;
                    }
                    sdate = DateTime.Parse(DateTime.Now.ToString("yyyy/") + fmonth + "/01");
                    edate = DateTime.Today.AddDays(1);
                    break;
                case 10:
                    Seasons s = new SeasonsRepository().GetNowSeason();
                    sdate = s.sdate;
                    edate = s.edate;
                    break;
            }


            string sqlquery = @"
select u.UserName, u.Email, u.Name,ars.assets
from (
	SELECT top {num} ar.UserId, sum(ar.assets)*-1 as assets
	FROM AssetsRecord ar
	where ar.type=-1 
	and ar.inpdate between {sdate} and {edate}
	and exists(
		select *
		from Users
		where Id=ar.UserId
		and (isnull(LockoutEndDateUtc,'19110101')<GETUTCDATE() or LockoutEnabled=0)
	)
	group by ar.UserId
	having sum(ar.assets)>0
	order by sum(ar.assets)*-1 desc
) as ars
join Users u on u.Id=ars.UserId
";
            using (IDbConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["SQLData"].ConnectionString.ToString()))
            {
                sqlquery = sqlquery.Replace("{sdate}", "'" + sdate.ToString("yyyyMMdd") + "'");
                sqlquery = sqlquery.Replace("{edate}", "'" + edate.ToString("yyyyMMdd") + "'");
                sqlquery = sqlquery.Replace("{num}", num.ToString());
                sqlquery = sqlquery.Replace("{userID}", "'" + userID + "'");
                using (var multi = cn.QueryMultiple(sqlquery))
                {
                    rankinglist = multi.Read<Ranking>().ToList();
                }
            }
            foreach (var r in rankinglist)
            {
                if (!r.ID.Equals(userID) && !isAdmin)
                {
                    r.UserName = r.UserName.Substring(0, 3) + "*****";
                }
            }
            vr.rankinglist = rankinglist;
            return vr;
        }
    }
}