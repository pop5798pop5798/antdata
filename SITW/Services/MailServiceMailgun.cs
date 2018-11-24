using RestSharp;
using RestSharp.Authenticators;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace SITW.Services
{
    public class MailServiceMailgun
    {
        private string processMsg = "";
        private string userID = System.Web.Configuration.WebConfigurationManager.AppSettings["Office365Account"];
        private string password = System.Web.Configuration.WebConfigurationManager.AppSettings["Office365PWD"];

        public MailServiceMailgun()
        { }



        /// <summary>
        /// 傳送Email
        /// </summary>
        /// <param name="subject">郵件主旨</param>
        /// <param name="content">郵件發送內容</param>
        /// <param name="mobile">接收人之郵件地址。格式為: abc@yahoo.com.tw。多筆接收人時，請以半形逗點隔開( , )，abc@yahoo.com.tw,def@yahoo.com.tw。</param>
        /// <returns>true:傳送成功；false:傳送失敗</returns>
        public bool Send(string subject, string content, string mailto,string BCC)
        {
            bool success = false;
            try
            {

                RestClient client = new RestClient();
                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator =
                new HttpBasicAuthenticator("api",
                                          "key-1fd567e096809bb7e98df418f34afff9");
                RestRequest request = new RestRequest();
                request.AddParameter("domain", "funbet.com.tw", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", "熊i猜 <services@funbet.tech>");
                if (!string.IsNullOrEmpty(mailto))
                    foreach (var m in mailto.Split(','))
                        request.AddParameter("to", m);
                if (!string.IsNullOrEmpty(BCC))
                    foreach (var m in BCC.Split(','))
                        request.AddParameter("bcc", m);
                request.AddParameter("subject", subject);
                request.AddParameter("html", content);
                request.Method = Method.POST;
                IRestResponse rp = client.Execute(request);
                success = rp.IsSuccessful;
            }
            catch (Exception ex)
            {
                this.processMsg = ex.ToString();
            }
            return success;
        }

        /*
        public bool SendSimpleMessage(string subject, string content, string mailto, string BCC)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator =
            new HttpBasicAuthenticator("api",
                                      "key-1fd567e096809bb7e98df418f34afff9");
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "funbet.com.tw", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "熊i猜 <services@funbet.tech>");
            request.AddParameter("to", mailto);
            request.AddParameter("subject", subject);
            request.AddParameter("html", content);
            request.Method = Method.POST;
            IRestResponse rp = client.Execute(request);
            return rp.IsSuccessful;
        }
        */
    }
}