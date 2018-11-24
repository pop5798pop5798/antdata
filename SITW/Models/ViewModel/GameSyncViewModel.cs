using SITDto;
using SITW.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SITW.Models.ViewModel
{

    public class GameSyncViewModel
    {
        public GameSyncViewModel(gameDto gd, string encryptedKey)
        {
            this.sn = gd.sn;
            this.title = gd.title;
            this.comment = gd.comment;
            this.edate = gd.edate;
            this.canbet = gd.canbet;
            this.encryptedKey = encryptedKey;
            this.gamedate = gd.gamedate;
            this.gameplace = gd.apiplace;
            this.topicList = new List<TopicSyncViewModel>();
            foreach (topicDto td in gd.topicList)
            {
                this.topicList.Add(new TopicSyncViewModel(td,encryptedKey));
            }
        }

        public string md5GameSn
        {
            get
            {
                if (string.IsNullOrEmpty(_md5GameSn))
                {
                    MD5 md5 = MD5.Create();
                    byte[] source = Encoding.Default.GetBytes(sn.ToString());
                    byte[] crypto = md5.ComputeHash(source);
                    string smd5 = Convert.ToBase64String(crypto);
                    string pattern = @"([^0-9a-zA-Z])+";
                    Regex rgx = new Regex(pattern);
                    smd5 = rgx.Replace(smd5, "");
                    return smd5;
                }
                else
                {
                    return _md5GameSn;
                }
            }
            set
            {
                _md5GameSn = value;
            }
        }

        private string _md5GameSn { get; set; }
        private int sn { get; set; }
        public bool canbet { get; set; }
        public string title { get; set; }
        public string comment { get; set; }
        public Nullable<DateTime> gamedate { get; set; }
        public string gameplace { get; set; }
        public DateTime? edate { get; set; }
        public string encryptedKey { private get; set; }

        public List<TopicSyncViewModel> topicList { get; set; }
    }

    public class TopicSyncViewModel
    {
        public TopicSyncViewModel(topicDto td, string encryptedKey)
        {
            this.sn = td.sn;
            this.title = td.title;
            this.comment = td.comment;
            this.edate = td.edate;
            this.canbet = td.canbet;
            this.encryptedKey = encryptedKey;
            this.choiceList = new List<ChoiceViewModel>();
            foreach (choiceDto cd in td.choiceList)
            {
                this.choiceList.Add(new ChoiceViewModel(cd, this.encryptedKey));
            }
            foreach(ChoiceViewModel d in this.choiceList)
            {
                d.betMoneyRate = Math.Round((d.betMoney * 100 / this.choiceList.Sum(p => p.betMoney)) - 0.005, 2);
            }
        }

        public string md5TopicSn
        {
            get
            {
                if (string.IsNullOrEmpty(_md5TopicSn))
                {
                    MD5 md5 = MD5.Create();
                    byte[] source = Encoding.Default.GetBytes(sn.ToString());
                    byte[] crypto = md5.ComputeHash(source);
                    string smd5 = Convert.ToBase64String(crypto);
                    string pattern = @"([^0-9a-zA-Z])+";
                    Regex rgx = new Regex(pattern);
                    smd5 = rgx.Replace(smd5, "");
                    return smd5;
                }
                else
                {
                    return _md5TopicSn;
                }
            }
            set
            {
                _md5TopicSn = value;
            }
        }

        private string _md5TopicSn { get; set; }
        private int sn { get; set; }
        public bool canbet { get; set; }
        public double? Odds { get; set; }
        public string title { get; set; }
        public string comment { get; set; }
        public DateTime? edate { get; set; }
        public string encryptedKey { private get; set; }

        public List<ChoiceViewModel> choiceList { get; set; }
    }



    public class ChoiceViewModel
    {
        public ChoiceViewModel(choiceDto cd,string encryptedKey)
        {
            this.choiceSn = cd.sn;
            this.choiceStr = cd.choiceStr;
            this.comment = cd.comment;
            this.betMoney = cd.betMoneygti;
            this.Odds = cd.Odds;
            this.encryptedKey = encryptedKey;
        }

        public string EchoiceSn
        {
            get
            {
                if (!string.IsNullOrEmpty(_encryptedKey))
                {
                    Encryption oEncrypt = new Encryption();
                    _encryptedChoiceSn = oEncrypt.EncryptString(_encryptedKey, _choiceSn.ToString());
                }
                return _encryptedChoiceSn;
            }
            set
            {
                _encryptedChoiceSn = value;
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

        public int choiceSn
        {
            set
            {
                _choiceSn = value;
            }
        }
        private int _choiceSn { get; set; }
        public string md5ChoiceSn
        {
            get
            {
                if (string.IsNullOrEmpty(_md5ChoiceSn))
                {
                    MD5 md5 = MD5.Create();
                    byte[] source = Encoding.Default.GetBytes(_choiceSn.ToString());
                    byte[] crypto = md5.ComputeHash(source);
                    string smd5 = Convert.ToBase64String(crypto);
                    string pattern = @"([^0-9a-zA-Z])+";
                    Regex rgx = new Regex(pattern);
                    smd5 = rgx.Replace(smd5, "");
                    return smd5;
                }
                else
                {
                    return _md5ChoiceSn;
                }
            }
            set
            {
                _md5ChoiceSn = value;
            }
        }

        private string _md5ChoiceSn { get; set; }

        public string choiceStr { get; set; }
        public string comment { get; set; }
        public Nullable<double> Odds { get; set; }

        public double betMoney { get; set; }
        public double betMoneyRate { get; set; }

    }

    public class betViewModel
    {
        public List<betDetail> betList { get; set; }
    }

    public class betDetail
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
        /*

        public int choiceSn
        {
            get
            {
                Encryption oEncrypt = new Encryption();
                if (!string.IsNullOrEmpty(_encryptedKey))
                {
                    string schoiceSn = oEncrypt.DecryptString(EncryptionProvider.Rijndael, _encryptedKey, _encryptedChoiceSn);
                    int ichoiceSn = 0;
                    if (int.TryParse(schoiceSn, out ichoiceSn))
                        _choiceSn = ichoiceSn;
                }
                return _choiceSn;
            }
        }
        */
        [RegularExpression(@"^\+?[1-9][0-9]*0$|^$|^0$", ErrorMessage ="下注金額請以10為單位(例：880)")]
        public Nullable<double> money { get; set; }



        public int getChoiceSn(string encryptedKey)
        {
            this.encryptedKey = encryptedKey;
            Encryption oEncrypt = new Encryption();
            if (!string.IsNullOrEmpty(_encryptedKey))
            {
                string schoiceSn = oEncrypt.DecryptString(_encryptedKey, _encryptedChoiceSn);
                int ichoiceSn = 0;
                if (int.TryParse(schoiceSn, out ichoiceSn))
                    _choiceSn = ichoiceSn;
            }
            return _choiceSn;


        }

    }

}