using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DeepZoom
{
  public partial class Page : UserControl
  {
    public Page()
    {
      InitializeComponent();
      ScrollHelper scroller = new ScrollHelper();
      scroller.ScrollChanged += new EventHandler<ScrollEventArgs>(scroller_ScrollChanged);
    }

    void scroller_ScrollChanged(object sender, ScrollEventArgs e)
    {
      Point logicalPoint = deepZoom.ElementToLogicalPoint(new Point(e.X, e.Y));
      if (e.ScrollDelta < 0)
      {
        deepZoom.ZoomAboutLogicalPoint(.66, logicalPoint.X, logicalPoint.Y);
      }
      else
      {
        deepZoom.ZoomAboutLogicalPoint(1.33, logicalPoint.X, logicalPoint.Y);
      }
    }
  }
}
