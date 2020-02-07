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

    public MainPage()
    {
      InitializeComponent();      
    }

    private void stopAnimations()
    {
      //TODO: stop storyboards
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      switch ((sender as Button).Content.ToString())
      {
        case "Pixelate":
          //TODO: start storyboard and set Path's effect
          break;
        case "Swirl":
          //TODO: start storyboard and set Path's effect
          break;
        case "Invert":
          //TODO: start storyboard and set Path's effect
          break;
        case "Ripple":
          //TODO: start storyboard and set Path's effect
          break;
        default:
          TO_FILL.Effect = null;
          break;
      }
    }          

    private void btnRotate_Click(object sender, RoutedEventArgs e)
    {
        // TODO: toggle the running state of the perspective rotation storyboard
    }


    private void btnStart_Click(object sender, RoutedEventArgs e)
    {
        // TODO:  enable / disable WebCaM video capture
    }        


    private void btnCapture_Click(object sender, RoutedEventArgs e)
    {
        // TODO: capture current video frame as a bitmap for printing via a dialog  
    }              

  }
}