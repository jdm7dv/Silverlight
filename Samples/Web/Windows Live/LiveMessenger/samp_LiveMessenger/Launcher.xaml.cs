using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace samp_LiveMessenger
{
    public partial class Launcher : UserControl
    {
        public Launcher()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //ucHome oHome = new ucHome();
            //LayoutRoot.Children.Add(oHome);

            scrLogin oLogin = new scrLogin();
            LayoutRoot.Children.Add(oLogin);
        }
    }
}
