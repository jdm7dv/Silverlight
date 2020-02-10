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
using System.Runtime.InteropServices.Automation;

namespace OOBWindows
{
    public partial class MainPage : UserControl
    {
        dynamic excel = null;
        dynamic outlook = null;
        bool firstTime = true;
        const int olFolderCalendar = 9;
        const int olAppointment = 26;
        Window mainWindow;
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
            mainWindow = Application.Current.MainWindow;
            if (Application.Current.IsRunningOutOfBrowser)
            {
                mainWindow.Closing += (s, args) =>
                {
                    WindowClosing win = new WindowClosing();
                    win.Closed += (sen, winArgs) =>
                    {
                        if (win.DialogResult.Value) args.Cancel = true;
                    };
                    win.Show();
                };

                Application.Current.CheckAndDownloadUpdateCompleted += (s, args) =>
                {
                    if (args.UpdateAvailable) MessageBox.Show("Application has been updated." +
                        "Please close and re-launch the application to pick up the changes.");
                };
                Application.Current.CheckAndDownloadUpdateAsync();
            }
            else
            {
                InstallWindow win = new InstallWindow();
                win.Closed += (s, args) =>
                    {
                        this.LayoutRoot.Visibility = Visibility.Collapsed;
                    };
                win.Show();
            }
        }

        private void LaunchExcel(object sender, RoutedEventArgs e)
        {
            string excelName = "Excel.Application";
            try
            {
                //See if Excel is already running
                excel = AutomationFactory.GetObject(excelName);
            }
            catch
            {
                excel = AutomationFactory.CreateObject(excelName);
            }

            excel.Visible = true;

            dynamic workbook = excel.workbooks;
            workbook.Add();

            dynamic sheet = excel.ActiveSheet;

            dynamic cell = null;
            int i = 1;
            foreach (Customer cust in dg.ItemsSource)
            {
                cell = sheet.Cells[i /*row*/, 1 /*col*/];
                cell.Value = cust.FirstName;
                cell.ColumnWidth = 25;

                cell = sheet.Cells[i /*row*/, 2 /*col*/];
                cell.Value = cust.LastName;
                cell.ColumnWidth = 25;

                cell = sheet.Cells[i /*row*/, 3 /*col*/];
                cell.Value = cust.Phone;
                cell.ColumnWidth = 25;

                cell = sheet.Cells[i /*row*/, 4 /*col*/];
                cell.Value = cust.State;
                cell.ColumnWidth = 25;

                i++;
            }

            if (firstTime)
            {
                excel.SheetChange += new SheetChangedDelegate(sheetChangedEventHandler);
                string name = sheet.Name;

                firstTime = false;
            }
        }


        private void sheetChangedEventHandler(dynamic excelSheet, dynamic rangeArgs)
        {
            dynamic sheet = excelSheet;
            string name = sheet.Name;
            dynamic range = rangeArgs;
            dynamic rowvalue = range.Row;

            IEnumerable<Customer> custs = dg.ItemsSource as IEnumerable<Customer>;
            int len = custs.Count();
            Customer[] newEntities = new Customer[len];

            dynamic col1range = sheet.Range("A1:A" + len.ToString());
            dynamic col2range = sheet.Range("B1:B" + len.ToString());
            dynamic col3range = sheet.Range("C1:C" + len.ToString());
            dynamic col4range = sheet.Range("D1:D" + len.ToString());

            for (int i = 0; i < len; i++)
            {
                dynamic item1 = col1range.Item(i + 1);
                dynamic item2 = col2range.Item(i + 1);
                dynamic item3 = col3range.Item(i + 1);
                dynamic item4 = col4range.Item(i + 1);

                Customer newEntity = new Customer();
                newEntity.CustomerID = i;
                newEntity.FirstName = item1.Value;
                newEntity.LastName = item2.Value;
                newEntity.Phone = item3.Value;
                newEntity.State = item4.Value;
                newEntities[i] = newEntity;
            }
            dg.ItemsSource = newEntities;
            dg.SelectedIndex = Convert.ToInt32(rowvalue) - 1;

            //Show NotificationWindow
            NotificationWindow win = new NotificationWindow();
            Grid grid = new Grid { Background = new SolidColorBrush(Colors.Yellow) };
            grid.Children.Add(new TextBlock { FontSize = 20, Text = "Customers Modified", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center });
            win.Content = grid;
            win.Show(3000);
        }

        private void CreateEmail(object sender, RoutedEventArgs e)
        {
            string outlookName = "Outlook.Application";
            try
            {
                //See if Outlook is already running
                outlook = AutomationFactory.GetObject(outlookName);
            }
            catch
            {
                outlook = AutomationFactory.CreateObject(outlookName);
            }
            dynamic mailItem = outlook.CreateItem(0);
            mailItem.Display();

            string msgBody = "<P>Customer Records</P>";
            msgBody += "<P>" + DataHelper.GetCustomersAsHtmlTable() + "</P>";

            mailItem.To = "BigBoss;";
            mailItem.Subject = "Updated Customer Records";
            mailItem.HTMLBody = msgBody;
        }
    }
}