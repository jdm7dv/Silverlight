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
using Silverlight.Weblog.Client.CoreBL.Widgets;
using Silverlight.Weblog.Client.DAL.ViewModel;
using Silverlight.Weblog.Client.Default.Widgets.DefaultTheme;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Shared.Common.Helpers;

namespace Silverlight.Weblog.Client.Default.Widgets.BaseUI
{
    public class UserControlBase : UserControl, IBlogWidget, IDefaultLayoutWidget
    {
        public UserControlBase()
        {
            ColumnSpan = 1;
            RowSpan = 1;
        }

        #region Implementation of IBlogWidget

        public virtual void InitializeWidget() { }
        public void Initialize()
        {
            SetViewModelView();
        }

        private void SetViewModelView()
        {
            if (this.PropertyExists("ViewModel"))
            {
                object curViewModel = IoC.Get(this.GetPropertyType("ViewModel"));
                this.SetProperty("ViewModel", curViewModel);
                curViewModel.SetProperty("View", this);
                this.DataContext = curViewModel;
            }

        }


        public FrameworkElement Control
        {
            get
            {
                return this;
            }
        }

        #endregion

        #region Implementation of IDisposable

        public virtual void Dispose()
        {
            IDisposable curViewModel = this.GetProperty("ViewModel") as IDisposable;
            if (curViewModel != null)
            {
                curViewModel.Dispose();
            }
        }

        #endregion

        #region Implementation of IDefaultLayoutWidget


        #endregion

        #region Implementation of IDefaultLayoutWidget

        public virtual int Row { get; private set; }
        public virtual int Column { get; private set; }
        public virtual int ColumnSpan { get; private set; }
        public virtual int RowSpan { get; private set; }
        
        private bool _isActive = true;
        public virtual bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                RaiseIsActiveChanged();
            }
        }

        public event EventHandler IsActiveChanged;

        protected void RaiseIsActiveChanged()
        {
            EventHandler handler = IsActiveChanged;
            if (handler != null) handler(this, new EventArgs());
        }

        #endregion
    }
}