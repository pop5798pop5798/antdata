using SITDto;
using SITW.Helper;
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
    public class Dota2UpcomViewModel
    {

        public class RootObject
        {
            
           // public string name { get; set; }
            public string begin_at { get; set; }
             public string number_of_games { get; set; }            
            //public List<Opponents> opponents { get; set; }
            public string o1name { get; set; }
            public string o1image_url { get; set; }
            public string o2name { get; set; }
            public string o2image_url { get; set; }
            //public string opponentsname { get; set; }
            //public string opponentsimage_url { get; set; }
            public string name { get; set; }
            public string image_url { get; set; }

        }
       
        public class Opponents
        {
            public string name { get; set; }
            public string image_url { get; set; }
        }


        public class Opponentslist
        {
            public string name { get; set; }
            public string image_url { get; set; }

        }



    }

}