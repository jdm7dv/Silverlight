﻿#pragma checksum "D:\Jonathan_Moore\Career\Source\Microsoft\VERVE\app\Silverlight\Silverlight3\Source\RiaClient.Toolkit.Tests\DataForm\TestApps\DataFormApp_TemplatesWithBinding.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "EADE141FFDAC0AA08E33D8DF1B7A54BE"
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


namespace System.Windows.Controls.UnitTests {
    
    
    public partial class DataFormApp_TemplatesWithBinding : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.DataForm dataForm;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Silverlight.Toolkit;component/DataForm/TestApps/DataFormApp_TemplatesWithBinding" +
                        ".xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.dataForm = ((System.Windows.Controls.DataForm)(this.FindName("dataForm")));
        }
    }
}

