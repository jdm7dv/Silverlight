using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Mix08
{
  public class VideoSource
  {
    #region Properties
    public string             Title {get; set;}
    public string             Description {get; set;}
    public string             VideoUrl { get; set; }    // 432x320
    public string             ImageUrl {get; set;}      // 136x102
    public string             LargeImageUrl {get; set;} // 400x300
    public string             Source {get; set;}
    public string             ViewCount {get; set; }
    public string             PublishDate {get; set;}
    public string             ShortDescription { get { return ((Description.Length>163)?Description.Substring(0,160)+"...":Description); }}
    public string             ShortTitle { get { return ((Title.Length > 47) ? Title.Substring(0, 44) + "..." : Title); } }
    #endregion
  }
}
