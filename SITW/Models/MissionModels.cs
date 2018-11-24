using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SITW.Models
{
    public class MissionModels
    {
    }


    public class MissionCycleModel
    {
        public MissionCycleModel()
        {
            MissionCycleList = new List<MissionCycle>();
            for (int i = 0; i <= 4; i++)
                MissionCycleList.Add(new MissionCycle { CycleSn = i });
        }
        public List<MissionCycle> MissionCycleList { get; set; }
    }
    public class MissionCycle
    {
        public int CycleSn { get; set; }
        public string CycleText
        {
            get
            {
                string t = "";
                switch(CycleSn)
                {
                    case 0:
                        t = "無";
                        break;
                    case 1:
                        t = "每日";
                        break;
                    case 2:
                        t = "每周";
                        break;
                    case 3:
                        t = "每月";
                        break;
                    case 4:
                        t = "每季";
                        break;
                }
                return t;
            }
        }

    }



    public class MissionCompareModel
    {
        public MissionCompareModel()
        {
            MissionCompareList = new List<MissionCompare>();
            for (int i = -2; i <= 2; i++)
                MissionCompareList.Add(new MissionCompare(i));
        }
        public List<MissionCompare> MissionCompareList { get; set; }
    }
    public class MissionCompare
    {
        public MissionCompare()
        {
        }

        public MissionCompare(int csn)
        {
            CompareSn = csn;
        }

        public int CompareSn { get; set; }
        public string CompareText
        {
            get
            {
                string t = "";
                switch (CompareSn)
                {
                    case -2:
                        t = "<";
                        break;
                    case -1:
                        t = "<=";
                        break;
                    case 0:
                        t = "=";
                        break;
                    case 1:
                        t = ">=";
                        break;
                    case 2:
                        t = ">";
                        break;
                }
                return t;
            }
        }
        public string CompareSQLText
        {
            get
            {
                string t = "";
                switch (CompareSn)
                {
                    case -2:
                        t = "<";
                        break;
                    case -1:
                        t = "<=";
                        break;
                    case 0:
                        t = "=";
                        break;
                    case 1:
                        t = ">=";
                        break;
                    case 2:
                        t = ">";
                        break;
                }
                return t;
            }
        }

    }

    public class CfgMissionEndModel
    {
        private sitwEntities db = new sitwEntities();

        public List<cfgMissionEnd> cfgMissionEndList { get; set; }
        public CfgMissionEndModel()
        {
            cfgMissionEndList = db.cfgMissionEnd.Where(p => p.valid == 1).ToList();
        }
    }

    public class CfgMissionStartModel
    {
        private sitwEntities db = new sitwEntities();

        public List<cfgMissionStart> cfgMissionStartList { get; set; }
        public CfgMissionStartModel()
        {
            cfgMissionStartList = db.cfgMissionStart.Where(p => p.valid == 1).ToList();
        }
    }

}