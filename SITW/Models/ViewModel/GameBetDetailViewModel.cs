using SITDto;
using SITW.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SITW.Models.ViewModel
{
    /// <summary>
    /// 下注畫面ViewModel
    /// </summary>
    public class GameBetDetailViewModel
    {
        public string EchoiceSn
        {
            set
            {
                _encryptedChoiceSn = value;
            }
            get
            {
                return _encryptedChoiceSn;
            }
        }
        private string _encryptedChoiceSn { get; set; }

        public string encryptedKey
        {
            set
            {
                _encryptedKey = value;
            }
        }
        private string _encryptedKey { get; set; }
        private int _choiceSn { get; set; }

        /// <summary>
        /// 玩家目前資產清單
        /// </summary>
        public List<AssetsViewModel> avList { get; set; }

        public string topicTitle { get; set; }

        public string choiceStr { get; set; }

        public byte betModel { get; set; }

        [DisplayName("可下注單位")]
        public string[] betUnitArray
        {
            get
            {
                if (string.IsNullOrEmpty(betUnit))
                    return new string[] { };
                else
                    return betUnit.Split(',');
            }
            set
            {
                betUnit = string.Join(",", value);
            }
        }

        [DisplayName("可下注單位")]
        public string betUnit { get; set; }

        public int unitSn { get; set; }

        //[RegularExpression(@"^\+?[1-9][0-9]*0$|^$|^0$", ErrorMessage = "下注金額請以10為單位(例：880)")]
        [RegularExpression(@"^[0-9]+(.[0-9]{2})?$", ErrorMessage = "请输入正确金额")]
        public Nullable<double> money { get; set; }

        /// <summary>
        /// 各選項賠率清單
        /// </summary>
        public List<choiceOddsViewModel> choiceBetList { get; set; }

        public int getChoiceSn(string encryptedKey)
        {
            this.encryptedKey = encryptedKey;
            if (!string.IsNullOrEmpty(_encryptedKey))
            {
                string schoiceSn = Encryption.DecryptString(_encryptedKey, _encryptedChoiceSn);
                int ichoiceSn = 0;
                if (int.TryParse(schoiceSn, out ichoiceSn))
                    _choiceSn = ichoiceSn;
            }
            return _choiceSn;


        }

    }

}