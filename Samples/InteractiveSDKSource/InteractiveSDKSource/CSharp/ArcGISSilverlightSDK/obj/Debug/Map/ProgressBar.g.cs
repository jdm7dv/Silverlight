﻿#pragma checksum "C:\v1.9\SphinxLogic\WinX\base\samples\Web Development\Silverlight\InteractiveSDKSource\InteractiveSDKSource\CSharp\ArcGISSilverlightSDK\Map\ProgressBar.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9828666AE327A01C766B1A45D8FCD8D4"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ESRI.ArcGIS.Client;
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
    
    
    public partial class ProgressBar : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal ESRI.ArcGIS.Client.Map MyMap;
        
        internal System.Windows.Controls.Grid progressGrid;
        
        internal System.Windows.Controls.ProgressBar MyProgressBar;
        
        internal System.Windows.Controls.TextBlock ProgressValueTextBlock;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/ArcGISSilverlightSDK;component/Map/ProgressBar.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.MyMap = ((ESRI.ArcGIS.Client.Map)(this.FindName("MyMap")));
            this.progressGrid = ((System.Windows.Controls.Grid)(this.FindName("progressGrid")));
            this.MyProgressBar = ((System.Windows.Controls.ProgressBar)(this.FindName("MyProgressBar")));
            this.ProgressValueTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("ProgressValueTextBlock")));
        }
    }
}
