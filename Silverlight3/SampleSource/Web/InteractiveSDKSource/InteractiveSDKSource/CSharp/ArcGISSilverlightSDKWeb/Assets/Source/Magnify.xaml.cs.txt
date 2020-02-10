using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ArcGISSilverlightSDK
{
    public partial class Magnify : UserControl
    {
        public Magnify()
        {
            InitializeComponent();

            MyMagnifier.Initialize(MyMap);
        }

        private void MyMagnifyImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MyMagnifier.Enabled = !MyMagnifier.Enabled;
        }
    }
}
