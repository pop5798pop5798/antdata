using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SITW.Models
{
    public class VerifyRecord
    {
        public string userId { get; set; }
        public byte VerifyType { get; set; }
        public string VerifyContent { get; set; }
        public DateTime VerifyDate { get; set; }
    }
}