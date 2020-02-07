using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Silverlight.Weblog.Client.DAL.Macros;

namespace Silverlight.Weblog.Client.Default.Widgets.XamlMacro
{
    [Export(typeof(IBlogMacro))]
    public class XamlMacro : MacroBase
    {
        #region Overrides of MacroBase

        public override string MacroName
        {
            get { return "Xaml"; }
        }

        public override Control ParseMacro(string UnparsedMacroString)
        {
            var properties = this.GetProperties(UnparsedMacroString);

            if (properties.ContainsKey("string"))
            {
                return LoadXaml(properties["string"]);
            }

            return new ContentControl()
                       {
                           Content = new TextBlock() { Text = "ERROR: Could not load embedded XAML." }
                       };
        }



        #endregion
    }
}
