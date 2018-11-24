using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SITW.Models;
using SITW.Models.Interface;
using SITW.Models.Repository;
using PagedList;
using GoogleCloudSamples.Services;
using SITW.Models.ViewModel;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;
using System.Text;

namespace SITW.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NewsController : Controller
    {
        private INewsRepository newsRepository;
        private int IndexpageSize = 6;
        private int NewspageSize = 5;
        private ImageUploader _imageUploader;

        public NewsController()
        {
            this.newsRepository = new NewsRepository();
        }

        [AllowAnonymous]
        public ActionResult Index()
        {   
            var news = this.newsRepository.GetAll().OrderByDescending(x => x.real_time).ToList();
            var newssize = news.ToPagedList(1, IndexpageSize);
            return View(newssize);
        }
        [AllowAnonymous]
        public ActionResult _NewsMain()
        {
            var news = this.newsRepository.GetAll().OrderByDescending(x => x.real_time).ToList();
            List<NewsViewModel> newsList = new List<NewsViewModel>();
            DateTime dt = DateTime.Now;
            foreach (var ns in news)
            {
                NewsViewModel newsm = new NewsViewModel();
                newsm.Id = ns.Id;
                newsm.title = ns.title;
                newsm.p_class = ns.p_class;
                newsm.image = ns.image;

                if (ns.time <= dt && ns.time != null)
                {
                    newsList.Add(newsm);
                    newsm.real_time = ns.time;
                }
                else if (ns.time == null)
                {
                    newsList.Add(newsm);
                    newsm.real_time = ns.real_time;
                }


            }       


            var newssize = newsList.ToPagedList(1, IndexpageSize);
            return View(newssize);
        }
        [AllowAnonymous]
        public ActionResult _News()
        {
            var news = this.newsRepository.GetAll().OrderByDescending(x => x.real_time).ToList();
            List<NewsViewModel> newsList = new List<NewsViewModel>();
            DateTime dt = DateTime.Now;
            foreach (var ns in news)
            {
                NewsViewModel newsm = new NewsViewModel();
                newsm.Id = ns.Id;
                newsm.title = ns.title;
                newsm.p_class = ns.p_class;


                if (ns.time <= dt && ns.time != null)
                {
                    newsList.Add(newsm);
                    newsm.real_time = ns.time;
                }
                else if (ns.time == null)
                {
                    newsList.Add(newsm);
                    newsm.real_time = ns.real_time;
                }

            }                   
    
            var newssize = newsList.ToPagedList(1, NewspageSize);
            return View(newssize);
        }

        public ActionResult News()
        {
            var news = this.newsRepository.GetAll().OrderByDescending(x => x.real_time).ToList();
            return View(news);
        }
        [AllowAnonymous]
        public ActionResult _NewsAll()
        {
            var news = this.newsRepository.GetAll().OrderByDescending(x => x.real_time).ToList();
            List<NewsViewModel> newsList = new List<NewsViewModel>();
            DateTime dt = DateTime.Now;
            foreach (var ns in news)
            {
                NewsViewModel newsm = new NewsViewModel();
                newsm.Id = ns.Id;
                newsm.title = ns.title;
                newsm.p_class = ns.p_class;
                newsm.image = ns.image;


                ns.content = Regex.Replace(ns.content, @"<[^>]*>", String.Empty);
                ns.content = Regex.Replace(ns.content, @"&nbsp;", String.Empty);
                ns.content = Regex.Replace(ns.content, @"\s", String.Empty);



                if (ns.content.Length > 65)
                {
                    ns.content = ns.content.Substring(0, 65);
                }
                ns.content = ns.content + "...(繼續閱讀)";
                newsm.news_content = ns.content;
                /*newsm.content.Substring(0, 60);*/


                if (ns.time <= dt && ns.time != null)
                {
                    newsList.Add(newsm);
                    newsm.real_time = ns.time;
                }
                else if (ns.time == null)
                {
                    newsList.Add(newsm);
                    newsm.real_time = ns.real_time;
                }



            }
           
            return View(newsList);    
        }
        [AllowAnonymous]
        public ActionResult _HomeNews()
        {
            var news = this.newsRepository.GetAll().OrderByDescending(x => x.real_time).ToList();
            List<NewsViewModel> newsList = new List<NewsViewModel>();
            DateTime dt = DateTime.Now;
            foreach (var ns in news)
            {
                NewsViewModel newsm = new NewsViewModel();
                newsm.Id = ns.Id;
                newsm.title = ns.title;               
                newsm.p_class = ns.p_class;
                newsm.image = ns.image;
            

                ns.content = Regex.Replace(ns.content, @"<[^>]*>", String.Empty);              
                ns.content = Regex.Replace(ns.content, @"&nbsp;", String.Empty);
                ns.content = Regex.Replace(ns.content, @"\s", String.Empty);



               if (ns.content.Length > 52)
                {
                    ns.content = ns.content.Substring(0, 52);
                }
                ns.content = ns.content + "...(繼續閱讀)";
                newsm.news_content = ns.content;
                /*newsm.content.Substring(0, 60);*/

               
                if(ns.time <= dt && ns.time != null)
                {
                    newsList.Add(newsm);
                    newsm.real_time = ns.time;
                }
                else if(ns.time == null)
                {
                    newsList.Add(newsm);
                    newsm.real_time = ns.real_time;
                }
                


            }

            var newssize = newsList.ToPagedList(1, 4);
            return View(newssize);
        }

        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("News");
            }
            else
            {
                var news = this.newsRepository.Get(id.Value);
                if (news.p_class == 1)
                {
                    ViewBag.Class = "活動";
                }
                else if (news.p_class == 2)
                {
                    ViewBag.Class = "停權";
                }
                else if (news.p_class == 3)
                {
                    ViewBag.Class = "公告";
                }
                return View(news);

            }
        }

        [AllowAnonymous]
        public ActionResult _Details(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("News");
            }
            else
            {
                var news = this.newsRepository.Get(id.Value);               
                return View(news);

            }
        }

        [ValidateInput(false)]
        public ActionResult Create()
        {
            var newsdata = this.newsRepository.GetAll().ToList();


            NewsViewModel news = new NewsViewModel
            {
                NewsData = newsdata
            };
       



            return View(news);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async System.Threading.Tasks.Task<ActionResult> Create(placard news, HttpPostedFileBase image)
        {            

            if (news != null && ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(news.image) && image != null)
                {
                    string filename = "";
                    filename = DateTime.Now.ToString("yyyyMMddTHHmmssfff");
                    Google.Apis.Auth.OAuth2.GoogleCredential credential = await Google.Apis.Auth.OAuth2.GoogleCredential.GetApplicationDefaultAsync();
                    _imageUploader = new ImageUploader(System.Web.Configuration.WebConfigurationManager.AppSettings["GoogleCloud:BucketName"]);
                    var imageUrl = await _imageUploader.UploadImage(image, filename, "News");
                    news.image = imageUrl;
                }

                //Send to all subscribers
                if (news.mail_bear == 1)
                {
                    string timemessenger;




                    var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

                    request.KeepAlive = true;
                    request.Method = "POST";
                    request.ContentType = "application/json; charset=utf-8";

                    request.Headers.Add("authorization", "Basic MTA0Yjg4OTQtMTEwZC00MGQzLWIyNzYtMWEzNTVjNzA5MjUz");
                    var serializer = new JavaScriptSerializer();                  
                    var obj = new object();
                    if (news.time != null)
                    {
                        timemessenger = Convert.ToDateTime(news.time).ToString("yyyy-MM-dd HH:mm:ss");
                        timemessenger += " GMT+0800";
                        obj = new
                        {
                            app_id = "6447188b-b261-4b0d-9130-4439e8ba90b9",
                            contents = new { en = news.title },
                            headings = new { en = "熊i猜-熊報信" },
                            included_segments = new string[] { "All" },
                            ttl = 2419200,
                            delayed_option = "last-active",
                            chrome_web_image = news.image,
                            chrome_web_badge = news.image,
                            send_after = timemessenger
                        };

                    }
                    else
                    {
                        obj = new
                        {
                            app_id = "6447188b-b261-4b0d-9130-4439e8ba90b9",
                            contents = new { en = news.title },
                            headings = new { en = "熊i猜-熊報信" },
                            included_segments = new string[] { "All" },
                            ttl = 2419200,
                            delayed_option = "last-active",
                            chrome_web_image = news.image,
                            chrome_web_badge = news.image
                        };
                    }
                    var param = serializer.Serialize(obj);
                    byte[] byteArray = Encoding.UTF8.GetBytes(param);

                    string responseContent = null;

                    try
                    {
                        using (var writer = request.GetRequestStream())
                        {
                            writer.Write(byteArray, 0, byteArray.Length);
                        }

                        using (var response = request.GetResponse() as HttpWebResponse)
                        {
                            using (var reader = new StreamReader(response.GetResponseStream()))
                            {
                                responseContent = reader.ReadToEnd();
                            }
                        }
                    }
                    catch (WebException ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                    }

                    System.Diagnostics.Debug.WriteLine(responseContent);
                }
                //Send to all subscribers END




                this.newsRepository.Create(news);
                return RedirectToAction("News");
            }
            else
            {
                return View(news);
            }
        }

        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("News");
            }
            else
            {
                var news = this.newsRepository.Get(id.Value);
                return View(news);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(placard news)
        {
            if (news != null && ModelState.IsValid)
            {
                this.newsRepository.Update(news);             
                return RedirectToAction("News");
            }
            else
            {
                return RedirectToAction("News");
            }
        }


        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("News");
            }
            else
            {
                var category = this.newsRepository.Get(id.Value);
                return View(category);
            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var category = this.newsRepository.Get(id);
                this.newsRepository.Delete(category);
            }
            catch (DataException)
            {
                return RedirectToAction("Delete", new { id = id });
            }
            return RedirectToAction("News");
        }

    }



}
