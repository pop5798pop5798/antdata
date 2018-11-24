using SITDto.ViewModel;
using SITW.Models;
using SITW.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace SITW.Helper
{

    public static class CacheHelper
    {
        private static MemoryCache _cache = MemoryCache.Default;

        public static Dictionary<string, string> GlobalSettingData
        {
            get
            {
                lock (_cache)
                {
                    if (!_cache.Contains("GlobalSettingData"))
                        RefreshGlobalSettingData();
                }
                return _cache.Get("GlobalSettingData") as Dictionary<string, string>;
            }
        }

        public static void RefreshGlobalSettingData()
        {
            //移除 cache 中資料
            _cache.Remove("GlobalSettingData");

            //存取資料
            var listAgency = new GlobalSettingRepository().getAll();

            //設定 cache 過期時間
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy() { AbsoluteExpiration = DateTime.Now.AddDays(1) };
            //加入 cache
            _cache.Add("GlobalSettingData", listAgency, cacheItemPolicy);
        }
        /*
        public static List<BetListDto> GetGameBetsData(int key)
        {
            lock (_cache)
            {
                //key = "GameBets_" + key;
                if (!_cache.Contains("GameBets_" + key))
                    RefreshGameBetsData(key);
            }
            return _cache.Get("GameBets_" + key) as List<BetListDto>;
        }

        public static void RefreshGameBetsData(int key)
        {
            //移除 cache 中資料
            _cache.Remove("GameBets_" + key);

            //存取資料

            //設定 cache 過期時間
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy() { AbsoluteExpiration = DateTime.Now.AddDays(1) };
            //加入 cache
            _cache.Add("GameBets_" + key, listAgency, cacheItemPolicy);
        }
        */
    }
}