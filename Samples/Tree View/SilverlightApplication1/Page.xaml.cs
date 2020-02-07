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

namespace SilverlightApplication1
{
    public partial class Page : UserControl
    {
        Entry[] entrys = {
            new Entry("Sphinx Logic partners with You Tube")
                         };

        public string Display()
        {
            var Dec2008 =
                from a in entrys
                orderby a.entryTitle
                select a;

            foreach (Entry localdec in Dec2008)
            {
                return localdec.ToString();
            }
            return "No entrties found";
        }
                 
        public Page()
        {
            InitializeComponent();
            this.Loaded += (sender, args) => { Storyboard1.Begin(); };
            
        }

        private void Dec08TreeView_Selected(object sender, RoutedEventArgs e)
        {
            DemoList.Items.Clear();
            Display();
            //string[] array = { "entry1", "entry2", "entry3" };
            //foreach (string number in array)
            //    DemoList.Items.Add(number);

        }       

        private void Nov08TreeView_Selected(object sender, RoutedEventArgs e)
        {
            DemoList.Items.Clear();
            string[] array = { "entry3", "entry4", "entry5" };
            foreach (string number in array)
                DemoList.Items.Add(number);
            

        }

        private void treeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            DemoList.Items.Clear();
        }

        

       
    }
}
