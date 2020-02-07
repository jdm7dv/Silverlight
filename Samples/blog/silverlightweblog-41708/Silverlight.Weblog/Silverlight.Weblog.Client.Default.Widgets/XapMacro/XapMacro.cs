using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Xml;
using Silverlight.Weblog.Client.DAL.Macros;
using System.Linq;

namespace Silverlight.Weblog.Client.Default.Widgets.XapMacro
{
    /// <summary>
    /// Based on Page Brooks' "A Smaller XAP Preloader for Silverlight" blog post @ 
    /// http://pagebrooks.com/archive/2009/05/17/a-smaller-xap-preloader-for-silverlight.aspx
    /// </summary>
    [Export(typeof(IBlogMacro))]
    public class XapMacro : MacroBase
    {
        #region Overrides of MacroBase

        public override string MacroName
        {
            get { return "Xap"; }
        }

        public override Control ParseMacro(string UnparsedMacroString)
        {
            var properties = GetProperties(UnparsedMacroString);

            if (properties.ContainsKey("source") == false)
            {
                return new ContentControl()
                {
                    Content = new TextBlock() { Text = "Could not load remote XAP." }
                };
            }

            ContentControl ReturnValue = new ContentControl();

            if (LoadedXaps.Contains(properties["source"]))
            {
                if (properties.ContainsKey("Xaml"))
                {
                    return LoadXaml(properties["Xaml"]);
                }
            }
            else if (LoadingXaps.Contains(properties["source"]))
            lock(XapsAndXamlToLoadForThem)
            {
                if (properties.ContainsKey("Xaml"))
                {
                    XapsAndXamlToLoadForThem.Add(new AsyncDataPack(properties, ReturnValue));
                }
            }
            else lock(LoadingXaps)
                 lock(XapsAndXamlToLoadForThem)
            {
                LoadingXaps.Add(properties["source"]);
                if (properties.ContainsKey("Xaml"))
                {
                    XapsAndXamlToLoadForThem.Add(new AsyncDataPack(properties, ReturnValue));
                }

                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                c.OpenReadAsync(new Uri(properties["source"], UriKind.RelativeOrAbsolute),
                                new AsyncDataPack(properties, ReturnValue));
            }

            return ReturnValue;
        }

        /// <summary>
        /// These 3 fields are used to sync up multiple XAMLs with a single XAP load. 
        /// LoadingXaps - Documents which XAPs are currently being loaded.
        /// LoadedXaps - Docuemnt which xaps have previously loaded. 
        /// XapsAndXamlToLoadForThem - Documents which XAMLs need to be loaded once the XAP arrives. 
        /// </summary>
        private List<AsyncDataPack> XapsAndXamlToLoadForThem = new List<AsyncDataPack>();
        private List<string> LoadingXaps = new List<string>();
        private List<string> LoadedXaps = new List<string>();

        private class AsyncDataPack
        {
            public AsyncDataPack(Dictionary<string, string> properties, ContentControl controlReturnedToUi)
            {
                Properties = properties;
                ControlReturnedToUI = controlReturnedToUi;
            }

            public Dictionary<string, string> Properties { get; set; }
            public ContentControl ControlReturnedToUI { get; set; }
        }

        private void c_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            AsyncDataPack state = (AsyncDataPack)e.UserState;
            Type uiElementType = null;

            try
            {
                uiElementType = LoadXapStream(e, state, uiElementType);

                InitializeMacroType(state, uiElementType);

                lock(LoadingXaps)
                lock(LoadedXaps)
                lock(XapsAndXamlToLoadForThem)
                {
                    LoadingXaps.Remove(state.Properties["source"]);
                    LoadedXaps.Add(state.Properties["source"]);

                    var delayedLoading = XapsAndXamlToLoadForThem.Where(a => a.Properties["source"] == state.Properties["source"]);
                    foreach (var asyncDataPack in delayedLoading)
                    {
                        try
                        {
                            string Xaml = asyncDataPack.Properties["Xaml"];
                            ContentControl control = asyncDataPack.ControlReturnedToUI;
                            control.Content = LoadXaml(Xaml);  
                        }
                        catch (Exception)
                        {
                            SetAsyncError(asyncDataPack, "Could not load the XAML: " + asyncDataPack.Properties["Xaml"]);
                        } 
                    }
                }

            }
            catch (Exception)
            {
                SetAsyncError(state, "Could not load the XAP: " + state.Properties["source"]);
            }
        }

        private void InitializeMacroType(AsyncDataPack state, Type uiElementType)
        {
            if (state.Properties.ContainsKey("type"))
            {
                if (uiElementType == null)
                {
                    SetAsyncError(state, "Could not find the type: " + state.Properties["type"]);
                }
                else
                {
                    object instance = Activator.CreateInstance(uiElementType);
                    if (instance == null)
                    {
                        SetAsyncError(state, "Could not load the type: " + state.Properties["type"]);
                    }
                    else
                    {
                        state.ControlReturnedToUI.Content = instance;
                    }
                }
            }
        }

        private void SetAsyncError(AsyncDataPack state, string ErrorText)
        {
            state.ControlReturnedToUI.Content =
                new TextBlock()
                    {
                        Text = ErrorText
                    };
        }

        private Type LoadXapStream(OpenReadCompletedEventArgs e, AsyncDataPack state, Type uiElementType)
        {
            var manifestStream = Application.GetResourceStream(
                new StreamResourceInfo(e.Result, null),
                new Uri("AppManifest.xaml", UriKind.Relative));

            string appManifest = new StreamReader(manifestStream.Stream).ReadToEnd();
            XmlReader reader = XmlReader.Create(new StringReader(appManifest));
            while (reader.Read())
            {
                if (reader.IsStartElement("AssemblyPart"))
                {
                    reader.MoveToAttribute("Source");
                    reader.ReadAttributeValue();
                    var assemblyStream = new StreamResourceInfo(e.Result, "application/binary");
                    var si = Application.GetResourceStream(assemblyStream, new Uri(reader.Value, UriKind.Relative));
                    AssemblyPart p = new AssemblyPart();
                    Assembly asm = p.Load(si.Stream);

                    if (state.Properties.ContainsKey("type") && uiElementType == null)
                    {
                        uiElementType = asm.GetTypes().FirstOrDefault(t => t.FullName == state.Properties["type"]);
                    }
                }
            }
            return uiElementType;
        }

        #endregion
    }
}
