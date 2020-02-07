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
    public partial class FullScreenButton : UserControl
    {
        public FullScreenButton()
        {
            InitializeComponent(); 
            this.MouseLeftButtonUp += new MouseButtonEventHandler(FullScreenButton_MouseLeftButtonUp);
        }

        public event MouseEventHandler Clicked;

        void FullScreenButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Clicked != null)
                Clicked(this, e);
        }

    }
}
