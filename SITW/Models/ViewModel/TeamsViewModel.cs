using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SITW.Models.ViewModel;

namespace SITW.Models.ViewModel
{
    public class TeamsViewModel
    {
        public List<Teams> Teams
        {
            get;
            set;
        }

        public List<Leagues> LeaguesData
        {
            get;
            set;
        }

        public List<ApiTypes> Playgame
        {
            get;
            set;
        }

        public int sn { get; set; }
        public string name { get; set; }
        public string shortName { get; set; }
        public string imageURL { get; set; }
        public int leagueSn { get; set; }
        public int valid { get; set; }

        public Teams teamspost { get; set; }
        public Leagues leagues { get; set; }
        public ApiTypes cfplaygame { get; set; }

    }

}