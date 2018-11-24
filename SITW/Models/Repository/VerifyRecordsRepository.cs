using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace SITW.Models.Repository
{
    public class VerifyRecordsRepository
    {
        public bool add(VerifyRecord vr)
        {
            bool isTrue = false;
            string query = @"
            INSERT INTO [dbo].[VerifyRecords]
                   ([userId]
                   ,[VerifyType]
                   ,[VerifyContent]
                   )
             VALUES
                   (@userId
                   ,@VerifyType
                   ,@VerifyContent
                   )
";
            using (SqlConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["SQLData"].ConnectionString.ToString()))
            {

                cn.Open();
                using (SqlCommand command = new SqlCommand(query, cn))
                {


                    try
                    {
                        command.Parameters.AddWithValue("@userId", vr.userId);
                        command.Parameters.AddWithValue("@VerifyType", vr.VerifyType);
                        command.Parameters.AddWithValue("@VerifyContent", vr.VerifyContent);
                        int _count = (int)command.ExecuteScalar();
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