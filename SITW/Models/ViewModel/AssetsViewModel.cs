using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SITW.Models.ViewModel
{
    public class AssetsViewModel
    {
        public double Asset {
            get
            {
                return Math.Round(_Asset, 2);
            }
            set
            {
                _Asset = value;
            }
        }
        private double _Asset { get; set; }
        public string unitName { get; set; }
        public int unitSn { get; set; }
        public string iconURL { get; set; }
    }
}