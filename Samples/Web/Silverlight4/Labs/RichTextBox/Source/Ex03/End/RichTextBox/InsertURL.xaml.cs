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
using System.Text.RegularExpressions;

namespace RichNotepad
{
    public partial class InsertURL : ChildWindow
    {
        public InsertURL(string selectedText)
        {
            InitializeComponent();

            txtURLDesc.Text = selectedText;

            this.Closing += (s, e) =>
            {
                if (this.DialogResult.Value)
                {
                    if (txtURL.Text.Length == 0)
                        e.Cancel = true;
                    else
                    {
                        if (txtURL.Text.IndexOf("http://") == -1 &&
                            txtURL.Text.IndexOf("https://") == -1 &&
                            txtURL.Text.IndexOf("ftp://") == -1)
                            txtURL.Text = "http://" + txtURL.Text;

                        if (!Regex.IsMatch(txtURL.Text,
                                              @"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?"))
                            e.Cancel = true;
                    }
                    if (e.Cancel)
                        MessageBox.Show("URL is empty or not valid.\nPlease try again");
                }
            };
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}