﻿#pragma checksum "C:\v1.9\SphinxLogic\WinX\base\samples\Web Development\Silverlight\InteractiveSDKSource\InteractiveSDKSource\CSharp\ArcGISSilverlightSDK\Query\BufferQuery.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5241DB5096177F8C4C63A66BBD106253"
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
using ESRI.ArcGIS.Client.Symbols;
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
    
    
    public partial class BufferQuery : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol DefaultMarkerSymbol;
        
        internal ESRI.ArcGIS.Client.Symbols.SimpleFillSymbol BufferSymbol;
        
        internal ESRI.ArcGIS.Client.Symbols.SimpleFillSymbol ParcelSymbol;
        
        internal ESRI.ArcGIS.Client.Map MyMap;
        
        internal System.Windows.Controls.TextBlock InformationTextBlock;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/ArcGISSilverlightSDK;component/Query/BufferQuery.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.DefaultMarkerSymbol = ((ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol)(this.FindName("DefaultMarkerSymbol")));
            this.BufferSymbol = ((ESRI.ArcGIS.Client.Symbols.SimpleFillSymbol)(this.FindName("BufferSymbol")));
            this.ParcelSymbol = ((ESRI.ArcGIS.Client.Symbols.SimpleFillSymbol)(this.FindName("ParcelSymbol")));
            this.MyMap = ((ESRI.ArcGIS.Client.Map)(this.FindName("MyMap")));
            this.InformationTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("InformationTextBlock")));
        }
    }
}

