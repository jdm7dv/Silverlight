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
using System.Windows.Ink;

namespace InkPresenter
{
    public partial class Page : UserControl
    {
        private Stroke newStroke = null;

        public Page()
        {
            InitializeComponent();
        }

        private void inkPresenterElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // capture mouse and create a new stroke

            inkCtrl.CaptureMouse();
            newStroke = new Stroke();
            inkCtrl.Strokes.Add(newStroke);

            // set the desired drawing attributes here

            newStroke.DrawingAttributes.Color = Colors.Black;
            //newStroke.DrawingAttributes.OutlineColor = Colors.Yellow;
            newStroke.DrawingAttributes.Width = 6d;
            newStroke.DrawingAttributes.Height = 6d;

            // add the stylus points
            newStroke.StylusPoints.Add(e.StylusDevice.GetStylusPoints(inkCtrl));

        }

        private void inkPresenterElement_MouseMove(object sender, MouseEventArgs e)
        {
            if (newStroke != null)
            {

                // add the stylus points
                newStroke.StylusPoints.Add(e.StylusDevice.GetStylusPoints(inkCtrl));

            }
        }

        private void inkPresenterElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (newStroke != null)
            {

                // add the stylus points
                newStroke.StylusPoints.Add(e.StylusDevice.GetStylusPoints(inkCtrl));


            }

            // release mouse capture and we are done
            inkCtrl.ReleaseMouseCapture();
            newStroke = null;
        }
    }
}
