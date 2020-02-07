using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Silverlight.Weblog.Client.CoreBL.Widgets;
using Silverlight.Weblog.Client.Default.Widgets.BaseUI;

namespace Silverlight.Weblog.Client.Default.Widgets
{
    [Export(typeof(IBlogWidget))]
    public partial class CommentWidget : UserControlBase
    {
        public ICommentWidgetViewModel ViewModel { get; set; }

        public CommentWidget()
        {
            // Required to initialize variables
            InitializeComponent();
        }

        public override int Row
        {
            get
            {
                return 3;
            }
        }

        public override int Column
        {
            get
            {
                return 1;
            }
        }

        private bool _isActive = false;
        public override bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                RaiseIsActiveChanged();
            }
        }

        private void ButtonClickHack(object sender, System.Windows.RoutedEventArgs e)
        {
            Button ClickButton = (Button)sender;
            Grid parent = (Grid)ClickButton.Parent;
            bool IsValid = true;

            // hack #1: perform validation on all textboxes nested in the parent
            // hack #2: Lock the button if entity is valid
            foreach (UIElement child in parent.Children)
            {
                if (child is TextBox)
                {
                    TextBox TextBoxToValidate = (TextBox)child;
                    TextBoxToValidate.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                    IsValid = IsValid && Validation.GetHasError(TextBoxToValidate);
                }
            }

            ClickButton.IsEnabled = !IsValid;
        }
    }
}