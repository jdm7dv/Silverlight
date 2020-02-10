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
    public partial class ucTemp : Application
    {

        public ucTemp()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Load the main control here
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {

        }


        [
            TemplatePart(Name = "RootElement", Type = typeof(FrameworkElement)),
            TemplatePart(Name = "FocusVisualElement", Type = typeof(UIElement)),

            TemplatePart(Name = "Normal State", Type = typeof(Storyboard)), 
            TemplatePart(Name = "MouseOver State", Type = typeof(Storyboard)), 
            TemplatePart(Name = "Pressed State", Type = typeof(Storyboard)), 
            TemplatePart(Name = "Disabled State", Type = typeof(Storyboard))
        ]
        public class Button : ButtonBase
        {
            // Fields
            internal UIElement _elementFocusVisual;
            internal FrameworkElement _elementRoot;
            internal Storyboard _stateDisabled;
            internal Storyboard _stateMouseOver;
            internal Storyboard _stateNormal;
            internal Storyboard _statePressed;
            internal const string ElementFocusVisualName = "FocusVisualElement";
            internal const string ElementRootName = "RootElement";
            internal const string StateDisabledName = "Disabled State";
            internal const string StateMouseOverName = "MouseOver State";
            internal const string StateNormalName = "Normal State";
            internal const string StatePressedName = "Pressed State";

            // Methods
            public Button();
            internal override void ChangeVisualState();
            protected override void OnApplyTemplate();
            protected override void OnIsEnabledChanged(bool isEnabled);
        }

 


    }
}
