using System.ComponentModel.Composition;
using Silverlight.Weblog.Client.CoreBL.Widgets;
using Silverlight.Weblog.Client.DAL.Macros;
using Silverlight.Weblog.Client.DAL.Theme;

namespace Silverlight.Weblog.Client.CoreBL.Parts
{
    public interface IParts
    {
        [ImportMany(typeof (IBlogWidget))]
        IBlogWidget[] Widgets { get; set; }

        [Import(typeof (ILayoutManager))]
        ILayoutManager Theme { get; set; }

        [ImportMany(typeof (IBlogMacro))]
        IBlogMacro[] Macros { get; set; }
    }
}