using System.Windows;
using System.Windows.Controls;
using MultiTouch.Behaviors.Silverlight4;

namespace MultiTouch.Controls.Silverlight4
{
    /// <summary>
    /// Silverlight Multi Touch enabler
    /// </summary>
    public class ManipulableControl : Control
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ManipulableControl()
        {
            this.DefaultStyleKey = typeof(ManipulableControl);

            //Add the MultiTouchManipulation behavior
            System.Windows.Interactivity.Interaction.GetBehaviors(this).Add(new MultiTouchManipulationBehavior()
            {
                //Not used
                /*
                TouchRotateEnabled = this.TouchRotateEnabled,
                TouchScaleEnabled = this.TouchScaleEnabled,
                TouchTranslateEnabled = this.TouchTranslateEnabled,
                InertiaEnabled = this.InertiaEnabled
                */
            });
        }

        /// <summary>
        /// Apply the template defined in Generic.xaml and initialize/handles the touch events
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        //TODO: Dependency Properties Not used
        /*
        //Dependency property for enabling the Inertia Manipulation
        public static readonly DependencyProperty InertiaEnabledProperty =
            DependencyProperty.Register("InertiaEnabled", typeof(bool),
                                        typeof(MultiTouchManipulationBehavior), new PropertyMetadata(true));

        public bool InertiaEnabled
        {
            get { return (bool)GetValue(InertiaEnabledProperty); }
            set { SetValue(InertiaEnabledProperty, value); }
        }

        //Dependency property for enabling the TouchTranslate
        public static readonly DependencyProperty TouchTranslateEnabledProperty =
            DependencyProperty.Register("TouchTranslateEnabled", typeof(bool),
                                        typeof(ManipulableControl), new PropertyMetadata(true));

        public bool TouchTranslateEnabled
        {
            get { return (bool)GetValue(TouchTranslateEnabledProperty); }
            set { SetValue(TouchTranslateEnabledProperty, value); }
        }

        //Dependency property for enabling the TouchScale
        public static readonly DependencyProperty TouchScaleEnabledProperty =
            DependencyProperty.Register("TouchScaleEnabled", typeof(bool),
                                        typeof(ManipulableControl), new PropertyMetadata(true));

        public bool TouchScaleEnabled
        {
            get { return (bool)GetValue(TouchScaleEnabledProperty); }
            set { SetValue(TouchScaleEnabledProperty, value); }
        }

        //Dependency property for enabling the TouchZoom
        public static readonly DependencyProperty TouchRotateEnabledProperty =
            DependencyProperty.Register("TouchRotateEnabled", typeof(bool),
                                        typeof(ManipulableControl), new PropertyMetadata(true));

        public bool TouchRotateEnabled
        {
            get { return (bool)GetValue(TouchRotateEnabledProperty); }
            set { SetValue(TouchRotateEnabledProperty, value); }
        }
        */
    }
}
