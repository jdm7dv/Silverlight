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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Xml.Linq;
using System.Windows.Markup;
using Microsoft.Windows.Controls;
using System.Windows.Browser;

namespace ArcGISSilverlightSDK
{
    public partial class LightPage : UserControl
    {
        private ScaleTransform _scale = new ScaleTransform();
        List<Category> _categoryList;
        string _xmlFile;
        CategoryItem _item;
        UserControl _control = null;
        string _target;
        ListBoxItem _targetListBoxItem;

        public LightPage()
        {
            InitializeComponent();
        }
        public LightPage(string xmlFile)
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
                            CodeVB = (string)o.Element("codevb"),
                            Icon = (string)o.Element("icon")
                        })
                        .ToArray()
                })
                .ToList();

            CreateList();

            ProcessTarget();
        }

        private void CreateList()
        {
            int listCount = ListOfSamples.Children.Count;
            foreach (Category category in _categoryList)
            {
                Grid g = new Grid()
                {
                    Margin = new Thickness(0, 5, 0, 0)
                };
                Rectangle rect = new Rectangle()
                {
                    //Fill = new SolidColorBrush(Color.FromArgb(255, 96, 120, 132)),
                    Fill = ExpanderGradient,
                    Stroke = new SolidColorBrush(Color.FromArgb(255, 70, 86, 109)),
                    Margin = new Thickness(0),
                    RadiusX = 5,
                    RadiusY = 5,
                };
                g.Children.Add(rect);
                Expander exp = new Expander()
                {
                    IsExpanded = false,
                    Name = String.Format("Category_{0}", listCount),
                    Foreground = new SolidColorBrush(Colors.Black),
                    Background = new SolidColorBrush(Colors.Transparent),
                    //BorderBrush= new SolidColorBrush(Color.FromArgb(255,70,86,109)),
                    FontWeight = FontWeights.Bold,
                    FontSize = 11,
                    Header = category.Name,
                    Tag = listCount,
                    Cursor = Cursors.Hand,
                    //Margin = new Thickness(0, 0, 4, 4)
                    Margin = new Thickness(4)
                };
                exp.Expanded += exp_Expanded;
                StackPanel sp2 = new StackPanel();
                sp2.Orientation = Orientation.Horizontal;
                ListBox lb = new ListBox()
                {
                    Name = String.Format("List_{0}", listCount),
                    Background = new SolidColorBrush(Colors.Transparent),
                    BorderBrush = new SolidColorBrush(Colors.Transparent),
                    Foreground = new SolidColorBrush(Colors.Black),
                    //FontSize = 10,
                    Margin = new Thickness(5, 0, 5, 5),
                    FontWeight = FontWeights.Normal,
                    ItemContainerStyle = SDKListBoxtItemStyle
                };
                int itemCount = 0;
                foreach (CategoryItem ci in category.CategoryItems)
                {
                    StackPanel sp1 = new StackPanel();
                    sp1.Orientation = Orientation.Horizontal;
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri(ci.Icon, UriKind.RelativeOrAbsolute));
                    sp1.Children.Add(img);
                    TextBlock tb1 = new TextBlock()
                    {
                        FontSize = 11,
                        Text = ci.ID
                    };
                    sp1.Children.Add(tb1);
                    ListBoxItem item = new ListBoxItem()
                    {
                        Content = sp1,
                        Background = new SolidColorBrush(Colors.Transparent),
                        BorderBrush = new SolidColorBrush(Colors.Transparent),
                        Name = String.Format("Item_{0}_{1}", listCount, itemCount),
                        Tag = ci,
                        Cursor = Cursors.Hand
                    };
                    lb.Items.Add(item);
                    itemCount++;
                }
                lb.SelectionChanged += lb_SelectionChanged;
                exp.Content = lb;
                listCount++;
                g.Children.Add(exp);
                ListOfSamples.Children.Add(g);
            }
        }

        void exp_Expanded(object sender, RoutedEventArgs e)
        {
            Expander exp1 = sender as Expander;
            Expander exp;
            int listCount = ListOfSamples.Children.Count;
            for (int i = 0; i < listCount; i++)
            {
                exp = FindName(String.Format("Category_{0}", i)) as Expander;
                if (exp.Name != exp1.Name) exp.IsExpanded = false;
            }
            int listIndex = Convert.ToInt32(exp1.Tag);
            string listName = String.Format("List_{0}", listIndex);
            ListBox lb = FindName(listName) as ListBox;
            if (lb != null) lb.SelectedIndex = -1;
            //SampleCaption.Text = String.Format("{0}", Convert.ToString(exp1.Header));
        }

        void pline_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Polygon pline = sender as Polygon;
            string index = Convert.ToString(pline.Tag);
            string name = _categoryList[Convert.ToInt32(index)].Name;
            //ToggleCategoryVisibility(index, name);
        }

        void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_targetListBoxItem != null)
            {
                _targetListBoxItem.IsSelected = false;
                _targetListBoxItem = null;
            }

            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                ListBoxItem item = e.AddedItems[0] as ListBoxItem;
                if (item != null)
                {
                    CategoryItem ci = item.Tag as CategoryItem;
                    _item = ci;
                    processitem(ci);
                }
            }
        }

        private void processitem(CategoryItem item)
        {            
            _control = null;
            tabSample.Content = null;
            //tabXamlScrollView.Content = null;
            txtSrc.Text = "";
            txtXaml.Text = "";
            txtSrcVB.Text = "";
            //tabSrcScrollView.Content = null;
            tabPanel.SelectedIndex = 0;

            Type t = Type.GetType(item.XAML);
            if (t != null)
            {
                _control = System.Activator.CreateInstance(t) as UserControl;
                tabSample.Content = _control;
                SampleCaption.Text = item.ID;
            }

            HtmlPage.Window.Invoke("setParentHash", new object[] { item.XAML.Split('.')[1]});
        }

        private void sideBar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double height = sideBar.ActualHeight - 10;
            double width = sideBar.ActualWidth - 10;
            //treeSamples.Width = width > 0 ? width : 1;
            //treeSamples.Height = height > 0 ? height : 1;
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
                case 3:
                    txtSrcVB.Text = src;
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
                    case 3:
                        sourceViewer(_item.CodeVB);
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
                case 3:
                    if (txtSrcVB == null)
                        return;
                    if (string.IsNullOrEmpty(txtSrcVB.SelectedText))
                        txtSrcVB.SelectAll();
                    text = txtSrcVB.SelectedText;
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
                mouseDelta = ((double)eventObject.GetProperty("detail")) * (-1);

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
                if (tabPanel.SelectedIndex == 3)
                    tabSrcScrollViewVB.ScrollToVerticalOffset(tabSrcScrollViewVB.VerticalOffset - 50);
            }

            if (mouseDelta < 0)
            {
                if (tabPanel.SelectedIndex == 1)
                    tabXamlScrollView.ScrollToVerticalOffset(tabXamlScrollView.VerticalOffset + 50);
                if (tabPanel.SelectedIndex == 2)
                    tabSrcScrollView.ScrollToVerticalOffset(tabSrcScrollView.VerticalOffset + 50);
                if (tabPanel.SelectedIndex == 3)
                    tabSrcScrollViewVB.ScrollToVerticalOffset(tabSrcScrollViewVB.VerticalOffset + 50);
            }
        }


        [System.Windows.Browser.ScriptableMember]
        public void SetTargetSample(string hash)
        {
            // HtmlPage.Window.CurrentBookmark does not work with SL app in a frame
            string targetValue = hash.Trim().TrimStart('#');
            if (targetValue.Length > 0 && targetValue.Length < 30)
                _target = targetValue;
        }

        public void ProcessTarget()
        {
            if (string.IsNullOrEmpty(_target))
                return;

            string targetXAML = "ArcGISSilverlightSDK." + _target;

            int categoryIndex = 0;
            foreach (Category category in _categoryList)
            {
                int categoryItemIndex = 0;
                foreach (CategoryItem categoryItem in category.CategoryItems)
                {
                    if (categoryItem.XAML.ToLower() == targetXAML.ToLower())
                    {
                        _item = categoryItem;
                        processitem(categoryItem);

                        Expander exp = FindName(String.Format("Category_{0}", categoryIndex)) as Expander;
                        exp.IsExpanded = true;

                        _targetListBoxItem = FindName(String.Format("Item_{0}_{1}", categoryIndex, categoryItemIndex)) as ListBoxItem;
                        _targetListBoxItem.IsSelected = true;

                        return;
                    }
                    categoryItemIndex++;
                }
                categoryIndex++;
            }
        }
    }
}
