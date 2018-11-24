using SITDto.function;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITDto
{
    public class choiceDto
    {
        public int sn { get; set; }
        public string hashSn
        {
            get
            {
                return new encrypt().Encrypt(sn.ToString());
            }
            set
            {
                string strSn = new encrypt().Decrypt(value);
                int s;
                if (int.TryParse(strSn, out s))
                    sn = s;

            }
        }
        public Nullable<int> comSn { get; set; }
        public Nullable<int> topicSn { get; set; }

        [Required]
        [DisplayName("選項名稱")]
        public string choiceStr { get; set; }
        [DisplayName("選項備註")]
        public string comment { get; set; }
        public Nullable<byte> valid { get; set; }
        [DisplayName("選項賠率")]
        public Nullable<double> Odds { get; set; }
        public Nullable<byte> isTrue { get; set; }
        public string bearSn { get; set; }
        public string isTrueStr
        {
            get
            {
                string str = "";
                if (isTrue.HasValue)
                {
                    eisTrue ei = (eisTrue)Enum.Parse(typeof(eisTrue), isTrue.Value.ToString());
                    str = ei.ToString();
                }
                else
                    str = "未設定";
                return str;
            }
        }
        public double betMoneygti { get; set; }
        public List<choiceBetMoneyDto> betMoney { get; set; }
        public Nullable<double> totalmoney { get; set; }
        public enum eisTrue { 錯誤, 正確, 返還 }
    }
}
