// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

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

namespace CustomChrome
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void topLeft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.Current.MainWindow.DragResize(WindowResizeEdge.TopLeft);
        }

        private void top_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.Current.MainWindow.DragResize(WindowResizeEdge.Top);
        }

        private void topRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.Current.MainWindow.DragResize(WindowResizeEdge.TopRight);
        }

        private void left_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.Current.MainWindow.DragResize(WindowResizeEdge.Left);
        }

        private void right_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.Current.MainWindow.DragResize(WindowResizeEdge.Right);
        }

        private void bottomLeft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.Current.MainWindow.DragResize(WindowResizeEdge.BottomLeft);
        }

        private void bottom_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.Current.MainWindow.DragResize(WindowResizeEdge.Bottom);
        }

        private void bottomRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.Current.MainWindow.DragResize(WindowResizeEdge.BottomRight);
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.Close();
        }

        private void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.Current.MainWindow.DragMove();
        }
    }
}