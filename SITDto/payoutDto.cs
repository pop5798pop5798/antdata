using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITDto
{
    public class payoutDto
    {
        public int comSn { get; set; }
        public int gameSn { get; set; }
        public int topicSn { get; set; }
        public int choiceSn { get; set; }
        public string userId { get; set; }
        public int betSn { get; set; }
        public float Odds { get; set; }
        public float money { get; set; }
        public int unitSn { get; set; }
        public float realMoney { get; set; }
        public float? topicTotalMoney { get; set; }
        public float? correctTotalMoney { get; set; }
        public byte? isTrue { get; set; }
        public float? rake { get; set; }
    }
}
