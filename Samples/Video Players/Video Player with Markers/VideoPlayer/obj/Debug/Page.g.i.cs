﻿#pragma checksum "G:\base\Shared\Windows\Program Files\SDK\Samples\Web Development\Silverlight\Video Players\Video Player with Markers\VideoPlayer\Page.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "337FB59F0A7FD3D1DD14DC1588AA7D57"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
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
using VideoPlayer;


namespace VideoPlayer {
    
    
    public partial class Page : System.Windows.Controls.UserControl {
        
        internal System.Windows.Media.Animation.Storyboard controlsIn;
        
        internal System.Windows.Media.Animation.Storyboard controlsOut;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Canvas PlayIcon;
        
        internal System.Windows.Controls.Canvas LargeSpinnerArea;
        
        internal VideoPlayer.spinner BigBuffer;
        
        internal System.Windows.Controls.Canvas Thumbnail;
        
        internal System.Windows.Controls.Image ThumbnailImage;
        
        internal System.Windows.Controls.StackPanel LayoutRoot2;
        
        internal System.Windows.Controls.MediaElement mediaPlayer;
        
        internal System.Windows.Controls.Grid controlsContainer;
        
        internal VideoPlayer.mediaControl mediaControls;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/VideoPlayer;component/Page.xaml", System.UriKind.Relative));
            this.controlsIn = ((System.Windows.Media.Animation.Storyboard)(this.FindName("controlsIn")));
            this.controlsOut = ((System.Windows.Media.Animation.Storyboard)(this.FindName("controlsOut")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.PlayIcon = ((System.Windows.Controls.Canvas)(this.FindName("PlayIcon")));
            this.LargeSpinnerArea = ((System.Windows.Controls.Canvas)(this.FindName("LargeSpinnerArea")));
            this.BigBuffer = ((VideoPlayer.spinner)(this.FindName("BigBuffer")));
            this.Thumbnail = ((System.Windows.Controls.Canvas)(this.FindName("Thumbnail")));
            this.ThumbnailImage = ((System.Windows.Controls.Image)(this.FindName("ThumbnailImage")));
            this.LayoutRoot2 = ((System.Windows.Controls.StackPanel)(this.FindName("LayoutRoot2")));
            this.mediaPlayer = ((System.Windows.Controls.MediaElement)(this.FindName("mediaPlayer")));
            this.controlsContainer = ((System.Windows.Controls.Grid)(this.FindName("controlsContainer")));
            this.mediaControls = ((VideoPlayer.mediaControl)(this.FindName("mediaControls")));
        }
    }
}

