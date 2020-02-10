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

namespace MagicMirror
{
  public partial class MainPage : UserControl
  {
    const double maxAngle = 20.0d;
    const double maxRotation = 45.0d;
    const int gridCount = 20;
    CaptureSource cs = null;

    public MainPage()
    {
      InitializeComponent();      
    }

    private void stopAnimations()
    {
        sbPixelate.Stop();
        sbRipple.Stop();
        sbSwirl.Stop();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      switch ((sender as Button).Content.ToString())
      {
        case "Pixelate":
          sbPixelate.Begin();
          TO_FILL.Effect = effectPixelate;
          break;
        case "Swirl":
          sbSwirl.Begin();
          TO_FILL.Effect = effectSwirl;
          break;
        case "Invert":          
          TO_FILL.Effect = effectInvert;
          break;
        case "Ripple":
          sbRipple.Begin();
          TO_FILL.Effect = effectRipple;
          break;
        default:
          TO_FILL.Effect = null;
          break;
      }
    }          

    private void btnRotate_Click(object sender, RoutedEventArgs e)
    {
        if (btnRotate.Content.ToString() == "Rotate")
        {
            btnRotate.Content = "Stop";
            sbPerspective.Begin();
        }
        else
        {
            btnRotate.Content = "Rotate";
            sbPerspective.Stop();
        }

    }


    private void btnStart_Click(object sender, RoutedEventArgs e)
    {
        if (btnStart.Content.ToString() == "Webcam On")
        {
            btnStart.Content = "Webcam Off";

            if (CaptureDeviceConfiguration.AllowedDeviceAccess || CaptureDeviceConfiguration.RequestDeviceAccess())
            {
                VideoCaptureDevice vcd = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice();
                AudioCaptureDevice acd = CaptureDeviceConfiguration.GetDefaultAudioCaptureDevice();
                if (null != vcd && null != acd)
                {
                    cs = new CaptureSource();
                    cs.VideoCaptureDevice = vcd;
                    cs.AudioCaptureDevice = acd;
                    cs.Start();

                    VideoBrush videoBrush = new VideoBrush();
                    videoBrush.Stretch = Stretch.Uniform;
                    videoBrush.SetSource(cs);
                    TO_FILL.Fill = videoBrush;
                }
                else
                    MessageBox.Show("Error initializing Webcam or Mic.\nPlease install device drivers and try again");
            }
        }
        else
        {
            btnStart.Content = "Webcam On";

            if (null != cs)
                cs.Stop();

            TO_FILL.Fill = this.Resources["imageBrush"] as ImageBrush;
        }
    }        


    private void btnCapture_Click(object sender, RoutedEventArgs e)
    {
        if (null != cs)
        {
            if (cs.State == CaptureState.Started)
            {
                cs.CaptureImageCompleted += new EventHandler<CaptureImageCompletedEventArgs>(cs_CaptureImageCompleted);
                cs.CaptureImageAsync();
            }
            else
            {
                MessageBox.Show("Start capture from Webcam and try again!");
            }
        }
        else
            MessageBox.Show("Start capture from Webcam and try again!");
 
    }

    void cs_CaptureImageCompleted(object sender, CaptureImageCompletedEventArgs e)
    {
        // initialize the child window
        CapturedImage ci = new CapturedImage();
        ci.HasCloseButton = false;
        ci.img.Source = e.Result;

        ci.Show();
        cs.CaptureImageCompleted -= new EventHandler<CaptureImageCompletedEventArgs>(cs_CaptureImageCompleted);
    }



    private void borderEffects_MouseMove(object sender, MouseEventArgs e)
    {
        Point mousePt = e.GetPosition(this);

        effectRipple.Center = new Point(mousePt.X / this.ActualWidth,
                                        mousePt.Y / this.ActualHeight);
    }
  }
}