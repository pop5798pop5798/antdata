using SITW.Models;
using SITW.Models.Repository;
using SITW.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SITW.Helper
{
    public static class SelectHelper
    {
        public static MvcHtmlString MissionCycleSelect(this HtmlHelper htmlhelper,string name,int? defualValue)//開放、靜態、的擴充方法
        {
            MissionCycleModel mcm = new MissionCycleModel();
            List<SelectListItem> sliList = new List<SelectListItem>();
            defualValue = (defualValue.HasValue ? defualValue.Value : 0);
            foreach (var s in mcm.MissionCycleList)
            {
                SelectListItem sli = new SelectListItem
                {
                    Text = s.CycleText,
                    Value = s.CycleSn.ToString(),
                    Selected = (s.CycleSn == defualValue.Value ? true : false)
                };
                sliList.Add(sli);
            }


            return System.Web.Mvc.Html.SelectExtensions.DropDownList(htmlhelper, name, sliList, new { @class = "form-control" });
        }

        public static MvcHtmlString MissionCompareSelect(this HtmlHelper htmlhelper, string name, int? defualValue)//開放、靜態、的擴充方法
        {
            MissionCompareModel mcm = new MissionCompareModel();
            List<SelectListItem> sliList = new List<SelectListItem>();
            defualValue = (defualValue.HasValue ? defualValue.Value : 0);
            foreach (var s in mcm.MissionCompareList)
            {
                SelectListItem sli = new SelectListItem
                {
                    Text = s.CompareText,
                    Value = s.CompareSn.ToString(),
                    Selected = (s.CompareSn == defualValue.Value ? true : false)
                };
                sliList.Add(sli);
            }


            return System.Web.Mvc.Html.SelectExtensions.DropDownList(htmlhelper, name, sliList, new { @class = "form-control" });
        }

        public static MvcHtmlString MissionMissionEndSelect(this HtmlHelper htmlhelper, string name, int? defualValue)//開放、靜態、的擴充方法
        {
            CfgMissionEndModel cme = new CfgMissionEndModel();
            List<SelectListItem> sliList = new List<SelectListItem>();
            sliList.Add(new SelectListItem
            {
                Text = "請選擇",
                Value = ""
            });
            defualValue = (defualValue.HasValue ? defualValue.Value : 0);
            foreach (var s in cme.cfgMissionEndList)
            {
                SelectListItem sli = new SelectListItem
                {
                    Text = s.name,
                    Value = s.sn.ToString(),
                    Selected = (s.sn == defualValue.Value ? true : false)
                };
                sliList.Add(sli);
            }


            return System.Web.Mvc.Html.SelectExtensions.DropDownList(htmlhelper, name, sliList, new { @class = "form-control" });
        }
        

        public static MvcHtmlString MissionMissionStartSelect(this HtmlHelper htmlhelper, string name, int? defualValue)//開放、靜態、的擴充方法
        {
            CfgMissionStartModel cms = new CfgMissionStartModel();
            List<SelectListItem> sliList = new List<SelectListItem>();
            sliList.Add(new SelectListItem
            {
                Text = "請選擇",
                Value = ""
            });
            defualValue = (defualValue.HasValue ? defualValue.Value : 0);
            foreach (var s in cms.cfgMissionStartList)
            {
                SelectListItem sli = new SelectListItem
                {
                    Text = s.name,
                    Value = s.sn.ToString(),
                    Selected = (s.sn == defualValue.Value ? true : false)
                };
                sliList.Add(sli);
            }


            return System.Web.Mvc.Html.SelectExtensions.DropDownList(htmlhelper, name, sliList, new { @class = "form-control" });
        }


        public static MvcHtmlString UnitSelect(this HtmlHelper htmlhelper, string name, int? defualValue)//開放、靜態、的擴充方法
        {
            List<cfgUnit> UnitList = new UnitsRepository().getAllValid();
            List<SelectListItem> sliList = new List<SelectListItem>();
            sliList.Add(new SelectListItem
            {
                Text = "請選擇",
                Value = ""
            });
            defualValue = (defualValue.HasValue ? defualValue.Value : 0);
            foreach (var s in UnitList)
            {
                SelectListItem sli = new SelectListItem
                {
                    Text = s.showStr,
                    Value = s.sn.ToString(),
                    Selected = (s.sn == defualValue.Value ? true : false)
                };
                sliList.Add(sli);
            }


            return System.Web.Mvc.Html.SelectExtensions.DropDownList(htmlhelper, name, sliList, new { @class = "form-control" });
        }

        public static MvcHtmlString TeamSelect(this HtmlHelper htmlhelper, string name,int? playgame, int? defualValue)//開放、靜態、的擴充方法
        {
            List<Teams> teamlist = new TeamsRepository().getAllValid().OrderBy(x => x.name).ToList();
            List<Leagues> Leaglist = new LeaguesRepository().getAll().ToList();
            List<SelectListItem> sliList = new List<SelectListItem>();
            sliList.Add(new SelectListItem
            {
                Text = "請選擇",
                Value = ""
            });
            defualValue = (defualValue.HasValue ? defualValue.Value : 0);
            List<SelectListGroup> sgList = new List<SelectListGroup>();
            foreach(var l in Leaglist)
            {
                if(l.playGamesn == playgame || playgame == 0)
                    sgList.Add(new SelectListGroup { Name = l.shortName });
                
                
            }

            foreach (var t in teamlist)
            {
                SelectListGroup sg = new SelectListGroup();
                string lgName = Leaglist.Where(p => p.sn == t.leagueSn).FirstOrDefault().shortName;
                sg = sgList.Where(p => p.Name == lgName).FirstOrDefault();        
                SelectListItem sli = new SelectListItem
                {
                    Text = t.name,
                    Value = t.sn.ToString(),
                    Selected = (t.sn == defualValue.Value ? true : false),
                    Group = sg,                   

                };
                if(sli.Group != null || playgame == 0)
                  sliList.Add(sli);
                
                 
                
                
            }


            return System.Web.Mvc.Html.SelectExtensions.DropDownList(htmlhelper, name, sliList, new { @class = "form-control" });
        }

        public static MvcHtmlString PlayGameSelect(this HtmlHelper htmlhelper, string name, int? defualValue)//開放、靜態、的擴充方法
        {
            List<ApiTypes> PlayGameList = new cfgPlayGameRepository().getAll();
            List<SelectListItem> sliList = new List<SelectListItem>();
            sliList.Add(new SelectListItem
            {
                Text = "請選擇",
                Value = ""
            });
            defualValue = (defualValue.HasValue ? defualValue.Value : 0);
            foreach (var s in PlayGameList)
            {
                SelectListItem sli = new SelectListItem
                {
                    Text = s.cName,
                    Value = s.sn.ToString(),
                    Selected = (s.sn == defualValue.Value ? true : false)
                };
                sliList.Add(sli);
            }


            return System.Web.Mvc.Html.SelectExtensions.DropDownList(htmlhelper, name, sliList, new { @class = "form-control", @multiple = "multiple" });
        }

        public static MvcHtmlString DataTypeSelect(this HtmlHelper htmlhelper, string name, int? defualValue)//開放、靜態、的擴充方法
        {
            List<ApiDataType> PlayGameList = new cfgPlayGameRepository().getDataAll();
            List<SelectListItem> sliList = new List<SelectListItem>();
            sliList.Add(new SelectListItem
            {
                Text = "請選擇類型",
                Value = ""
            });
            defualValue = (defualValue.HasValue ? defualValue.Value : 0);
            foreach (var s in PlayGameList)
            {
                SelectListItem sli = new SelectListItem
                {
                    Text = s.cName,
                    Value = s.id.ToString(),
                    Selected = (s.id == defualValue.Value ? true : false)
                };
                sliList.Add(sli);
            }


            return System.Web.Mvc.Html.SelectExtensions.DropDownList(htmlhelper, name, sliList, new { @class = "form-control"});
        }

        public static MvcHtmlString AreaGameSelect(this HtmlHelper htmlhelper, string name, int? defualValue)//開放、靜態、的擴充方法
        {
            List<Leagues> leaguedata = new LeaguesRepository().getAll();
            TeamsViewModel LeaguesList = new TeamsViewModel
            {               
                LeaguesData = leaguedata
            };
            List<SelectListItem> sliList = new List<SelectListItem>();
            sliList.Add(new SelectListItem
            {
                Text = "請選擇",
                Value = ""
            });
            defualValue = (defualValue.HasValue ? defualValue.Value : 0);
            foreach (var s in LeaguesList.LeaguesData)
            {
                SelectListItem sli = new SelectListItem
                {
                    Text = s.shortName,
                    Value = s.sn.ToString(),
                    Selected = (s.sn == defualValue.Value ? true : false)
                };
                sliList.Add(sli);
            }


            return System.Web.Mvc.Html.SelectExtensions.DropDownList(htmlhelper, name, sliList, new { @class = "form-control" });
        }

        public static MvcHtmlString NewsSelect(this HtmlHelper htmlhelper, string name, int? defualValue)//開放、靜態、的擴充方法
        {
            List<NewsMenu> newsmenuList = new NewsMenuRepository().getAll();
            List<SelectListItem> sliList = new List<SelectListItem>();
            sliList.Add(new SelectListItem
            {
                Text = "請選擇",
                Value = ""
            });
            defualValue = (defualValue.HasValue ? defualValue.Value : 0);
            foreach (var s in newsmenuList)
            {
                SelectListItem sli = new SelectListItem
                {
                    Text = s.name,
                    Value = s.Id.ToString(),
                    Selected = (s.Id == defualValue.Value ? true : false)
                };
                sliList.Add(sli);
            }


            return System.Web.Mvc.Html.SelectExtensions.DropDownList(htmlhelper, name, sliList, new { @class = "form-control" });
        }




    }
}