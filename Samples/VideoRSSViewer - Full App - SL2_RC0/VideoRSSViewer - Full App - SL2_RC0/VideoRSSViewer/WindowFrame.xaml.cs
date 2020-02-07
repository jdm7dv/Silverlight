using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace VideoRSSViewer
{
    public partial class WindowFrame : UserControl
    {
        public WindowFrame()
        {
            InitializeComponent();
            myResizeRectangle.Cursor = Cursors.Hand;
            myResizeRectangle.MouseLeftButtonDown += new MouseButtonEventHandler(myResizeRectangle_MouseLeftButtonDown);
            myResizeRectangle.MouseLeftButtonUp += new MouseButtonEventHandler(myResizeRectangle_MouseLeftButtonUp);
            myResizeRectangle.MouseMove += new MouseEventHandler(myResizeRectangle_MouseMove);
            
            myCloseRectangle.Cursor = Cursors.Hand;
            myCloseRectangle.MouseLeftButtonUp += new MouseButtonEventHandler(myCloseRectangle_MouseLeftButtonUp);
        }

        void myCloseRectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Closed != null)
                Closed(this, new EventArgs());
        }

        public FrameworkElement ResizeRectangle
        {
            get
            {
                return myResizeRectangle;
            }
        }

        public event EventHandler<MouseEventArgs> Resized;
        public event EventHandler Closed;

        private bool _isResizing = false;
        private Point _origin = new Point(0, 0);

        void myResizeRectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UIElement s = sender as UIElement; 
            s.CaptureMouse();
            _isResizing = true;
        }

        void myResizeRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if ((Resized != null) && (_isResizing))
            {
                Resized(this, e);
            }
        }

        void myResizeRectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UIElement s = sender as UIElement;
            s.ReleaseMouseCapture();
            _isResizing = false;
        }

    }
}
