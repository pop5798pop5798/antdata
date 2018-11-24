using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace SITW.Models.Repository
{
    public class SMSRecordRepository
    {
        public SMSRecordRepository()
        {
            //SqlConnection dataConnection = new SqlConnection();
            //String sqlConnectionStr = WebConfigurationManager.ConnectionStrings["SQLData"].ConnectionString.ToString();
        }

        public bool add(string UserId, string mobile, string content)
        {
            using (SqlConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["SQLData"].ConnectionString.ToString()))
            {

                string query = @"INSERT INTO SMSRecord
           ([UserId]
           ,[phoneNumber]
           ,[msg])
     VALUES
           (@UserId
           ,@PhoneNum
           ,@msg)
";

                cn.Open();
                using (SqlCommand command = new SqlCommand(query, cn))
                {


                    try
                    {
                        command.Parameters.AddWithValue("@UserId", UserId);
                        command.Parameters.AddWithValue("@PhoneNum", mobile);
                        command.Parameters.AddWithValue("@msg", content);
                        command.ExecuteNonQuery();
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

            }

            return true;
        }

        /// <summary>
        /// 檢查幾秒內此號碼是不是已經發過簡訊
        /// </summary>
        /// <param name="phoneNum">電話號碼</param>
        /// <param name="sec">秒數</param>
        /// <param name="count">檢查次數</param>
        /// <returns>true:此號碼在此秒數內有發送達count的次數</returns>
        public bool checkPhoneNumSend(string phoneNum, int sec, int count)
        {
            bool isTrue = false;
            string query = @"
            select count(*) as sendCount
            from SMSRecord sr
            where sr.phoneNumber=@phoneNum
            and DATEADD(SECOND,@sec, sr.inpdate)>getdate()
            ";
            using (SqlConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["SQLData"].ConnectionString.ToString()))
            {

                cn.Open();
                using (SqlCommand command = new SqlCommand(query, cn))
                {


                    try
                    {
                        command.Parameters.AddWithValue("@phoneNum", phoneNum);
                        command.Parameters.AddWithValue("@sec", sec);
                        int _count = (int)command.ExecuteScalar();
                        if (_count >= count)
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

        /// <summary>
        /// 檢查幾秒內此號碼是不是已經發過簡訊
        /// </summary>
        /// <param name="phoneNum">電話號碼</param>
        /// <param name="sec">秒數</param>
        /// <param name="count">檢查次數</param>
        /// <returns>true:此號碼在此秒數內有發送達count的次數</returns>
        public bool checkUserIdSend(string UserId, int sec, int count)
        {
            bool isTrue = false;
            string query = @"
            select count(*) as sendCount
            from SMSRecord sr
            where sr.UserId=@UserId
            and DATEADD(SECOND,@sec, sr.inpdate)>getdate()
            ";
            using (SqlConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["SQLData"].ConnectionString.ToString()))
            {

                cn.Open();
                using (SqlCommand command = new SqlCommand(query, cn))
                {


                    try
                    {
                        command.Parameters.AddWithValue("@UserId", UserId);
                        command.Parameters.AddWithValue("@sec", sec);
                        int _count = (int)(long)command.ExecuteScalar();
                        if (_count >= count)
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