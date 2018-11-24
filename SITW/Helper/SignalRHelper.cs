using Newtonsoft.Json;
using SITDto;
using SITW.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SITW.Helper
{
    public class SignalRHelper
    {
        public void UpdateTopic(gameDto game,string encryptedKey,string GroupKey)
        {
            List<topicDto> topicList = game.topicList;
            List<TopicSyncViewModel> tvmList = new List<TopicSyncViewModel>();
            foreach (topicDto t in topicList)
            {
                tvmList.Add(new TopicSyncViewModel(t, encryptedKey));
            }
            string returnJson = JsonConvert.SerializeObject(tvmList);
            var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<SITW.Hubs.TopicHub>();
            context.Clients.Group(GroupKey).updateTopic(returnJson);
        }

        /// <summary>
        /// 更新用戶端此選項的下注金額
        /// </summary>
        /// <param name="game">game物件</param>
        /// <param name="encryptedKey">gameSn解密的key</param>
        /// <param name="GroupKey">signalR Group的key，也是game.md5GameSn</param>
        public void UpdateChoiceMoney(gameDto game,string encryptedKey,string GroupKey)
        {
            List<topicDto> topicList = game.topicList;
            List<TopicSyncViewModel> tvmList = new List<TopicSyncViewModel>();
            foreach (topicDto t in topicList)
            {
                tvmList.Add(new TopicSyncViewModel(t, encryptedKey));
            }
            string returnJson = JsonConvert.SerializeObject(tvmList);
            var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<SITW.Hubs.TopicHub>();
            context.Clients.Group(GroupKey).updateBetMoney(returnJson);
        }

    }
}