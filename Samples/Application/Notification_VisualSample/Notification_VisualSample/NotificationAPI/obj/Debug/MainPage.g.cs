﻿#pragma checksum "C:\Users\CorrinaB\Desktop\NotificationVisual\NotificationAPI\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3DE4B94644C3E856A7C2CDED1E6FFC39"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.21006.1
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


namespace NotificationAPI {
    
    
    public partial class MainPage : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel ControlSamples;
        
        internal System.Windows.Controls.TextBlock WarningText;
        
        internal System.Windows.Controls.Button InstallButton;
        
        internal System.Windows.Controls.Button CustomNotificationButton;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/NotificationAPI;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ControlSamples = ((System.Windows.Controls.StackPanel)(this.FindName("ControlSamples")));
            this.WarningText = ((System.Windows.Controls.TextBlock)(this.FindName("WarningText")));
            this.InstallButton = ((System.Windows.Controls.Button)(this.FindName("InstallButton")));
            this.CustomNotificationButton = ((System.Windows.Controls.Button)(this.FindName("CustomNotificationButton")));
        }
    }
}
