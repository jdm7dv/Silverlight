using System;
using System.Collections.Generic;
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

namespace Silverlight.Weblog.Client.DAL.Macros
{
    /// <summary>
    /// Supports Macro syntax [MacroName:foo=bar|boo=baz]
    /// </summary>
    public abstract class MacroBase : IBlogMacro
    {
        private Dictionary<string, string> ParseString(string UnparsedMacroString)
        {
            Dictionary<string, string> ReturnValues = new Dictionary<string, string>();

            string[] MacroPropertiesPairs = UnparsedMacroString.Split('|');

            foreach (string macroPropertyPair in MacroPropertiesPairs)
            {
                int LocationOfEqualSign = macroPropertyPair.IndexOf("=");
                string key = macroPropertyPair.Substring(0, LocationOfEqualSign);
                string value = macroPropertyPair.Substring(LocationOfEqualSign + 1, macroPropertyPair.Length - LocationOfEqualSign - 1);

                ReturnValues.Add(key.Trim(), value.Trim());
            }

            return ReturnValues;
        }

        protected Dictionary<string, string> GetProperties(string UnparsedMacroString)
        {
            if (!ContainsMacroStartString(UnparsedMacroString))
                return null;

            string MacroNameInString = GetMacroNameInString(UnparsedMacroString);

            if (MacroNameInString.ToLower() != MacroName.ToLower())
                return null;

            string MacroProperties = GetMacroPropertiesString(UnparsedMacroString);

            return ParseString(MacroProperties);
        }

        public abstract string MacroName { get; }

        #region Implementation of IBlogMacro

        public bool IsStringMacro(string UnparsedMacroString)
        {
            if (!ContainsMacroStartString(UnparsedMacroString))
                return false;

            string MacroNameInString = GetMacroNameInString(UnparsedMacroString);

            if (MacroNameInString.ToLower() != MacroName.ToLower())
                return false;

            string MacroProperties = GetMacroPropertiesString(UnparsedMacroString);

            if (ParseString(MacroProperties) == null)
                return false;

            return true;
        }

        private string GetMacroPropertiesString(string UnparsedMacroString)
        {
            int IndexOfColon = UnparsedMacroString.IndexOf(":");
            int IndexOfAfterColon = IndexOfColon + 1;
            int LengtOfProperties = UnparsedMacroString.Length - 1 /* ] */- IndexOfAfterColon;
            return UnparsedMacroString.Substring(IndexOfAfterColon, LengtOfProperties);
        }

        private string GetMacroNameInString(string UnparsedMacroString)
        {
            int IndexOfColon = UnparsedMacroString.IndexOf(":");
            int StartLocationAfterInitialMark = 1;
            return UnparsedMacroString.Substring(StartLocationAfterInitialMark,
                                                 IndexOfColon - StartLocationAfterInitialMark);
        }

        private bool ContainsMacroStartString(string UnparsedMacroString)
        {
            return UnparsedMacroString.Trim().StartsWith("[")
                   && UnparsedMacroString.Trim().EndsWith("]")
                   &&  UnparsedMacroString.Contains(":");
        }

        public abstract Control ParseMacro(string UnparsedMacroString);

        #endregion


        protected Control LoadXaml(string Xaml)
        {
                                // Wrap the XAML in a contentControl with the right XMLNS
            string XamlToLoad = @"<ContentControl xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"""
                                + @" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">"
                                + Xaml
                                + "</ContentControl>";
            // char 160 is double whitespace inserted by Live Writer
            // we'll remove it since the XAML parser burfs at it.
            XamlToLoad = string.Join(string.Empty, XamlToLoad.Split((char) 160));

            return (ContentControl)
                   XamlReader.Load(XamlToLoad);
        }
    }
}
