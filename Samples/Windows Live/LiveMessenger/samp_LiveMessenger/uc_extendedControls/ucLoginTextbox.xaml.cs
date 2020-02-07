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
    public partial class ucLoginTextbox : UserControl
    {
        public event CallbackDelegate ShowPreviousLogins;

        public ucLoginTextbox()
        {
            InitializeComponent();
        }

        private void butShowLogins_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ShowPreviousLogins != null) { ShowPreviousLogins(new CallbackEventArgs("ShowPreviousLogins", "PreInit")); }
        }

        private void butShowLogins_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ShowPreviousLogins != null) { ShowPreviousLogins(new CallbackEventArgs("ShowPreviousLogins", "PostInit")); }
        }
    }
}
