using Microsoft.AspNet.Identity;
using SITW.Models.Repository;
using SITW.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SITW.Controllers
{
    [RoutePrefix("Ranking")]
    public class RankingController : Controller
    {
        /// <summary>
        /// 下注排行榜
        /// </summary>
        /// <param name="cycle">週期。1、日 2、周 3、月 4、季 10、賽季</param>
        /// <returns></returns>
        [Route("Bet")]
        public ActionResult Bet(int? cycle,int num)
        {
            RankingViewModel vr;
            vr = new RankingRepository().GetRankingOfBet(cycle, num, User.Identity.GetUserName(), User.IsInRole("Admin"));
            return View(vr);
        }

        /// <summary>
        /// 下注排行榜
        /// </summary>
        /// <param name="cycle">週期。1、日 2、周 3、月 4、季 10、賽季</param>
        /// <returns></returns>
        [Route("_Bet")]
        public ActionResult _Bet(int? cycle, int num)
        {
            RankingViewModel vr;
            vr = new RankingRepository().GetRankingOfBet(cycle, num, User.Identity.GetUserName(), User.IsInRole("Admin"));
            return View(vr);
        }

        /// <summary>
        /// 贏錢排行榜
        /// </summary>
        /// <param name="cycle">週期。1、日 2、周 3、月 4、季 10、賽季</param>
        /// <returns></returns>
        [Route("Win")]
        public ActionResult Win(int? cycle, int num)
        {
            RankingViewModel vr;
            vr = new RankingRepository().GetRankingOfWin(cycle, num, User.Identity.GetUserName(), User.IsInRole("Admin"));
            return View(vr);
        }

        /// <summary>
        /// 贏錢排行榜
        /// </summary>
        /// <param name="cycle">週期。1、日 2、周 3、月 4、季 10、賽季</param>
        /// <returns></returns>
        [Route("_Win")]
        public ActionResult _Win(int? cycle, int num)
        {
            RankingViewModel vr;
            vr = new RankingRepository().GetRankingOfWin(cycle, num, User.Identity.GetUserId(), User.IsInRole("Admin"));
            return View(vr);
        }
        [Route("Rank")]
        public ActionResult _Rank(int? cycle, int num)
        {
            RankingViewModel vr;
            vr = new RankingRepository().GetRankingOfWin(cycle, num, User.Identity.GetUserId(), User.IsInRole("Admin"));
            Ranking rk = vr.rankinglist.Where(p => p.isUser).FirstOrDefault();
            if (rk != null)
            {
                ViewBag.UserName = User.Identity.GetUserName();
                ViewBag.WinMoney = rk.Assets;
                ViewBag.ord = rk.ord;
            }
            return View(vr);
        }
        [Route("_PartialRank")]
        public ActionResult _PartialRank(int? cycle, int num,int? pclass)
        {
            RankingViewModel vr;
            vr = new RankingRepository().GetRankingOfWin(cycle, num, User.Identity.GetUserId(), User.IsInRole("Admin"));
            Ranking rk = vr.rankinglist.Where(p => p.isUser).FirstOrDefault();
            if (rk != null)
            {            
                ViewBag.WinMoney = rk.Assets;
                ViewBag.ord = rk.ord;
            }
            if(pclass == 1)
            {
                ViewBag.pclass = 1;
            }
            else
            {
                ViewBag.pclass = 2;
            }

            return View(vr);
        }
    }
}