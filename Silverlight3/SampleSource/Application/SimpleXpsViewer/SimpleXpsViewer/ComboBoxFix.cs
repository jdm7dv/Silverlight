using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SimpleXpsViewer
{
    /// <summary>
    /// This class fixes the ComboBox drop down resize problem. The SL 2.0 RTM
    /// does not resize the drop down window after the first time it drops down
    /// even though items may have been added or removed from the collection.
    /// 
    /// Taken from http://silverlight.net/forums/p/50143/132212.aspx
    /// 
    /// 
    /// </summary>
    public class ComboBoxFix : ComboBox
    {

        ScrollViewer _scrollViewer;
        Border _popupBorder;

        double _itemHeight;
        double _popupBorderPadding;

        int _lastItemsCount;
        int _maxItemsWithoutScroll;

        bool _isNewTemplate = true;
        protected override void OnDropDownOpened(EventArgs e)
        {

            base.OnDropDownOpened(e);

            if (_isNewTemplate == true) {

                // The template is new, so the border padding may have changed

                _isNewTemplate = false;
                UpdateLayout();

                _popupBorderPadding = _popupBorder.ActualHeight - _scrollViewer.ViewportHeight;

            }

            // If we don't yet know how many items we can hold without scrolling,

            // figure it out here

            if (_maxItemsWithoutScroll == -1 && Items.Count > 0) {

                // Assume that UpdateLayout is quick if the layout isn't dirty

                UpdateLayout();

                _itemHeight = _scrollViewer.ExtentHeight / Items.Count;

                double maxNoScrollExtent = _popupBorder.MaxHeight - _popupBorderPadding; _maxItemsWithoutScroll = (int)(0.001 + maxNoScrollExtent / _itemHeight);
            }

            // Resize the drop-down window fi it needs it

            if (_maxItemsWithoutScroll > 0 && _lastItemsCount != Items.Count) {

                int numShown = Items.Count <= _maxItemsWithoutScroll ? Items.Count : _maxItemsWithoutScroll; double result = _itemHeight * numShown + _popupBorderPadding;
                _popupBorder.Height = result;

                _lastItemsCount = Items.Count;

            }

        }

        public override void OnApplyTemplate()
        {

            base.OnApplyTemplate();
            _scrollViewer = GetTemplateChild("ScrollViewer") as ScrollViewer;

            _popupBorder = GetTemplateChild("PopupBorder") as Border; _isNewTemplate = true;
            _lastItemsCount = -1;

            _maxItemsWithoutScroll = -1;

            _itemHeight = 0.0;

        }

    }

}
