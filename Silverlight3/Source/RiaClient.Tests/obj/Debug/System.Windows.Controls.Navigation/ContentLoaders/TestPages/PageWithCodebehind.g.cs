﻿#pragma checksum "D:\Jonathan_Moore\Career\Source\Microsoft\VERVE\app\Silverlight\Silverlight3\Source\RiaClient.Tests\System.Windows.Controls.Navigation\ContentLoaders\TestPages\PageWithCodebehind.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5EDA8C92AF794B38A13F00CF62FB8420"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
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


namespace System.Windows.Navigation.UnitTests {
    
    
    public partial class PageWithCodebehind : System.Windows.Controls.Page {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock _txtBlock;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Microsoft.AppFx.UnitTests.Silverlight;component/System.Windows.Controls.Navigati" +
                        "on/ContentLoaders/TestPages/PageWithCodebehind.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this._txtBlock = ((System.Windows.Controls.TextBlock)(this.FindName("_txtBlock")));
        }
    }
}

