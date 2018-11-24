using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SITW.Models.ViewModel
{
    public class MissionViewModel : Missions
    {
        public List<MissionStart> MissionStartList { get; set; }
        public List<MissionEnd> MissionEndList { get; set; }
        public List<MissionAssets> MissionAssetList { get; set; }
    }

    public class MissionEndViewModel
    {
        public int missionSn { get; set; }
        public string finshName { get; set; }
        public int Compare { get; set; }
        public string CompareStr {
            get
            {
                return new MissionCompare(Compare).CompareText;
            }
        }
        public int num { get; set; }
        public string sqlquery { get; set; }
        public int nownum { get; set; }
    }

    public class MissionNoteModel
    {
        public MissionNoteModel()
        {
            mevList = new List<MissionEndViewModel>();
        }

        public MissionNoteModel(MissionViewModel mvm)
        {
            sn = mvm.sn;
            Name = mvm.name;
            comment = mvm.comment;
            edate = mvm.edate;
        }
        public int sn { get; set; }
        public int userMissionSn { get; set; }
        public string Name { get; set; }
        public string comment { get; set; }
        public DateTime edate { get; set; }
        public string imgURL { get; set; }
        public bool isFinsh {
            get
            {
                bfinsh = true;
                foreach(var m in mevList)
                {
                    switch (m.Compare)
                    {
                        case -2:
                            if (!(m.nownum < m.num))
                                bfinsh = false;
                            break;
                        case -1:
                            if (!(m.nownum <= m.num))
                                bfinsh = false;
                            break;
                        case 0:
                            if (!(m.nownum == m.num))
                                bfinsh = false;
                            break;
                        case 1:
                            if (!(m.nownum >= m.num))
                                bfinsh = false;
                            break;
                        case 2:
                            if (!(m.nownum > m.num))
                                bfinsh = false;
                            break;
                    }

                }
                return bfinsh;
            }
        }
        private bool bfinsh { get; set; }
        public List<MissionEndViewModel> mevList { get; set; }
        public List<MissionAssetsViewModel> maList { get; set; }
    }

    public class MissionNoteViewModel
    {
        public MissionNoteViewModel()
        {
            MissionNoteList = new List<MissionNoteModel>();
        }

        public int MissionNumber
        {
            get
            {
                return MissionNoteList.Count();
            }
        }

        public int MissionFinshNumber
        {
            get
            {
                return MissionNoteList.Where(p=>p.isFinsh).Count();
            }
        }

        public List<MissionNoteModel> MissionNoteList { get; set; }
        //List<MissionNoteModel> MissionNoteList { get; set; }
    }
}