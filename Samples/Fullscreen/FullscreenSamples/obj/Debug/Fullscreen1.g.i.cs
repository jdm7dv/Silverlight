﻿#pragma checksum "D:\SDK\Code Library\Web Development\Silverlight\Fullscreen\FullscreenSamples\Fullscreen1.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "23E239641347B1DC86B866012B04D021"
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


namespace FullscreenSamples {
    
    
    public partial class Fullscreen1 : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Canvas LayoutRoot;
        
        internal System.Windows.Media.ScaleTransform layoutScale;
        
        internal System.Windows.Media.TranslateTransform layoutTranslate;
        
        internal System.Windows.Shapes.Ellipse ellipse;
        
        internal System.Windows.Controls.TextBlock DebugText;
        
        internal System.Windows.Controls.Button FullScreenButton;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/FullscreenSamples;component/Fullscreen1.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Canvas)(this.FindName("LayoutRoot")));
            this.layoutScale = ((System.Windows.Media.ScaleTransform)(this.FindName("layoutScale")));
            this.layoutTranslate = ((System.Windows.Media.TranslateTransform)(this.FindName("layoutTranslate")));
            this.ellipse = ((System.Windows.Shapes.Ellipse)(this.FindName("ellipse")));
            this.DebugText = ((System.Windows.Controls.TextBlock)(this.FindName("DebugText")));
            this.FullScreenButton = ((System.Windows.Controls.Button)(this.FindName("FullScreenButton")));
        }
    }
}

