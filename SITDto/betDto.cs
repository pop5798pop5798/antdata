using SITDto.function;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITDto
{
    public class betDto
    {
        public int sn { get; set; }
        public string hashSn {
            get
            {
                return new encrypt().Encrypt(sn.ToString());
            }
            set {
                string strSn = new encrypt().Decrypt(value);
                int s;
                if (int.TryParse(strSn, out s))
                    sn = s;

            }
        }
        public string userId { get; set; }
        public Nullable<int> comSn { get; set; }
        public Nullable<int> userSn { get; set; }
        public Nullable<int> gameSn { get; set; }
        public Nullable<int> topicSn { get; set; }
        public Nullable<int> choiceSn { get; set; }

        [Required]
        public Nullable<double> money { get; set; }
        public Nullable<int> unitSn { get; set; }
        public Nullable<double> Odds { get; set; }
        public Nullable<byte> valid { get; set; }

    }
}
