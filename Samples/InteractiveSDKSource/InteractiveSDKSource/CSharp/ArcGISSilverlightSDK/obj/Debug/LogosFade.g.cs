﻿#pragma checksum "C:\v1.9\SphinxLogic\WinX\base\samples\Web Development\Silverlight\InteractiveSDKSource\InteractiveSDKSource\CSharp\ArcGISSilverlightSDK\LogosFade.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "DC7B3869A844342621B79D0901983C02"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
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


namespace ArcGISSilverlightSDK {
    
    
    public partial class LogosFade : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid Logos;
        
        internal System.Windows.Media.Animation.Storyboard slStoryboard;
        
        internal System.Windows.Controls.Image slLogoImage;
        
        internal System.Windows.Controls.Image esriLogoImage;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/ArcGISSilverlightSDK;component/LogosFade.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Logos = ((System.Windows.Controls.Grid)(this.FindName("Logos")));
            this.slStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("slStoryboard")));
            this.slLogoImage = ((System.Windows.Controls.Image)(this.FindName("slLogoImage")));
            this.esriLogoImage = ((System.Windows.Controls.Image)(this.FindName("esriLogoImage")));
        }
    }
}

