using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace SITW.Models.Repository
{
    public class GlobalSettingRepository
    {

        public Dictionary<string, string> getAll()
        {
            Dictionary<string, string> gs = new Dictionary<string, string>();
            using (SqlConnection cn = new SqlConnection(WebConfigurationManager.ConnectionStrings["SQLData"].ConnectionString.ToString()))
            {

                string query = @"
select settingName,settingValue
from GlobalSetting
where valid=1
";
                cn.Open();
                using (SqlCommand command = new SqlCommand(query, cn))
                {
                    SqlDataReader reader = command.ExecuteReader();


                    try
                    {
                        while (reader.Read())
                        {
                            gs.Add(reader["settingName"].ToString(), reader["settingValue"].ToString());
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }


                }

            }

            return gs;
        }

    }
}