using SITDto.function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITDto.ViewModel
{
    public class BetListDto
    {
        public int gamepostsn { get; set; }
        public int gameSn { get; set; }
        public string game { get; set; }
        public byte betModel { get; set; }
        public int topicSn { get; set; }
        public string topic { get; set; }
        public int choiceSn { get; set; }
        public string hashChoice
        {
            get
            {
                return new encrypt().Encrypt(choiceSn.ToString());
            }
            set
            {
                string strSn = new encrypt().Decrypt(value);
                int s;
                if (int.TryParse(strSn, out s))
                    choiceSn = s;

            }
        }
        public string choice { get; set; }
        public int betSn { get; set; }

        public string hashBetSn
        {
            get
            {
                return new encrypt().Encrypt(betSn.ToString());
            }
            set
            {
                string strSn = new encrypt().Decrypt(value);
                int s;
                if (int.TryParse(strSn, out s))
                    betSn = s;

            }
        }

        public double Odds { get; set; }
        public double money { get; set; }
        public double realmoney { get; set; }
        public Nullable<double> topicMoney { get; set; }
        public Nullable<double> choiceMoney { get; set; }
        public double rake { get; set; }
        public string unit { get; set; }
        public string isTrue { get; set; }
        public string isTrueValue { get; set; }
        public string userID { get; set; }
        public int comSn { get; set; }
        public DateTime betDatetime { get; set; }
        /// <summary>
        /// 當滑鼠移到結果時要顯示的資訊，拿來顯示計算公式
        /// </summary>
        public string showTitle { get; set; }
        public int gameStatus { get; set; }
    }
}
