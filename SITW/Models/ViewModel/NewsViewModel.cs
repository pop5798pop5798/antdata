using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SITW.Models.ViewModel;

namespace SITW.Models.ViewModel
{
    public class NewsViewModel
    {
        public NewsViewModel()
        {
          
           
        }
        public placard NewsDataGet
        {
            get;
            set;
        }

        public List<placard> NewsData
        {
            get;
            set;
        }



        public int Id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public Nullable<System.DateTime> real_time { get; set; }
        public string manager { get; set; }
        public Nullable<System.DateTime> time { get; set; }
        public Nullable<System.DateTime> endtime { get; set; }
        public string image { get; set; }
        public Nullable<int> p_class { get; set; }
        public string news_content{ get; set; }
        public Nullable<int> mail_bear { get; set; }
    }

}