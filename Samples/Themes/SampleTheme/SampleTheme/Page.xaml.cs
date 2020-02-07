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

namespace SampleTheme
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
            //popluating the list with dummy data
            List1.ItemsSource = Fruit.FruitsSample;
            List2.ItemsSource = Fruit.FruitsSample;
            List3.ItemsSource = Fruit.FruitsSample;
        }
    }
}
