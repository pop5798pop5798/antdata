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
    public class MissionsRepository
    {
        private sitwEntities db = new sitwEntities();

        public List<Missions> getAll()
        {
            return db.Missions.ToList();
        }

        public Missions Details(int? id)
        {
            Missions missions = db.Missions.Find(id);
            if (missions == null)
            {
                missions = new Missions();
            }
            return missions;
        }

        public Missions AddMission(Missions missions)
        {
            db.Missions.Add(missions);
            db.SaveChanges();
            return missions;
        }

        public MissionStart AddMissionStart(MissionStart missionStart)
        {
            db.MissionStart.Add(missionStart);
            db.SaveChanges();
            return missionStart;
        }

        public MissionEnd AddMissionEnd(MissionEnd missionEnd)
        {
            db.MissionEnd.Add(missionEnd);
            db.SaveChanges();
            return missionEnd;
        }

        public MissionAssets AddMissionAsset(MissionAssets missionAsset)
        {
            db.MissionAssets.Add(missionAsset);
            db.SaveChanges();
            return missionAsset;
        }

        private class missionSql
        {
            public int sn { get; set; }
            public string sqlQuery { get; set; }
            public int Compare { get; set; }
            public int num { get; set; }
        }

        public IEnumerable<MissionNoteModel> GetMissionCode(string UserID)
        {
            IEnumerable<MissionNoteModel> Mission = new List<MissionNoteModel>();
            if(string.IsNullOrEmpty(UserID))
            {
                return Mission;
            }
            using (IDbConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["SQLData"].ConnectionString.ToString()))
            {
                string sqlGetMissionStart = @"
SELECT m.sn, cms.sqlquery,ms.Compare,ms.num
FROM dbo.Missions m
JOIN dbo.MissionStart ms ON ms.missionSn=m.sn
JOIN dbo.cfgMissionStart cms ON cms.sn=ms.missionStartSn
WHERE m.valid=1
AND getdate() BETWEEN m.sdate AND m.edate
";
                IEnumerable<missionSql> sqllist;
                using (var multi = cn.QueryMultiple(sqlGetMissionStart, new { userId = UserID }))
                {
                    sqllist = multi.Read<missionSql>();
                    
                }

                string sqlWhere = "";
                int lastSn = 0;
                foreach(var ms in sqllist.OrderBy(p=>p.sn))
                {
                    if (lastSn != ms.sn)
                    {
                        sqlWhere += (string.IsNullOrEmpty(sqlWhere) ? "" : " UNION ALL ");
                        sqlWhere += @"
	SELECT @userId AS userID,m.sn AS missionSn
	,NULL AS assetsRecordSn
	,@today AS inpdate
	,1 as valid
	,(convert(varchar,(case m.cycle 
	when 0 then '2017/01/01'
	when 1 then @today
	when 2 then dateadd(day,(DATEPART(WEEKDAY,dateadd(day,-1,@today))-1)*-1,@today)
	when 3 then substring(convert(varchar,@today,111),1,8)+'01'
	when 5 then substring(convert(varchar,@today,111),1,5)+@QuarterlyFirstMonth+'/01'
	end),111)) as sdate
	,dateadd(s,-1, (convert(varchar,(case m.cycle 
	when 0 then dateadd(day,1,m.edate)
	when 1 then dateadd(day,1,@today)
	when 2 then dateadd(day,7-DATEPART(WEEKDAY, dateadd(day,-1,@today)),@today)
	when 3 then dateadd(month,1,substring(convert(varchar,@today,111),1,8)+'01')
	when 5 then dateadd(month,1,substring(convert(varchar,@today,111),1,5)+@QuarterlyFirstMonth+'/01')
	end),111))) edate
	,m.sdate as msdate,m.edate as medate
	FROM dbo.Missions m
	WHERE m.sn=" + ms.sn;
                    }
                    MissionCompare mc = new MissionCompare { CompareSn = ms.Compare };
                    sqlWhere += @"
	AND EXISTS(
		" + ms.sqlQuery.Replace("{Compare}", mc.CompareSQLText).Replace("{num}", ms.num.ToString()) + @"
	)
    ";
                    lastSn = ms.sn;
                }

                //把任務寫入個人的任務table
                string sqlquery = @"
declare @QuarterlyFirstMonth varchar(2),@QuarterlyLastMonth varchar(2),@today datetime
set @today=getdate()
declare @fishMission table(
	missionSn int
)

SELECT @QuarterlyFirstMonth=case when datepart(month,@today) between 1 and 3 then 1
		when datepart(month,@today) between 4 and 6 then 4
		when datepart(month,@today) between 7 and 9 then 7
		when datepart(month,@today) between 10 and 12 then 10
		end
SELECT @QuarterlyLastMonth=case when datepart(month,@today) between 1 and 3 then 3
		when datepart(month,@today) between 4 and 6 then 5
		when datepart(month,@today) between 7 and 9 then 9
		when datepart(month,@today) between 10 and 12 then 12
		end


INSERT INTO [dbo].[UserMissions]
([userID]
,[missionSn]
,[assetsRecordSn]
,[inpdate]
,[valid]
,[sdate]
,[edate])
SELECT userID,missionSn,assetsRecordSn,inpdate,valid
,(case when sdate<msdate then msdate else sdate end) as sdate
,(case when edate>medate then medate else edate end) as edate
FROM (
";
                sqlquery += sqlWhere;
                sqlquery += @"
) AS mis
WHERE NOT EXISTS(
	SELECT *
	FROM dbo.UserMissions um
	WHERE um.userID=mis.userID
	AND um.missionSn=mis.missionSn
	and @today between um.sdate and um.edate
)


SELECT m.sn,um.sn as userMissionSn, m.name ,m.comment,m.edate,m.imgURL
FROM dbo.UserMissions um
JOIN dbo.Missions m ON m.sn=um.missionSn
WHERE um.userID=@userId
and @today between um.sdate and um.edate
and um.valid=1

SELECT um.missionSn, cme.name AS finshName,me.Compare,me.num
,replace(
	replace(cme.sqlquery,'{sdate}',''''+convert(varchar,um.sdate,120)+'''')
	,'{edate}',''''+convert(varchar,um.edate,120)+''''
) as sqlquery
FROM dbo.UserMissions um
JOIN dbo.MissionEnd me ON um.missionSn = me.missionSn
JOIN dbo.cfgMissionEnd cme ON cme.sn=me.missionEndtSn
JOIN dbo.Missions m on m.sn=um.missionSn
WHERE um.userID=@userId
and @today between um.sdate and um.edate
and um.valid=1


SELECT ma.sn,ma.missionSn,ma.assets,ma.unitSn,cu.showStr as unitStr
FROM dbo.UserMissions um
JOIN MissionAssets ma on ma.missionSn=um.missionSn
join cfgUnit cu on cu.sn=ma.unitSn
WHERE um.userID=@userId
and @today between um.sdate and um.edate
and um.valid=1
";
                List<MissionEndViewModel> mevmList = new List<MissionEndViewModel>();
                List<MissionAssetsViewModel> maList = new List<MissionAssetsViewModel>();
                if (!string.IsNullOrEmpty(sqlWhere))
                {
                    using (var multi = cn.QueryMultiple(sqlquery, new { userId = UserID }))
                    {
                        Mission = multi.Read<MissionNoteModel>();
                        mevmList = multi.Read<MissionEndViewModel>().ToList();
                        maList = multi.Read<MissionAssetsViewModel>().ToList();
                    }

                    string sql = "";
                    foreach (var m in mevmList)
                    {
                        sql += (string.IsNullOrEmpty(sql) ? "" : " UNION ALL ");
                        sql += @"
select " + m.missionSn + @" as missionSn,'" + m.finshName + "' as finshName," + m.Compare + " as Compare," + m.num + " as num,isnull((" + m.sqlquery + "),0) as nownum";
                    }
                    if (!string.IsNullOrEmpty(sql))
                    {
                        using (var multi = cn.QueryMultiple(sql, new { userId = UserID }))
                        {
                            mevmList = multi.Read<MissionEndViewModel>().ToList();
                        }
                    }


                    foreach (var m in Mission)
                    {
                        m.mevList = mevmList.Where(p => p.missionSn == m.sn).ToList();
                        m.maList = maList.Where(p => p.missionSn == m.sn).ToList();
                    }
                }
            }


            return Mission;
        }


        public bool SetMissionFinsh(string UserID, int userMissionSn)
        {
            List<MissionNoteModel> mnList = GetMissionCode(UserID).ToList();
            MissionNoteModel mn = mnList.Where(p => p.userMissionSn == userMissionSn).FirstOrDefault();
            if (mn == null)
                return false;
            if (mn.isFinsh)
            {
                foreach(MissionAssets ma in mn.maList)
                {
                    AssetsRecord ar = new AssetsRecord
                    {
                        type = 3,
                        unitSn = ma.unitSn,
                        assets = ma.assets,
                        UserId = UserID,
                        inpdate = DateTime.Now
                    };
                    new AssetsRepository().AddAssetsByAssets(ar);

                    UserMissions um = db.UserMissions.Where(p => p.sn == userMissionSn && p.userID == UserID).FirstOrDefault();
                    um.valid = 2;
                    db.SaveChanges();
                }
                return true;
            }
            else
                return false;
        }
    }
}