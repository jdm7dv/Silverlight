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
using MultiTouch.Controls.Silverlight4;
using MultiTouch.Behaviors.Silverlight4;

namespace Silverlight4MultiTouch
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();

            this.Loaded += (s, a) =>
                {
                    //Initialize the items
                    System.Windows.Interactivity.Interaction.GetBehaviors(item1).OfType<MultiTouchManipulationBehavior>().First().Move(new Point(200, 150), 45, 100);
                    System.Windows.Interactivity.Interaction.GetBehaviors(item2).OfType<MultiTouchManipulationBehavior>().First().Move(new Point(400, 350), 0, 80);
                    System.Windows.Interactivity.Interaction.GetBehaviors(item3).OfType<MultiTouchManipulationBehavior>().First().Move(new Point(600, 200), -45, 150);

                    System.Windows.Interactivity.Interaction.GetBehaviors(item4).OfType<MultiTouchManipulationBehavior>().First().Move(new Point(200, 300), 45, 100);
                    System.Windows.Interactivity.Interaction.GetBehaviors(item5).OfType<MultiTouchManipulationBehavior>().First().Move(new Point(400, 500), 0, 80);
                    System.Windows.Interactivity.Interaction.GetBehaviors(item6).OfType<MultiTouchManipulationBehavior>().First().Move(new Point(600, 350), -45, 150);
                };
        }
    }
}
