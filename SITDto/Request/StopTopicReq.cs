using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITDto.Request
{
    public class StopTopicReq
    {
        public string UserID { get; set; }
        public int comSn { get; set; }
        public int topicSn { get; set; }
    }
}
