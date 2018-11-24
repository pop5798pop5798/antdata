using Dapper;
using Newtonsoft.Json;
using SITW.Models.GameAPIModels;
using SITW.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using unirest_net.http;

 


namespace SITW.Models.Repository
{
    public class LOLRepository
    {
       
        public async System.Threading.Tasks.Task<List<LOLUpcoming.RootObject>> GetLOLUpcoming()
        {
            //Dota2Model.RootObject dota2 = null;
            string url = "https://api.pandascore.co/lol/matches/upcoming/?token=lhqJrfIEI-Qsz1FX0AV1W4b8QJtOPTS78ykqU2p6zI2BWA1D9KY";

            JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };


            List<LOLUpcoming.RootObject> data = null;
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<List<LOLUpcoming.RootObject>>(jsonResponse, microsoftDateFormatSettings);


                }
            }
                 

            return data;
        }


        public async System.Threading.Tasks.Task<List<LOLALL.RootObject>> GetLOLAll()
        {
            //Dota2Model.RootObject dota2 = null;
            string url = "https://api.pandascore.co/lol/matches/?token=lhqJrfIEI-Qsz1FX0AV1W4b8QJtOPTS78ykqU2p6zI2BWA1D9KY";

            JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };


            List<LOLALL.RootObject> data = null;
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<List<LOLALL.RootObject>>(jsonResponse, microsoftDateFormatSettings);


                }
            }


            return data;
        }




    }
}