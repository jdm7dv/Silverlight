﻿#pragma checksum "D:\Jonathan_Moore\Career\Source\Microsoft\VERVE\app\Silverlight\Silverlight3\Source\RiaClient.Tests\System.Windows.Controls.Data.Input\Validation\TestPages\LabelTestPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D2BE3210D3583D95EC87483B7D4A4B33"
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
    
    
    public partial class LabelTestPage : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid mainGrid;
        
        internal System.Windows.Controls.Label nameLabel;
        
        internal System.Windows.Controls.TextBox nameTextBox;
        
        internal System.Windows.Controls.Label emailLabel;
        
        internal System.Windows.Controls.TextBox emailTextBox;
        
        internal System.Windows.Controls.ValidationSummary validationSummary;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Microsoft.AppFx.UnitTests.Silverlight;component/System.Windows.Controls.Data.Inp" +
                        "ut/Validation/TestPages/LabelTestPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.mainGrid = ((System.Windows.Controls.Grid)(this.FindName("mainGrid")));
            this.nameLabel = ((System.Windows.Controls.Label)(this.FindName("nameLabel")));
            this.nameTextBox = ((System.Windows.Controls.TextBox)(this.FindName("nameTextBox")));
            this.emailLabel = ((System.Windows.Controls.Label)(this.FindName("emailLabel")));
            this.emailTextBox = ((System.Windows.Controls.TextBox)(this.FindName("emailTextBox")));
            this.validationSummary = ((System.Windows.Controls.ValidationSummary)(this.FindName("validationSummary")));
        }
    }
}

