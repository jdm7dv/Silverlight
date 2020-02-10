using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace MultiTouch.Controls.WPF
{
    /// <summary>
    /// WPF 4 Multi-Touch and Inertia enabler
    /// Uses code from "INTRODUCTION TO WPF 4 MULTITOUCH" by Jaime Rodriguez - http://blogs.msdn.com/jaimer/archive/2009/11/04/introduction-to-wpf-4-multitouch.aspx
    /// </summary>
    public class MultiTouchView : Control
    {
        private UIElement last;

        static MultiTouchView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiTouchView), new FrameworkPropertyMetadata(typeof(MultiTouchView)));
        }

        /// <summary>
        /// Apply the template defined in Generic.xaml and initialize/handles the touch events
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //Get the touch Area
            UIElement touchArea = (UIElement)GetTemplateChild("_touchArea");

            //Initialize the Multi-Touch Events
            if (touchArea != null)
            {
                touchArea.ManipulationStarting += new EventHandler<ManipulationStartingEventArgs>(touchArea_ManipulationStarting);
                touchArea.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(touchArea_ManipulationDelta);
                touchArea.ManipulationInertiaStarting += new EventHandler<ManipulationInertiaStartingEventArgs>(touchArea_ManipulationInertiaStarting);
            }
        }

        #region Inertia Dependency properties
        /// <summary>
        /// InertiaTranslationDeceleration
        /// </summary>
        public double InertiaTranslationDeceleration
        {
            get { return (double)GetValue(InertiaTranslationDecelerationProperty); }
            set { SetValue(InertiaTranslationDecelerationProperty, value); }
        }

        public static readonly DependencyProperty InertiaTranslationDecelerationProperty =
            DependencyProperty.Register("InertiaTranslationDeceleration", typeof(double), typeof(Control), new UIPropertyMetadata(10.0 * 96.0 / (1000.0 * 1000.0)));

        /// <summary>
        /// InertiaExpansionDeceleration
        /// </summary>
        public double InertiaExpansionDeceleration
        {
            get { return (double)GetValue(InertiaExpansionDecelerationProperty); }
            set { SetValue(InertiaExpansionDecelerationProperty, value); }
        }

        public static readonly DependencyProperty InertiaExpansionDecelerationProperty =
            DependencyProperty.Register("InertiaExpansionTranslationDeceleration", typeof(double), typeof(Control), new UIPropertyMetadata(0.1 * 96 / (1000.0 * 1000.0)));

        /// <summary>
        /// InertiaRotationDeceleration
        /// </summary>
        public double InertiaRotationDeceleration
        {
            get { return (double)GetValue(InertiaRotationDecelerationProperty); }
            set { SetValue(InertiaRotationDecelerationProperty, value); }
        }

        public static readonly DependencyProperty InertiaRotationDecelerationProperty =
            DependencyProperty.Register("InertiaRotationDeceleration", typeof(double), typeof(Control), new UIPropertyMetadata(720 / (1000.0 * 1000.0)));        
 
        #endregion

        #region Multi-Touch and Inertia Events
        /// <summary>
        /// Manipulation Inertia Event
        /// </summary>
        void touchArea_ManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            e.TranslationBehavior = new InertiaTranslationBehavior()
            {
                InitialVelocity = e.InitialVelocities.LinearVelocity,
                DesiredDeceleration = InertiaTranslationDeceleration
            };

            e.ExpansionBehavior = new InertiaExpansionBehavior()
            {
                InitialVelocity = e.InitialVelocities.ExpansionVelocity,
                DesiredDeceleration = InertiaExpansionDeceleration
            };

            e.RotationBehavior = new InertiaRotationBehavior()
            {
                InitialVelocity = e.InitialVelocities.AngularVelocity,
                DesiredDeceleration = InertiaRotationDeceleration
            };
            e.Handled = true;
        }

        /// <summary>
        /// Manipulation Touch Start Event
        /// </summary>
        void touchArea_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            var uie = e.OriginalSource as UIElement;
            if (uie != null)
            {
                if (last != null) Canvas.SetZIndex(last, 0);
                Canvas.SetZIndex(uie, 2);
                last = uie;
            }

            if ((e.Source as FrameworkElement).Parent is Canvas)
            {
                e.ManipulationContainer = (e.Source as FrameworkElement).Parent as Canvas;
                e.Handled = true;
            }
            e.Mode = ManipulationModes.All;             
        }

        /// <summary>
        /// Manipulation Touch Delta
        /// </summary>
        void touchArea_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            var element = e.Source as FrameworkElement;
            if (element != null)
            {
                var deltaManipulation = e.DeltaManipulation;
                var matrix = ((MatrixTransform)element.RenderTransform).Matrix;
                Point center = new Point(element.ActualWidth / 2, element.ActualHeight / 2);
                center = matrix.Transform(center);
                matrix.ScaleAt(deltaManipulation.Scale.X, deltaManipulation.Scale.Y, center.X, center.Y);
                matrix.RotateAt(e.DeltaManipulation.Rotation, center.X, center.Y);
                matrix.Translate(e.DeltaManipulation.Translation.X, e.DeltaManipulation.Translation.Y);
                element.RenderTransform = new MatrixTransform(matrix);

                e.Handled = true;

                if (e.IsInertial)
                {
                    Rect containingRect = new Rect(((FrameworkElement)e.ManipulationContainer).RenderSize);
                    Rect shapeBounds = element.RenderTransform.TransformBounds(new Rect(element.RenderSize));
                    if (e.IsInertial && !containingRect.Contains(shapeBounds))
                    {
                        e.ReportBoundaryFeedback(e.DeltaManipulation);             
                        e.Complete();
                    }
                }
            }
        }
        #endregion
    }
}
