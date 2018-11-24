using Microsoft.AspNet.Identity;
using SITW.Models.Repository;
using SITW.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace SITW.Filter
{
    public class MissionFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext actionContext)
        {
            base.OnResultExecuting(actionContext);
            MissionsRepository _mission = new MissionsRepository();
            IEnumerable<MissionNoteModel> Mission= _mission.GetMissionCode(HttpContext.Current.User.Identity.GetUserId());
            actionContext.HttpContext.Session["MissionCode"] = Mission;

            //已完成任務直接自動發送獎勵，等畫面上任務系統完成就可以關閉了 START
            //IEnumerable<MissionNoteModel> FinshMission = Mission.Where(p => p.isFinsh);
            //foreach (var m in FinshMission)
            //{
            //    _mission.SetMissionFinsh(HttpContext.Current.User.Identity.GetUserId(), m.userMissionSn);
            //}
            //已完成任務直接自動發送獎勵，等畫面上任務系統完成就可以關閉了 END

        }
    }
}