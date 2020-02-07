using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace EasingFunctions
{
	public partial class Page : UserControl
	{
		public Page()
		{
			InitializeComponent();
		}

		public void StartAnimation (object sender, EventArgs args)
		{
			Storyboard sb = Resources["animation"] as Storyboard;

			sb.Stop ();

			EasingView view = sender as EasingView;
			DoubleAnimation anim = (DoubleAnimation)sb.Children[0];
			Storyboard.SetTarget (anim, rect);
			anim.EasingFunction = view.EasingFunction;

			sb.Begin ();
		}
	}
}
