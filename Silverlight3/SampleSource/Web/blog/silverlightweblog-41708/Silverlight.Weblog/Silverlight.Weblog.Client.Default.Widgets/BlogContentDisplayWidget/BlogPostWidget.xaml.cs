using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using C1.Silverlight.RichTextBox.Documents;
using Microsoft.Practices.Unity;
using Silverlight.Weblog.Client.CoreBL.Parts;
using Silverlight.Weblog.Client.CoreBL.Widgets;
using Silverlight.Weblog.Client.Default.Widgets.BaseUI;
using Silverlight.Weblog.Client.Default.Widgets.ViewModels;
using Silverlight.Weblog.Common.IoC;
using System.Linq;

namespace Silverlight.Weblog.Client.Default.Widgets
{
    [Export(typeof(IBlogWidget))]
    public partial class BlogPostWidget : UserControlBase
    {
        public IBlogPostViewModel ViewModel { get; set; }

        // [Dependency] - Causes dead lock with Parts.Widgets
        public Parts Parts { get; set; }

        public BlogPostWidget()
        {
            // Required to initialize variables
            InitializeComponent();

            IoC.BuildUp(this);

            this.c1RTB.DocumentChanged += new EventHandler(c1RTB_DocumentChanged);
        }

        private bool _ignoreDocumentChange = false;

        /// <summary>
        /// Apply Macros. 
        /// </summary>
        void c1RTB_DocumentChanged(object sender, EventArgs e)
        {
            if (_ignoreDocumentChange)
                return;
            _ignoreDocumentChange = true;

            Hack_ResolveIoCPartsManaullyToAvoidDeadlocking();

            Dictionary<int, Control> BlockIndexForControls =
                GetIndexForBlocksWithMacroTextAndTheirRespectiveMacroControls();
            ReplaceExistingTextBlocksWithControls(BlockIndexForControls);

            _ignoreDocumentChange = false;
        }

        private Dictionary<int, Control> GetIndexForBlocksWithMacroTextAndTheirRespectiveMacroControls()
        {
            Dictionary<int, Control> BlockIndexForControls = new Dictionary<int, Control>();

            foreach (C1Block curC1Block in c1RTB.Document.Blocks)
            {
                if (curC1Block is C1Paragraph)
                {
                    C1Paragraph c1Paragraph = (C1Paragraph)curC1Block;
                    string paragraphText = String.Join(string.Empty,
                                            c1Paragraph.Inlines.OfType<C1Run>().Select(r => r.Text).ToArray());

                    var ParagraphMacro =
                        (from macro in Parts.Macros
                         where macro.IsStringMacro(paragraphText)
                         select macro.ParseMacro(paragraphText)).FirstOrDefault();

                    if (ParagraphMacro != null)
                        BlockIndexForControls.Add(c1RTB.Document.Blocks.IndexOf(curC1Block), ParagraphMacro);
                }
            }
            return BlockIndexForControls;
        }

        private void ReplaceExistingTextBlocksWithControls(Dictionary<int, Control> BlockIndexForControls)
        {
            foreach (KeyValuePair<int, Control> IndexAndControl in BlockIndexForControls)
            {
                c1RTB.Document.Blocks.RemoveAt(IndexAndControl.Key);

                C1BlockUIContainer container = new C1BlockUIContainer()
                {
                    Child = IndexAndControl.Value,
                    TextAlignment = ConverttoC1TextAlignment(IndexAndControl.Value.HorizontalAlignment)
                };

                c1RTB.Document.Blocks.Insert(IndexAndControl.Key, container);
            }
        }

        private C1TextAlignment ConverttoC1TextAlignment(HorizontalAlignment horizontalAlignment)
        {
            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Center:
                    return C1TextAlignment.Center;
                case HorizontalAlignment.Left:
                    return C1TextAlignment.Left;
                case HorizontalAlignment.Right:
                    return C1TextAlignment.Right;
            }
            return C1TextAlignment.Left;
        }

        private void Hack_ResolveIoCPartsManaullyToAvoidDeadlocking()
        {
            if (Parts == null)
            {
                Parts = IoC.Get<Parts>();
            }
        }

        #region Overrides of UserControlBase

        /// <summary>
        /// Place the content blog post widget in the centeral cell in the default theme grid.
        /// </summary>
        public override int Row
        {
            get { return 1; }
        }

        /// <summary>
        /// Place the content blog post widget in the centeral cell in the default theme grid.
        /// </summary>
        public override int Column
        {
            get { return 1; }
        }

        public override int ColumnSpan
        {
            get { return 1; }
        }

        public override int RowSpan
        {
            get { return 1; }
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

        #endregion
    }
}