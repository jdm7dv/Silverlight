﻿#pragma checksum "C:\v1.9\SphinxLogic\WinX\base\samples\Web Development\Silverlight\InteractiveSDKSource\InteractiveSDKSource\CSharp\ArcGISSilverlightSDK\LocatorTask\AddressToLocation.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "83DF033BAFFCCBF8AB2800C1C4B802E4"
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
    
    
    public partial class AddressToLocation : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol DefaultMarkerSymbol;
        
        internal ESRI.ArcGIS.Client.ValueConverters.DictionaryConverter MyDictionaryConverter;
        
        internal ESRI.ArcGIS.Client.Map MyMap;
        
        internal System.Windows.Controls.TextBox Address;
        
        internal System.Windows.Controls.TextBox City;
        
        internal System.Windows.Controls.TextBox State;
        
        internal System.Windows.Controls.TextBox Zip;
        
        internal System.Windows.Controls.Button FindAddressButton;
        
        internal System.Windows.Controls.Grid CandidatePanelGrid;
        
        internal System.Windows.Controls.ScrollViewer CandidateScrollViewer;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/ArcGISSilverlightSDK;component/LocatorTask/AddressToLocation.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.DefaultMarkerSymbol = ((ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol)(this.FindName("DefaultMarkerSymbol")));
            this.MyDictionaryConverter = ((ESRI.ArcGIS.Client.ValueConverters.DictionaryConverter)(this.FindName("MyDictionaryConverter")));
            this.MyMap = ((ESRI.ArcGIS.Client.Map)(this.FindName("MyMap")));
            this.Address = ((System.Windows.Controls.TextBox)(this.FindName("Address")));
            this.City = ((System.Windows.Controls.TextBox)(this.FindName("City")));
            this.State = ((System.Windows.Controls.TextBox)(this.FindName("State")));
            this.Zip = ((System.Windows.Controls.TextBox)(this.FindName("Zip")));
            this.FindAddressButton = ((System.Windows.Controls.Button)(this.FindName("FindAddressButton")));
            this.CandidatePanelGrid = ((System.Windows.Controls.Grid)(this.FindName("CandidatePanelGrid")));
            this.CandidateScrollViewer = ((System.Windows.Controls.ScrollViewer)(this.FindName("CandidateScrollViewer")));
        }
    }
}

