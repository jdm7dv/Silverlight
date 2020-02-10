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

namespace ArcGISSilverlightSDK
{
    public class Category //: Microsoft.Windows.Controls.TreeViewItem
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public CategoryItem[] CategoryItems { get; set; }

        public Category() { }
        public Category(string name)
        {
            Name = name;
        }
    }

    public class CategoryItem //: Microsoft.Windows.Controls.TreeViewItem
    {
        public string ID { get; set; }
        public string XAML { get; set; }
        public string Source { get; set; }
        public string Code { get; set; }
        public string CodeVB { get; set; }
        public string Icon { get; set; }

        public CategoryItem() {}
        public CategoryItem(string name, string xaml) 
        {
            ID = name;
            XAML = xaml;
        }
    }
}
