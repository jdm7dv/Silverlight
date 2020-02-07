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
using System.Windows.Navigation;
using System.IO;
using ImageTools;
using ImageTools.IO.Png;
using SlEventManager.Web.Services;

namespace SlEventManager.Views
{
    public partial class UserPicture : Page
    {
        public UserPicture()
        {
            InitializeComponent();

            cameraList.ItemsSource =
                CaptureDeviceConfiguration.GetAvailableVideoCaptureDevices();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (cs != null)
            {
                cs.Stop();
                cs = null;
            }
        }

        CaptureSource cs;
        private void cameraList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cs != null)
            {
                cs.Stop();
            }

            VideoCaptureDevice videoDevice = cameraList.SelectedItem as VideoCaptureDevice;
            if (videoDevice != null)
            {
                // We need user permission to use the webcam.
                if (CaptureDeviceConfiguration.AllowedDeviceAccess ||
                    CaptureDeviceConfiguration.RequestDeviceAccess())
                {
                    if (cs == null)
                    {
                        cs = new CaptureSource();

                        VideoBrush vb = new VideoBrush();
                        vb.SetSource(cs);
                        vb.Stretch = Stretch.UniformToFill;
                        webcamLivePreview.Background = vb;
                    }
                    cs.VideoCaptureDevice = videoDevice;
                    cs.Start();
                }
            }
        }

        private void takePictureButton_Click(object sender, RoutedEventArgs e)
        {
            if (cs == null) return;

            cs.CaptureImageCompleted += (s, pe) =>
            {
                var bmp = pe.Result;
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = bmp;
                brush.Stretch = Stretch.UniformToFill;
                userPictureBorder.Background = brush;
                PngEncoder encoder = new PngEncoder();
                using (MemoryStream ms = new MemoryStream())
                {
                    var itImage = bmp.ToImage();
                    encoder.Encode(itImage, ms);

                    imageBytes = ms.ToArray();
                }

            };

            cs.CaptureImageAsync();
        }

        byte[] imageBytes;

        private void savePictureButton_Click(object sender, RoutedEventArgs e)
        {
            if (imageBytes != null)
            {
                savePictureButton.IsEnabled = false;
                var context = new EventManagerDomainContext();
                context.SetCurrentUserImage(imageBytes, operation =>
                {
                    savePictureButton.IsEnabled = true;
                }, null);
            }

        }
    }
}