#pragma checksum "C:\Users\mharsh\Desktop\Mix08 - Copy\Mix08\DetailsView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "21C60095B1DB25CF2DA68D66BA0ABC3D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Hosting;
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


namespace Mix08 {
    
    
    public partial class DetailsView : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.StackPanel LayoutRoot;
        
        internal System.Windows.Controls.TextBlock _title;
        
        internal System.Windows.Controls.MediaElement _media;
        
        internal System.Windows.Controls.TextBlock _description;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Mix08;component/DetailsView.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.StackPanel)(this.FindName("LayoutRoot")));
            this._title = ((System.Windows.Controls.TextBlock)(this.FindName("_title")));
            this._media = ((System.Windows.Controls.MediaElement)(this.FindName("_media")));
            this._description = ((System.Windows.Controls.TextBlock)(this.FindName("_description")));
        }
    }
}