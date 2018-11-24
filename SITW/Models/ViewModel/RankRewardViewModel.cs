using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SITW.Models.ViewModel;

namespace SITW.Models.ViewModel
{
    public class RankRewardViewModel
    {
        public IEnumerable<Ranking_content> RewardData
        {
            get;
            set;
        }
        public IEnumerable<Ranking_title> RewardTitle
        {
            get;
            set;
        }
    }

}