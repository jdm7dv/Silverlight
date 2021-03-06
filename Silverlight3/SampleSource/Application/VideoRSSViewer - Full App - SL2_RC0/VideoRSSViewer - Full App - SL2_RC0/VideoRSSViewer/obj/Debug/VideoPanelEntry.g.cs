#pragma checksum "C:\Users\Jonathan Moore\v1.8\TODO\Silverlight\VideoRSSViewer - Full App - SL2_RC0\VideoRSSViewer - Full App - SL2_RC0\VideoRSSViewer\VideoPanelEntry.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3210F0EAB7104A91280AE1434CD88E78"
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
    
    
    public partial class VideoPanelEntry : System.Windows.Controls.UserControl {
        
        internal System.Windows.Media.ScaleTransform rootScaleTransform;
        
        internal System.Windows.Controls.Canvas vi;
        
        internal System.Windows.Media.Animation.Storyboard MouseEnterStoryboard;
        
        internal System.Windows.Media.Animation.Storyboard MouseLeaveStoryboard;
        
        internal System.Windows.Media.Animation.Storyboard HorizontalScrollStoryboard;
        
        internal System.Windows.Media.Animation.SplineDoubleKeyFrame HorizontalScrollKeyFrame;
        
        internal System.Windows.Media.Animation.Storyboard VerticalScrollStoryboard;
        
        internal System.Windows.Media.Animation.SplineDoubleKeyFrame VerticalScrollKeyFrame;
        
        internal System.Windows.Media.Animation.Storyboard MouseDownStoryboard;
        
        internal System.Windows.Media.Animation.Storyboard MouseUpStoryboard;
        
        internal System.Windows.Media.Animation.Storyboard ShowDragAndDropButtonStoryboard;
        
        internal System.Windows.Media.Animation.Storyboard HideDragAndDropButtonStoryboard;
        
        internal System.Windows.Media.Animation.Storyboard AddDragAndDropButtonFaceStoryboard;
        
        internal System.Windows.Media.Animation.Storyboard MoveDragAndDropButtonFaceStoryboard;
        
        internal System.Windows.Controls.Image entryImage;
        
        internal System.Windows.Shapes.Rectangle rectangle;
        
        internal System.Windows.Controls.TextBlock titleTextShadow;
        
        internal System.Windows.Controls.TextBlock titleText;
        
        internal System.Windows.Shapes.Rectangle backgroundRect_Copy;
        
        internal System.Windows.Controls.Canvas dragAndDropButton;
        
        internal System.Windows.Shapes.Path addButtonFace;
        
        internal System.Windows.Shapes.Path moveButtonFace;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/VideoRSSViewer;component/VideoPanelEntry.xaml", System.UriKind.Relative));
            this.rootScaleTransform = ((System.Windows.Media.ScaleTransform)(this.FindName("rootScaleTransform")));
            this.vi = ((System.Windows.Controls.Canvas)(this.FindName("vi")));
            this.MouseEnterStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("MouseEnterStoryboard")));
            this.MouseLeaveStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("MouseLeaveStoryboard")));
            this.HorizontalScrollStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("HorizontalScrollStoryboard")));
            this.HorizontalScrollKeyFrame = ((System.Windows.Media.Animation.SplineDoubleKeyFrame)(this.FindName("HorizontalScrollKeyFrame")));
            this.VerticalScrollStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("VerticalScrollStoryboard")));
            this.VerticalScrollKeyFrame = ((System.Windows.Media.Animation.SplineDoubleKeyFrame)(this.FindName("VerticalScrollKeyFrame")));
            this.MouseDownStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("MouseDownStoryboard")));
            this.MouseUpStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("MouseUpStoryboard")));
            this.ShowDragAndDropButtonStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("ShowDragAndDropButtonStoryboard")));
            this.HideDragAndDropButtonStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("HideDragAndDropButtonStoryboard")));
            this.AddDragAndDropButtonFaceStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("AddDragAndDropButtonFaceStoryboard")));
            this.MoveDragAndDropButtonFaceStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("MoveDragAndDropButtonFaceStoryboard")));
            this.entryImage = ((System.Windows.Controls.Image)(this.FindName("entryImage")));
            this.rectangle = ((System.Windows.Shapes.Rectangle)(this.FindName("rectangle")));
            this.titleTextShadow = ((System.Windows.Controls.TextBlock)(this.FindName("titleTextShadow")));
            this.titleText = ((System.Windows.Controls.TextBlock)(this.FindName("titleText")));
            this.backgroundRect_Copy = ((System.Windows.Shapes.Rectangle)(this.FindName("backgroundRect_Copy")));
            this.dragAndDropButton = ((System.Windows.Controls.Canvas)(this.FindName("dragAndDropButton")));
            this.addButtonFace = ((System.Windows.Shapes.Path)(this.FindName("addButtonFace")));
            this.moveButtonFace = ((System.Windows.Shapes.Path)(this.FindName("moveButtonFace")));
        }
    }
}
