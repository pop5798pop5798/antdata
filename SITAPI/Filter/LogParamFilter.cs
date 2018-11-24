using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SITAPI.Filter
{
    public class LogParamFilter : ActionFilterAttribute
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            List<HttpParameterDescriptor> pdList = actionContext.ActionDescriptor.GetParameters().ToList();
            string pdstring = "";
            foreach(var pd in pdList)
            {
                pdstring += pd.ParameterName + "=" + JsonConvert.SerializeObject(actionContext.ActionArguments[pd.ParameterName], Formatting.Indented) + System.Environment.NewLine;
            }
            logger.Info("[OnActionExecuting]" + System.Environment.NewLine + pdstring);
            /*
            var stream = actionContext.HttpContext.Request.InputStream;
            
            var data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            */
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response.Content != null)
                logger.Info("[OnActionExecuted]" + System.Environment.NewLine + actionExecutedContext.Response.Content.ReadAsStringAsync().Result);
        }
    }
}