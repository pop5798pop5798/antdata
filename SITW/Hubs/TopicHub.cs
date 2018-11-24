using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using SITDto;
using SITW.Models.Repository;
using Newtonsoft.Json;
using SITW.Models.ViewModel;
using SITW.Helper;

namespace SITW.Hubs
{
    public class TopicHub : Hub
    {
        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public async Task JoinGroup(string roomName, string encryptedID)
        {
            string encryptedKey = System.Web.Configuration.WebConfigurationManager.AppSettings["encryptedKey"];
            Encryption oEncrypt = new Encryption();
            string sGameSn = oEncrypt.DecryptString(encryptedKey, encryptedID);
            int gameSn;
            gameDto gd = new gameDto();
            if (int.TryParse(sGameSn, out gameSn))
            {
                gd = await new GamesRepository().GetGameDetail(gameSn);

            }
            List<TopicSyncViewModel> tvmList = new List<TopicSyncViewModel>();
            foreach(topicDto td in gd.topicList)
            {
                tvmList.Add(new TopicSyncViewModel(td, encryptedKey));
            }

            string returnJson = JsonConvert.SerializeObject(tvmList);

            Clients.Caller.ShowTopic(returnJson);

            await Groups.Add(Context.ConnectionId, roomName);
        }

    }
}