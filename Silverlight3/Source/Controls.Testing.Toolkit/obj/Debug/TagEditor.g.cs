﻿#pragma checksum "D:\Jonathan_Moore\Career\Source\Microsoft\VERVE\app\Silverlight\Silverlight3\Source\Controls.Testing.Toolkit\TagEditor.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A8D531CF961AFF93B9F2C6267CF540EE"
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


namespace System.Windows.Controls.Testing.Toolkit {
    
    
    public partial class TagEditor : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid Root;
        
        internal System.Windows.Controls.StackPanel pnlEditor;
        
        internal System.Windows.Controls.StackPanel pnlAutoRun;
        
        internal System.Windows.Controls.TextBlock txtTime;
        
        internal System.Windows.Controls.TextBox txtTag;
        
        internal System.Windows.Controls.StackPanel pnlHistory;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/System.Windows.Controls.Testing.Toolkit;component/TagEditor.xaml", System.UriKind.Relative));
            this.Root = ((System.Windows.Controls.Grid)(this.FindName("Root")));
            this.pnlEditor = ((System.Windows.Controls.StackPanel)(this.FindName("pnlEditor")));
            this.pnlAutoRun = ((System.Windows.Controls.StackPanel)(this.FindName("pnlAutoRun")));
            this.txtTime = ((System.Windows.Controls.TextBlock)(this.FindName("txtTime")));
            this.txtTag = ((System.Windows.Controls.TextBox)(this.FindName("txtTag")));
            this.pnlHistory = ((System.Windows.Controls.StackPanel)(this.FindName("pnlHistory")));
        }
    }
}

