using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SITW.Models
{
    public class UserInformation
    {
        public UserInformation(string UserID)
        {

        }
        public UserBasic UserBasic { get; set; }
        public List<LoginData> LoginList { get; set; }
        public List<AssetData> AssetList { get; set; }


    }

    public class UserBasic
    {
        public string Name { get; set; }
        public string email { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }

    }

    public class LoginData
    {
        public DateTime LoginDate { get; set; }
    }


    public class AssetData
    {
        public string unitName { get; set; }
        public double Number { get; set; }
    }


}