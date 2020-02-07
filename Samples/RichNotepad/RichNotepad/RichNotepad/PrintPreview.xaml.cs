using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RichNotepad
{
    public partial class PrintPreview : ChildWindow
    {
        public PrintPreview()
        {
            InitializeComponent();
        }

        public void ShowPreview(RichTextBox rta)
        {            
            WriteableBitmap image = new WriteableBitmap(rta, null);
            previewImage.Source = image;            
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

