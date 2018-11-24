using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace SITW.Services
{
    public class MailServiceOffice365
    {
        private string processMsg = "";
        private string userID = System.Web.Configuration.WebConfigurationManager.AppSettings["Office365Account"];
        private string password = System.Web.Configuration.WebConfigurationManager.AppSettings["Office365PWD"];

        public MailServiceOffice365()
        { }

        /// <summary>
        /// 傳送簡訊
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
                string template = "";
                using (var sr = new StreamReader(HttpContext.Current.Server.MapPath("\\Templates\\") + "GeneralEmail.txt"))
                {
                    template = sr.ReadToEnd();
                }

                string messageBody = string.Format(template, content);

                SendWithoutTeplate(subject, messageBody, mailto, BCC);
            }
            catch (Exception ex)
            {
                this.processMsg = ex.ToString();
            }
            return success;
        }

        /// <summary>
        /// 傳送簡訊
        /// </summary>
        /// <param name="subject">郵件主旨</param>
        /// <param name="content">郵件發送內容</param>
        /// <param name="mobile">接收人之郵件地址。格式為: abc@yahoo.com.tw。多筆接收人時，請以半形逗點隔開( , )，abc@yahoo.com.tw,def@yahoo.com.tw。</param>
        /// <returns>true:傳送成功；false:傳送失敗</returns>
        public bool SendWithoutTeplate(string subject, string content, string mailto,string BCC)
        {
            bool success = false;
            try
            {

                SmtpClient client = new SmtpClient("smtp.office365.com", 587);
                client.Credentials = new NetworkCredential(userID, password);
                client.EnableSsl = true;
                MailMessage pp = new MailMessage();
                pp.From = new MailAddress("services@funbet.tech","熊i猜");
                if (!string.IsNullOrEmpty(mailto))
                    pp.To.Add(new MailAddress(mailto));
                if (!string.IsNullOrEmpty(BCC))
                    pp.Bcc.Add(BCC);
                pp.Subject = subject;
                pp.Body = content;
                pp.IsBodyHtml = true;
                pp.BodyEncoding = System.Text.Encoding.UTF8;

                client.Send(pp);
                success = true;
            }
            catch (Exception ex)
            {
                this.processMsg = ex.ToString();
            }
            return success;
        }
    }
}