﻿#pragma checksum "D:\Projects\Silverlight\v4\CountDown\SilverlightApplication15\FlipClock.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "40C0D666BE19857643E2B5ACF8ECEB67"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ReflectionIT.Silverlight.CountDown;
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


namespace ReflectionIT.Silverlight.CountDown {
    
    
    public partial class FlipClock : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal ReflectionIT.Silverlight.CountDown.Flipper FlipDays;
        
        internal ReflectionIT.Silverlight.CountDown.Flipper FlipHours;
        
        internal ReflectionIT.Silverlight.CountDown.Flipper FlipSeconds;
        
        internal ReflectionIT.Silverlight.CountDown.Flipper FlipMinutes;
        
        internal System.Windows.Controls.TextBlock textBlockTitle;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/ReflectionIT.Silverlight.CountDown;component/FlipClock.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.FlipDays = ((ReflectionIT.Silverlight.CountDown.Flipper)(this.FindName("FlipDays")));
            this.FlipHours = ((ReflectionIT.Silverlight.CountDown.Flipper)(this.FindName("FlipHours")));
            this.FlipSeconds = ((ReflectionIT.Silverlight.CountDown.Flipper)(this.FindName("FlipSeconds")));
            this.FlipMinutes = ((ReflectionIT.Silverlight.CountDown.Flipper)(this.FindName("FlipMinutes")));
            this.textBlockTitle = ((System.Windows.Controls.TextBlock)(this.FindName("textBlockTitle")));
        }
    }
}
