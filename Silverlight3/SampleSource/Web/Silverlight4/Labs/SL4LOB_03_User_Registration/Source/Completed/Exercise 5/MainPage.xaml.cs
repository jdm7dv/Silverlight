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

namespace RichTextApp
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void boldButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleSetting(TextElement.FontWeightProperty,
                          FontWeights.Bold, FontWeights.Normal);
        }

        private void italicButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleSetting(TextElement.FontStyleProperty,
                          FontStyles.Italic, FontStyles.Normal);
        }

        private void ToggleSetting(DependencyProperty property,
                           object onValue, object offValue)
        {
            object currentValue = rtb.Selection.GetPropertyValue(property);
            bool alreadyApplied = currentValue.ToString() == onValue.ToString();

            rtb.Selection.ApplyPropertyValue(property,
                                             alreadyApplied ? offValue : onValue);
        }

        private void rtb_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            textAsXaml.Text = rtb.Xaml;
        }

    }
}