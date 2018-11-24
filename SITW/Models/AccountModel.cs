using SITDto;
using SITW.Helper;
using SITW.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace SITW.Models
{
    public class AccountModel
    {

        public async System.Threading.Tasks.Task<bool> RegisterFinsh(ApplicationUser user)
        {


            //給予使用者預設金額
            int unitSn, assets;
            if (int.TryParse(CacheHelper.GlobalSettingData.Where(p => p.Key == "NewUserAssetsUnit").FirstOrDefault().Value, out unitSn))
            {

            }
            else
            {
                unitSn = 1;
            }

            if (int.TryParse(CacheHelper.GlobalSettingData.Where(p => p.Key == "NewUserAssets").FirstOrDefault().Value, out assets))
            {

            }
            else
            {
                assets = 0;
            }

            AssetsRecord ar = new AssetsRecord
            {
                type = 3,
                unitSn = unitSn,
                assets = assets,
                UserId = user.Id,
                inpdate = DateTime.Now
            };
            new AssetsRepository().AddAssetsByAssets(ar);
            string tokencreate = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            tokencreate = Convert.ToBase64String(time.Concat(key).ToArray());

            UserToken token = new UserToken
            {
                UsersId = user.Id,
                token = tokencreate,
                t_level = 0,
                t_sdate = DateTime.UtcNow,
                t_edate = DateTime.UtcNow
            };


            new TokenRepository().addtoken(token);
            //註冊成功同時到sitapi註冊使用者
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(System.Web.Configuration.WebConfigurationManager.AppSettings["apiurl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                userDto userd = new userDto
                {
                    comSn = 1,
                    email = user.Email,
                    name = user.Name,
                    userID = user.Id
                };
                HttpResponseMessage response = await client.PostAsJsonAsync("api/users", userd);
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                return false;
            }
            return true;
        }

    }
}