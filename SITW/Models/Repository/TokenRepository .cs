using Dapper;
using SITDto;
using SITW.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace SITW.Models.Repository
{
    public class TokenRepository
    {
        private sitwEntities db = new sitwEntities();

        public void addtoken(UserToken token)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }
            else
            {
                db.UserToken.Add(token);
                db.SaveChanges();
            }
        }

        public UserToken gettoken(string userid)
        {
            if (userid == null)
            {
                throw new ArgumentNullException("userid");
            }
            else
            {
               return db.UserToken.Where(x => x.UsersId == userid).FirstOrDefault();
            }
        }


    }
}