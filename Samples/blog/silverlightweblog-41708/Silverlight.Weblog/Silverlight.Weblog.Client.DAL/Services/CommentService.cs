using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Ria;
using System.Windows.Shapes;
using Microsoft.Practices.Unity;
using Silverlight.Weblog.Client.DAL.Services.Interfaces;
using Silverlight.Weblog.Server.DAL;
using System.Linq;

namespace Silverlight.Weblog.Client.DAL.Services
{
    public class CommentService : ICreate<Comment>
    {
        [Dependency]
        public IDBContextContainer DBContextContainer { get; set; }

        public void Create(Comment comment)
        {
            DBContextContainer.Current.Comments.Add(comment);
            DBContextContainer.Current.SubmitChanges(
                 (callback) =>
                 {
                     // hack: If 2 new comments are created.
                     // the first is sent OK.
                     // the second one is sent with the first one. 
                     // the first one then fails validation.
                     // which, is fucked up. 
                     // so if we're sending more then 2 comment entities
                     // we'll ignore validation errors, cuz it's all good. 

                     if (callback.EntitiesInError.All(entity => entity is Comment)
                      && callback.ChangeSet.AddedEntities.Count() >= 2)
                     {
                         callback.MarkErrorAsHandled();
                     }


                 }, comment);
        }
    }
}
