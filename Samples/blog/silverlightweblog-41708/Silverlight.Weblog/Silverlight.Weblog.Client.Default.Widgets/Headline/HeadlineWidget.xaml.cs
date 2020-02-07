using System;
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
	public partial class HeadlineWidget : UserControlBase
	{
		public HeadlineWidget()
		{
			// Required to initialize variables
			InitializeComponent();
		}

        public IHeadlineViewModel ViewModel { get; set; }

	    #region Overrides of UserControlBase

        // Place headline on the entire top row of the default Theme
	    public override int Row
	    {
            get { return 0; }
	    }

	    public override int Column
	    {
	        get { return 0; }
	    }

	    public override int ColumnSpan
	    {
	        get { return 3; }
	    }

	    public override int RowSpan
	    {
	        get { return 1; }
	    }

	    #endregion
	}
}