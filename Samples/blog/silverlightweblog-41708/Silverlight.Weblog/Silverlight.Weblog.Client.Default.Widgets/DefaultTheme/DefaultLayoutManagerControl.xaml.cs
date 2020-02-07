using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Silverlight.Weblog.Client.CoreBL.Widgets;
using Silverlight.Weblog.Client.DAL.Theme;
using Silverlight.Weblog.Client.Default.Widgets.DefaultTheme;
using Silverlight.Weblog.Shared.Common.Exception;

namespace Silverlight.Weblog.Client.Default.Widgets
{
    [Export(typeof(ILayoutManager))]
    public partial class DefaultLayoutManagerControl : UserControl, ILayoutManager
    {
        public DefaultLayoutManagerControl()
        {
            // Required to initialize variables
            InitializeComponent();

            scrollViewer.MouseWheel += new MouseWheelEventHandler(scrollViewer_MouseWheel);
        }

        /// <summary>
        /// Silverlight 3 scrollviewer mouse wheel hack.
        /// When upgrading to silverlight 4, remove this hack. 
        /// Though, we'd probably not get the event in Silverlight 4.
        /// So this can stay here forever, until Silverlight 10 at least.
        /// </summary>
        private void scrollViewer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - 3 * e.Delta);
        }

        #region Implementation of ITheme

        public void AddChild(IBlogWidget widget)
        {
            IDefaultLayoutWidget layoutElementToAddToGrid = widget as IDefaultLayoutWidget;

            if (layoutElementToAddToGrid == null)
                throw new SilverlightBlogException(widget.GetType().Name + " does not implement IDefaultLayoutWidget and cannot be nested in the default SilverlightBlog Theme. So, implement IDefaultLayoutWidget.");

            if (layoutElementToAddToGrid.IsActive)
                AddWidgetControlToGrid(layoutElementToAddToGrid);

            layoutElementToAddToGrid.IsActiveChanged += new EventHandler(themeElementToAddToGrid_IsActiveChanged);
        }

        void themeElementToAddToGrid_IsActiveChanged(object sender, EventArgs e)
         {
            IDefaultLayoutWidget layoutWidgetToEitherRemoveOrAdd = (IDefaultLayoutWidget) sender;

            if (layoutWidgetToEitherRemoveOrAdd.IsActive)
                AddWidgetControlToGrid(layoutWidgetToEitherRemoveOrAdd);
            else
                RemoveWidgetControlFromGrid(layoutWidgetToEitherRemoveOrAdd);
        }

        private void RemoveWidgetControlFromGrid(IDefaultLayoutWidget widget)
        {
            LayoutRoot.Children.Remove(widget.Control);
        }

        private void AddWidgetControlToGrid(IDefaultLayoutWidget layoutElementToAddToGrid)
        {
            Grid.SetRow(layoutElementToAddToGrid.Control, layoutElementToAddToGrid.Row);
            Grid.SetColumn(layoutElementToAddToGrid.Control, layoutElementToAddToGrid.Column);
            Grid.SetColumnSpan(layoutElementToAddToGrid.Control, layoutElementToAddToGrid.ColumnSpan);
            Grid.SetRowSpan(layoutElementToAddToGrid.Control, layoutElementToAddToGrid.RowSpan);
            LayoutRoot.Children.Add(layoutElementToAddToGrid.Control);
        }

        #endregion
    }
}