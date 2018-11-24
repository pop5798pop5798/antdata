using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SITW.Models.ViewModel
{
    public class levelExpViewModel
    {
        public int levelNum { get; set; }
        public int expNum { get; set; }
        public int nowexp
        {
            get
            {
                return expNum - leftexp;
            }
        }
        public int leftexp { get; set; }
    }
}