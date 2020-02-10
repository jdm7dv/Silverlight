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
using System.Windows.Data;

namespace SlEventManager.Views
{
    public partial class SchedulePrintView : UserControl
    {
        public SchedulePrintView()
        {
            InitializeComponent();
        }

        public int PopulatePage(IEnumerable<object> items, int startingPoint)
        {
            double containerWidth = timeSlotContainer.ActualWidth;
            double containerHeight = timeSlotContainer.ActualHeight;

            StackPanel timeSlotPanel = new StackPanel();
            timeSlotPanel.Width = containerWidth;
            timeSlotContainer.Child = timeSlotPanel;
            int itemsAdded = 0;
            this.UpdateLayout();
            foreach (object item in items.Skip(startingPoint))
            {
                ScheduleTimeSlotPrintView uc = new ScheduleTimeSlotPrintView();
                uc.SetData((CollectionViewGroup)item);
                uc.DataContext = item;
                timeSlotPanel.Children.Add(uc);
                this.UpdateLayout();
                timeSlotPanel.Measure(new Size(containerWidth, double.PositiveInfinity));
                if (timeSlotPanel.DesiredSize.Height > containerHeight && itemsAdded > 0)
                {
                    timeSlotPanel.Children.Remove(uc);
                    break;
                }
                itemsAdded += 1;
            }
            return itemsAdded;
        }
    }
}