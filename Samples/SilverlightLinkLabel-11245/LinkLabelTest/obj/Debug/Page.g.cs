﻿#pragma checksum "C:\Users\Jonathan Moore\Desktop\SilverlightLinkLabel-11245\LinkLabelTest\Page.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B95576DC023CB87FCB4DA8E4A01DD749"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.21006.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using CompletIT.Windows.Controls;
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


namespace LinkLabelTest {
    
    
    public partial class Page : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.StackPanel LayoutRoot;
        
        internal System.Windows.Controls.TextBox TextToProccess;
        
        internal System.Windows.Controls.Button ProccessText;
        
        internal CompletIT.Windows.Controls.LinkLabel MyLinkLabel;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/LinkLabelTest;component/Page.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.StackPanel)(this.FindName("LayoutRoot")));
            this.TextToProccess = ((System.Windows.Controls.TextBox)(this.FindName("TextToProccess")));
            this.ProccessText = ((System.Windows.Controls.Button)(this.FindName("ProccessText")));
            this.MyLinkLabel = ((CompletIT.Windows.Controls.LinkLabel)(this.FindName("MyLinkLabel")));
        }
    }
}
