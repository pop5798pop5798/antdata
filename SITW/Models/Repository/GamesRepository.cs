using SITDto;
using SITDto.Request;
using SITDto.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Web;
using System.Web.Script.Serialization;

namespace SITW.Models.Repository
{
    public class GamesRepository
    {
        private static MemoryCache _cache = MemoryCache.Default;
        HttpClient client;
        public GamesRepository()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["apiurl"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async System.Threading.Tasks.Task<List<gameDto>> GetGameList()
        {
            //List<gameDto> gameList = null;

            lock (_cache)
            {
                if (_cache.Contains("GameList"))
                    return _cache.Get("GameList") as List<gameDto>;
            }

            

            return await reflashGameListAsync();
        }

        public async System.Threading.Tasks.Task<List<gameDto>> GetValidGameList()
        {
            List<gameDto> gameList = null;
            gameList = await GetGameList();
            gameList = gameList.Where(p => p.sdate<=DateTime.Now && p.edate >= DateTime.Now && p.canbet).ToList();
            return gameList;
        }

        public async System.Threading.Tasks.Task<List<gameDto>> GetMainGameList()
        {
            List<gameDto> gameList = null;
            gameList = await GetGameList();
            DateTime dt = DateTime.Now.AddHours(-2);
            gameList = gameList.Where(p => p.sdate <= DateTime.Now && p.edate >= dt && p.apiStatus == 0).ToList();
            return gameList;
        }

        public async System.Threading.Tasks.Task<List<gameDto>> reflashGameListAsync()
        {
            List<gameDto> gameList = null;
            HttpResponseMessage response = await client.GetAsync("/api/games");
            if (response.IsSuccessStatusCode)
            {
                gameList = await response.Content.ReadAsAsync<List<gameDto>>();
            }

            lock (_cache)
            {
                CacheItemPolicy cacheItemPolicy = new CacheItemPolicy() { AbsoluteExpiration = DateTime.Now.AddDays(1) };
                _cache.Remove("GameList");
                _cache.Add("GameList", gameList, cacheItemPolicy);
            }

            return gameList;
        }


        public async System.Threading.Tasks.Task<gameDto> GetGameDetail(int id)
        {
            gameDto game = null;
            //HttpResponseMessage response = await client.GetAsync("/api/games/" + id);
            //if (response.IsSuccessStatusCode)
            //{
            //    game = await response.Content.ReadAsAsync<gameDto>();
            //}
            game = (await GetGameList()).Where(p => p.sn == id).FirstOrDefault();
            return game;
        }

        public async System.Threading.Tasks.Task<gameDto> GetGameDetailByTopicSn(int id)
        {
            List<gameDto> gameList = await GetGameList();
            gameDto game = new gameDto();
            foreach (gameDto g in gameList)
            {
                if (g.topicList.Where(p => p.sn == id).Count() >= 1)
                {
                    game = g;
                    break;
                }
            }

            return game;
        }

        public async System.Threading.Tasks.Task<gameDto> GetGameDetailByChoiceSn(int id)
        {
            List<gameDto> gameList = await GetGameList();
            gameDto game = new gameDto();
            foreach (gameDto g in gameList)
            {
                foreach(topicDto t in g.topicList)
                {
                    if (t.choiceList.Where(p => p.sn == id).Count() >= 1)
                    {
                        game = g;
                        break;
                    }
                }
            }

            return game;
        }

        public async System.Threading.Tasks.Task<topicDto> GetTopicByChoiceSn(int id)
        {
            List<gameDto> gameList = await GetGameList();
            topicDto topic = new topicDto();
            foreach (gameDto g in gameList)
            {
                foreach(topicDto t in g.topicList)
                {
                    if (t.choiceList.Where(p => p.sn == id).Count() >= 1)
                    {
                        topic = t;
                        break;
                    }
                }
            }

            return topic;
        }

        public async System.Threading.Tasks.Task<gameDto> Create(gameDto game)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/games", game);
            var result = await response.Content.ReadAsStringAsync();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            game = json_serializer.Deserialize<gameDto>(result);
            response.EnsureSuccessStatusCode();

            await reflashGameListAsync();

            return game;
        }


        public async System.Threading.Tasks.Task<gameDto> Edit(int id)
        {
            gameDto game = null;
            HttpResponseMessage response = await client.GetAsync("/api/games/" + id);
            if (response.IsSuccessStatusCode)
            {
                game = await response.Content.ReadAsAsync<gameDto>();
            }
            response.EnsureSuccessStatusCode();

            return game;
        }

        public async System.Threading.Tasks.Task<gameDto> Edit(int id, gameDto game)
        {
            game.comSn = 1;
            HttpResponseMessage response = await client.PutAsJsonAsync("api/games/" + id, game);
            response.EnsureSuccessStatusCode();

            await reflashGameListAsync();
            game = await GetGameDetail(game.sn);
            return game;
        }


        public async System.Threading.Tasks.Task<bool> setWinner(SetWinnerReq swq)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/setWinner", swq);
            await reflashGameListAsync();
            return response.IsSuccessStatusCode;

        }

        public async System.Threading.Tasks.Task<bool> StartBet(StartBetReq sbr)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/StartBet", sbr);

