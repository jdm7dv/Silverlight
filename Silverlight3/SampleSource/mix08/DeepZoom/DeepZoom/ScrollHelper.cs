using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;
using System.IO;
using System.Reflection;

namespace DeepZoom
{
    [ScriptableType]
    public class ScrollHelper
    {
        private static HtmlElement loadedScript;
        public event EventHandler<ScrollEventArgs> ScrollChanged;

        public ScrollHelper()
        {
            // Only load the script block once in to the HTML DOM
            if (loadedScript == null)
            {
                InitailizeScript(HtmlPage.Plugin.Id);
            }

            HtmlPage.RegisterScriptableObject("Application", this);
        }

        private void InitailizeScript(string plugInId)
        {
            StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(this.GetType().Namespace + ".scroller.js"));
            string script = reader.ReadToEnd();
            script = script.Replace("<id>", plugInId);

            loadedScript = HtmlPage.Document.CreateElement("script");
            loadedScript.SetAttribute("type", "text/javascript");
            loadedScript.SetProperty("text", script);
            HtmlPage.Document.GetElementsByTagName("body")[0].AppendChild(loadedScript);
        }

        [ScriptableMember]
        public void ScriptScrollChangedCallback(string x, string y, string scrollDelta)
        {
            OnScrollChanged(new ScrollEventArgs(Double.Parse(x), Double.Parse(y), Double.Parse(scrollDelta)));
        }

        protected virtual void OnScrollChanged(ScrollEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<ScrollEventArgs> handler = ScrollChanged;

            // Event will be null if there are no subscribers
            if (handler != null)
            {
                // Use the () operator to raise the event.
                handler(this, e);
            }
        }
    }

    public class ScrollEventArgs : EventArgs
    {
        Double x, y, scrollDelta;

        public ScrollEventArgs(Double x, Double y, Double scrollDelta)
        {
            this.x = x;
            this.y = y;
            this.scrollDelta = scrollDelta;
        }

        public Double X
        {
            get
            {
                return x;
            }
        }

        public Double Y
        {
            get
            {
                return y;
            }
        }

        public Double ScrollDelta
        {
            get
            {
                return scrollDelta;
            }
        }
    }
}
