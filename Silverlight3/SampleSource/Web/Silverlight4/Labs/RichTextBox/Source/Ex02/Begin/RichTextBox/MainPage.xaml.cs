// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Controls.Primitives;
using System.Text;
using System.Xml.Linq;
using System.Windows.Printing;
using System.Threading;
using System;
using System.Windows.Media.Imaging;
using System.Linq;
using System.IO;
using System.Resources;
using System.Windows.Resources;
using System.Collections.Generic;
using System.Windows.Shapes;

namespace RichNotepad
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            this.DataContext = this;
            IsRTL = Thread.CurrentThread.CurrentUICulture.Parent.Name.ToLower() == "he" || Thread.CurrentThread.CurrentUICulture.Parent.Name.ToLower() == "ar";
            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }


        
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //   ************   TODO    Add sample Xaml file here    ************
            StreamResourceInfo sr = App.GetResourceStream(new
                Uri("/RichNotepad;component/sampleNew.sav", UriKind.Relative));
            StreamReader sread = new StreamReader(sr.Stream);
            string xaml = sread.ReadToEnd();
            rtb.Xaml = xaml;
            sread.Close();


        }


        #region Properties

        public bool IsDirty
        {
            get { return (bool)GetValue(IsDirtyProperty); }
            set { SetValue(IsDirtyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsRTL.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDirtyProperty = DependencyProperty.Register("IsDirty", typeof(bool), typeof(MainPage), null);

        public bool IsRTL
        {
            get { return (bool)GetValue(IsRTLProperty); }
            set { SetValue(IsRTLProperty, value); }
        }

        public static readonly DependencyProperty IsRTLProperty = DependencyProperty.Register("IsRTL", typeof(bool), typeof(MainPage), null);

        #endregion



        //   ************   TODO    Add code for unctionality here    ************





        //  Callback for returning focus to RichTextBox
        private void ReturnFocus()
        {
            if (rtb != null)
                rtb.Focus();
        }


        //   Position TextPointer in Text
        private void PositionHand()
        {
            Rect r;
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                TextPointer tp = rtb.GetPositionFromPoint(lastRTAMouseMove.GetPosition(rtb));
                r = tp.GetCharacterRect(LogicalDirection.Forward);
            }
            else
            {
                r = rtb.Selection.End.GetCharacterRect(LogicalDirection.Forward);
            }

            if (r != Rect.Empty)
            {
                Canvas.SetLeft(caretHand, r.Left);
                Canvas.SetTop(caretHand, r.Bottom);
            }

        }

        private void rtb_SelectionChanged(object sender, RoutedEventArgs e)
        {
            PositionHand();
        }

        private Image CreateImageFromUri(Uri URI, double width, double height)
        {
            Image img = new Image();
            img.Stretch = Stretch.Uniform;
            img.Width = width;
            img.Height = height;
            BitmapImage bi = new BitmapImage(URI);
            img.Source = bi;
            img.Tag = bi.UriSource.ToString();
            return img;
        }

        //Mouse Events BEGIN
        private void mainPanel_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void mainPanel_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            MPContextMenu menu = new MPContextMenu(this);
            menu.Show(e.GetPosition(LayoutRoot));
        }

        private void rtb_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void rtb_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            RTBContextMenu menu = new RTBContextMenu(rtb);
            menu.Show(e.GetPosition(LayoutRoot));
        }
        Rectangle highlightRect;
        MouseEventArgs lastRTAMouseMove;
        private void rtb_MouseMove(object sender, MouseEventArgs e)
        {
            lastRTAMouseMove = e;
            if (showHighlight)
            {
                foreach (Rect r in m_selectionRect)
                {
                    if (r.Contains(e.GetPosition(highlightCanvas)))
                    {
                        if (highlightRect == null)
                        {
                            highlightRect = CreateHighlightRectangle(r);
                        }
                        else
                        {
                            highlightRect.Visibility = System.Windows.Visibility.Visible;
                            highlightRect.Width = r.Width;
                            highlightRect.Height = r.Height;
                            Canvas.SetLeft(highlightRect, r.Left);
                            Canvas.SetTop(highlightRect, r.Top);
                        }
                    }
                }
            }
            PositionHand();
        }

        private Rectangle CreateHighlightRectangle(Rect bounds)
        {
            Rectangle r = new Rectangle();
            r.Fill = new SolidColorBrush(Color.FromArgb(75, 0, 0, 200));
            r.Stroke = new SolidColorBrush(Color.FromArgb(230, 0, 0, 254));
            r.StrokeThickness = 1;
            r.Width = bounds.Width;
            r.Height = bounds.Height;
            Canvas.SetLeft(r, bounds.Left);
            Canvas.SetTop(r, bounds.Top);

            highlightCanvas.Children.Add(r);

            return r;

        }

        // Right Button Events
        private void rtb_DragEnter(object sender, System.Windows.DragEventArgs e)
        {
            VisualStateManager.GoToState(this, "DragOver", true);
        }

        private void rtb_DragLeave(object sender, System.Windows.DragEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", true);
        }

        private void rtb_KeyUp(object sender, KeyEventArgs e)
        {
            if (rtb.Blocks.Count > 1 || (rtb.Blocks.Count == 1 && (rtb.Blocks[0] as Paragraph).Inlines.Count > 0))
                IsDirty = true;
            else
                IsDirty = false;
        }


        //Mouse Events END


        // Highlight Feature
        private bool showHighlight = false;
        private List<Rect> m_selectionRect = new List<Rect>();
        public void btnHighlight_Checked(object sender, RoutedEventArgs e)
        {
            if (!showHighlight)
            {
                showHighlight = true;

                TextPointer tp = rtb.ContentStart;
                TextPointer nextTp = null;
                Rect nextRect = Rect.Empty;
                Rect tpRect = tp.GetCharacterRect(LogicalDirection.Forward);
                Rect lineRect = Rect.Empty;

                int lineCount = 1;

                while (tp != null)
                {
                    nextTp = tp.GetNextInsertionPosition(LogicalDirection.Forward);
                    if (nextTp != null && nextTp.IsAtInsertionPosition)
                    {
                        nextRect = nextTp.GetCharacterRect(LogicalDirection.Forward);
                        // this occurs for more than one line
                        if (nextRect.Top > tpRect.Top)
                        {
                            if (m_selectionRect.Count < lineCount)
                                m_selectionRect.Add(lineRect);
                            else
                                m_selectionRect[lineCount - 1] = lineRect;

                            lineCount++;

                            if (m_selectionRect.Count < lineCount)
                                m_selectionRect.Add(nextRect);

                            lineRect = nextRect;
                        }
                        else if (nextRect != Rect.Empty)
                        {
                            if (tpRect != Rect.Empty)
                                lineRect.Union(nextRect);
                            else
                                lineRect = nextRect;
                        }
                    }
                    tp = nextTp;
                    tpRect = nextRect;
                }
                if (lineRect != Rect.Empty)
                {
                    if (m_selectionRect.Count < lineCount)
                        m_selectionRect.Add(lineRect);
                    else
                        m_selectionRect[lineCount - 1] = lineRect;
                }
                while (m_selectionRect.Count > lineCount)
                {
                    m_selectionRect.RemoveAt(m_selectionRect.Count - 1);
                    //DeleteRect();
                }
            }
            else
            {
                showHighlight = false;
                if (highlightRect != null)
                {
                    highlightRect.Visibility = System.Windows.Visibility.Collapsed;
                }
            }

        }

        // Convert to XAML Button
        public void btnMarkUp_Checked(object sender, RoutedEventArgs e)
        {


        }

 

            //  TODO   Add btnRTL_Checked code here
        public void btnRTL_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void btnBold_Click(object sender, RoutedEventArgs e)
        {
            if (rtb != null && rtb.Selection.Text.Length > 0)
            {
                if (rtb.Selection.GetPropertyValue(Run.FontWeightProperty) is FontWeight &&
                    ((FontWeight)rtb.Selection.GetPropertyValue(Run.FontWeightProperty)) == FontWeights.Normal)
                    rtb.Selection.ApplyPropertyValue(Run.FontWeightProperty, FontWeights.Bold);
                else
                    rtb.Selection.ApplyPropertyValue(Run.FontWeightProperty, FontWeights.Normal);

            }
            ReturnFocus();
        }

        private void btnItalic_Click(object sender, RoutedEventArgs e)
        {
            if (rtb != null && rtb.Selection.Text.Length > 0)
            {
                if (rtb.Selection.GetPropertyValue(Run.FontStyleProperty) is FontStyle &&
                    ((FontStyle)rtb.Selection.GetPropertyValue(Run.FontStyleProperty)) == FontStyles.Normal)
                    rtb.Selection.ApplyPropertyValue(Run.FontStyleProperty, FontStyles.Italic);
                else
                    rtb.Selection.ApplyPropertyValue(Run.FontStyleProperty, FontStyles.Normal);
            }
            ReturnFocus();

        }

        private void btnUnderline_Click(object sender, RoutedEventArgs e)
        {
            if (rtb != null && rtb.Selection.Text.Length > 0)
            {
                if (rtb.Selection.GetPropertyValue(Run.TextDecorationsProperty) == null)
                    rtb.Selection.ApplyPropertyValue(Run.TextDecorationsProperty, TextDecorations.Underline);
                else
                    rtb.Selection.ApplyPropertyValue(Run.TextDecorationsProperty, null);
            }
            ReturnFocus();

        }

        private void cmbFontSizes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rtb != null && rtb.Selection.Text.Length > 0)
            {
                rtb.Selection.ApplyPropertyValue(Run.FontSizeProperty,
                    double.Parse((cmbFontSizes.SelectedItem as ComboBoxItem).Tag.ToString()));
            }
            ReturnFocus();


        }

        private void cmbFonts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rtb != null && rtb.Selection.Text.Length > 0)
            {
                rtb.Selection.ApplyPropertyValue(Run.FontFamilyProperty,
                    new FontFamily((cmbFonts.SelectedItem as ComboBoxItem).Tag.ToString()));
            }
            ReturnFocus();


        }

        private void cmbFontColors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rtb != null && rtb.Selection.Text.Length > 0)
            {
                string color = (cmbFontColors.SelectedItem as ComboBoxItem).Tag.ToString();

                SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(
                    byte.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(6, 2), System.Globalization.NumberStyles.HexNumber)));

                rtb.Selection.ApplyPropertyValue(Run.ForegroundProperty, brush);
            }

        }

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            InlineUIContainer container = new InlineUIContainer();
            container.Child = getImage();
            rtb.Selection.Insert(container);
            ReturnFocus();
        }

        private Image getImage()
        {
            return CreateImageFromUri(new Uri("desert.jpg", UriKind.RelativeOrAbsolute), 200, 150);
        }

        private void btnHyperlink_Click(object sender, RoutedEventArgs e)
        {
            InsertURL cw = new InsertURL(rtb.Selection.Text);
            cw.HasCloseButton = false;
            cw.Closed += (s, args) =>
            {
                if (cw.DialogResult.Value)
                {
                    Hyperlink hyperlink = new Hyperlink();
                    hyperlink.TargetName = "_blank";
                    hyperlink.NavigateUri = new Uri(cw.txtURL.Text);

                    if (cw.txtURLDesc.Text.Length > 0)
                        hyperlink.Inlines.Add(cw.txtURLDesc.Text);
                    else
                        hyperlink.Inlines.Add(cw.txtURL.Text);

                    rtb.Selection.Insert(hyperlink);
                }
            };

            cw.Show();
        }

        private void btnDatagrid_Click(object sender, RoutedEventArgs e)
        {
            InlineUIContainer container = new InlineUIContainer();

            container.Child = getDataGrid();

            rtb.Selection.Insert(container);
            ReturnFocus();
        }

        private DataGrid getDataGrid()
        {
            DataGrid dg = new DataGrid();
            dg.AutoGenerateColumns = true;
            dg.Width = 500;
            dg.Height = 150;
            dg.ItemsSource = Customer.GetSampleCustomerList();
            dg.Style = (Style)this.Resources["DataGridStyle1"];

            return dg;
        }

        private void btnCalendar_Click(object sender, RoutedEventArgs e)
        {
            InlineUIContainer container = new InlineUIContainer();
            container.Child = getCalendar();
            rtb.Selection.Insert(container);
            ReturnFocus();
        }

        private Calendar getCalendar()
        {
            Calendar cal = new Calendar();
            cal.Width = 179;
            cal.Height = 169;
            cal.FontFamily = new FontFamily("Portable User Interface");
            cal.Style = (Style)this.Resources["CalendarStyle1"];

            return cal;
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            bool clear = true;
            if (IsDirty)
            {
                if (MessageBox.Show("All changes will be lost. Continue?",
                    "Continue", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                {
                    clear = false;
                }
            }

            if (clear)
            {
                rtb.Blocks.Clear();
                IsDirty = false;
            }
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string xaml = rtb.Xaml;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".sav";
            sfd.Filter = "Saved Files|*.sav|All Files|*.*";
            if (sfd.ShowDialog().Value)
            {
                using (FileStream fs = (FileStream)sfd.OpenFile())
                {
                    System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                    byte[] buffer = enc.GetBytes(xaml);
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Close();
                    IsDirty = false;
                }
            }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "Saved Files|*.sav|All Files|*.*";

            if (ofd.ShowDialog().Value)
            {
                FileInfo fi = ofd.File;
                using (StreamReader reader = fi.OpenText())
                {
                    rtb.Xaml = reader.ReadToEnd();
                }
            }
        }
    }
}