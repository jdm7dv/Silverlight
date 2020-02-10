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

namespace Styles_Red
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
        }

        private void dataGridInstance_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string test = e.Column.ToString();

            if (e.Column is DataGridTextBoxColumn)
            {
                //    ((DataGridTextBoxColumn)e.Column).Foreground = new SolidColorBrush(Colors.White);
                ((DataGridTextBoxColumn)e.Column).FontFamily = new FontFamily("Arial");
                ((DataGridTextBoxColumn)e.Column).FontSize = 11;
            }
            //else if (e.Column is DataGridCheckBoxColumn)
            //{
            //    e.Column.ElementStyle = (Style)Resources["checkBoxStyle"];
            //    e.Column.EditingElementStyle = (Style)Resources["checkBoxStyle"];
            //}
        }

    }
}
