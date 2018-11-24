using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using SITDto;
using SITDto.Request;
using SITDto.ViewModel;
using SITW.Helper;
using SITW.Models;
using SITW.Models.Repository;
using SITW.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;
using System.Text;
using unirest_net.http;
using System.Threading.Tasks;
using System.Net.Http;
using System.Data;
using System.Collections;
using Newtonsoft.Json.Converters;
using HtmlAgilityPack;
using SITW.Models.GameAPIModels;
using SITW.Models.ViewModel.GameAPIViewModel;

namespace SIT.Controllers
{
    [Authorize]
    public class GameAPIController : Controller
    {

        [Authorize(Roles = "Admin")]
        public async System.Threading.Tasks.Task<ActionResult> Dota2Details()
        {

            //Dota2LeagueModel.RootObject dota2leagues = null;
            //dota2leagues = await new Dota2Repository().Dota2LeagueList();


            List<Dota2League> dota2leagues = new Dota2Repository().Dota2LeagueList();
            List<object> dota2gamematchid = new List<object>();

            List<Dota2ViewModel> d2vmList = new List<Dota2ViewModel>();


            foreach (var res in dota2leagues)
            {
                Dota2ViewModel d2vm = new Dota2ViewModel();

                Dota2PlayerModel.RootObject dota2game = await new Dota2Repository().GetDota2Game(res.leagueid);


                //leaguesname.Add(res.name);
                d2vm.leaguename = res;
                //d2vm.match = dota2game;
                List<Dota2PlayerMatchModel> d2pm = new List<Dota2PlayerMatchModel>();
                foreach (var match in dota2game.result.matches)
                {
                    if (match.series_type != 0 || match.dire_team_id != 0 && match.radiant_team_id != 0)
                    {
                        //string m = match.match_id.ToString();
                        //dota2gamematchid.Add(m);
                        Dota2PlayerMatchModel d2 = new Dota2PlayerMatchModel();
                        string d2tmradian = "未知";
                        string d2tmdire = "未知";

                        d2tmradian = await new Dota2Repository().GetDota2Teams(match.radiant_team_id.ToString());
                        d2tmdire = await new Dota2Repository().GetDota2Teams(match.dire_team_id.ToString());


                        // gpvm.TeamA = teamlist.Where(p => p.sn == gamepost.TeamASn).FirstOrDefault();
                        d2.radiant_team = d2tmradian;
                        d2.dire_team = d2tmdire;
                        d2.strar_time = (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddSeconds(match.start_time);
                        d2.timeid = match.series_id;
                        d2.match_id = match.match_id;
                        Dota2PlayerListModel.RootObject dota2gamelist = await new Dota2Repository().GetDota2List(match.match_id.ToString());
                        d2.match_name = dota2gamelist.result.dire_name + " VS " + dota2gamelist.result.radiant_name;
                        d2.Dota2PlayerList = dota2gamelist;

                        if (d2.d2BO == null)
                        {
                            var bo = 1;
                            foreach (var item in d2pm)
                            {

                                if (d2.timeid == item.timeid)
                                {
                                    bo += 1;
                                }

                            }
                            d2.d2BO = bo;

                        }

                        d2pm.Add(d2);

                    }

                }
                foreach (var itemcount in d2pm)
                {
                    var d2count = 0;
                    for (int d2i = 0; d2i < d2pm.Count(); d2i++)
                    {
                        if (itemcount.timeid == d2pm[d2i].timeid)
                        {
                            d2count++;
                        }
                    }
                    itemcount.d2BOcount = d2count;


                }


                d2vm.d2match = d2pm;
                d2vmList.Add(d2vm);


            }


            return View(d2vmList);
        }


        /* [Authorize(Roles = "Admin")]
         public ActionResult Dota2Upcoming()
         {
             List<Dota2UpcomViewModel.RootObject> d2upcoming = new List<Dota2UpcomViewModel.RootObject>();


             var webGet = new HtmlWeb();
             var doc = webGet.Load("https://www.dotabuff.com/esports/");
             // HtmlNode dota2all = doc.DocumentNode.SelectSingleNode("//div[@class='home-ti8-series'][1]");

             for (int i = 1; i < 5; i++)
             {
                 Dota2UpcomViewModel.RootObject d2vm = new Dota2UpcomViewModel.RootObject();
                 d2vm.image_url = "https://www.dotabuff.com" + doc.DocumentNode.SelectSingleNode("//article[@class='home-ti8-article-logo'][1]//a[1]//img[1]").Attributes["src"].Value; ;
                 var dt = doc.DocumentNode.SelectSingleNode("//div[@class='home-ti8-series'][1]//div[@class='ti8-series'][" + i + "]//div[@class='datetime'][1]//time[1]").Attributes["datetime"].Value;
                 DateTime d2time = DateTime.Parse(dt);
                 d2vm.begin_at = d2time.ToString("yyyy-MM-dd HH:mm:ss");
                 d2vm.number_of_games = doc.DocumentNode.SelectSingleNode("//div[@class='home-ti8-series'][1]//div[@class='ti8-series'][" + i + "]//div[@class='bracket-name'][1]").InnerHtml;
                 if (doc.DocumentNode.SelectSingleNode("//div[@class='home-ti8-series'][1]//div[@class='ti8-series'][" + i + "]//div[@class='mid-info'][1]/div[1]//span[@class='team-image']").InnerHtml != "")
                 {
                     d2vm.o1name = doc.DocumentNode.SelectSingleNode("//div[@class='home-ti8-series'][1]//div[@class='ti8-series'][" + i + "]//div[@class='mid-info'][1]/div[1]//img[1]").Attributes["alt"].Value;
                     d2vm.o1image_url = doc.DocumentNode.SelectSingleNode("//div[@class='home-ti8-series'][1]//div[@class='ti8-series'][" + i + "]//div[@class='mid-info'][1]/div[1]//img[1]").Attributes["src"].Value;
                 }
                 else {
                     d2vm.o1name = "未知隊伍";
                 }


                     d2vm.o2name = doc.DocumentNode.SelectSingleNode("//div[@class='home-ti8-series'][1]//div[@class='ti8-series'][" + i + "]//div[@class='mid-info'][1]/div[3]//img[1]").Attributes["alt"].Value;
                     d2vm.o2image_url = doc.DocumentNode.SelectSingleNode("//div[@class='home-ti8-series'][1]//div[@class='ti8-series'][" + i + "]//div[@class='mid-info'][1]/div[3]//img[1]").Attributes["src"].Value;


                 //team += doc.DocumentNode.SelectSingleNode("//div[@class='home-ti8-series'][1]//div[@class='ti8-series'][" + i + "]//div[@class='mid-info'][1]//img[1]").Attributes["src"].Value;
                 //team += doc.DocumentNode.SelectSingleNode("//div[@class='home-ti8-series'][1]//a["+ i +"]").InnerHtml;
                 d2upcoming.Add(d2vm);
             }


             //ViewData["dota2"] = team;



            /* List<Dota2Upcoming.RootObject> d2upcoming = await new Dota2Repository().GetDota2Upcoming();

             d2upcoming = d2upcoming.Where(x => x.opponents.Count() != 0).ToList();
             var d2upcomingDy =  d2upcoming.OrderByDescending(x => x.begin_at).ToList();*/

        /*

            return View(d2upcoming);
        }*/

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Dota2Upcoming()
        {
            List<Dota2Upcoming.RootObject> d2upcoming = await new Dota2Repository().GetDota2Upcoming();

            d2upcoming = d2upcoming.Where(x => x.opponents.Count() > 1).ToList();
            foreach (var d2 in d2upcoming)
            {
                d2.dtstring = d2.begin_at.ToString("yyyy-MM-dd HH:mm:ss");

            }
            var d2upcomingDy = d2upcoming.OrderBy(x => x.begin_at).ToList();

            return View(d2upcomingDy);

        }



        [Authorize(Roles = "Admin")]
        public async System.Threading.Tasks.Task<ActionResult> LOLUpcoming()
        {


            List<LOLUpcoming.RootObject> lolupcoming = await new LOLRepository().GetLOLUpcoming();

            lolupcoming = lolupcoming.Where(x => x.opponents.Count() > 1 ).ToList();
            foreach(var lol in lolupcoming)
            {
                lol.dtstring = lol.begin_at.ToString("yyyy-MM-dd HH:mm:ss");

            }
            var lolupcomingDy = lolupcoming.OrderBy(x => x.begin_at).ToList();
                  
            return View(lolupcomingDy);
        }


    }
}
