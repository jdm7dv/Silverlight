﻿#pragma checksum "C:\Silverlight4\Labs\RichTextBox\Source\Ex03\End\RichTextBox\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "57F51357D03F9C533B3E3DCFF12C26A0"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30128.1
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


namespace RichNotepad {
    
    
    public partial class MainPage : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.VisualStateGroup DragStates;
        
        internal System.Windows.VisualState Normal;
        
        internal System.Windows.VisualState DragOver;
        
        internal System.Windows.Controls.RichTextBox rtb;
        
        internal System.Windows.Controls.Canvas highlightCanvas;
        
        internal System.Windows.Controls.Image caretHand;
        
        internal System.Windows.Controls.TextBox xamlTb;
        
        internal System.Windows.Shapes.Rectangle mainPanel;
        
        internal System.Windows.Controls.ComboBox cmbFonts;
        
        internal System.Windows.Controls.ComboBox cmbFontSizes;
        
        internal System.Windows.Controls.Button btnBold;
        
        internal System.Windows.Controls.Button btnItalic;
        
        internal System.Windows.Controls.Button btnUnderline;
        
        internal System.Windows.Controls.ComboBox cmbFontColors;
        
        internal System.Windows.Controls.Button btnPaste;
        
        internal System.Windows.Controls.Button btnCut;
        
        internal System.Windows.Controls.Button btnCopy;
        
        internal System.Windows.Controls.Button btnImage;
        
        internal System.Windows.Controls.Button btnHyperlink;
        
        internal System.Windows.Controls.Button btnDatagrid;
        
        internal System.Windows.Controls.Button btnCalendar;
        
        internal System.Windows.Controls.Button btnPrint;
        
        internal System.Windows.Controls.Primitives.ToggleButton btnRTL;
        
        internal System.Windows.Controls.Primitives.ToggleButton btnHighlight;
        
        internal System.Windows.Controls.Primitives.ToggleButton btnMarkUp;
        
        internal System.Windows.Controls.Button btnNew;
        
        internal System.Windows.Controls.Button btnOpen;
        
        internal System.Windows.Controls.Button btnSave;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/RichNotepad;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.DragStates = ((System.Windows.VisualStateGroup)(this.FindName("DragStates")));
            this.Normal = ((System.Windows.VisualState)(this.FindName("Normal")));
            this.DragOver = ((System.Windows.VisualState)(this.FindName("DragOver")));
            this.rtb = ((System.Windows.Controls.RichTextBox)(this.FindName("rtb")));
            this.highlightCanvas = ((System.Windows.Controls.Canvas)(this.FindName("highlightCanvas")));
            this.caretHand = ((System.Windows.Controls.Image)(this.FindName("caretHand")));
            this.xamlTb = ((System.Windows.Controls.TextBox)(this.FindName("xamlTb")));
            this.mainPanel = ((System.Windows.Shapes.Rectangle)(this.FindName("mainPanel")));
            this.cmbFonts = ((System.Windows.Controls.ComboBox)(this.FindName("cmbFonts")));
            this.cmbFontSizes = ((System.Windows.Controls.ComboBox)(this.FindName("cmbFontSizes")));
            this.btnBold = ((System.Windows.Controls.Button)(this.FindName("btnBold")));
            this.btnItalic = ((System.Windows.Controls.Button)(this.FindName("btnItalic")));
            this.btnUnderline = ((System.Windows.Controls.Button)(this.FindName("btnUnderline")));
            this.cmbFontColors = ((System.Windows.Controls.ComboBox)(this.FindName("cmbFontColors")));
            this.btnPaste = ((System.Windows.Controls.Button)(this.FindName("btnPaste")));
            this.btnCut = ((System.Windows.Controls.Button)(this.FindName("btnCut")));
            this.btnCopy = ((System.Windows.Controls.Button)(this.FindName("btnCopy")));
            this.btnImage = ((System.Windows.Controls.Button)(this.FindName("btnImage")));
            this.btnHyperlink = ((System.Windows.Controls.Button)(this.FindName("btnHyperlink")));
            this.btnDatagrid = ((System.Windows.Controls.Button)(this.FindName("btnDatagrid")));
            this.btnCalendar = ((System.Windows.Controls.Button)(this.FindName("btnCalendar")));
            this.btnPrint = ((System.Windows.Controls.Button)(this.FindName("btnPrint")));
            this.btnRTL = ((System.Windows.Controls.Primitives.ToggleButton)(this.FindName("btnRTL")));
            this.btnHighlight = ((System.Windows.Controls.Primitives.ToggleButton)(this.FindName("btnHighlight")));
            this.btnMarkUp = ((System.Windows.Controls.Primitives.ToggleButton)(this.FindName("btnMarkUp")));
            this.btnNew = ((System.Windows.Controls.Button)(this.FindName("btnNew")));
            this.btnOpen = ((System.Windows.Controls.Button)(this.FindName("btnOpen")));
            this.btnSave = ((System.Windows.Controls.Button)(this.FindName("btnSave")));
        }
    }
}

