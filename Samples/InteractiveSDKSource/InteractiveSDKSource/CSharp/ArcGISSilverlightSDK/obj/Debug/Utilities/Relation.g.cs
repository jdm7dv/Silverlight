﻿#pragma checksum "C:\v1.9\SphinxLogic\WinX\base\samples\Web Development\Silverlight\InteractiveSDKSource\InteractiveSDKSource\CSharp\ArcGISSilverlightSDK\Utilities\Relation.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FB34093E04A182925EF3A4F25167CC88"
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
using ESRI.ArcGIS.Client.Geometry;
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
    
    
    public partial class Relation : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol DefaultPointMarkerSymbol;
        
        internal ESRI.ArcGIS.Client.Symbols.SimpleFillSymbol DefaultPolygonFillSymbol;
        
        internal ESRI.ArcGIS.Client.ValueConverters.DictionaryConverter MyDictionaryConverter;
        
        internal ESRI.ArcGIS.Client.Map MyMap;
        
        internal ESRI.ArcGIS.Client.Graphic Graphic0;
        
        internal ESRI.ArcGIS.Client.Geometry.Polygon Polygon_0;
        
        internal ESRI.ArcGIS.Client.Graphic Graphic1;
        
        internal ESRI.ArcGIS.Client.Geometry.Polygon Polygon_1;
        
        internal ESRI.ArcGIS.Client.Graphic Graphic2;
        
        internal ESRI.ArcGIS.Client.Geometry.Polygon Polygon_2;
        
        internal ESRI.ArcGIS.Client.Graphic Graphic3;
        
        internal ESRI.ArcGIS.Client.Geometry.Polygon Polygon_3;
        
        internal ESRI.ArcGIS.Client.Graphic Graphic4;
        
        internal ESRI.ArcGIS.Client.Geometry.Polygon Polygon_4;
        
        internal System.Windows.Controls.TextBlock InformationText;
        
        internal System.Windows.Controls.Button ExecuteRelationButton;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/ArcGISSilverlightSDK;component/Utilities/Relation.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.DefaultPointMarkerSymbol = ((ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol)(this.FindName("DefaultPointMarkerSymbol")));
            this.DefaultPolygonFillSymbol = ((ESRI.ArcGIS.Client.Symbols.SimpleFillSymbol)(this.FindName("DefaultPolygonFillSymbol")));
            this.MyDictionaryConverter = ((ESRI.ArcGIS.Client.ValueConverters.DictionaryConverter)(this.FindName("MyDictionaryConverter")));
            this.MyMap = ((ESRI.ArcGIS.Client.Map)(this.FindName("MyMap")));
            this.Graphic0 = ((ESRI.ArcGIS.Client.Graphic)(this.FindName("Graphic0")));
            this.Polygon_0 = ((ESRI.ArcGIS.Client.Geometry.Polygon)(this.FindName("Polygon_0")));
            this.Graphic1 = ((ESRI.ArcGIS.Client.Graphic)(this.FindName("Graphic1")));
            this.Polygon_1 = ((ESRI.ArcGIS.Client.Geometry.Polygon)(this.FindName("Polygon_1")));
            this.Graphic2 = ((ESRI.ArcGIS.Client.Graphic)(this.FindName("Graphic2")));
            this.Polygon_2 = ((ESRI.ArcGIS.Client.Geometry.Polygon)(this.FindName("Polygon_2")));
            this.Graphic3 = ((ESRI.ArcGIS.Client.Graphic)(this.FindName("Graphic3")));
            this.Polygon_3 = ((ESRI.ArcGIS.Client.Geometry.Polygon)(this.FindName("Polygon_3")));
            this.Graphic4 = ((ESRI.ArcGIS.Client.Graphic)(this.FindName("Graphic4")));
            this.Polygon_4 = ((ESRI.ArcGIS.Client.Geometry.Polygon)(this.FindName("Polygon_4")));
            this.InformationText = ((System.Windows.Controls.TextBlock)(this.FindName("InformationText")));
            this.ExecuteRelationButton = ((System.Windows.Controls.Button)(this.FindName("ExecuteRelationButton")));
        }
    }
}

