using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace FullscreenSamples
{
  public partial class Fullscreen2 : UserControl
  {
    private double x, y;

    public Fullscreen2()
    {
      InitializeComponent();
      Application.Current.Host.Content.FullScreenChanged += new EventHandler(Content_FullScreenChanged);
      CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
      this.Loaded += new RoutedEventHandler(Page_Loaded);
    }

    void Page_Loaded(object sender, RoutedEventArgs e)
    {
      x = Convert.ToDouble(ellipse.GetValue(Canvas.LeftProperty));
      y = Convert.ToDouble(ellipse.GetValue(Canvas.TopProperty));
    }

    void CompositionTarget_Rendering(object sender, EventArgs e)
    {
      x += .8;
      y += .8;

      ellipse.SetValue(Canvas.LeftProperty, x);
      ellipse.SetValue(Canvas.TopProperty, y);

      DebugText.Text = "X = " + x + " Y = " + y;
      DebugText.Text += "\n Width = " + this.ActualWidth + " and Height = " + this.ActualHeight;
    }

    protected void Content_FullScreenChanged(object sender, EventArgs e)
    {
      //scale content if we are in full screen.
      if (Application.Current.Host.Content.IsFullScreen)
      {
        double heightRatio = Application.Current.Host.Content.ActualHeight / this.Height;
        double widthRatio = Application.Current.Host.Content.ActualWidth / this.Width;
        ScaleTransform scale = new ScaleTransform();
        scale.ScaleX = widthRatio;
        scale.ScaleY = heightRatio;
        this.RenderTransform = scale;
      }
      else
      {
        this.RenderTransform = null;
      }
    }

    private void FullScreenButton_Click(object sender, RoutedEventArgs e)
    {
      Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen;
    }
  }
}
