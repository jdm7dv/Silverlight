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
        



    }
}