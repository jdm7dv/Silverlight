#pragma checksum "C:\Users\Jonathan Moore\v1.8\TODO\Silverlight\VideoRSSViewer - Full App - SL2_RC0\VideoRSSViewer - Full App - SL2_RC0\VideoRSSViewer\WindowFrame.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7E1E58621CD5AD60B47ACD5C86B12761"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3074
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


namespace VideoRSSViewer {
    
    
    public partial class WindowFrame : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Shapes.Rectangle myCloseRectangle;
        
        internal System.Windows.Shapes.Rectangle myResizeRectangle;
        
        internal System.Windows.Shapes.Rectangle myBorder;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/VideoRSSViewer;component/WindowFrame.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.myCloseRectangle = ((System.Windows.Shapes.Rectangle)(this.FindName("myCloseRectangle")));
            this.myResizeRectangle = ((System.Windows.Shapes.Rectangle)(this.FindName("myResizeRectangle")));
            this.myBorder = ((System.Windows.Shapes.Rectangle)(this.FindName("myBorder")));
        }
    }
}
