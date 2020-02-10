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
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Media.Imaging;

namespace RichNotepad
{
    public static class DocumentPersister
    {
        #region Save
        public static StringBuilder prepareDocumentXML(BlockCollection blocks)
        {
            var res = from block in blocks
                      from inline in (block as Paragraph).Inlines
                      where inline.GetType() == typeof(InlineUIContainer)
                      select inline;

            if (res.Count() == 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<DOC>");

                foreach (var block in blocks)
                {
                    Paragraph p = block as Paragraph;

                    sb.Append("<Paragraph ");
                    sb.Append("Name='" + p.Name + "' ");
                    sb.Append("FontFamily='" + p.FontFamily + "' ");
                    sb.Append("FontSize='" + p.FontSize + "' ");
                    sb.Append("FontStretch='" + p.FontStretch + "' ");
                    sb.Append("FontStyle='" + p.FontStyle + "' ");
                    sb.Append("FontWeight='" + p.FontWeight + "' ");
                    sb.Append("Foreground='" + (p.Foreground as SolidColorBrush).Color.ToString() + "' ");
                    sb.Append(">");

                    foreach (var inline in (block as Paragraph).Inlines)
                    {
                        if (inline is Run)
                            sb.Append(prepareRun(inline as Run));
                        else if (inline is Hyperlink)
                            sb.Append(prepareHyperlink(inline as Hyperlink));
                        else if (inline is InlineUIContainer)
                            sb.Append(prepareImage(inline as InlineUIContainer));
                    }
                    sb.Append("</Paragraph>");
                }

                sb.Append("</DOC>");
                return sb;
            }
            else
                return null;
        }

        private static string prepareImage(InlineUIContainer r)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Inline Type='InlineUIContainer' ");

            sb.Append("Name='" + r.Name + "' ");
            sb.Append("FontFamily='" + r.FontFamily + "' ");
            sb.Append("FontSize='" + r.FontSize + "' ");
            sb.Append("FontStretch='" + r.FontStretch + "' ");
            sb.Append("FontStyle='" + r.FontStyle + "' ");
            sb.Append("FontWeight='" + r.FontWeight + "' ");
            sb.Append("Foreground='" + (r.Foreground as SolidColorBrush).Color.ToString() + "' ");
            sb.Append("TextDecorations='" + r.TextDecorations + "' ");
            sb.Append("ImageSource='" + (r.Child as Image).Tag.ToString() + "'/>");

            return sb.ToString();
        }

        private static string prepareHyperlink(Hyperlink r)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Inline Type='Hyperlink' ");

            sb.Append("Name='" + r.Name + "' ");
            sb.Append("FontFamily='" + r.FontFamily + "' ");
            sb.Append("FontSize='" + r.FontSize + "' ");
            sb.Append("FontStretch='" + r.FontStretch + "' ");
            sb.Append("FontStyle='" + r.FontStyle + "' ");
            sb.Append("FontWeight='" + r.FontWeight + "' ");
            sb.Append("Foreground='" + (r.Foreground as SolidColorBrush).Color.ToString() + "' ");
            sb.Append("NavigateUri='" + r.NavigateUri + "' ");
            sb.Append("TextDecorations='" + r.TextDecorations + "'>");

            sb.Append("<Inlines>");
            foreach (var inline in r.Inlines)
            {
                if (inline is Run)
                    sb.Append(prepareRun(inline as Run, IsHyperlink: true));
            }
            sb.Append("</Inlines>");

            sb.Append("</Inline>");

