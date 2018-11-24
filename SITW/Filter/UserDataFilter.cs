using Microsoft.AspNet.Identity;
using SITW.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace SITW.Filter
{
    public class UserDataFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            actionContext.HttpContext.Session["Assets"] = new AssetsRepository().getAssetsListByUserID(HttpContext.Current.User.Identity.GetUserId());
            //actionContext.HttpContext.Session["levelExp"] = new AssetsRepository().getExpByUserID(HttpContext.Current.User.Identity.GetUserId());

        }
    }
}