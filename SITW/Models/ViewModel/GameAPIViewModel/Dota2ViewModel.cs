using SITDto;
using SITW.Helper;
using SITW.Models.GameAPIModels;
using SITW.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SITW.Models.ViewModel.GameAPIViewModel
{
    public class Dota2ViewModel
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

        public List<cfgPlayGame> Playgame
        {
            get;
            set;
        }

        public int sn { get; set; }
        public string name { get; set; }
        public string shortName { get; set; }
        public string imageURL { get; set; }
        public List<Dota2PlayerMatchModel> d2match { get; set; }
        /*public GamePostViewModel()
        {
            matchid
        }*/


        public Dota2League leaguename { get; set; }
        public int valid { get; set; }

        public Teams teamspost { get; set; }
        public Leagues leagues { get; set; }
        public cfgPlayGame cfplaygame { get; set; }

    }

}