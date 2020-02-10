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
using System.Windows.Controls.Primitives;

namespace ContextMenu
{
    [TemplatePart(Name = "LayoutRoot", Type = typeof(Grid)),
    TemplatePart(Name = "MenuItemIcon", Type = typeof(Image)),
    TemplatePart(Name = "MenuItemContent", Type = typeof(ContentPresenter))]
    public class MenuItem : ButtonBase
    {
        Grid layoutRoot;
        Image itemImage;
        ContentPresenter itemContent;

        public MenuItem()
        {
            this.DefaultStyleKey = typeof(MenuItem);
            this.DataContext = this;
        }

        public string ItemContent
        {
            get { return (string)GetValue(ItemContentProperty); }
            set { SetValue(ItemContentProperty, value); }
        }

        public static readonly DependencyProperty ItemContentProperty =
            DependencyProperty.Register("ItemContent", typeof(string), typeof(MenuItem), null);

        public ImageSource MenuItemImage
        {
            get { return (ImageSource)GetValue(MenuItemImageProperty); }
            set { SetValue(MenuItemImageProperty, value); }
        }

        public static readonly DependencyProperty MenuItemImageProperty =
            DependencyProperty.Register("MenuItemImage", typeof(ImageSource), typeof(MenuItem), null);

        private void HandleEnabledVisualAid()
        {
            if (!this.IsEnabled)
                VisualStateManager.GoToState(this, "Disabled", true);
            else
                VisualStateManager.GoToState(this, "Normal", true);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            layoutRoot = GetTemplateChild("LayoutRoot") as Grid;
            itemImage = GetTemplateChild("MenuItemIcon") as Image;
            itemContent = GetTemplateChild("MenuItemContent")
                        as ContentPresenter;

            layoutRoot.MouseEnter += (s, e) =>
            {
                VisualStateManager.GoToState(this, "MouseOver", true);
            };

            layoutRoot.MouseLeave += (s, e) =>
            {
                VisualStateManager.GoToState(this, "Normal", true);
            };

            this.IsEnabledChanged += (s, e) =>
            {
                HandleEnabledVisualAid();
            };

            HandleEnabledVisualAid();
        }

    }
}