            await reflashGameListAsync();
            return response.IsSuccessStatusCode;

        }

        public async System.Threading.Tasks.Task<bool> reopen(StartBetReq sbr)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/reopen", sbr);

            await reflashGameListAsync();
            return response.IsSuccessStatusCode;

        }

        public async System.Threading.Tasks.Task<bool> setClose(StartBetReq sbr)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/setClose", sbr);

            await reflashGameListAsync();
            return response.IsSuccessStatusCode;

        }
        public async System.Threading.Tasks.Task<bool> deleteClose(StartBetReq sbr)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/deleteClose", sbr);
            new AssetsRepository().AddAssetsByRollback(sbr.gameSn);
            await reflashGameListAsync();
            return response.IsSuccessStatusCode;

        }

        public async System.Threading.Tasks.Task<aJaxDto> pays(StartBetReq sbr)
        {
            aJaxDto ajd = new aJaxDto();
            HttpResponseMessage response = await client.PostAsJsonAsync("api/pays", sbr);
            if (response.IsSuccessStatusCode)
            {
                List<payoutDto> payoutList = await response.Content.ReadAsAsync<List<payoutDto>>();
                new AssetsRepository().AddAssetsByPay(payoutList);

                response = await client.PostAsJsonAsync("api/setDone", sbr);
                if (response.IsSuccessStatusCode)
                {
                    ajd.isTrue = true;
                    await reflashGameListAsync();
                }
                else
                {
                    ajd.isTrue = false;
                    ajd.ErrorCode = 500;
                    ajd.ErrorMsg = response.Content.ReadAsAsync<string>().Result;
                }
            }
            else
            {
                ajd.isTrue = false;
                ajd.ErrorCode = 500;
                ajd.ErrorMsg = response.ReasonPhrase;
            }
            await reflashGameListAsync();
            return ajd;

        }

        public async System.Threading.Tasks.Task<aJaxDto> paysRollback(StartBetReq sbr)
        {
            aJaxDto ajd = new aJaxDto();
            HttpResponseMessage response = await client.PostAsJsonAsync("api/paysRollback", sbr);
            if (response.IsSuccessStatusCode)
            {
                List<payoutDto> payoutList = await response.Content.ReadAsAsync<List<payoutDto>>();
                new AssetsRepository().AddAssetsByPayRollback(payoutList);

                ajd.isTrue = true;
                await reflashGameListAsync();
            }
            else
            {
                ajd.isTrue = false;
                ajd.ErrorCode = 500;
                ajd.ErrorMsg = response.ReasonPhrase;
            }
            await reflashGameListAsync();
            return ajd;

        }


        public async System.Threading.Tasks.Task<aJaxDto> stopTopic(StopTopicReq str)
        {
            aJaxDto ajd = new aJaxDto();
            HttpResponseMessage response = await client.PostAsJsonAsync("api/stopTopic", str);
            if (response.IsSuccessStatusCode)
            {
                ajd.isTrue = true;
            }
            else
            {
                ajd.isTrue = false;
                ajd.ErrorCode = 500;
                ajd.ErrorMsg = response.ReasonPhrase;
            }
            await reflashGameListAsync();
            return ajd;

        }

        public async System.Threading.Tasks.Task<bool> gamereturn(StartBetReq sbr)
        {

            HttpResponseMessage response = await client.PostAsJsonAsync("api/gamereturn", sbr);
            await reflashGameListAsync();
            return response.IsSuccessStatusCode;

        }



        public async System.Threading.Tasks.Task<aJaxDto> reopenTopic(StopTopicReq str)
        {
            aJaxDto ajd = new aJaxDto();
            HttpResponseMessage response = await client.PostAsJsonAsync("api/reopenTopic", str);
            if (response.IsSuccessStatusCode)
            {
                ajd.isTrue = true;
            }
            else
            {
                ajd.isTrue = false;
                ajd.ErrorCode = 500;
                ajd.ErrorMsg = response.ReasonPhrase;
            }
            await reflashGameListAsync();
            return ajd;

        }

        /// <summary>
        /// 寫入下注資料，並扣除錢幣
        /// </summary>
        /// <param name="bet"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<bool> CreateBet(betDto bet)
        {
            try
            {
                bool isTrue = true;
                HttpResponseMessage response = await client.PostAsJsonAsync("api/bets", bet);
                if (response.IsSuccessStatusCode)
                    new AssetsRepository().AddAssetsByBet(bet);

                isTrue = addChoiceMoney(bet.choiceSn.Value, bet.money.Value);


                return isTrue;
            }
            catch
            {
                return false;
            }
        }

        public bool addChoiceMoney(int choiceSn, double money)
        {
            bool isTure = true;

            lock (_cache)
            {
                if (_cache.Contains("GameList"))
                {
                    List<gameDto> gameList = new List<gameDto>();
                    gameList = _cache.Get("GameList") as List<gameDto>;
                    foreach(gameDto g in gameList)
                    {
                        foreach(topicDto t in g.topicList)
                        {
                            if(t.choiceList.Where(p=>p.sn==choiceSn).Count()>0)
                            {
                                choiceDto c = t.choiceList.Where(p => p.sn == choiceSn).FirstOrDefault();
                                c.betMoneygti += money;
                            }

                        }

                    }
                    CacheItemPolicy cacheItemPolicy = new CacheItemPolicy() { AbsoluteExpiration = DateTime.Now.AddDays(1) };
                    _cache.Remove("GameList");
                    _cache.Add("GameList", gameList, cacheItemPolicy);

                }
                else
                {
                    isTure = false;
                }
            }

            return isTure;
        }

    }
}