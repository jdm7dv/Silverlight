using System.Windows.Ria;
using Silverlight.Weblog.UI.Web.RiaServices;

namespace Silverlight.Weblog.Client.DAL.Services.Interfaces
{
    public interface IDBContextContainer
    {
        void Initialize();
        void Dispose();
        void CommitChanges();
        IBlogDomainContext Current { get; set; }
    }
}