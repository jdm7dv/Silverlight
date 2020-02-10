using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Collections.Generic;

namespace ReflectionIT.Silverlight.CountDown
{
    public partial class MainPage : UserControl {

        public MainPage(IDictionary<string, string> initParams) {
            // Required to initialize variables
            InitializeComponent();

            flipClockCountDown.CountDownDate = DateTime.Parse(initParams["date"]);
            flipClockCountDown.Title = initParams["title"];
        }

    }
}