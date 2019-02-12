﻿using Microsoft.AspNet.Identity;
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
using System.Web.Helpers;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;
using System.Web.UI.HtmlControls;

namespace SITW.Controllers
{
    //[Authorize]
    public class AdminUserController : Controller
    {
        string encryptedKey;
        private static MemoryCache _cache = MemoryCache.Default;
        List<gameDto> gList;
        public AdminUserController()
        {
            encryptedKey = System.Web.Configuration.WebConfigurationManager.AppSettings["encryptedKey"];
            if (_cache.Contains("GameList"))
                gList = _cache.Get("GameList") as List<gameDto>;
            //client = new HttpClient();
            //client.BaseAddress = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["apiurl"]);
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public ActionResult Index()
        {
            return View();
        }

        // GET: game/Create
        //[Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            gameDto game = new gameDto { topicList = new List<topicDto>(), priceList = new List<priceDto>() };
            GamePostViewModel gpvm = new GamePostViewModel { game = game };
            gpvm.topicsetting = new GamePostsRepository().GetTopicsAll();


            return View(gpvm);
        }
        // POST: game/Create
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async System.Threading.Tasks.Task<ActionResult> Create(GamePostViewModel gpvm)
        {
            try
            {

                gpvm.game.topicList = gpvm.game.topicList.Where(x => x.valid != 0).ToList();
                gameDto game = gpvm.game;               
                game.userId = User.Identity.GetUserId();
                game.comSn = 1;
                game.comment = gpvm.comment;
                game = await new GamesRepository().Create(game);




                APIPosts gp = new APIPosts { ApiSn = game.sn, valid = 1, inpdate = DateTime.Now };


                gp.sdate = gpvm.game.sdate;
                gp.edate = gpvm.game.edate;
                gp.categorySn = gpvm.gamepost.categorySn;
                gp.coverhref = gpvm.gamepost.coverhref;
                new GamePostsRepository().add(gp);            

                APIPosts gamepost = new GamePostsRepository().getgame(game.sn);
                return RedirectToAction("DetailsAdmin", new { id = gamepost.sn });
            }
            catch
            {
                return View(gpvm);
            }
        }

        /*[Authorize(Roles = "Admin")]*/
        public ActionResult _priceCreate(gameDto model, int? index)
        {
            index = index ?? 0;
            ViewBag.Index = index;
            model.priceList = new List<priceDto>();
            for (int i = 0; i <= index; i++)
            {
                model.priceList.Add(new priceDto { valid = 1});
            }
            GamePostViewModel gpvm = new GamePostViewModel { game = model };
            return PartialView(gpvm);
        }

        public async System.Threading.Tasks.Task<ContentResult> DataJson()
        {
            string[] b = { "sn", "a2", "a3", "unitSn" };
            object[] v = { "sn", 1, true, "unitSn" };

            JObject array = new JObject();
            array.Add(new JProperty( "Manual text", v[0] ));
            array.Add(new JProperty("time", new DateTime(2000, 5, 23) ));
            array.Add(new JProperty(b[3], v[2]));


            return Content(array.ToString(), "application/json");
  
     
        }

        public ActionResult GetWeb()
        {
            


            return View();
        }

        //htmlcode
        public string jquerytest() {
            HtmlDocument dochtml = new HtmlDocument();
            string html = "<body>"+"<img alt='Team Secret' class='img-team img-avatar' src='https://riki.dotabuff.com/t/l/e73oP7ItcP.png'>"+ "</body>";

            dochtml.LoadHtml(html);
            var divsWithText = dochtml.DocumentNode.SelectSingleNode("//body");
            /*var innerHtmlAndClass =
               divsWithText
                   .Select(div =>
                       new
                       {
                           Class = div.Attributes["class"].Value
                       });*/
            

            /*var divsWithText = dochtml
                 .DocumentNode
                 .Descendants("div")
                 .Where(node => node.Descendants()
                                    .Any(des => des.NodeType == HtmlNodeType.Text))
                 .ToList();
            var innerHtmlAndClass =
               divsWithText
                   .Select(div =>
                       new
                       {
                           Class = div.Attributes["class"].Value
                       });*/
           /* foreach (var item in innerHtmlAndClass)
            {
                Console.WriteLine("class='{0}' innerHtml='{1}'", item.Class, item.InnerHtml);
            }*/


            var webGet = new HtmlWeb();
            var doc = webGet.Load("https://www.dotabuff.com/");
            //HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class='team left-team']//span[@class='name']//div[@class='r-none-mobile']//a[@class='esports-team esports-link team-link']//span[@class='team-text team-text-name']//div[@class='r-only-mobile']//a[@class='esports-team esports-link team-link']//span[@class='team-text team-text-tag']//span[@class='image']//span[@class='esports-team ']//span[@class='team-image']//img[@class='img-team img-avatar']//span[@class='line low-light']//span[@class='important']");


            string d = "";
            foreach (var div in divsWithText.Descendants())
            {

                if (div.NodeType == HtmlNodeType.Element)
                {
                    d += "//" + div.Name + ((div.Attributes["class"] != null) ? "[@class='"+div.Attributes["class"].Value +"']" : "");
                    
                   
                }
            }

            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(d);
            /* var sb = new StringBuilder();
             sb.AppendFormat("<html>");
             sb.AppendFormat("<div class='x'>");
             sb.AppendFormat("<p>this is the text I have.</p>");
             sb.AppendFormat("<p>Another part of text.</p>");
             sb.AppendFormat("</div>");
             sb.AppendFormat("</html>");

             const string stringToSearch = "<p>this is the text I have.</p><p>Another part of text.</p>";

             var document = new HtmlDocument();
             document.LoadHtml(sb.ToString());

             var divsWithText = document
                 .DocumentNode
                 .Descendants("div")
                 .Where(node => node.Descendants()
                                    .Any(des => des.NodeType == HtmlNodeType.Text))
                 .ToList();

             var divsWithInnerHtmlMatching =
                 divsWithText
                     .Where(div => div.InnerHtml.Equals(stringToSearch))
                     .ToList();

             var innerHtmlAndClass =
                 divsWithInnerHtmlMatching
                     .Select(div =>
                         new
                         {
                             InnerHtml = div.InnerHtml,
                             Class = div.Attributes["class"].Value
                         });

             foreach (var item in innerHtmlAndClass)
             {
                 Console.WriteLine("class='{0}' innerHtml='{1}'", item.Class, item.InnerHtml);
             }*/
            string vr = "";
            foreach (var item in nodes)
            {
                //vr += item.InnerText;
                vr += item.Attributes["src"].Value;
            }
            return vr;

        }






        public ActionResult Sitemap()
        {
            Response.ContentType = "application/xml";
            return View();
        }
        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        public ActionResult _Banner()
        {
            DateTime date1 = DateTime.Parse("2018-05-01T00:00:00");
            DateTime dt = DateTime.Now;

            if (DateTime.Compare(date1, dt) < 0)
            {               
                ViewBag.Banner = 1;

            }
            else
            {
                ViewBag.Banner = 0;
            }

            

            return View();
        }

        public ActionResult IndexPage()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult News()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Repair()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
    }
}