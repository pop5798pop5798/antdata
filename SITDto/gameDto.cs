using SITDto.function;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace SITDto
{
    public class gameDto
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
        public string md5GameSn
        {
            get
            {
                MD5 md5 = MD5.Create();
                byte[] source = Encoding.Default.GetBytes(sn.ToString());
                byte[] crypto = md5.ComputeHash(source);
                return Convert.ToBase64String(crypto);
            }
        }
        public Nullable<int> comSn { get; set; }
        public Nullable<int> userSn { get; set; }
        public string userId { get; set; }

        [Required]
        [DisplayName("競猜開始日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> sdate { get; set; }

        [Required]
        [DisplayName("競猜終止日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> edate { get; set; }

        [Required]
        [DisplayName("競猜主題")]
        public string title { get; set; }

        [DisplayName("競猜備註")]
        public string comment { get; set; }

        public Nullable<byte> apiStatus { get; set; }

        [DisplayName("競猜狀態")]
        public string Status
        {
            get
            {
                switch (apiStatus)
                {
                    case 0:
                        return "開盤";
                    case 1:
                        return "未開盤";
                    case 2:
                        return "封盤";
                    case 3:
                        return "已派彩";
                    case 4:
                        return "已設定結果";
                    default:
                        return "";
                }
            }
        }
        [Display(Name ="遊戲模式")]
       // [Required]
        public Nullable<byte> apiModel { get; set; }
        public string apiModelString
        {
            get
            {
                switch(apiModel)
                {
                    case 1:
                        return "精準預測";
                    case 2:
                        return "總彩池競猜";
                    default:
                        return "";
                }
            }
        }


        [DisplayName("比賽時間")]
        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> gamedate { get; set; }

        [DisplayName("API地點")]
        public string apiplace { get; set; }
        public Nullable<byte> valid { get; set; }

        [DisplayName("抽成比例(扣5% 輸入5)")]
        public Nullable<double> rake { get; set; }

        [DisplayName("題目清單")]
        public List<topicDto> topicList {get;set;}

        [DisplayName("可否下注")]
        public bool canbet
        {
            get
            {
                if (_canbet.HasValue)
                    return _canbet.Value;
                bool c = true;
                if (apiStatus != 0)
                    c = false;
                if (DateTime.Now < sdate)
                    c = false;
                if (DateTime.Now > edate)
                    c = false;
                return c;
            }
            set
            {
                _canbet = value;
            }
        }

        private bool? _canbet { get; set; }
        public List<priceDto> priceList { get; set; }


        
    }
}
