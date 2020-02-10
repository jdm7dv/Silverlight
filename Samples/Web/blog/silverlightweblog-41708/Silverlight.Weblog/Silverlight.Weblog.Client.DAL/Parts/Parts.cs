using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Silverlight.Weblog.Client.CoreBL.Widgets;
using Silverlight.Weblog.Client.DAL.Macros;
using Silverlight.Weblog.Client.DAL.Theme;
using Silverlight.Weblog.Shared.Common.IoC;

namespace Silverlight.Weblog.Client.CoreBL.Parts
{
    public class Parts // : IParts
    {
        public Parts()
        {
            Composer.Compose(this);
        }

        [ImportMany(typeof(IBlogWidget))]
        public IBlogWidget[] Widgets { get; set; }

        [Import(typeof(ILayoutManager))]
        public ILayoutManager Theme { get; set; }

        [ImportMany(typeof(IBlogMacro))]
        public IBlogMacro[] Macros { get; set; }
    }
}
