using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Silverlight.Weblog.Client.Default.Widgets.BaseUI;
using Silverlight.Weblog.Shared.Common.Web.Helpers;

namespace Silverlight.Weblog.Client.DAL.ViewModel
{
    public class ViewModelBase  : INotifyPropertyChanged
    {
        public UserControlBase View { get; set; }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler changed = PropertyChanged;
            if (changed != null) changed(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void RaisePropertyChanged<T, TReturn>(Expression<Func<T, TReturn>> property)
        {
            RaisePropertyChanged(ExpressionHelper.GetPropertyName(property));
        }

        #endregion
    }
}
