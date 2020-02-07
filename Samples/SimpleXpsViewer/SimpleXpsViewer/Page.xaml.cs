using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Resources;
using System.Globalization;

namespace SimpleXpsViewer
{
    public partial class Page : UserControl
    {
        private const double MinimumScale = 0.2;
        private const double MaximumScale = 4.0;

        private string prevPageText = "1";

        private int pageNr = 1;
        private double pageScale = 1;
        private Point? lastMousePosition;
        private bool firstPageLoaded = false;
        private XpsReader reader;
        private WebClient webClient = new WebClient();

        public Page()
        {
            InitializeComponent();

            this.cmbSelect.Items.Add(new XpsInfo() { Name = "Intro.xps"});
            
            this.cmbSelect.SelectedIndex = 0;

            HtmlPage.Document.AttachEvent("onmousewheel", OnMouseWheel);
            this.SizeChanged += OnSizeChanged;
            this.contentPanel.MouseLeftButtonDown += contentPanel_MouseLeftButtonDown;
            this.contentPanel.MouseLeftButtonUp += contentPanel_MouseLeftButtonUp;
            this.contentPanel.MouseMove += contentPanel_MouseMove;
            this.contentPanel.MouseLeave += contentPanel_MouseLeave;

            this.webClient.DownloadProgressChanged += webClient_DownloadProgressChanged;
            this.webClient.OpenReadCompleted += webClient_OpenReadCompleted;
        }

        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            SetStatus(string.Format("Downloading {0:##.#}%", ((100d * e.BytesReceived) / e.TotalBytesToReceive)), false);
        }

        private void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null) {
                try {
                    DisplayXps(XpsReader.Create(new StreamResourceInfo(e.Result, null)));
                }
                catch (Exception ex) {
                    SetStatus("Xps read failed: " + ex.Message, true);
                }
            }
            else {
                SetStatus(string.Format("Download failed: {0}", e.Error.Message), true);
            }
        }

        private void OnMouseWheel(object sender, HtmlEventArgs e)
        {
            double delta = (double)e.EventObject.GetProperty("wheelDelta");
            Zoom(this.pageScale * (delta < 0 ? .9 : 1 / .9));
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            CenterPage();
        }

        private void CenterPage()
        {
            if (pageContents.Child != null) {
                Zoom(Math.Min(contentPanel.ActualWidth / page.ActualWidth, contentPanel.ActualHeight / page.ActualHeight));
                scale.CenterX = page.ActualWidth / 2;
                scale.CenterY = page.ActualHeight / 2;

                Canvas.SetLeft(page, (contentPanel.ActualWidth - page.ActualWidth) / 2);
                Canvas.SetTop(page, (contentPanel.ActualHeight - page.ActualHeight) / 2);
            }
        }

        private void SetStatus(string status, bool error)
        {
            this.txtStatus.Text = status;
            this.txtStatus.Foreground = error ? new SolidColorBrush(Colors.Red) : this.Foreground;
        }

        private void DisplayXps(XpsReader reader)
        {
            if (this.reader != null) {
                this.reader.Close();
            }

            this.reader = reader;
            this.pageNr = 1;
            this.firstPageLoaded = false;
            this.page.Opacity = 0;

            DisplayPage();
        }

        private void DisplayPage()
        {
            if (this.reader != null && this.pageNr > 0 && this.pageNr <= this.reader.PageCount) {
                txtPage.Text = string.Format("{0}", this.pageNr);
                txtTotal.Text = string.Format("of {0}", this.reader.PageCount);
                btnNext.IsEnabled = this.pageNr < this.reader.PageCount;
                btnPrev.IsEnabled = this.pageNr > 1;

                try {
                    FrameworkElement content = this.reader.ParsePage(this.pageNr);
                    content.SizeChanged += content_SizeChanged;
                    pageContents.Child = content;

                    SetStatus("Status: Ready", false);
                }
                catch (Exception e) {
                    SetStatus("Error: " + e.Message, true);
                }
            }
        }

        private bool OpenXpsFromFile(FileInfo fileInfo)
        {
            try {
                DisplayXps(XpsReader.Create(fileInfo));
                return true;
            }
            catch (Exception ex) {
                SetStatus("Open failed: " + ex.Message, true);
                return false;
            }
        }

        private void contentPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.lastMousePosition = e.GetPosition(contentPanel);
        }

        private void contentPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.lastMousePosition = null;
        }

        private void contentPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.lastMousePosition != null) {
                // Compute the delta from the last position (adjusted for current scale)
                var position = e.GetPosition(contentPanel);
                var dx = (position.X - lastMousePosition.Value.X) / pageScale;
                var dy = (position.Y - lastMousePosition.Value.Y) / pageScale;
                lastMousePosition = position;

                // Move the page accordingly
                page.SetValue(Canvas.LeftProperty, (double)page.GetValue(Canvas.LeftProperty) + dx);
                page.SetValue(Canvas.TopProperty, (double)page.GetValue(Canvas.TopProperty) + dy);

                // Update the center of the scale transform
                scale.CenterX -= dx;
                scale.CenterY -= dy;
            }
        }

        private void contentPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            this.lastMousePosition = null;
        }

        private void content_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!firstPageLoaded) {
                CenterPage();
                sbShowPage.Begin();
            }
            firstPageLoaded = true;
        }

        private void Zoom(double s)
        {
            this.pageScale = Math.Max(MinimumScale, Math.Min(MaximumScale, s));
            scale.ScaleX = pageScale;
            scale.ScaleY = pageScale;

            txtZoom.Text = string.Format("{0:0}%", pageScale*100);
            btnZoomIn.IsEnabled = pageScale < MaximumScale;
            btnZoomOut.IsEnabled = pageScale > MinimumScale;
        }

        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            Zoom(this.pageScale / .9);
        }

        private void btnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            Zoom(this.pageScale * .9);
        }

        private void txtPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) {
                this.pageNr = int.Parse(txtPage.Text, CultureInfo.InvariantCulture);
                DisplayPage();
            }
        }

        private void txtPage_TextChanged(object sender, TextChangedEventArgs e)
        {
            try {
                int nr = int.Parse(txtPage.Text, CultureInfo.InvariantCulture);
                if (this.reader == null || nr < 1 || nr > this.reader.PageCount) {
                    ResetPageText();
                }
                else {
                    prevPageText = txtPage.Text;
                }
            }
            catch (Exception) {
                ResetPageText();
            }
        }

        private void ResetPageText()
        {
            int start = txtPage.SelectionStart;
            int length = txtPage.SelectionLength;

            txtPage.Text = prevPageText;
            txtPage.SelectionStart = start;
            txtPage.SelectionLength = length;
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (this.pageNr > 1) {
                this.pageNr--;
                DisplayPage();
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (this.reader != null && this.pageNr < this.reader.PageCount) {
                this.pageNr++;
                DisplayPage();
            }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() ?? false) {
                if (OpenXpsFromFile(dlg.File)) {
                    XpsInfo info = new XpsInfo() { Name = dlg.File.Name, File = dlg.File };
                    this.cmbSelect.Items.Add(info);
                    this.cmbSelect.SelectedItem = info;
                }
            }
        }

        private void cmbSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            XpsInfo item = (XpsInfo)cmbSelect.SelectedItem;

            if (item.File != null) {
                OpenXpsFromFile(item.File);
            }
            else {
                // download
                SetStatus("Downloading...", false);
                this.webClient.OpenReadAsync(new Uri(HtmlPage.Document.DocumentUri, item.Name));
            }
        }
    }
}
