﻿#pragma checksum "C:\v1.9\SphinxLogic\WinX\base\samples\Web Development\Silverlight\InteractiveSDKSource\InteractiveSDKSource\CSharp\ArcGISSilverlightSDK\Query\Identify.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2CFDB9BF18ED512102924F8BEC95CC32"
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
    
    
    public partial class Identify : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol DefaultPictureSymbol;
        
        internal ESRI.ArcGIS.Client.Map MyMap;
        
        internal System.Windows.Controls.Grid IdentifyGrid;
        
        internal System.Windows.Controls.TextBlock DataDisplayTitleTop;
        
        internal System.Windows.Controls.TextBlock DataDisplayTitleBottom;
        
        internal System.Windows.Controls.ComboBox IdentifyComboBox;
        
        internal System.Windows.Controls.ScrollViewer DataGridScrollViewer;
        
        internal System.Windows.Controls.DataGrid IdentifyDetailsDataGrid;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/ArcGISSilverlightSDK;component/Query/Identify.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.DefaultPictureSymbol = ((ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol)(this.FindName("DefaultPictureSymbol")));
            this.MyMap = ((ESRI.ArcGIS.Client.Map)(this.FindName("MyMap")));
            this.IdentifyGrid = ((System.Windows.Controls.Grid)(this.FindName("IdentifyGrid")));
            this.DataDisplayTitleTop = ((System.Windows.Controls.TextBlock)(this.FindName("DataDisplayTitleTop")));
            this.DataDisplayTitleBottom = ((System.Windows.Controls.TextBlock)(this.FindName("DataDisplayTitleBottom")));
            this.IdentifyComboBox = ((System.Windows.Controls.ComboBox)(this.FindName("IdentifyComboBox")));
            this.DataGridScrollViewer = ((System.Windows.Controls.ScrollViewer)(this.FindName("DataGridScrollViewer")));
            this.IdentifyDetailsDataGrid = ((System.Windows.Controls.DataGrid)(this.FindName("IdentifyDetailsDataGrid")));
        }
    }
}

