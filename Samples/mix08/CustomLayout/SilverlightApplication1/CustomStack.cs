using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverlightApplication1
{
  public class CustomStack : Panel
  {
    public CustomStack() { }

    protected override Size MeasureOverride(Size availableSize)
    {
      foreach (UIElement e in this.Children)
      {
        e.Measure(new Size(this.DesiredSize.Width, Double.NaN));
      }
      return availableSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      double top = 0;
      foreach (UIElement e in this.Children)
      {
        e.Arrange(new Rect(0, top, this.DesiredSize.Width, e.DesiredSize.Height));
        top += e.DesiredSize.Height;
      }
      return finalSize;
    }
  }
}
