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
    public class Dota2Repository
    {
        private sitwEntities Db = new sitwEntities();

        public List<Dota2League>  Dota2LeagueList()
        {
            return Db.Dota2League.Where(p => p.valid == 1).ToList();
        }


        public async System.Threading.Tasks.Task<Dota2PlayerModel.RootObject> GetDota2Game(string leagueid)
        {
            //Dota2Model.RootObject dota2 = null;
            string url = "https://api.steampowered.com/IDOTA2Match_570/GetMatchHistory/V001/?league_id=" + leagueid + "&key=10608324BF5A95D48936E6471867E64E&matches_requested=10";          

            Dota2PlayerModel.RootObject data = null;

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<Dota2PlayerModel.RootObject>(jsonResponse);                 
                }
            }


          
          

            return data;
        }

        public async System.Threading.Tasks.Task<Dota2PlayerListModel.RootObject> GetDota2List(string gameid)
        {
            //Dota2Model.RootObject dota2 = null;
            //string url = "https://api.steampowered.com/IDOTA2Match_570/GetMatchHistory/V001/?league_id=" + gameid + "&key=10608324BF5A95D48936E6471867E64E&matches_requested=5";
            string url = "https://api.steampowered.com/IDOTA2Match_570/GetMatchDetails/V001/?match_id="+ gameid + "&key=10608324BF5A95D48936E6471867E64E";
            Dota2PlayerListModel.RootObject data = null;

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<Dota2PlayerListModel.RootObject>(jsonResponse);
                }
            }

            return data;
        }



        public async System.Threading.Tasks.Task<Dota2PlayerModel.RootObject> Dota2Game()
        {
            //Dota2Model.RootObject dota2 = null;
            string url = "https://api.steampowered.com/IDOTA2Match_570/GetMatchHistory/V001/?key=10608324BF5A95D48936E6471867E64E&matches_requested=10";

            Dota2PlayerModel.RootObject data = null;

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<Dota2PlayerModel.RootObject>(jsonResponse);
                }
            }

            return data;
        }


        public async System.Threading.Tasks.Task<string> GetDota2Teams(string teams)
        {
            //Dota2Model.RootObject dota2 = null;
            string url = "https://api.steampowered.com/IDOTA2Match_570/GetTeamInfoByTeamID/v1/?start_at_team_id="+ teams +"&key=10608324BF5A95D48936E6471867E64E&teams_requested=1";

            Dota2TeamsModel.RootObject data = null;
            string[] d2string = { }; 
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<Dota2TeamsModel.RootObject>(jsonResponse);
                    

                }
            }
            foreach(var d2 in data.result.teams)
            {
                d2string = new string[] { d2.name };
            }
            string d2teamsname;
            if (teams != "Zenith")
            {
                d2teamsname = d2string.FirstOrDefault();
            }
            else
            {
                d2teamsname = "未知";

            }
            

        
            return d2teamsname;
        }


        public async System.Threading.Tasks.Task<List<Dota2Upcoming.RootObject>> GetDota2Upcoming()
        {
            //Dota2Model.RootObject dota2 = null;
            string url = "https://api.pandascore.co/dota2/matches/?token=lhqJrfIEI-Qsz1FX0AV1W4b8QJtOPTS78ykqU2p6zI2BWA1D9KY";

            JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };


            List<Dota2Upcoming.RootObject> data = null;
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<List<Dota2Upcoming.RootObject>>(jsonResponse, microsoftDateFormatSettings);


                }
            }
                 

            return data;
        }





    }
}