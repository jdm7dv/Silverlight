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

namespace Mix08
{
  public partial class DetailsView : UserControl
  {
    public DetailsView()
    {
      InitializeComponent();
      _media.MouseLeftButtonUp += new MouseButtonEventHandler(_media_MouseLeftButtonUp);
    }

    void _media_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.Filter = "Media Files|*.wma;*.wmv;*.mp3";
      ofd.EnableMultipleSelection = true;
      if (ofd.ShowDialog() == DialogResult.OK)
      {
        VideoSource vs = new VideoSource();
        vs.Title = "Media from local machine";
        vs.Description = "";
        this.DataContext = vs;
        _media.SetSource(ofd.SelectedFile.OpenRead());
      }
    }
  }
}
