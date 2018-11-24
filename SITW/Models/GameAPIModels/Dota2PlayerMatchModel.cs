using SITDto;
using SITW.Helper;
using SITW.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace SITW.Models.GameAPIModels
{
    public class Dota2PlayerMatchModel
    {
            public int timeid { get; set; }
            public object match_id { get; set; }
            public string match_name { get; set; }
            public string dire_team { get; set; }
            public string radiant_team { get; set; }
            public int dire_team_id { get; set; }
            public int radiant_team_id { get; set; }
            public Nullable<int> d2BO { get; set; }
            public Nullable<int> d2BOcount { get; set; }
            public DateTime strar_time { get; set; }
            public Dota2PlayerListModel.RootObject Dota2PlayerList { get; set; }



    }

}
