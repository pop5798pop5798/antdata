using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SITW.Models.Interface
{
    public interface IRewardTitleRepository : IDisposable
    {
        void Create(Ranking_title instance);

        void Update(Ranking_title instance);

        void Delete(Ranking_title instance);

        Ranking_title Get(int RewardID);

        IQueryable<Ranking_title> GetAll();

        void SaveChanges();
    }
}