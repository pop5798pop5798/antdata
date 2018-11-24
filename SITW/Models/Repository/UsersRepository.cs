using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace SITW.Models.Repository
{
    public class UserRepository
    {
        public UserRepository()
        {

        }

        public bool checkPhoneNumber(string phoneNum)
        {
            bool isTrue = false;
            string query = @"
            select count(*) as sendCount
            from Users u
            where u.PhoneNumber=@phoneNum
			and u.PhoneNumberConfirmed=1
            ";
            using (SqlConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["SQLData"].ConnectionString.ToString()))
            {

                cn.Open();
                using (SqlCommand command = new SqlCommand(query, cn))
                {


                    try
                    {
                        command.Parameters.AddWithValue("@phoneNum", phoneNum);
                        int _count = (int)command.ExecuteScalar();
                        if (_count >= 1)
                            isTrue = true;
                    }
                    catch (Exception e)
                    {
                        //抓錯誤訊息
                        return false;
                    }
                    finally
                    {
                    }
                }

                return isTrue;

            }

        }

        public bool checkUserHavePhone(string userId)
        {
            bool isTrue = false;
            string query = @"
            select count(*) as phoneCount
            from Users u
            where Id=@userId
            and u.PhoneNumber is not null
            and u.PhoneNumberConfirmed=1
            ";
            using (SqlConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["SQLData"].ConnectionString.ToString()))
            {

                cn.Open();
                using (SqlCommand command = new SqlCommand(query, cn))
                {


                    try
                    {
                        command.Parameters.AddWithValue("@userId", userId);
                        int _count = (int)command.ExecuteScalar();
                        if (_count >= 1)
                            isTrue = true;
                    }
                    catch (Exception e)
                    {
                        //抓錯誤訊息
                        return false;
                    }
                    finally
                    {
                    }
                }

                return isTrue;

            }
        }
    }
}