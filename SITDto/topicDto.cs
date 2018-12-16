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
    public class topicDto
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
        public Nullable<int> gameSn { get; set; }

        [Required]
        [DisplayName("題目主題")]
        public string title { get; set; }
        [DisplayName("數據名稱")]
        public string apieTitle { get; set; }
        [DisplayName("題目備註")]
        public string comment { get; set; }
        public Nullable<double> totalmoney { get; set; }

        [DisplayName("題目開始日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> sdate { get; set; }

        [DisplayName("題目結束日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> edate { get; set; }
        public Nullable<byte> valid { get; set; }
        public Nullable<int> unitSn { get; set; }

        [DisplayName("選項清單")]
        public List<choiceDto> choiceList { get; set; }

        public bool canbet {
            get
            {
                bool c = true;
                if (sdate.HasValue && DateTime.Now < sdate)
                    c = false;
                if (edate.HasValue && DateTime.Now > edate)
                    c = false;
                if (c && _canbet.HasValue)
                    return _canbet.Value;
                return c;
            }
            set
            {
                _canbet = value;
            }
        }

        private bool? _canbet { get; set; }

        [DisplayName("顯示於主頁")]
        public bool? main { get; set; }
        public Nullable<int> apitypeid { get; set; }
        public Nullable<int> layer { get; set; }
    }
}
