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
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    ///     Provides extension methods for performing operations on a <see cref="DataField"/>.
    /// </summary>
    public static class DataFieldExtensions
    {
        /// <summary>
        ///     Replaces a <see cref="DataField" />'s <see cref="TextBox" /> control with another control,
        ///     taking care of automatically updating the bindings.
        /// </summary>
        /// <param name="newControl">The new control you're going to set as <see cref="DataField.Content" /></param>
        /// <param name="dataBindingProperty">The control's property that will be used for data binding</param>        
        public static void ReplaceTextBox(this DataField field, FrameworkElement newControl, DependencyProperty dataBindingProperty)
        {
            field.ReplaceTextBox(newControl, dataBindingProperty, binding => { });
        }

        /// <summary>
        ///     Replaces a <see cref="DataField" />'s <see cref="TextBox" /> control with another control,
        ///     taking care of automatically updating the bindings and overriding the existing converter
        ///     with another one
        /// </summary>
        /// <param name="newControl">The new control you're going to set as <see cref="DataField.Content" /></param>
        /// <param name="dataBindingProperty">The control's property that will be used for data binding</param>        
        /// <param name="bindingSetupFunction">
        ///     A function you can use to change parameters on the newly generated binding before
        ///     it is applied to <paramref name="newControl"/>
        /// </param>
        public static void ReplaceTextBox(this DataField field, FrameworkElement newControl, DependencyProperty dataBindingProperty, Action<Binding> bindingSetupFunction)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field");
            }

            if (newControl == null)
            {
                throw new ArgumentNullException("newControl");
            }

            // Construct new binding by copying existing one, and sending it to bindingSetupFunction
            // for any changes the caller wants to perform
            Binding newBinding = field.Content.GetBindingExpression(TextBox.TextProperty).ParentBinding.CreateCopy();

            if (bindingSetupFunction != null)
            {
                bindingSetupFunction(newBinding);
            }

            // Replace field
            newControl.SetBinding(dataBindingProperty, newBinding);
            field.Content = newControl;
        }
    }
}