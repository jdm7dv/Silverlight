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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Printing;
using ContextMenu;

namespace ImageGallery
{
    internal enum ActionType { Selecting, Moving, RotatingScaling, Touching };

    public partial class MainPage : UserControl
    {
        ContextMenu.ContextMenu contextMenu;
        private Photo _activePhoto;
        private Photo _pendingPhoto;
        private Photo _lastActivePhoto;
        private ActionType _actionType;
        private Point _photoCenter;
        private Point _lastPosition;
        private bool _photosLoaded;

        public MainPage()
        {
            InitializeComponent();

            this.SizeChanged += new SizeChangedEventHandler(MainPage_SizeChanged);

            MouseLeftButtonUp += new MouseButtonEventHandler(Page_MouseLeftButtonUp);
            MouseLeave += new MouseEventHandler(Page_MouseLeave);
            MouseMove += new MouseEventHandler(Page_MouseMove);

            this.DataContext = this;
        }

        void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!_photosLoaded && (0 != Width) && (0 != Height))
            {

                // If photos haven't been loaded yet and the control is initialized, load them now
                foreach (var file in new string[] { "img6.jpg", "img7.jpg", "img9.jpg", "img10.jpg", "img18.jpg", "img19.jpg", "img21.jpg" })
                {
                    string uri = string.Format("{0}/../../{1}", Application.Current.Host.Source, file);

                    new Photo(this, new Uri(uri), file);
                }

                _photosLoaded = true;
            }
        }

        void Page_MouseLeave(object sender, MouseEventArgs e)
        {
            OnMouseLeftButtonUpOrLeave();
        }

        void Page_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OnMouseLeftButtonUpOrLeave();
        }

        void OnMouseLeftButtonUpOrLeave()
        {
            if (null != _activePhoto)
            {
                // Tell the previously active photo to hide its controls (in case the mouse already left it)
                var previouslyActivePhoto = _activePhoto;
                _lastActivePhoto = _activePhoto;
                _activePhoto = null;
                previouslyActivePhoto.HideControls();
            }
            if (null != _pendingPhoto)
            {
                // Tell the pending photo (if any) to show its controls
                _pendingPhoto.ShowControls();
                _pendingPhoto = null;
            }
        }

        void Page_MouseMove(object sender, MouseEventArgs e)
        {
            if (null != _activePhoto)
            {
                // Perform the appropriate transform on the active photo
                var position = e.GetPosition(null);
                switch (_actionType)
                {
                    case ActionType.Moving:
                        // Move it by the amount of the mouse move
                        _activePhoto.Translate(position.X - _lastPosition.X, position.Y - _lastPosition.Y);
                        break;
                    case ActionType.RotatingScaling:
                        // Rotate it according to the angle the mouse moved around the photo's center
                        var radiansToDegrees = 360 / (2 * Math.PI);
                        var lastAngle = Math.Atan2(_lastPosition.Y - _photoCenter.Y, _lastPosition.X - _photoCenter.X) * radiansToDegrees;
                        var currentAngle = Math.Atan2(position.Y - _photoCenter.Y, position.X - _photoCenter.X) * radiansToDegrees;
                        _activePhoto.Rotate(currentAngle - lastAngle);

                        // Scale it according to the distance the mouse moved relative to the photo's center
                        var lastLength = Math.Sqrt(Math.Pow(_lastPosition.Y - _photoCenter.Y, 2) + Math.Pow(_lastPosition.X - _photoCenter.X, 2));
                        var currentLength = Math.Sqrt(Math.Pow(position.Y - _photoCenter.Y, 2) + Math.Pow(position.X - _photoCenter.X, 2));
                        _activePhoto.Scale(currentLength / lastLength);
                        break;
                }
                _lastPosition = position;
            }
        }

        internal void SetActivePhoto(Photo photo, ActionType actionType, Point photoCenter, Point lastPosition)
        {
            if (null == _activePhoto || actionType == ActionType.Touching)
            {
                // Set the active photo and note the relevant details
                _activePhoto = photo;
                _actionType = actionType;
                _photoCenter = photoCenter;
                _lastPosition = lastPosition;

                // Bring the active photo to the top
                LayoutRoot.Children.Remove(_activePhoto);
                LayoutRoot.Children.Add(photo);
            }
        }

        internal Photo GetActivePhoto()
        {
            return _activePhoto;
        }

        internal void SetPendingPhoto(Photo photo)
        {
            _pendingPhoto = photo;
        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            IDataObject theDo = e.Data;

            if (theDo.GetDataPresent("FileDrop"))
            {
                FileInfo[] files = theDo.GetData("FileDrop") as FileInfo[];
                foreach (var file in files)
                {
                    if (file.Name.ToLower().Contains(".jpg") ||
                        file.Name.ToLower().Contains(".png"))
                    {
                        FileStream reader = file.OpenRead();

                        byte[] array = new byte[reader.Length];
                        int count = reader.Read(array, 0, (int)reader.Length);
                        MemoryStream str = new MemoryStream(array);
                        BitmapImage img = new BitmapImage();
                        img.SetSource(str);

                        //Initialize new instance of Photo with received image
                        new Photo(this, img, file.Name);
                    }
                }
            }
        }

        private void LayoutRoot_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point pos = e.GetPosition(null);

            if (null != contextMenu)
                LayoutRoot.Children.Remove(contextMenu);

            GenerateContextMenu(pos);
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (null != contextMenu)
                LayoutRoot.Children.Remove(contextMenu);
        }
        private void GenerateContextMenu(Point pos)
        {
            contextMenu = new ContextMenu.ContextMenu();
            contextMenu.SetValue(Canvas.LeftProperty, pos.X);
            contextMenu.SetValue(Canvas.TopProperty, pos.Y);
            contextMenu.Visibility = Visibility.Visible;

            MenuItem menuItem1 = new MenuItem();
            menuItem1.ItemContent = "Delete";
            menuItem1.MenuItemImage = new BitmapImage(new Uri("Images/Delete.png", UriKind.RelativeOrAbsolute));
            menuItem1.Click += DeleteMenuItem_Click;
            if (null == _lastActivePhoto)
                menuItem1.IsEnabled = false;
            contextMenu.Items.Add(menuItem1);

            MenuItem menuItem2 = new MenuItem();
            menuItem2.ItemContent = "Print";
            menuItem2.MenuItemImage = new BitmapImage(new Uri("Images/Print.png", UriKind.RelativeOrAbsolute));
            menuItem2.Click += PrintMenuItem_Click;
            contextMenu.Items.Add(menuItem2);

            LayoutRoot.Children.Add(contextMenu);
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LayoutRoot.Children.Remove(contextMenu);
            LayoutRoot.Children.Remove(_lastActivePhoto);
        }

        private void PrintMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //First, close the context menu
            LayoutRoot.Children.Remove(contextMenu);

            PrintDocument pd = new PrintDocument();
            //Subscribe to the PrintPage event
            pd.PrintPage += (s, args) =>
            {
                args.PageVisual = LayoutRoot;
                args.HasMorePages = false;
            };

            //Begin print – will show standard Windows Print Dialog
            pd.Print("PrintDoc");
        }


    }
}