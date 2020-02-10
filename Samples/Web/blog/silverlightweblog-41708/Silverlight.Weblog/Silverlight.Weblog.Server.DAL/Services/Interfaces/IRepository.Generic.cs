using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silverlight.Weblog.Server.DAL.Services.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> Get();
        //T GetByKey(int ID);
        void Insert(T t);
        void Delete(T t);
    }
}
