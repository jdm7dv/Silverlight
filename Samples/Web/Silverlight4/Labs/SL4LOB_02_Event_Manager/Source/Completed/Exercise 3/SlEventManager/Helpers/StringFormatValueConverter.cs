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

namespace SlEventManager
{
    using System;
    using System.Windows.Data;

    /// <summary>
    ///     Two way IValueConverter that lets you bind a property on a bindable object
    ///     that can be an empty string value to a dependency property that should 
    ///     be set to null in that case
    /// </summary>
    public class StringFormatValueConverter : IValueConverter
    {
        private readonly string formatString;

        /// <summary>
        ///     Creates a new <see cref="StringFormatValueConverter"/>
        /// </summary>
        /// <param name="formatString">Format string, it can take zero or one parameters, the first one being replaced by the source value</param>
        public StringFormatValueConverter(string formatString)
        {
            this.formatString = formatString;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Format(System.Globalization.CultureInfo.CurrentUICulture, this.formatString, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}