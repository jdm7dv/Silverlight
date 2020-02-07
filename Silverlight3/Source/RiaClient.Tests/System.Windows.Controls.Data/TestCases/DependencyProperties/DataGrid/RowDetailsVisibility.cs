﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.Reflection;
using System.Windows.Controls.Data.Test.DataClasses;
using System.Windows.Controls.Test;
using System.Windows.Markup;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Windows.Controls.Data.Test
{
    public partial class DataGrid_DependencyProperties_TestClass
    {
        [TestMethod]
        [Description("Verify Dependency Property: (DataGridRowDetailsVisibilityMode) DataGrid.RowDetailsVisibilityMode.")]
        public void RowDetailsVisibilityMode()
        {
            Type propertyType = typeof(DataGridRowDetailsVisibilityMode);
            bool expectGet = true;
            bool expectSet = true;
            bool hasSideEffects = true;

            DataGrid control = new DataGrid();
            Assert.IsNotNull(control);

            // Verify dependency property member
            FieldInfo fieldInfo = typeof(DataGrid).GetField("RowDetailsVisibilityModeProperty", BindingFlags.Static | BindingFlags.Public);
            Assert.AreEqual(typeof(DependencyProperty), fieldInfo.FieldType, "DataGrid.RowDetailsVisibilityModeProperty not expected type 'DependencyProperty'.");

            // Verify dependency property's value type
            DependencyProperty property = fieldInfo.GetValue(null) as DependencyProperty;

            Assert.IsNotNull(property);

            // 


            // Verify dependency property CLR property member
            PropertyInfo propertyInfo = typeof(DataGrid).GetProperty("RowDetailsVisibilityMode", BindingFlags.Instance | BindingFlags.Public);
            Assert.IsNotNull(propertyInfo, "Expected CLR property DataGrid.RowDetailsVisibilityMode does not exist.");
            Assert.AreEqual(propertyType, propertyInfo.PropertyType, "DataGrid.RowDetailsVisibilityMode not expected type 'DataGridRowDetailsVisibilityMode'.");

            // Verify getter/setter access
            Assert.AreEqual(expectGet, propertyInfo.CanRead, "Unexpected value for propertyInfo.CanRead.");
            Assert.AreEqual(expectSet, propertyInfo.CanWrite, "Unexpected value for propertyInfo.CanWrite.");

            // Verify that we set what we get
            if (expectSet) // if expectSet == false, this block can be removed
            {
                control.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;

                Assert.AreEqual(DataGridRowDetailsVisibilityMode.Collapsed, control.RowDetailsVisibilityMode);

                control.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Visible;

                Assert.AreEqual(DataGridRowDetailsVisibilityMode.Visible, control.RowDetailsVisibilityMode);

                control.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;

                Assert.AreEqual(DataGridRowDetailsVisibilityMode.VisibleWhenSelected, control.RowDetailsVisibilityMode);

                control.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;

                Assert.AreEqual(DataGridRowDetailsVisibilityMode.Collapsed, control.RowDetailsVisibilityMode);
            }

            // Verify dependency property callback
            if (hasSideEffects)
            {
                MethodInfo methodInfo = typeof(DataGrid).GetMethod("OnRowDetailsVisibilityModePropertyChanged", BindingFlags.Static | BindingFlags.NonPublic);
                Assert.IsNotNull(methodInfo, "Expected DataGrid.RowDetailsVisibilityMode to have static, non-public side-effect callback 'OnRowDetailsVisibilityModePropertyChanged'.");

                // 
            }
            else
            {
                MethodInfo methodInfo = typeof(DataGrid).GetMethod("OnRowDetailsVisibilityModePropertyChanged", BindingFlags.Static | BindingFlags.NonPublic);
                Assert.IsNull(methodInfo, "Expected DataGrid.RowDetailsVisibilityMode NOT to have static side-effect callback 'OnRowDetailsVisibilityModePropertyChanged'.");
            }
        }

        /// <summary>
        /// Collapases a single row details that's visible when no other details is expanded and the vertical scrollbar is visible before and after the collapse.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        [Description("Collapases a single row details that's visible when no other details is expanded and the vertical scrollbar is visible before and after the collapse.")]
        public void CollapseSingleDetailsWithoutOtherExpansionWithScrollBarVisible()
        {
            this._autoGeneratedCount = 0;
            this._isLoaded = false;
            DataGrid dataGrid = new DataGrid();
            dataGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
            dataGrid.Width = 250;
            dataGrid.Height = 300;
            dataGrid.RowDetailsTemplate = (DataTemplate)XamlReader.Load(@"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" ><Button Height=""30"" /></DataTemplate>");
            Assert.IsNotNull(dataGrid);
            dataGrid.Loaded += this.dataGrid_Loaded;

            CustomerList customers = new CustomerList(30);
            dataGrid.ItemsSource = customers;
            dataGrid.SelectedItem = customers[0];

            TestPanel.Children.Add(dataGrid);
            this.EnqueueConditional(delegate { return this._isLoaded; });

            double verticalMax = 0;
            this.EnqueueYieldThread();
            this.EnqueueCallback(delegate
            {
                Assert.IsNotNull(dataGrid.VerticalScrollBar);
                Assert.AreEqual(Visibility.Visible, dataGrid.VerticalScrollBar.Visibility);
                Assert.AreEqual(customers[0], dataGrid.SelectedItem);

                verticalMax = dataGrid.VerticalScrollBar.Maximum;

                // Collapse details with no animation
                dataGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
            });

            this.EnqueueYieldThread();
            this.EnqueueCallback(delegate
            {
                // Check to make sure the vertical scrollbar was adjusted correctly
                Assert.IsTrue(DoubleUtil.AreClose(30, verticalMax - dataGrid.VerticalScrollBar.Maximum));

                dataGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
            });

            this.EnqueueYieldThread();
            this.EnqueueCallback(delegate
            {
                Assert.IsTrue(DoubleUtil.AreClose(verticalMax, dataGrid.VerticalScrollBar.Maximum));

                // Collapse with the animation
                dataGrid.SelectedItem = null;
            });

            // Wait for the details animation to complete
            this.EnqueueDelay(100);
            this.EnqueueYieldThread();
            this.EnqueueCallback(delegate
            {
                // Check to make sure the vertical scrollbar was adjusted correctly
                Assert.IsTrue(DoubleUtil.AreClose(30, verticalMax - dataGrid.VerticalScrollBar.Maximum));
            });

            EnqueueTestComplete();
        }
    }
}
