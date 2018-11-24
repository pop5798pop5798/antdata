using SITDto.Request;
using SITDto.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Web;

namespace SITW.Models.Repository
{
    public class BetsRepository
    {
        private static MemoryCache _cache = MemoryCache.Default;
        HttpClient client;
        public BetsRepository()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["apiurl"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async System.Threading.Tasks.Task<List<BetListDto>> BetsByUserID(string UserID)
        {
            List<BetListDto> BetList = new List<BetListDto>();

            HttpResponseMessage response = await client.GetAsync("api/BetsByUserID?userID=" + UserID);
            if (response.IsSuccessStatusCode)
            {
                BetList = await response.Content.ReadAsAsync<List<BetListDto>>();
            }

            return BetList;
        }


        public async System.Threading.Tasks.Task<List<BetListDto>> GetBetsByGame(int id/*,string UserID*/)
        {
            BetsByGameReq bbgr = new BetsByGameReq
            {
                GameSn = id
                //,
                //      UserID = UserID
            };

            List<BetListDto> BetList = new List<BetListDto>();

            lock (_cache)
            {
                if (_cache.Contains("GameBets_" + id))
                    return _cache.Get("GameBets_" + id) as List<BetListDto>;
            }

            HttpResponseMessage response = await client.PostAsJsonAsync("api/BetsByGame", bbgr);
            if (response.IsSuccessStatusCode)
            {
                BetList = await response.Content.ReadAsAsync<List<BetListDto>>();
            }

            lock (_cache)
            {
                CacheItemPolicy cacheItemPolicy = new CacheItemPolicy() { AbsoluteExpiration = DateTime.Now.AddDays(1) };
                _cache.Add("GameBets_" + id, BetList, cacheItemPolicy);
            }


            return BetList;
        }

        public async System.Threading.Tasks.Task<List<BetListDto>> GetBetsByTopic(int id, string UserID)
        {
            BetsByGameReq bbgr = new BetsByGameReq
            {
                GameSn = id
                //,
                //      UserID = UserID
            };

            List<BetListDto> BetList = new List<BetListDto>();

            HttpResponseMessage response = await client.PostAsJsonAsync("api/BetsByGame", bbgr);
            if (response.IsSuccessStatusCode)
            {
                BetList = await response.Content.ReadAsAsync<List<BetListDto>>();
            }

            return BetList;
        }




    }
}