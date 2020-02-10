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

namespace ArcGISSilverlightSDK
{
	public partial class ElementLayer : UserControl
	{
		public ElementLayer()
		{
			InitializeComponent();
		}

		private void RedlandsButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("You found Redlands");
		}
	}
}
