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
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Resources;
using System.Threading;

namespace RichNotepad
{
    public class BoolToFlowDirectionConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool val = bool.Parse(value.ToString());

            if (targetType == typeof(string))
            {
                ResourceManager resourceManager = new ResourceManager("RichNotepad.Strings", GetType().Assembly);

                return (string)resourceManager.GetString("txt_Header_Direction", Thread.CurrentThread.CurrentUICulture);
            }
            else
            {
                if (val)
                    return FlowDirection.RightToLeft;
                else
                    return FlowDirection.LeftToRight;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            FlowDirection fd = (FlowDirection)Enum.Parse(typeof(FlowDirection), value.ToString(), true);

            if (fd == FlowDirection.RightToLeft)
                return true;
            else
                return false;

        }

        #endregion
    }
}