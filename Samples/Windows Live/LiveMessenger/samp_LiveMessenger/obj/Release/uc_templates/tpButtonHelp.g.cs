#pragma checksum "C:\JonathanMoore\TODO\Silverlight\LiveMessenger\LiveMessenger\samp_LiveMessenger\uc_templates\tpButtonHelp.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B6244B9489EC7EB5D0CCC38250B1044E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
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


namespace samp_LiveMessenger {
    
    
    public partial class tpButtonHelp : System.Windows.Controls.UserControl {
        
        internal System.Windows.Media.Animation.Storyboard Hover;
        
        internal System.Windows.Media.Animation.Storyboard Normal;
        
        internal System.Windows.Media.Animation.Storyboard Selected;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Shapes.Rectangle FocusVisualElement;
        
        internal System.Windows.Shapes.Rectangle ws;
        
        internal System.Windows.Shapes.Rectangle bord;
        
        internal System.Windows.Controls.TextBlock txt;
        
        internal System.Windows.Shapes.Path da;
        
        internal System.Windows.Shapes.Rectangle gl;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/samp_LiveMessenger;component/uc_templates/tpButtonHelp.xaml", System.UriKind.Relative));
            this.Hover = ((System.Windows.Media.Animation.Storyboard)(this.FindName("Hover")));
            this.Normal = ((System.Windows.Media.Animation.Storyboard)(this.FindName("Normal")));
            this.Selected = ((System.Windows.Media.Animation.Storyboard)(this.FindName("Selected")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.FocusVisualElement = ((System.Windows.Shapes.Rectangle)(this.FindName("FocusVisualElement")));
            this.ws = ((System.Windows.Shapes.Rectangle)(this.FindName("ws")));
            this.bord = ((System.Windows.Shapes.Rectangle)(this.FindName("bord")));
            this.txt = ((System.Windows.Controls.TextBlock)(this.FindName("txt")));
            this.da = ((System.Windows.Shapes.Path)(this.FindName("da")));
            this.gl = ((System.Windows.Shapes.Rectangle)(this.FindName("gl")));
        }
    }
}
