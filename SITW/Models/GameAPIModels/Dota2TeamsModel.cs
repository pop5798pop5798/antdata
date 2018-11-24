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
    public class Dota2TeamsModel
    {
        public class Team
        {
            public string name { get; set; }
            public string tag { get; set; }
            public int time_created { get; set; }
            public int calibration_games_remaining { get; set; }
            public long logo { get; set; }
            public long logo_sponsor { get; set; }
            public string country_code { get; set; }
            public string url { get; set; }
            public int games_played { get; set; }
            public int player_0_account_id { get; set; }
            public int player_1_account_id { get; set; }
            public int player_2_account_id { get; set; }
            public int player_3_account_id { get; set; }
            public int player_4_account_id { get; set; }
            public int admin_account_id { get; set; }
            public int league_id_0 { get; set; }
            public int league_id_1 { get; set; }
            public int league_id_2 { get; set; }
            public int league_id_3 { get; set; }
            public int league_id_4 { get; set; }
            public int league_id_5 { get; set; }
            public int league_id_6 { get; set; }
            public int league_id_7 { get; set; }
            public int league_id_8 { get; set; }
            public int league_id_9 { get; set; }
            public int league_id_10 { get; set; }
            public int league_id_11 { get; set; }
            public int league_id_12 { get; set; }
            public int league_id_13 { get; set; }
            public int league_id_14 { get; set; }
            public int league_id_15 { get; set; }
            public int league_id_16 { get; set; }
            public int league_id_17 { get; set; }
            public int league_id_18 { get; set; }
            public int league_id_19 { get; set; }
            public int league_id_20 { get; set; }
            public int league_id_21 { get; set; }
            public int league_id_22 { get; set; }
            public int league_id_23 { get; set; }
            public int league_id_24 { get; set; }
            public int league_id_25 { get; set; }
            public int league_id_26 { get; set; }
            public int league_id_27 { get; set; }
            public int league_id_28 { get; set; }
            public int league_id_29 { get; set; }
            public int league_id_30 { get; set; }
            public int league_id_31 { get; set; }
            public int league_id_32 { get; set; }
            public int league_id_33 { get; set; }
            public int league_id_34 { get; set; }
            public int league_id_35 { get; set; }
            public int league_id_36 { get; set; }
            public int league_id_37 { get; set; }
            public int league_id_38 { get; set; }
            public int league_id_39 { get; set; }
            public int league_id_40 { get; set; }
            public int league_id_41 { get; set; }
            public int league_id_42 { get; set; }
            public int league_id_43 { get; set; }
            public int league_id_44 { get; set; }
            public int league_id_45 { get; set; }
            public int league_id_46 { get; set; }
            public int league_id_47 { get; set; }
            public int league_id_48 { get; set; }
            public int league_id_49 { get; set; }
            public int league_id_50 { get; set; }
            public int league_id_51 { get; set; }
            public int league_id_52 { get; set; }
            public int league_id_53 { get; set; }
            public int league_id_54 { get; set; }
            public int league_id_55 { get; set; }
            public int league_id_56 { get; set; }
            public int league_id_57 { get; set; }
            public int league_id_58 { get; set; }
            public int league_id_59 { get; set; }
            public int league_id_60 { get; set; }
            public int league_id_61 { get; set; }
            public int league_id_62 { get; set; }
            public int league_id_63 { get; set; }
            public int league_id_64 { get; set; }
            public int league_id_65 { get; set; }
            public int league_id_66 { get; set; }
            public int league_id_67 { get; set; }
            public int league_id_68 { get; set; }
            public int league_id_69 { get; set; }
            public int league_id_70 { get; set; }
            public int league_id_71 { get; set; }
            public int league_id_72 { get; set; }
            public int league_id_73 { get; set; }
            public int league_id_74 { get; set; }
            public int league_id_75 { get; set; }
            public int league_id_76 { get; set; }
            public int league_id_77 { get; set; }
            public int league_id_78 { get; set; }
            public int league_id_79 { get; set; }
            public int league_id_80 { get; set; }
            public int league_id_81 { get; set; }
            public int league_id_82 { get; set; }
            public int league_id_83 { get; set; }
            public int league_id_84 { get; set; }
            public int league_id_85 { get; set; }
            public int league_id_86 { get; set; }
            public int league_id_87 { get; set; }
            public int league_id_88 { get; set; }
            public int league_id_89 { get; set; }
            public int league_id_90 { get; set; }
            public int league_id_91 { get; set; }
            public int league_id_92 { get; set; }
            public int league_id_93 { get; set; }
            public int league_id_94 { get; set; }
            public int league_id_95 { get; set; }
            public int league_id_96 { get; set; }
            public int league_id_97 { get; set; }
            public int league_id_98 { get; set; }
            public int league_id_99 { get; set; }
            public int league_id_100 { get; set; }
            public int league_id_101 { get; set; }
            public int league_id_102 { get; set; }
            public int league_id_103 { get; set; }
            public int league_id_104 { get; set; }
            public int league_id_105 { get; set; }
            public int league_id_106 { get; set; }
            public int league_id_107 { get; set; }
            public int league_id_108 { get; set; }
            public int league_id_109 { get; set; }
            public int league_id_110 { get; set; }
            public int league_id_111 { get; set; }
            public int league_id_112 { get; set; }
            public int league_id_113 { get; set; }
            public int league_id_114 { get; set; }
            public int league_id_115 { get; set; }
            public int league_id_116 { get; set; }
            public int league_id_117 { get; set; }
            public int league_id_118 { get; set; }
            public int league_id_119 { get; set; }
            public int league_id_120 { get; set; }
            public int league_id_121 { get; set; }
            public int league_id_122 { get; set; }
            public int league_id_123 { get; set; }
            public int league_id_124 { get; set; }
            public int league_id_125 { get; set; }
            public int league_id_126 { get; set; }
            public int league_id_127 { get; set; }
            public int league_id_128 { get; set; }
            public int league_id_129 { get; set; }
        }

        public class Result
        {
            public int status { get; set; }
            public List<Team> teams { get; set; }
        }

        public class RootObject
        {
            public Result result { get; set; }
        }

    }

}