            return sb.ToString();
        }

        private static string prepareRun(Run r, bool IsHyperlink = false)
        {
            string suffix = "";
            if (IsHyperlink)
                suffix = "HL";

            StringBuilder sb = new StringBuilder();
            sb.Append("<Inline" + suffix + " Type='Run' ");

            sb.Append("Name='" + r.Name + "' ");
            sb.Append("FontFamily='" + r.FontFamily + "' ");
            sb.Append("FontSize='" + r.FontSize + "' ");
            sb.Append("FontStretch='" + r.FontStretch + "' ");
            sb.Append("FontStyle='" + r.FontStyle + "' ");
            sb.Append("FontWeight='" + r.FontWeight + "' ");
            sb.Append("Foreground='" + (r.Foreground as SolidColorBrush).Color.ToString() + "' ");
            sb.Append("Text='" + r.Text + "' ");
            sb.Append("TextDecorations='" + r.TextDecorations + "'/>");

            return sb.ToString();
        }

        #endregion

        #region Load
        public static void parseSavedDocument(System.Xml.XmlReader r, BlockCollection blocks)
        {
            XDocument document = XDocument.Load(r);

            foreach (XElement element in document.Descendants("Paragraph"))
            {
                Paragraph p = new Paragraph();
                p.FontFamily = new FontFamily(element.Attribute(XName.Get("FontFamily")).Value);
                p.FontSize = double.Parse(element.Attribute(XName.Get("FontSize")).Value);

                p.FontStretch = (FontStretch)typeof(FontStretches).GetProperty(element.Attribute(XName.Get("FontStretch")).Value).GetValue(null, null);
                p.FontStyle = (FontStyle)typeof(FontStyles).GetProperty(element.Attribute(XName.Get("FontStyle")).Value).GetValue(null, null);
                p.FontWeight = (FontWeight)typeof(FontWeights).GetProperty(element.Attribute(XName.Get("FontWeight")).Value).GetValue(null, null);
                string color = element.Attribute(XName.Get("Foreground")).Value;
                color = color.Remove(0, 1);
                p.Foreground = new SolidColorBrush(Color.FromArgb(byte.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                                                                  byte.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                                                                  byte.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber),
                                                                  byte.Parse(color.Substring(6, 2), System.Globalization.NumberStyles.HexNumber)));


                foreach (XElement inline in element.Descendants("Inline"))
                {
                    if (inline.Attribute(XName.Get("Type")).Value == "Run")
                        p.Inlines.Add(parseRun(inline));
                    else if (inline.Attribute(XName.Get("Type")).Value == "Hyperlink")
                        p.Inlines.Add(parseHyperlink(inline));
                    else if (inline.Attribute(XName.Get("Type")).Value == "InlineUIContainer")
                        p.Inlines.Add(parseImage(inline));

                }

                blocks.Add(p);
            }
        }

        private static InlineUIContainer parseImage(XElement inline)
        {
            InlineUIContainer i = new InlineUIContainer();
            i.FontFamily = new FontFamily(inline.Attribute(XName.Get("FontFamily")).Value);
            i.FontSize = double.Parse(inline.Attribute(XName.Get("FontSize")).Value);
            i.FontStretch = (FontStretch)typeof(FontStretches).GetProperty(inline.Attribute(XName.Get("FontStretch")).Value).GetValue(null, null);
            i.FontStyle = (FontStyle)typeof(FontStyles).GetProperty(inline.Attribute(XName.Get("FontStyle")).Value).GetValue(null, null);
            i.FontWeight = (FontWeight)typeof(FontWeights).GetProperty(inline.Attribute(XName.Get("FontWeight")).Value).GetValue(null, null);
            string color = inline.Attribute(XName.Get("Foreground")).Value;
            color = color.Remove(0, 1);
            i.Foreground = new SolidColorBrush(Color.FromArgb(byte.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                                                              byte.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                                                              byte.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber),
                                                              byte.Parse(color.Substring(6, 2), System.Globalization.NumberStyles.HexNumber)));

            if (inline.Attribute(XName.Get("TextDecorations")).Value == "Underline")
                i.TextDecorations = TextDecorations.Underline;

            Image img = createImageFromUri(new Uri(inline.Attribute(XName.Get("ImageSource")).Value, UriKind.RelativeOrAbsolute), 23, 23);
            img.ImageFailed += (s, e) =>
            {
                Console.WriteLine(e.ErrorException);
            };
            img.ImageOpened += (s, e) =>
            {
                Console.WriteLine("ok");
            };
            i.Child = img;

            return i;
        }

        private static Hyperlink parseHyperlink(XElement inline)
        {
            Hyperlink i = new Hyperlink();
            i.FontFamily = new FontFamily(inline.Attribute(XName.Get("FontFamily")).Value);
            i.FontSize = double.Parse(inline.Attribute(XName.Get("FontSize")).Value);
            i.FontStretch = (FontStretch)typeof(FontStretches).GetProperty(inline.Attribute(XName.Get("FontStretch")).Value).GetValue(null, null);
            i.FontStyle = (FontStyle)typeof(FontStyles).GetProperty(inline.Attribute(XName.Get("FontStyle")).Value).GetValue(null, null);
            i.FontWeight = (FontWeight)typeof(FontWeights).GetProperty(inline.Attribute(XName.Get("FontWeight")).Value).GetValue(null, null);
            string color = inline.Attribute(XName.Get("Foreground")).Value;
            color = color.Remove(0, 1);
            i.Foreground = new SolidColorBrush(Color.FromArgb(byte.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                                                              byte.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                                                              byte.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber),
                                                              byte.Parse(color.Substring(6, 2), System.Globalization.NumberStyles.HexNumber)));

            if (inline.Attribute(XName.Get("TextDecorations")).Value == "Underline")
                i.TextDecorations = TextDecorations.Underline;

            i.NavigateUri = new Uri(inline.Attribute(XName.Get("NavigateUri")).Value);

            var ils = inline.Descendants(XName.Get("Inlines"));
            foreach (var item in ils.Descendants())
            {
                if (item.Attribute(XName.Get("Type")).Value == "Run")
                    i.Inlines.Add(parseRun(item));
            }

            return i;
        }

        private static Run parseRun(XElement inline)
        {
            Run i = new Run();
            i.FontFamily = new FontFamily(inline.Attribute(XName.Get("FontFamily")).Value);
            i.FontSize = double.Parse(inline.Attribute(XName.Get("FontSize")).Value);
            i.FontStretch = (FontStretch)typeof(FontStretches).GetProperty(inline.Attribute(XName.Get("FontStretch")).Value).GetValue(null, null);
            i.FontStyle = (FontStyle)typeof(FontStyles).GetProperty(inline.Attribute(XName.Get("FontStyle")).Value).GetValue(null, null);
            i.FontWeight = (FontWeight)typeof(FontWeights).GetProperty(inline.Attribute(XName.Get("FontWeight")).Value).GetValue(null, null);
            string color = inline.Attribute(XName.Get("Foreground")).Value;
            color = color.Remove(0, 1);
            i.Foreground = new SolidColorBrush(Color.FromArgb(byte.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                                                              byte.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                                                              byte.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber),
                                                              byte.Parse(color.Substring(6, 2), System.Globalization.NumberStyles.HexNumber)));

            if (inline.Attribute(XName.Get("TextDecorations")).Value == "Underline")
                i.TextDecorations = TextDecorations.Underline;

            (i as Run).Text = inline.Attribute(XName.Get("Text")).Value;
            return i;
        }
        #endregion

        #region Helper methods
        public static Image createImageFromUri(Uri URI, double width, double height)
        {
            Image img = new Image();
            img.Stretch = Stretch.Uniform;
            img.Width = width;
            img.Height = height;
            BitmapImage bi = new BitmapImage(URI);
            img.Source = bi;
            img.Tag = bi.UriSource.ToString();
            return img;
        }

        #endregion
    }
}
