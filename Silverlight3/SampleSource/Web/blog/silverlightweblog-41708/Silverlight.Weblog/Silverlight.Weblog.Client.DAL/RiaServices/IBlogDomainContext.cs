using System;
using System.Windows.Ria;
using Silverlight.Weblog.Server.DAL;

namespace Silverlight.Weblog.UI.Web.RiaServices
{
    public interface IBlogDomainContext
    {
        EntitySet<Comment> Comments { get; }
        SubmitOperation SubmitChanges();
        SubmitOperation SubmitChanges(System.Action<SubmitOperation> callback, object userState);

        EntityQuery<User> InitialDataQuery(Uri Uri);



        LoadOperation<TEntity> Load<TEntity>(EntityQuery<TEntity> query, 
                                             Action<LoadOperation<TEntity>> callback,
                                             object userState)
            where TEntity : System.Windows.Ria.Entity;

    }
}