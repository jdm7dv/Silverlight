namespace LinkLabelTest
{
    using System.Windows;
    using System.Windows.Controls;
    using CompletIT.Windows;
    using System;

    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
            this.MyLinkLabel.LinkClick += new EventHandler<LinkClickEventArgs>( this.MyLinkLabel_LinkClick );
        }

        private void MyLinkLabel_LinkClick( object sender, CompletIT.Windows.LinkClickEventArgs e )
        {
            if ( !string.IsNullOrEmpty( e.Action ) )
            {
                MessageBox.Show( "You just clicked a link with a custom action: " + e.Action, e.Action, MessageBoxButton.OK );
            }
        }

        private void Process( object sender, RoutedEventArgs e )
        {
            this.MyLinkLabel.Text = this.TextToProccess.Text;
        }
    }
}
