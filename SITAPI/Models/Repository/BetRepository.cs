using SITDto.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Dapper;
using SITDto.Request;

namespace SITAPI.Models.Repository
{
    public class BetRepository
    {

        public List<BetListDto> getBetsByUserID(string userID)
        {

            List<BetListDto> BetList = new List<BetListDto>();

            using (IDbConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["SQLData"].ConnectionString.ToString()))
            {
                string query = @"
                SELECT g.sn AS gameSn, g.title AS game
                ,g.gameStatus
                ,g.betModel
                ,t.sn AS topicSn,t.title AS topic
                ,c.sn AS choiceSn,c.choiceStr AS choice
                ,b.Odds ,b.money,b.sn as betSn
                ,(CASE c.isTrue WHEN 1 THEN pa.realMoney when 2 then 0 ELSE b.money*-1 END) realmoney
                ,t.totalmoney as topicMoney,c.totalMoney as choiceMoney
                ,isnull(g.rake,0) as rake
                ,cu.name AS unit
                ,(case c.isTrue WHEN 1 THEN '勝' WHEN 0 THEN '負' when 2 then '無效' ELSE '未定' END) AS isTrue
                ,(case c.isTrue WHEN 1 THEN 'true' WHEN 0 THEN 'false' when 2 then 'invalid' ELSE 'unknow' END) AS isTrueValue
                ,u.userID,b.comSn
                ,b.createDate as betDatetime
                FROM dbo.bets b
                JOIN dbo.choices c ON c.sn=b.choiceSn
                JOIN dbo.topics t ON t.sn=b.topicSn
                JOIN dbo.games g ON g.sn=t.gameSn
                JOIN dbo.users u ON u.sn=b.userSn
                JOIN dbo.cfgUnit cu ON cu.sn=b.unitSn
                left join dbo.payouts pa on pa.betSn=b.sn
                WHERE u.userID=@userID
                and b.valid in (1,2)
";
                using (var multi = cn.QueryMultiple(query, new { userID = userID }))
                {
                    BetList = multi.Read<BetListDto>().ToList();
                }
            }
            return BetList;


        }

        internal List<BetListDto> getBetsByGame(BetsByGameReq bbgr)
        {
            List<BetListDto> BetList = new List<BetListDto>();

            using (IDbConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["SQLData"].ConnectionString.ToString()))
            {
                string query = @"
                SELECT g.sn AS gameSn, g.title AS game
                ,g.betModel
                ,t.sn AS topicSn,t.title AS topic
                ,c.sn AS choiceSn,c.choiceStr AS choice
                ,b.Odds ,b.money,b.sn as betSn
                ,(CASE c.isTrue WHEN 1 THEN pa.realMoney when 2 then 0 ELSE b.money*-1 END) realmoney
                ,t.totalmoney as topicMoney,c.totalMoney as choiceMoney
                ,isnull(g.rake,0) as rake
                ,cu.name AS unit
                ,(case c.isTrue WHEN 1 THEN '勝' WHEN 0 THEN '負' when 2 then '無效' ELSE '未定' END) AS isTrue
                ,(case c.isTrue WHEN 1 THEN 'true' WHEN 0 THEN 'false' when 2 then 'invalid' ELSE 'unknow' END) AS isTrueValue
                ,u.userID,b.comSn
                ,b.createDate as betDatetime
                FROM dbo.bets b
                JOIN dbo.choices c ON c.sn=b.choiceSn
                JOIN dbo.topics t ON t.sn=b.topicSn
                JOIN dbo.games g ON g.sn=t.gameSn
                JOIN dbo.users u ON u.sn=b.userSn
                JOIN dbo.cfgUnit cu ON cu.sn=b.unitSn
                JOIN dbo.users uAdmin ON uAdmin.sn=g.userSn
                left join dbo.payouts pa on pa.betSn=b.sn
                WHERE g.sn=@GameSn
                and b.valid in (1,2)
";
                using (var multi = cn.QueryMultiple(query, new { GameSn= bbgr.GameSn }))
                {
                    BetList = multi.Read<BetListDto>().ToList();
                }
            }
            return BetList;
        }
    }
}