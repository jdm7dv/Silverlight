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
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace ImageGallery
{
    public partial class Photo : UserControl
    {
        private static Random random = new Random();

        private MainPage _parent;
        private bool _mouseOver;


        private string _name;
        public Photo(MainPage parent, Uri photo, string name)
        {
            InitializeComponent();

            intializePhoto(parent);

            _name = name;
            // Create a WebClient to fetch the image
            WebClient webClientDownloader = new WebClient();
            webClientDownloader.OpenReadCompleted += new OpenReadCompletedEventHandler(webClientDownloader_OpenReadCompleted);
            webClientDownloader.OpenReadAsync(photo);
        }

        public Photo(MainPage parent, BitmapImage photo, string name)
        {
            InitializeComponent();

            intializePhoto(parent);
            _name = name;
            image.Source = photo;

            // Position and rotate the photo randomly
            Translate(random.Next((int)(this._parent.ActualWidth - Width)), random.Next((int)(this._parent.ActualHeight - Height)));
            Rotate(random.Next(-30, 30));

            // Add the photo to the parent and play the display animation
            _parent.LayoutRoot.Children.Add(this);

            display.Begin();
        }


        private void intializePhoto(MainPage parent)
        {
            // Initialize
            _parent = parent;
            allControls.Opacity = 0;

            // Hook up event handlers
            MouseLeftButtonDown += new MouseButtonEventHandler(_root_MouseLeftButtonDown);
            allControls.MouseEnter += new MouseEventHandler(_allControls_MouseEnter);
            allControls.MouseLeave += new MouseEventHandler(_allControls_MouseLeave);

            translateControls.MouseLeftButtonDown += new MouseButtonEventHandler(translateControls_MouseLeftButtonDown);
            rotateScaleControls.MouseLeftButtonDown += new MouseButtonEventHandler(rotateScaleControls_MouseLeftButtonDown);
        }

        void webClientDownloader_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            // Set the graphic into the Image element
            BitmapImage bi = new BitmapImage();
            bi.SetSource(e.Result);
            image.Source = bi;

            // Position and rotate the photo randomly
            Translate(random.Next((int)(this._parent.ActualWidth - Width)), random.Next((int)(this._parent.ActualHeight - Height)));
            Rotate(random.Next(-30, 30));

            // Add the photo to the parent and play the display animation
            _parent.LayoutRoot.Children.Add(this);

            display.Begin();
        }

        void _allControls_MouseEnter(object sender, MouseEventArgs e)
        {
            _mouseOver = true;
            ShowControls();
        }

        void _allControls_MouseLeave(object sender, MouseEventArgs e)
        {
            _mouseOver = false;
            HideControls();
        }

        void _root_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HandleMouseLeftButtonDown(ActionType.Selecting, e);
        }

        void translateControls_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HandleMouseLeftButtonDown(ActionType.Moving, e);
        }

        void rotateScaleControls_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HandleMouseLeftButtonDown(ActionType.RotatingScaling, e);
        }


        void HandleMouseLeftButtonDown(ActionType actionType, MouseEventArgs e)
        {
            // Give the parent information about the new active photo
            _parent.SetActivePhoto(this, actionType, new Point(translateTransform.X + rotateTransform.CenterX, translateTransform.Y + rotateTransform.CenterY), e.GetPosition(null));
        }


        public void Translate(double deltaX, double deltaY)
        {
            translateTransform.X += deltaX;
            translateTransform.Y += deltaY;
        }

        public void Rotate(double deltaAngle)
        {
            rotateTransform.Angle += deltaAngle;
        }

        public void Scale(double deltaScale)
        {
            scaleTransform.ScaleX *= deltaScale;
            scaleTransform.ScaleY *= deltaScale;
        }

        public void ShowControls()
        {
            if (_mouseOver)
            {
                // If the mouse is (still) over the photo
                if (null == _parent.GetActivePhoto())
                {
                    // No active photo, so this photo can show its controls
                    allControls.Opacity = 0.4;
                }
                else
                {
                    // Let the parent know this photo is ready to show its controls
                    _parent.SetPendingPhoto(this);
                }
            }
        }

        public void HideControls()
        {
            if (!_mouseOver && (this != _parent.GetActivePhoto()))
            {
                // If the mouse is not over the photo and it's not the active photo, hide its controls
                allControls.Opacity = 0;
            }
        }


    }
}