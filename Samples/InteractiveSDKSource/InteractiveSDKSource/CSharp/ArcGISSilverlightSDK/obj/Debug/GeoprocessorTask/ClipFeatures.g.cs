﻿#pragma checksum "C:\v1.9\SphinxLogic\WinX\base\samples\Web Development\Silverlight\InteractiveSDKSource\InteractiveSDKSource\CSharp\ArcGISSilverlightSDK\GeoprocessorTask\ClipFeatures.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "CFD471A71082417612C9705D59591B79"
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
using ESRI.ArcGIS.Client.ValueConverters;
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
    
    
    public partial class ClipFeatures : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal ESRI.ArcGIS.Client.Symbols.SimpleLineSymbol ClipLineSymbol;
        
        internal ESRI.ArcGIS.Client.Symbols.SimpleFillSymbol ClipFeaturesFillSymbol;
        
        internal ESRI.ArcGIS.Client.ValueConverters.DictionaryConverter MyDictionaryConverter;
        
        internal ESRI.ArcGIS.Client.Map MyMap;
        
        internal System.Windows.Controls.TextBlock InformationTextBlock;
        
        internal System.Windows.Controls.TextBox DistanceTextBox;
        
        internal System.Windows.Controls.Button ClearButton;
        
        internal System.Windows.Controls.TextBlock ProcessingTextBlock;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/ArcGISSilverlightSDK;component/GeoprocessorTask/ClipFeatures.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ClipLineSymbol = ((ESRI.ArcGIS.Client.Symbols.SimpleLineSymbol)(this.FindName("ClipLineSymbol")));
            this.ClipFeaturesFillSymbol = ((ESRI.ArcGIS.Client.Symbols.SimpleFillSymbol)(this.FindName("ClipFeaturesFillSymbol")));
            this.MyDictionaryConverter = ((ESRI.ArcGIS.Client.ValueConverters.DictionaryConverter)(this.FindName("MyDictionaryConverter")));
            this.MyMap = ((ESRI.ArcGIS.Client.Map)(this.FindName("MyMap")));
            this.InformationTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("InformationTextBlock")));
            this.DistanceTextBox = ((System.Windows.Controls.TextBox)(this.FindName("DistanceTextBox")));
            this.ClearButton = ((System.Windows.Controls.Button)(this.FindName("ClearButton")));
            this.ProcessingTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("ProcessingTextBlock")));
        }
    }
}

