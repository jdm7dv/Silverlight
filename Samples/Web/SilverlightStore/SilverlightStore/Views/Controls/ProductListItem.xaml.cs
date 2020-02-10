namespace SilverlightStore.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public partial class ProductListItem : UserControl
    {
        public ProductListItem()
        {
            InitializeComponent();
        }

        private void ProductItemGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "MouseEnterState", true);            
        }

        private void ProductItemGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "MouseLeaveState", true);
        }
    }
}
