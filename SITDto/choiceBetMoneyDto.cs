using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITDto
{
    public class choiceBetMoneyDto
    {
        public int choiceSn { get; set; }
        public double betMoney { get; set; }
        [DisplayName("幣別")]
        public int unitSn { get; set; }
        [DisplayName("賠率")]
        public double Odds { get; set; }
    }
}


