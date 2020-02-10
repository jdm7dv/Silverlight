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

namespace Silverlight.Weblog.Client.Default.Widgets.DefaultTheme
{
    public interface IDefaultLayoutWidget : IBlogWidget
    {
        int Row { get; }
        int Column { get; }
        int ColumnSpan { get; }
        int RowSpan { get; }

        bool IsActive { get; }
        event EventHandler IsActiveChanged;
    }
}
