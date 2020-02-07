using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ReflectionIT.Silverlight.CountDown
{
	public partial class Flipper : UserControl
	{
		public Flipper()
		{
			// Required to initialize variables
			InitializeComponent();
		}


        #region Text1 Dependency Property

        /// <summary> 
        /// Get or Sets the Text1 dependency property.  
        /// </summary> 
        public string Text1 {
            get { return (string)GetValue(Text1Property); }
            set { SetValue(Text1Property, value); }
        }

        /// <summary> 
        /// Identifies the Text1 dependency property. This enables animation, styling, binding, etc...
        /// </summary> 
        public static readonly DependencyProperty Text1Property =
            DependencyProperty.Register("Text1",
                                        typeof(string),
                                        typeof(Flipper),
                                        new PropertyMetadata("00", OnText1PropertyChanged));

        /// <summary>
        /// Text1 changed handler. 
        /// </summary>
        /// <param name="d">MainPage that changed its Text1.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param> 
        private static void OnText1PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var _MainPage = d as Flipper;
            if (_MainPage != null) {
                _MainPage.textBlockBottom.Text = (string)e.NewValue;
                _MainPage.textBlockFlipTop.Text = (string)e.NewValue;
            }
        }

        #endregion Text1 Dependency Property


        #region Text2 Dependency Property

        /// <summary> 
        /// Get or Sets the Text2 dependency property.  
        /// </summary> 
        public string Text2 {
            get { return (string)GetValue(Text2Property); }
            set { SetValue(Text2Property, value); }
        }

        /// <summary> 
        /// Identifies the Text2 dependency property. This enables animation, styling, binding, etc...
        /// </summary> 
        public static readonly DependencyProperty Text2Property =
            DependencyProperty.Register("Text2",
                                        typeof(string),
                                        typeof(Flipper),
                                        new PropertyMetadata("01", OnText2PropertyChanged));

        /// <summary>
        /// Text2 changed handler. 
        /// </summary>
        /// <param name="d">MainPage that changed its Text2.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param> 
        private static void OnText2PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var _MainPage = d as Flipper;
            if (_MainPage != null) {
                _MainPage.textBlockTop.Text = (string)e.NewValue;
                _MainPage.textBlockFlipBottom.Text = (string)e.NewValue;
            }
        }

        #endregion Text2 Dependency Property


        public void Flip(string text1, string text2) {
            Text1 = text1;
            Text2 = text2;
            Storyboard1.Begin();
        }

        private void SizeChaned(object sender, System.Windows.SizeChangedEventArgs e)
        {
        	LayoutRoot.Height = this.ActualHeight;
        }
        
	}
}