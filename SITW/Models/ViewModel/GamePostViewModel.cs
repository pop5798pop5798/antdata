using SITDto;
using SITW.Helper;
using SITW.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SITW.Models.ViewModel
{
    public class GamePostViewModel
    {
        public GamePostViewModel()
        {
            vedio = new VedioRecord();
            gamepost = new APIPosts();
            game = new gameDto();
            cfgVedio = new cfgVedio();
            TeamA = new Teams();
            TeamB = new Teams();
            PlayGame = new ApiTypes();      
        }
        public GamePostViewModel(int sn,string encryptedKey, gameDto gd)
        {
            APIPosts gamepost = new GamePostsRepository().get(sn);
            this.encryptedKey = encryptedKey;
            this.game = gd;
            this.gamepost = gamepost;


        }

        public string md5GameSn
        {
            get
            {
                if (game != null)
                {
                    MD5 md5 = MD5.Create();
                    byte[] source = Encoding.Default.GetBytes(game.sn.ToString());
                    byte[] crypto = md5.ComputeHash(source);
                    return Convert.ToBase64String(crypto);
                }
                else
                    return "";
            }
        }
        public string encryptedGameSn
        {
            get
            {
                if (game != null && !string.IsNullOrEmpty(_encryptedKey))
                {
                    Encryption oEncrypt = new Encryption();
                    _encryptedGameSn = oEncrypt.EncryptString(_encryptedKey, game.sn.ToString());
                }
                return _encryptedGameSn;
            }
        }

        private string _encryptedGameSn { get; set; }

        public string encryptedGamePostSn
        {
            get
            {
                if (gamepost != null && !string.IsNullOrEmpty(_encryptedKey))
                {
                    Encryption oEncrypt = new Encryption();
                    _encryptedGamePostSn = oEncrypt.EncryptString(_encryptedKey, gamepost.sn.ToString());
                }
                return _encryptedGamePostSn;
            }
            set
            {
                _encryptedGamePostSn = value;
            }
        }

        private string _encryptedGamePostSn { get; set; }


        public int getGamePostSn(string _encryptedKey)
        {
            this._encryptedKey = _encryptedKey;
            int gps = 0;
            string sgps = "";
            if (!string.IsNullOrEmpty(_encryptedGamePostSn) && !string.IsNullOrEmpty(_encryptedKey))
            {
                Encryption oEncrypt = new Encryption();
                sgps = oEncrypt.DecryptString(_encryptedKey, _encryptedGamePostSn);
                if (int.TryParse(sgps, out gps))
                {

                }
            }
            return gps;

        }

        


        public string encryptedKey
        {
            set
            {
                _encryptedKey = value;
            }
        }
        private string _encryptedKey { get; set; }

        public APIPosts gamepost { get; set; }
        public gameDto game { get; set; }
        public VedioRecord vedio { get; set; }
        public cfgVedio cfgVedio { get; set; }
        public Teams TeamA { get; set; }
        public Teams TeamB { get; set; }
        public ApiTypes PlayGame { get; set; }
        public int endguess { get; set; }
        public string gamesearch { get; set; }
        public string vedio_url { get; set; }




        public string iframestring {
            get
            {
                string _iframe = "";
                if (vedio != null && cfgVedio != null && !string.IsNullOrEmpty(vedio.vediourl))
                {
                    Regex defaultRegex = new Regex(this.cfgVedio.RegularStr);
                    _iframe = defaultRegex.Replace(this.vedio.vediourl, this.cfgVedio.ReplaceStr);
                    _iframe = this.cfgVedio.iframehtml.Replace("{vediourl}", _iframe);
                }
                return _iframe;
            }
        }

    }
}