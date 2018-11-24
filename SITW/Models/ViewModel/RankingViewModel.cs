using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SITW.Models.ViewModel
{
    public class RankingViewModel
    {
        public RankingViewModel()
        {
            rankinglist = new List<Ranking>();
        }
        public List<Ranking> rankinglist { get; set; }
        public DateTime sdate { get; set; }
        public DateTime edate { get; set; }
    }

    public class Ranking
    {
        public string ord { get; set; }
        public string ID { get; set; }
        public string UserName { get; set; }
        public double Assets { get; set; }
        public string email { get; set; }
        public bool isUser { get; set; }
    }
}