using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SITW.Services
{
    public static class EmailTemplatesService
    {
        public static string GetVerifyEmailHTML(string VerifyLink)
        {
            string template = "";
            using (var sr = new StreamReader(HttpContext.Current.Server.MapPath("\\Content\\Templates\\") + "VerifyEmail.txt"))
            {
                template = sr.ReadToEnd();
            }

            string messageBody = string.Format(template, VerifyLink);
            return messageBody;
        }
    }
}