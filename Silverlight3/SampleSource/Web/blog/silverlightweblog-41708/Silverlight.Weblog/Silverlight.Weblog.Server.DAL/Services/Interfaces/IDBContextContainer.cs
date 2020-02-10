using System.Data.Objects;

namespace Silverlight.Weblog.Server.DAL.Services.Interfaces
{
    public interface IDBContextContainer
    {
        void Initialize();
        void Dispose();
        void CommitChanges();
        ObjectContext DBContext { get; set; }
    }
}