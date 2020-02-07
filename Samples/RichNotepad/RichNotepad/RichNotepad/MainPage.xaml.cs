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
            System.Xml.XmlReader r = System.Xml.XmlReader.Create("/RichNotepad;component/sample.sav");

            DocumentPersister.parseSavedDocument(r, rta.Blocks);
            r.Close();
        }

        #region Properties

        public bool IsDirty
        {
            get { return (bool)GetValue(IsDirtyProperty); }
            set { SetValue(IsDirtyProperty, value); }
        }

        public static readonly DependencyProperty IsDirtyProperty = DependencyProperty.Register("IsDirty", typeof(bool), typeof(MainPage), null);

        public bool IsRTL
        {
            get { return (bool)GetValue(IsRTLProperty); }
            set { SetValue(IsRTLProperty, value); }
        }

        public static readonly DependencyProperty IsRTLProperty = DependencyProperty.Register("IsRTL", typeof(bool), typeof(MainPage), null);

        #endregion

        private void ReturnFocus()
        {
            if (rta != null)
                rta.Focus();
        }

        private void btnBold_Click(object sender, RoutedEventArgs e)
        {
            if (rta != null && rta.Selection.Text.Length > 0)
            {
                if (rta.Selection.GetPropertyValue(Run.FontWeightProperty) is FontWeight && ((FontWeight)rta.Selection.GetPropertyValue(Run.FontWeightProperty)) == FontWeights.Normal)
                    rta.Selection.ApplyPropertyValue(Run.FontWeightProperty, FontWeights.Bold);
                else
                    rta.Selection.ApplyPropertyValue(Run.FontWeightProperty, FontWeights.Normal);
            }
            ReturnFocus();
        }

        private void btnItalic_Click(object sender, RoutedEventArgs e)
        {
            if (rta != null && rta.Selection.Text.Length > 0)
            {
                if (rta.Selection.GetPropertyValue(Run.FontStyleProperty) is FontStyle && ((FontStyle)rta.Selection.GetPropertyValue(Run.FontStyleProperty)) == FontStyles.Normal)
                    rta.Selection.ApplyPropertyValue(Run.FontStyleProperty, FontStyles.Italic);
                else
                    rta.Selection.ApplyPropertyValue(Run.FontStyleProperty, FontStyles.Normal);
            }
            ReturnFocus();
        }

        private void btnUnderline_Click(object sender, RoutedEventArgs e)
        {
            if (rta != null && rta.Selection.Text.Length > 0)
            {
                if (rta.Selection.GetPropertyValue(Run.TextDecorationsProperty) == null)
                    rta.Selection.ApplyPropertyValue(Run.TextDecorationsProperty, TextDecorations.Underline);
                else
                    rta.Selection.ApplyPropertyValue(Run.TextDecorationsProperty, null);
            }
            ReturnFocus();

        }

        private void cmbFonts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rta != null && rta.Selection.Text.Length > 0)
            {
                rta.Selection.ApplyPropertyValue(Run.FontFamilyProperty, new FontFamily((cmbFonts.SelectedItem as ComboBoxItem).Tag.ToString()));
            }
            ReturnFocus();
        }

        private void cmbFontSizes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rta != null && rta.Selection.Text.Length > 0)
            {
                rta.Selection.ApplyPropertyValue(Run.FontSizeProperty, double.Parse((cmbFontSizes.SelectedItem as ComboBoxItem).Tag.ToString()));
            }
            ReturnFocus();
        }

        private void cmbFontColors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rta != null && rta.Selection.Text.Length > 0)
            {
                string color = (cmbFontColors.SelectedItem as ComboBoxItem).Tag.ToString();

                SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(
                    byte.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(6, 2), System.Globalization.NumberStyles.HexNumber)));

                rta.Selection.ApplyPropertyValue(Run.ForegroundProperty, brush);
            }
            ReturnFocus();
        }

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            InlineUIContainer container = new InlineUIContainer();

            container.Child = getImage();

            rta.Selection.Insert(container);
            ReturnFocus();
        }

        private void btnDatagrid_Click(object sender, RoutedEventArgs e)
        {
            InlineUIContainer container = new InlineUIContainer();

            container.Child = getDataGrid();

            rta.Selection.Insert(container);
            ReturnFocus();
        }

        private void btnCalendar_Click(object sender, RoutedEventArgs e)
        {
            InlineUIContainer container = new InlineUIContainer();

            container.Child = getCalendar();

            rta.Selection.Insert(container);
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

        private Image getImage()
        {
            return DocumentPersister.createImageFromUri(new Uri("desert.jpg", UriKind.RelativeOrAbsolute), 200, 150);
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

        private void btnHyperlink_Click(object sender, RoutedEventArgs e)
        {
            InsertURL cw = new InsertURL(rta.Selection.Text);
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

                    rta.Selection.Insert(hyperlink);
                    ReturnFocus();
                }
            };
            cw.Show();
        }

        private void btnCut_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(rta.Selection.Text);
            rta.Selection.Text = "";
            ReturnFocus();
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(rta.Selection.Text);
            ReturnFocus();
        }

        private void btnPaste_Click(object sender, RoutedEventArgs e)
        {
            rta.Selection.Text = Clipboard.GetText();
            ReturnFocus();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintPreview cw = new PrintPreview();
            cw.ShowPreview(rta);
            cw.HasCloseButton = false;
            cw.Closed += (t, a) =>
            {
                if (cw.DialogResult.Value)
                {
                    PrintDocument theDoc = new PrintDocument();
                    RichTextBox rta2 = new RichTextBox();
                    rta2.Width = 816;
                    rta2.TextWrapping = TextWrapping.Wrap;
                    rta.FontSize = 20;
                    Paragraph p = null;
                    Paragraph bp = null;
                    InlineUIContainer iuic = null;
                    Run r = null;
                    Run br = null;
                    for(int i = 0; i < rta.Blocks.Count; i++)
                    {
                        if (rta.Blocks[i] is Paragraph)
                        {
                            p = new Paragraph();
                            bp = ((Paragraph)rta.Blocks[i]);
                            for (int j = 0; j < bp.Inlines.Count; j++)
                            {
                                if (bp.Inlines[j] is Run)
                                {
                                    br = ((Run)bp.Inlines[j]);

                                    r = new Run();
                                    r.Text = br.Text;
                                    r.FontFamily = br.FontFamily;
                                    r.FontSize = br.FontSize;
                                    r.FontStretch = br.FontStretch;
                                    r.FontStyle = br.FontStyle;
                                    r.FontWeight = br.FontWeight;
                                    r.Foreground = br.Foreground;
                                    r.TextDecorations = br.TextDecorations;
                                    p.Inlines.Add(r);
                                }
                                if (bp.Inlines[j] is InlineUIContainer)
                                {
                                    iuic = new InlineUIContainer();
                                    if (((InlineUIContainer)bp.Inlines[j]).Child is Image)
                                    {
                                        iuic.Child = getImage();
                                    }
                                    else if (((InlineUIContainer)bp.Inlines[j]).Child is DataGrid)
                                    {
                                        iuic.Child = getDataGrid();
                                    }
                                    else if (((InlineUIContainer)bp.Inlines[j]).Child is Calendar)
                                    {
                                        iuic.Child = getCalendar();
                                    }
                                    p.Inlines.Add(iuic);
                                }
                            }
                            rta2.Blocks.Add(p);
                        }
                    }                         

                    theDoc.PrintPage += (s, args) =>
                    {
                        //Size sz = args.PrintableArea;

                        //layout again
                        //WriteableBitmap wb = new WriteableBitmap(rta2, null);
                        //Image previewImage = new Image();
                        //previewImage.Source = wb;
                        //args.PageVisual = previewImage;
                        args.PageVisual = rta2;
                        args.HasMorePages = false;
                    };

                    theDoc.EndPrint += (s, args) =>
                    {
                        MessageBox.Show("The document printed successfully", "Text Editor", MessageBoxButton.OK);
                    };

                    theDoc.Print("Silverlight 4 Text Editor");
                    ReturnFocus();
                }
            };
            cw.Show();
        }

        public void btnRTL_Checked(object sender, RoutedEventArgs e)
        {
            IsRTL = !IsRTL;
            if(IsRTL)
                btnRTL.Content = DocumentPersister.createImageFromUri(new Uri("/RichNotepad;component/images/rtl.png", UriKind.RelativeOrAbsolute), 30, 32);
            else
                btnRTL.Content = DocumentPersister.createImageFromUri(new Uri("/RichNotepad;component/images/ltr.png", UriKind.RelativeOrAbsolute), 30, 32);
            ReturnFocus();
            
        }

        public void btnMarkUp_Checked(object sender, RoutedEventArgs e)
        {
            if (btnMarkUp.IsChecked.Value)
            {
                xamlTb.Visibility = System.Windows.Visibility.Visible;
                xamlTb.IsTabStop = true;
                xamlTb.Text = rta.Xaml;
            }
            else
            {
                rta.Xaml = xamlTb.Text;
                xamlTb.Visibility = System.Windows.Visibility.Collapsed;
                xamlTb.IsTabStop = false;
            }

        }
        private bool showHighlight = false;
        private List<Rect> m_selectionRect = new List<Rect>();
        public void btnHighlight_Checked(object sender, RoutedEventArgs e)
        {
            if (!showHighlight)
            {
                showHighlight = true;

                TextPointer tp = rta.ContentStart;
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

        public void btnRO_Checked(object sender, RoutedEventArgs e)
        {
            rta.IsReadOnly = !rta.IsReadOnly;
            if (rta.IsReadOnly)
                btnRO.Content = DocumentPersister.createImageFromUri(new Uri("/RichNotepad;component/images/view.png", UriKind.RelativeOrAbsolute), 29, 32);
            else
                btnRO.Content = DocumentPersister.createImageFromUri(new Uri("/RichNotepad;component/images/edit.png", UriKind.RelativeOrAbsolute), 29, 32);
            ReturnFocus();
        }

        private void mainPanel_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void mainPanel_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            MPContextMenu menu = new MPContextMenu(this);
            menu.Show(e.GetPosition(LayoutRoot));
        }

        private void rta_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void rta_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            RTAContextMenu menu = new RTAContextMenu(rta);
            menu.Show(e.GetPosition(LayoutRoot));
        }
        Rectangle highlightRect;
        MouseEventArgs lastRTAMouseMove;
        private void rta_MouseMove(object sender, MouseEventArgs e)
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

        private void rta_Drop(object sender, System.Windows.DragEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", true);
            if (e.Data != null)
            {
                IDataObject f = e.Data as IDataObject;

                if (f != null)
                {
                    object data = f.GetData(DataFormats.FileDrop);
                    FileInfo[] files = data as FileInfo[];

                    if (files != null)
                    {
                        foreach (FileInfo file in files)
                        {
                            if (file != null)
                            {
                                if (file.Extension.Equals(".txt"))
                                {
                                    Stream sr = file.OpenRead();
                                    string contents;
                                    using (StreamReader reader = new StreamReader(sr))
                                    {
                                        contents = reader.ReadToEnd();
                                    }
                                    rta.Selection.Text = contents;                                    
                                }
                                else if (file.Extension.Equals(".docx"))
                                {
                                    Stream sr = file.OpenRead();
                                    string contents;                                    

                                    StreamResourceInfo zipInfo = new StreamResourceInfo(sr, null);
                                    StreamResourceInfo wordInfo = Application.GetResourceStream(zipInfo, new Uri("word/document.xml", UriKind.Relative));

                                    using (StreamReader reader = new StreamReader(wordInfo.Stream))
                                    {
                                        contents = reader.ReadToEnd();
                                    }
                                    XDocument xmlFile = XDocument.Parse(contents);
                                    XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

                                    var query = from xp in xmlFile.Descendants(w + "p")
                                                select xp;
                                    Paragraph p = null;
                                    Run r = null;
                                    foreach (XElement xp in query)
                                    {
                                        p = new Paragraph();
                                        var query2 = from xr in xp.Descendants(w + "r")
                                                    select xr;
                                        foreach (XElement xr in query2)
                                        {
                                            r = new Run();
                                            var query3 = from xt in xr.Descendants()
                                                         select xt;
                                            foreach (XElement xt in query3)
                                            {
                                                if(xt.Name == (w + "t"))
                                                    r.Text = xt.Value.ToString();
                                                else if(xt.Name == (w + "br"))
                                                    p.Inlines.Add(new LineBreak());
                                            }                                            
                                            p.Inlines.Add(r);
                                        }
                                        p.Inlines.Add(new LineBreak());
                                        rta.Blocks.Add(p);
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        //byte[] byteData;
                                        //// get the information out of the file, and print it to screen
                                        //Stream sr = file.OpenRead();

                                        //byteData = ReadStream(sr, 1000);

                                        //using (MemoryStream mStream = new MemoryStream(byteData, 0, byteData.Length))
                                        //{
                                        //    BitmapImage b = new BitmapImage();
                                        //    b.SetSource(new MemoryStream(byteData, false));
                                        //    _image.Source = b;
                                        //}
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }
                        }
                    }
                }
            }
            ReturnFocus();
        }

        private void rta_DragEnter(object sender, System.Windows.DragEventArgs e)
        {
            VisualStateManager.GoToState(this, "DragOver", true);
        }

        private void rta_DragLeave(object sender, System.Windows.DragEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", true);        
        }

        private void rta_KeyUp(object sender, KeyEventArgs e)
        {
            if (rta.Blocks.Count > 1 || (rta.Blocks.Count == 1 && (rta.Blocks[0] as Paragraph).Inlines.Count > 0))
                IsDirty = true;
            else
                IsDirty = false;
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            if (IsDirty)
            {
                if (MessageBox.Show("Do you want to save current document?", "Save document", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    btnSave_Click(sender, e);
            }
            rta.Blocks.Clear();
            IsDirty = false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = DocumentPersister.prepareDocumentXML(rta.Blocks);

            if (sb == null)
            {
                MessageBox.Show("Saving documents with images is not supported");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".sav";
            sfd.Filter = "Saved Files|*.sav|All Files|*.*";

            if (sfd.ShowDialog().Value)
            {
                using (FileStream fs = (FileStream)sfd.OpenFile())
                {
                    System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                    byte[] buffer = enc.GetBytes(sb.ToString());
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Close();
                    IsDirty = false;
                }

            }

        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            btnNew_Click(sender, e);

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "Saved Files|*.sav|All Files|*.*";

            if (ofd.ShowDialog().Value)
            {
                FileInfo fi = ofd.File;
                System.Xml.XmlReader r = System.Xml.XmlReader.Create(fi.OpenText());

                DocumentPersister.parseSavedDocument(r, rta.Blocks);
                r.Close();
            }
        }
        private void PositionHand()
        {
            Rect r;
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                TextPointer tp = rta.GetPositionFromPoint(lastRTAMouseMove.GetPosition(rta));
                r = tp.GetCharacterRect(LogicalDirection.Forward);
            }
            else
            {
                r = rta.Selection.End.GetCharacterRect(LogicalDirection.Forward);
            }

            if (r != Rect.Empty)
            {
                Canvas.SetLeft(caretHand, r.Left);
                Canvas.SetTop(caretHand, r.Bottom);
            }

        }

        private void rta_SelectionChanged(object sender, RoutedEventArgs e)
        {
            PositionHand();
        }

    }
}
