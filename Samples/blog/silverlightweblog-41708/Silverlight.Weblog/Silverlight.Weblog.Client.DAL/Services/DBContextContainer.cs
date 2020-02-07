using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Practices.Unity;
using Silverlight.Weblog.Client.DAL.Services.Interfaces;
using Silverlight.Weblog.UI.Web.RiaServices;

namespace Silverlight.Weblog.Client.DAL.Services
{
    public class DBContextContainer : IDBContextContainer
    {
        #region Implementation of IDBContextContainer

        [Dependency]
        public IBlogDomainContext Current { get; set; }

        public void Initialize()
        {
        }

        public void Dispose()
        {
        }

        public void CommitChanges()
        {
            Current.SubmitChanges();
        }

        #endregion
    }
}
