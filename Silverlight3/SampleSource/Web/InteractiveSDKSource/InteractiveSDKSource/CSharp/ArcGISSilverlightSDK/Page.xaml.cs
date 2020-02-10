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
using System.IO;
using System.Xml.Linq;
using System.Windows.Markup;
using Microsoft.Windows.Controls;
using System.Windows.Browser;


namespace ArcGISSilverlightSDK
{
    public partial class Page : UserControl
    {
        private ScaleTransform _scale = new ScaleTransform();
        List<Category> _categoryList;
        string _xmlFile;
        CategoryItem _item;
        UserControl _control = null;

        public Page()
        {
            InitializeComponent();
        }

        public Page(string xmlFile)
            : this()
        {
            _xmlFile = xmlFile;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            WebClient client = new WebClient();
            client.OpenReadCompleted += client_OpenReadCompleted;
            Uri uri = new Uri(_xmlFile, UriKind.RelativeOrAbsolute);
            Dispatcher.BeginInvoke(() => { client.OpenReadAsync(uri); });
            InitializeMouseWheel();
        }

        void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error != null)
                return;

            XDocument doc = null;
            using (Stream s = e.Result)
            {
                doc = XDocument.Load(s);
            }
            _categoryList = (
                from f in doc.Root.Elements("Category")
                select new Category
                {
                    Name = (string)f.Element("name"),
                    Icon = (string)f.Element("icon"),
                    CategoryItems = (
                        from o in f.Elements("items").Elements("item")
                        select new CategoryItem
                        {
                            ID = (string)o.Element("id"),
                            XAML = (string)o.Element("xaml"),
                            Source = (string)o.Element("source"),
                            Code = (string)o.Element("code"),
                            Icon =(string)o.Element("icon")
                        })
                        .ToArray()
                })
                .ToList();
            // set up the binding with the TreeView
            treeSamples.ItemsSource = _categoryList;
            treeSamples.Visibility = Visibility.Visible;
            StackPanel sp = new StackPanel();
            
            
            // Select the first item automatically
            //SelectFirstTreeItem();
            //Dispatcher.BeginInvoke(() => { SelectFirstTreeItem(); });
        }

        private void SelectFirstTreeItem()
        {
            
            if (treeSamples.Items.Count > 0)
            {
                TreeViewItem i1 = (TreeViewItem)treeSamples.Items[0];
                i1.IsExpanded = true;
                
                //Category cat = (Category)treeSamples.Items[0];
                //((TreeViewItem)cat.CategoryItems[0]).IsExpanded = true;
                //((Category)treeSamples.Items[0]).CategoryItems
                
                
                //    Dispatcher.BeginInvoke(() => { item.IsSelected = true; });
            }
        }

        private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {

        }

        private void treeSamples_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is CategoryItem)
            {
                _item = e.NewValue as CategoryItem;
                processitem(_item);
            }
        }

        private void processitem(CategoryItem item)
        {
            _control = null;
            tabSample.Content = null;
            //tabXamlScrollView.Content = null;
            txtSrc.Text = "";
            txtXaml.Text = "";
            //tabSrcScrollView.Content = null;
            tabPanel.SelectedIndex = 0;

            Type t = Type.GetType(item.XAML);
            if (t != null)
            {                    
                _control = System.Activator.CreateInstance(t) as UserControl;
                tabSample.Content = _control;
            }
        }

        private void sourceViewer(string srcFile)
        {
            WebClient client = new WebClient();
            client.OpenReadCompleted += sourceView_OpenReadCompleted;
            client.OpenReadAsync(new Uri(srcFile, UriKind.RelativeOrAbsolute));
        }

        void sourceView_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
             if (e.Error != null)
                return;

            string src = null;
            using (Stream s = e.Result)
            {
                StreamReader sr = new StreamReader(s);
                src = sr.ReadToEnd();
            }

            switch (tabPanel.SelectedIndex)
            {
                case 1:
                    txtXaml.Text = src;
                    break;
                case 2:
                    txtSrc.Text = src;
                    break;
            }
        }
        
        private void tabPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabPanel == null) return;
            if (_item != null)
            {
                switch (tabPanel.SelectedIndex)
                {
                    case 1:
                        sourceViewer(_item.Source);
                        copyToClipboard.Visibility = Visibility.Visible;
                        break;
                    case 2:
                        sourceViewer(_item.Code);
                        copyToClipboard.Visibility = Visibility.Visible;
                        break;
                    default:
                        copyToClipboard.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }

        public void copyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            string text = String.Empty;
            switch (tabPanel.SelectedIndex)
            {
                case 1:
                    if (txtXaml == null)
                        return;
                    if (string.IsNullOrEmpty(txtXaml.SelectedText))
                        txtXaml.SelectAll();
                    text = txtXaml.SelectedText;
                    break;
                case 2:
                    if (txtSrc == null)
                        return;
                    if (string.IsNullOrEmpty(txtSrc.SelectedText))
                        txtSrc.SelectAll();
                    text = txtSrc.SelectedText;
                    break;
            }

            Clipboard.SetText(text);
        }

        private void InitializeMouseWheel()
        {
            HtmlPage.Window.AttachEvent("DOMMouseScroll", this.OnMouseWheel); // Mozilla
            HtmlPage.Window.AttachEvent("onmousewheel", this.OnMouseWheel);
            HtmlPage.Document.AttachEvent("onmousewheel", this.OnMouseWheel); // IE
        }

        protected void OnMouseWheel(object sender, HtmlEventArgs e)
        {
            double mouseDelta = 0;
            ScriptObject eventObject = e.EventObject;

            // Mozilla and Safari
            if (eventObject.GetProperty("detail") != null)
                mouseDelta = ((double)eventObject.GetProperty("detail")) *(-1);

            // IE and Opera
            else if (eventObject.GetProperty("wheelDelta") != null)
                mouseDelta = ((double)eventObject.GetProperty("wheelDelta"));

            mouseDelta = Math.Sign(mouseDelta);

            if (mouseDelta > 0)
            {
                if (tabPanel.SelectedIndex == 1)
                    tabXamlScrollView.ScrollToVerticalOffset(tabXamlScrollView.VerticalOffset - 50);
                if (tabPanel.SelectedIndex == 2)
                    tabSrcScrollView.ScrollToVerticalOffset(tabSrcScrollView.VerticalOffset - 50);
            }

            if (mouseDelta < 0)
            {
                if (tabPanel.SelectedIndex == 1)
                    tabXamlScrollView.ScrollToVerticalOffset(tabXamlScrollView.VerticalOffset + 50);
                if (tabPanel.SelectedIndex == 2)
                    tabSrcScrollView.ScrollToVerticalOffset(tabSrcScrollView.VerticalOffset + 50);

            }
        }

        private void sideBar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double height = sideBar.ActualHeight - 10;
            double width = sideBar.ActualWidth - 10;
            treeSamples.Width = width > 0 ? width : 1;
            treeSamples.Height = height > 0 ? height : 1;
        }
      
    }
}
