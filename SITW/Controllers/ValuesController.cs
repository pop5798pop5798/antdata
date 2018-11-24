using Microsoft.AspNet.Identity;
using SITDto.ViewModel;
using SITW.Models.Repository;
using SITW.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication1.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public async System.Threading.Tasks.Task<IEnumerable<string>> Get()
        {
            string UserID = User.Identity.GetUserId();
            BetsByUserID bbui = new BetsByUserID();
            List<BetListDto> BetList = new List<BetListDto>();



            return new string[] { "test", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
