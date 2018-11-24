using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;

namespace SITW.Models.Interface
{
    public interface INewsRepository:IDisposable
    {      
            void Create(placard instance);

            void Update(placard instance);

            void Delete(placard instance);

            placard Get(int placardID);

            IQueryable<placard> GetAll();

            IQueryable<placard> GetAllaction();

            IQueryable<placard> GetAllstop();

            IQueryable<placard> GetAllnews();

        void SaveChanges();
        
    }
}