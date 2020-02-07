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
using System.Globalization;
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

namespace OOBWindows
{
    public partial class MainPage : UserControl
    {
        dynamic excel = null;
        dynamic outlook = null;
        bool firstTime = true;
        const int olFolderCalendar = 9;
        const int olAppointment = 26;
        Application app;
        public delegate void SheetChangedDelegate(dynamic excelSheet, dynamic rangeArgs);

        public MainPage()
        {
            InitializeComponent();
            dg.AutoGenerateColumns = true;
            dg.HeadersVisibility = DataGridHeadersVisibility.All;
            dg.IsReadOnly = false;
            dg.UseLayoutRounding = true;
            dg.ItemsSource = DataHelper.GetCustomers();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void LaunchExcel(object sender, RoutedEventArgs e)
        {

        }

        private void CreateEmail(object sender, RoutedEventArgs e)
        {

        } 
    }
}