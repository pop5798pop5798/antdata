using AutoMapper;
using Dapper;
using SITDto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace SITAPI.Models.Repository
{
    public class GameRepository
    {
        private sitdbEntities db = new sitdbEntities();

        public List<gameDto> GetGame()
        {
            List<gameDto> gamedtoList = new List<gameDto>();

            using (IDbConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["SQLData"].ConnectionString.ToString()))
            {
                string query = @"
SELECT g.*,u.userID
FROM dbo.games g
JOIN dbo.users u ON u.sn=g.userSn
where g.valid=1
";
                using (var multi = cn.QueryMultiple(query, new { }))
                {
                    IEnumerable<gameDto> gtolist = multi.Read<gameDto>();
                    if (gtolist.Count() > 0)
                    {
                        gamedtoList = gtolist.ToList();
                        
                    }
                }
            }
            return gamedtoList;

        }

        public List<gameDto> GetGameList()
        {
            List<gameDto> gamedtoList = new List<gameDto>();

            using (IDbConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["SQLData"].ConnectionString.ToString()))
            {
                string query = @"
SELECT g.*,u.userID
into #games
FROM dbo.apis g
JOIN dbo.users u ON u.sn=g.userSn
where g.valid=1

select *
from #games

SELECT t.*
FROM dbo.apis g
JOIN dbo.topics t ON 
 t.apiSn=g.sn
WHERE exists(
	select *
	from #games
	where sn=g.sn
)
and t.valid=1

SELECT c.*
,isnull((
	select sum(b.money) as betMoneygti
	from bets b
	where b.choiceSn=c.sn
),0) as betMoneygti
FROM dbo.apis g
JOIN dbo.topics t ON  t.apiSn=g.sn
JOIN dbo.choices c ON
	c.topicSn=t.sn
WHERE exists(
	select *
	from #games
	where sn=g.sn
)
select c.sn as choiceSn, isnull(sum(b.money),0) as betMoney
from #games g
left join topics t on t.apiSn=g.sn
left join choices c on c.topicSn=t.sn
left join bets b on b.choiceSn=c.sn 
left join choiceOdds co on co.choiceSn=c.sn 
group by c.sn
order by c.sn

";
                using (var multi = cn.QueryMultiple(query))
                {
                    gamedtoList = multi.Read<gameDto>().ToList();
                    var topic = multi.Read<topicDto>();
                    var choice = multi.Read<choiceDto>();
                    var choiceBet = multi.Read<choiceBetMoneyDto>();
                    foreach (gameDto gamedto in gamedtoList)
                    {
                        gamedto.topicList = topic.Where(p => p.gameSn == gamedto.sn).ToList();

                        foreach (var t in gamedto.topicList)
                        {
                            if (!gamedto.canbet)
                                t.canbet = false;
                            t.choiceList = choice.Where(p => p.topicSn == t.sn).ToList();
                            foreach(var c in t.choiceList)
                            {
                                c.betMoney = choiceBet.Where(p => p.choiceSn == c.sn).ToList();
                                if (gamedto.apiModel == 2)
                                {
                                    c.Odds = null;
                                }
                                c.valid = 1;
                            }
                        }
                    }
                }
            }
            return gamedtoList;

        }

        public gameDto GetGame(int id)
        {
            gameDto gamedto = new gameDto();

            using (IDbConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["SQLData"].ConnectionString.ToString()))
            {
                string query = @"
SELECT g.*,u.userID
FROM dbo.games g
JOIN dbo.users u ON u.sn=g.userSn
WHERE g.sn=@gameSn

SELECT t.*
FROM dbo.games g
JOIN dbo.topics t ON 
	g.comSn = t.comSn
	AND t.gameSn=g.sn
WHERE g.sn=@gameSn


SELECT c.*
FROM dbo.games g
JOIN dbo.topics t ON 
	g.comSn = t.comSn
	AND t.gameSn=g.sn
JOIN dbo.choices c ON
	c.topicSn=t.sn
WHERE g.sn=@gameSn
";
                using (var multi = cn.QueryMultiple(query, new { gameSn = id }))
                {
                    IEnumerable<gameDto> gtolist = multi.Read<gameDto>();
                    if (gtolist.Count() > 0)
                    {
                        gamedto = gtolist.Single();
                        var topic = multi.Read<topicDto>().ToList();
                        gamedto.topicList = topic;
                        var choice = multi.Read<choiceDto>().ToList();

                        foreach (var t in gamedto.topicList)
                        {
                            if (!gamedto.canbet)
                                t.canbet = false;
                            t.choiceList = choice.Where(p => p.topicSn == t.sn).ToList();
                        }
                    }
                }
            }
            return gamedto;

        }

        public api setData(api g)
        {
            if (g.sn == 0)
                return AddNew(g);
            else
                return Edit(g);
        }

        private api AddNew(api g)
        {
            db.apis.Add(g);
            db.SaveChanges();
            return g;
        }

        private api Edit(api g)
        {
            g.modiDate = DateTime.Now;
            api gdb = db.apis.Where(p => p.sn == g.sn).FirstOrDefault();
            gdb.sdate = g.sdate;
            gdb.edate = g.edate;
            gdb.apiModel = g.apiModel;
            gdb.modiDate = g.modiDate;
            gdb.title = g.title;
            gdb.comment = g.comment;
            gdb.apidate = g.apidate;
            gdb.apiplace = g.apiplace;
            gdb.apidate = g.apidate;
            db.SaveChanges();
            return g;
        }


        public bool SetGame(int id, gameDto gamed)
        {
            try
            {
                gameDto gameDB = GetGame(gamed.sn);
                byte gameStatus = (gameDB.apiStatus.HasValue ? gameDB.apiStatus.Value : (byte)1);
                //if (!new byte[] { 1 }.Contains(gameStatus))
                //    return false;
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<gameDto, api>();
                    cfg.CreateMap<topicDto, topic>();
                    cfg.CreateMap<choiceDto, choice>();
                });
                api game = Mapper.Map<api>(gamed);
                game = setData(game);


                foreach (topicDto topicd in gamed.topicList)
                {
                    if (topicd.valid.HasValue && topicd.valid.Value == 1)
                    {
                        topic topic = Mapper.Map<topic>(topicd);
                        topic.apiSn = game.sn;
                        topic = new TopicRepository().setData(topic);

                        foreach (choiceDto choiced in topicd.choiceList)
                        {
                            if (choiced.valid.HasValue && choiced.valid.Value == 1)
                            {
                                choice choice = Mapper.Map<choice>(choiced);
                                choice.topicSn = topic.sn;
                                choice = new ChoiceRepository().setData(choice);
                            }
                        }
                    }
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!gameExists(id))
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public byte? getGameCloseStatus(int id)
        {
            if (gameExists(id))
            {
                api g = db.apis.Where(p => p.sn == id).FirstOrDefault();
                return g.apiStatus;
            }
            else
                return null;
        }

        public bool setGameCloseStatus(int id,byte gameStatus)
        {
            if (gameExists(id))
            {
                api g = db.apis.Where(p => p.sn == id).FirstOrDefault();
                byte gs = (g.apiStatus.HasValue ? g.apiStatus.Value : (byte)0);
                if (gameStatus == 2 && !new byte[] { 0, 3 , 4 }.Contains(gs))
                    return false;
                g.apiStatus = gameStatus;
				
                //g.modiDate = System.DateTime.Now;
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool setWinner(List<choiceDto> choiceList)
        {
            foreach(choiceDto choiced in choiceList)
            {
                choice choice = db.choices.Where(p => p.sn == choiced.sn).FirstOrDefault();
                db.SaveChanges();
            }
            return true;
        }

        public List<payoutDto> pays(int id)
        {
            List<payoutDto> payoutList = new List<payoutDto>();
            using (SqlConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["SQLData"].ConnectionString.ToString()))
            {
                string sql = @"
                BEGIN tran
                BEGIN TRY

                    update t set t.totalmoney=(
	                    select sum(b.money) from bets b 
	                    where b.topicSn=t.sn and b.valid in (1,2)
	                    and not exists(select * from choices c where c.isTrue in (2) and b.choiceSn=c.sn)
                    )
                    from topics t
                    where t.gameSn=@gameSn
	                and not exists(
		                select *
		                from payouts p
		                where p.topicSn=t.sn
	                )

                    update c set c.totalMoney=(select sum(b.money) from bets b where b.choiceSn=c.sn and b.valid in (1,2))
                    from choices c
                    join topics t on t.sn=c.topicSn
                    where t.gameSn=@gameSn
	                and not exists(
		                select *
		                from payouts p
		                where p.topicSn=t.sn
	                )
                    
                    --賠率模式
	                INSERT INTO [dbo].[payouts]
			            ([betSn]
			            ,[comSn]
			            ,[gameSn]
			            ,[topicSn]
						,[choiceSn]
			            ,[userSn]
			            ,[Odds]
			            ,[money]
			            ,[realMoney]
                        ,[unitSn]
                        ,[isTrue])
	                SELECT b.sn,b.comSn,g.sn,t.sn,c.sn,b.userSn
	                ,b.Odds,b.[money],b.Odds*b.[money] as [realMoney]
					,b.unitSn,c.isTrue
	                FROM dbo.games g
	                JOIN dbo.topics t ON t.gameSn=g.sn
	                JOIN dbo.choices c ON c.topicSn=t.sn
	                JOIN dbo.bets b ON b.choiceSn=c.sn
	                WHERE g.sn=@gameSn
	                AND c.isTrue=1
	                AND b.valid=1
	                AND g.gameStatus=4
					AND g.betModel=1
					AND b.money>0
	                and not exists(
		                select *
		                from payouts p
		                where p.betSn=b.sn
	                )

                    --總彩池模式
                    INSERT INTO [dbo].[payouts]
	                    ([betSn]
	                    ,[comSn]
	                    ,[gameSn]
	                    ,[topicSn]
						,[choiceSn]
	                    ,[userSn]
	                    ,[Odds]
	                    ,[money]
	                    ,[realMoney]
                        ,[unitSn]
                        ,[topicTotalMoney]
                        ,[correctTotalMoney]
                        ,[isTrue]
	                    ,[rake])
                    select *
                    from(
	                    SELECT b.sn as betSn,b.comSn,g.sn as gameSn,t.sn as topicSn
						,c.sn as choiceSn,b.userSn,b.Odds,b.[money]
	                    ,round(sum(b.money) over(PARTITION BY c.topicSn)*b.money/sum(b.money) over(PARTITION BY c.topicSn,c.isTrue)
		                    * (CASE WHEN c.isTrue =1 THEN 1 ELSE 0 END)
		                    * (100-isnull(g.rake,0))/100
		                    ,0) as [realMoney]
	                    ,b.unitSn
	                    ,sum(b.money) over(PARTITION BY c.topicSn) as topicTotalMoney
	                    ,sum(b.money) over(PARTITION BY c.topicSn,c.isTrue) as correctTotalMoney
	                    ,c.isTrue,isnull(g.rake,0) as rake
	                    FROM dbo.games g
	                    JOIN dbo.topics t ON t.gameSn=g.sn
	                    JOIN dbo.choices c ON c.topicSn=t.sn
	                    JOIN dbo.bets b ON b.choiceSn=c.sn
	                    WHERE g.sn=@gameSn
	                    AND b.valid in (1,2)
	                    --AND g.gameStatus=4
	                    AND g.betModel=2
	                    AND b.money>0
	                    and c.isTrue not in (2)
						and not exists(
							select *
							from payouts p
							where p.betSn=b.sn
						)
                    ) as payt
                    where isTrue=1

                    --返還
                    INSERT INTO [dbo].[payouts]
	                    ([betSn]
	                    ,[comSn]
	                    ,[gameSn]
	                    ,[topicSn]
						,[choiceSn]
	                    ,[userSn]
	                    ,[Odds]
	                    ,[money]
	                    ,[realMoney]
                        ,[unitSn]
                        ,[isTrue]
	                    ,[rake])
                    SELECT b.sn,b.comSn,g.sn,t.sn,c.sn,b.userSn
                    ,b.Odds,b.[money],b.[money] as [realMoney]
                    ,b.unitSn,c.isTrue,g.rake
                    FROM dbo.games g
                    JOIN dbo.topics t ON t.gameSn=g.sn
                    JOIN dbo.choices c ON c.topicSn=t.sn
                    JOIN dbo.bets b ON b.choiceSn=c.sn
                    WHERE g.sn=@gameSn
                    AND c.isTrue=2
                    AND b.valid=1
                    AND g.gameStatus=4
                    AND b.money>0
	                and not exists(
		                select *
		                from payouts p
		                where p.betSn=b.sn
	                )

	                UPDATE b SET b.valid=3
	                FROM dbo.games g
	                JOIN dbo.topics t ON t.gameSn=g.sn
	                JOIN dbo.choices c ON c.topicSn=t.sn
	                JOIN dbo.bets b ON b.choiceSn=c.sn
	                WHERE g.sn=@gameSn
	                AND EXISTS(
		                SELECT * FROM dbo.payouts pp
		                WHERE pp.gameSn=g.sn
	                )
	                AND b.valid=1

	                COMMIT
                END TRY
                BEGIN CATCH
                    ROLLBACK
                END CATCH;
                ";
                cn.Open();
                SqlCommand myCommand = new SqlCommand(sql, cn);
                myCommand.Parameters.Add("@gameSn", SqlDbType.Int);
                myCommand.Parameters["@gameSn"].Value = id;


                myCommand.ExecuteReader();


                string query = @"
	                select p.*,u.userId
	                from payouts p
	                join users u on u.sn=p.userSn
	                join bets b on b.sn=p.betSn
	                where p.gameSn=@gameSn
	                and b.valid=3
";
                using (var multi = cn.QueryMultiple(query, new { gameSn= id }))
                {
                    IEnumerable<payoutDto> pdlist = multi.Read<payoutDto>();
                    if (pdlist.Count() > 0)
                    {
                        payoutList = pdlist.ToList();
                    }
                }


                sql = @"
	                UPDATE b SET b.valid=2
	                FROM dbo.games g
	                JOIN dbo.topics t ON t.gameSn=g.sn
	                JOIN dbo.choices c ON c.topicSn=t.sn
	                JOIN dbo.bets b ON b.choiceSn=c.sn
	                WHERE g.sn=@gameSn
	                AND EXISTS(
		                SELECT * FROM dbo.payouts pp
		                WHERE pp.gameSn=g.sn
	                )
	                AND b.valid=3
";
                myCommand = new SqlCommand(sql, cn);
                myCommand.Parameters.Add("@gameSn", SqlDbType.Int);
                myCommand.Parameters["@gameSn"].Value = id;
                myCommand.ExecuteReader();



            }
            return payoutList;
        }

        public List<payoutDto> paysRollback(int id)
        {
            List<payoutDto> payoutList = new List<payoutDto>();
            using (SqlConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["SQLData"].ConnectionString.ToString()))
            {

                string query = @"
					select p.*,u.userId
					from payouts p
					join users u on u.sn=p.userSn
					where p.gameSn=@gameSn
";
                using (var multi = cn.QueryMultiple(query, new { gameSn = id }))
                {
                    IEnumerable<payoutDto> pdlist = multi.Read<payoutDto>();
                    if (pdlist.Count() > 0)
                    {
                        payoutList = pdlist.ToList();
                    }
                }


                string sql = @"
	                UPDATE b SET b.valid=1
	                FROM dbo.games g
	                JOIN dbo.topics t ON t.gameSn=g.sn
	                JOIN dbo.choices c ON c.topicSn=t.sn
	                JOIN dbo.bets b ON b.choiceSn=c.sn
	                WHERE g.sn=@gameSn
	                AND EXISTS(
		                SELECT * FROM dbo.payouts pp
		                WHERE pp.gameSn=g.sn
	                )
	                AND b.valid=2

                    delete p
                    from payouts p
                    where p.gameSn=@gameSn
                ";
                cn.Open();
                SqlCommand myCommand = new SqlCommand(sql, cn);
                myCommand.Parameters.Add("@gameSn", SqlDbType.Int);
                myCommand.Parameters["@gameSn"].Value = id;


                myCommand.ExecuteReader();
            }
            return payoutList;
        }


            private bool gameExists(int id)
        {
            return db.apis.Count(e => e.sn == id) > 0;
        }


    }
}