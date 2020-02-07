#pragma checksum "C:\Users\Jonathan Moore\v1.8\TODO\Silverlight\VideoRSSViewer - Full App - SL2_RC0\VideoRSSViewer - Full App - SL2_RC0\VideoRSSViewer\VideoPlayer.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "378309DB711A9B46691F4F9CD6B819CE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3074
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using VideoRSSViewer;


namespace VideoRSSViewer {
    
    
    public partial class VideoPlayer : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Shapes.Rectangle backgroundRectangle;
        
        internal System.Windows.Controls.MediaElement myME;
        
        internal VideoRSSViewer.PlayPauseButton myPlayPauseButton;
        
        internal VideoRSSViewer.ProgressBar myProgressBar;
        
        internal VideoRSSViewer.FullScreenButton myFullScreenButton;
        
        internal VideoRSSViewer.WindowFrame myWindowFrame;
        
        internal System.Windows.Shapes.Ellipse bufferingEllipse;
        
        internal System.Windows.Media.Animation.Storyboard bufferingEllipseAnimation;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/VideoRSSViewer;component/VideoPlayer.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.backgroundRectangle = ((System.Windows.Shapes.Rectangle)(this.FindName("backgroundRectangle")));
            this.myME = ((System.Windows.Controls.MediaElement)(this.FindName("myME")));
            this.myPlayPauseButton = ((VideoRSSViewer.PlayPauseButton)(this.FindName("myPlayPauseButton")));
            this.myProgressBar = ((VideoRSSViewer.ProgressBar)(this.FindName("myProgressBar")));
            this.myFullScreenButton = ((VideoRSSViewer.FullScreenButton)(this.FindName("myFullScreenButton")));
            this.myWindowFrame = ((VideoRSSViewer.WindowFrame)(this.FindName("myWindowFrame")));
            this.bufferingEllipse = ((System.Windows.Shapes.Ellipse)(this.FindName("bufferingEllipse")));
            this.bufferingEllipseAnimation = ((System.Windows.Media.Animation.Storyboard)(this.FindName("bufferingEllipseAnimation")));
        }
    }
}
