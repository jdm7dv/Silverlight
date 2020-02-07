using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using C1.Silverlight.RichTextBox.Documents;
using Microsoft.Practices.Unity;
using Silverlight.Weblog.Client.CoreBL.Parts;
using Silverlight.Weblog.Client.CoreBL.Widgets;
using Silverlight.Weblog.Client.DAL.Macros;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.UI.UILogic;

namespace Silverlight.Weblog.UI
{
    public partial class MainPage : UserControl, IChangeRootVisual
    {
        [Dependency]
        public Parts Parts { get; set; }

        public MainPage()
        {
            IoC.BuildUp(this);
            IoC.Register<IChangeRootVisual>(this);

            InitializeComponent(); 

            SetupThemeAndWidgets();

            // TODO: Clean this hack up
            IoC.Get<IHtmlInitialData>().Initialize();
        }

        private void SetupThemeAndWidgets()
        {
            ThemePlaceHolder.Content = Parts.Theme;
            foreach (IBlogWidget blogWidget in Parts.Widgets)
            {
                blogWidget.Initialize();
                Parts.Theme.AddChild(blogWidget);
            }
        }

        ////POCRichTextBox.DocumentChanged += new EventHandler(POCRichTextBox_DocumentChanged);
        ////POCRichTextBox.Html = GetHostHtmlBlock("BlogPost Content");
        //void POCRichTextBox_DocumentChanged(object sender, EventArgs e)
        //{
        //    POCRichTextBox.Document.Blocks.Add(new C1BlockUIContainer() { Child = new Button() { Content = "foo", Width = 100, Height = 100 } });

        //    POCRichTextBox.Document.Blocks.Add(new C1BlockUIContainer() { Child = new Button() { Content = "foo", Width = 100, Height = 100 } });
        //}

        #region Implementation of IChangeRootVisual

        private UIElement oldRootVisual = null;
        public void ChangeTo(UIElement NewRootvisual)
        {
            oldRootVisual = (UIElement) ThemePlaceHolder.Content;
            ThemePlaceHolder.Content = NewRootvisual;
        }

        public void RevertToOriginalRootVisual()
        {
            ThemePlaceHolder.Content = oldRootVisual;
            oldRootVisual = null;
        }

        #endregion
    }
}
