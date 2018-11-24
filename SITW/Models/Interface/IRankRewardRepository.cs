using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SITW.Models.Interface
{
    public interface IRankRewardRepository : IDisposable
    {
        void Create(Ranking_content instance);

        void Update(Ranking_content instance);

        void Delete(Ranking_content instance);

        Ranking_content Get(int RewardID);

        IQueryable<Ranking_content> GetAll();      

        void SaveChanges();
    }
}