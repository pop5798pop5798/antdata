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
    public class Dota2PlayerModel
    {
        public class Player
        {
            public int account_id { get; set; }
            public int player_slot { get; set; }
            public int hero_id { get; set; }
        }

        public class Match
        {
            public int series_id { get; set; }
            public int series_type { get; set; }
            public object match_id { get; set; }
            public object match_seq_num { get; set; }
            public int start_time { get; set; }
            public int lobby_type { get; set; }
            public int radiant_team_id { get; set; }
            public int dire_team_id { get; set; }
            public List<Player> players { get; set; }
        }

        public class Result
        {
            public int status { get; set; }
            public int num_results { get; set; }
            public int total_results { get; set; }
            public int results_remaining { get; set; }
            public List<Match> matches { get; set; }
        }

        public class RootObject
        {
            public Result result { get; set; }
        }

    }

}
