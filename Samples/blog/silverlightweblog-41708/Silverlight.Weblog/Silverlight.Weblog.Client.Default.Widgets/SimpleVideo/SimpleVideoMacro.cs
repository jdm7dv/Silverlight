using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Silverlight.Weblog.Client.DAL.Macros;
using VideoPlayer;

namespace Silverlight.Weblog.Client.Default.Widgets.SimpleVideo
{
    [Export(typeof(IBlogMacro))]
    public class SimpleVideoMacro : MacroBase
    {
        #region Overrides of MacroBase

        public override string MacroName
        {
            get { return "SimpleVideo"; }
        }

        public override Control ParseMacro(string UnparsedMacroString)
        {
            var properties = this.GetProperties(UnparsedMacroString);

            VideoPlayer.MainPage videoPlayer = new MainPage() { HorizontalAlignment= HorizontalAlignment.Center };

            if (properties.ContainsKey("source"))
                videoPlayer.MediaSource = new Uri(properties["source"]);

            if (properties.ContainsKey("width"))
                videoPlayer.Width = Double.Parse(properties["width"]);

            if (properties.ContainsKey("height"))
                videoPlayer.Height = Double.Parse(properties["height"]);

            return videoPlayer;

        }

        #endregion
    }
}
