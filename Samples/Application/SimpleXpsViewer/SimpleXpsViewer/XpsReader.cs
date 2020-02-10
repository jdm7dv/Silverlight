using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Xml;
using System.Xml.Linq;

namespace SimpleXpsViewer
{
    /// <summary>
    /// Reads XPS packages and returns Silverlight XAML.
    /// </summary>
    public class XpsReader
    {
        class FontData { public string FamilyName { get; set; } public FontSource Source { get; set; } public Stream Stream { get; set; } }
        class ImageData { public BitmapImage Source { get; set; } public Stream Stream { get; set; } } 

        delegate void Translate(XmlReader reader, XmlWriter writer);

        static Dictionary<string, Translate> map;
        static Dictionary<string, List<string>> attributesIgnore;

        private StreamResourceInfo xps;
        private Uri[] pages;

        static XpsReader()
        {
            attributesIgnore = new Dictionary<string, List<string>>();
            attributesIgnore["Canvas"] = new List<string> { "RenderOptions.EdgeMode" };
            attributesIgnore["Glyphs"] = new List<string> { "BidiLevel" };
            attributesIgnore["ImageBrush"] = new List<string> { "TileMode", "Viewbox", "ViewboxUnits", "Viewport", "ViewportUnits" };
            attributesIgnore["Path"] = new List<string> { "FixedPage.NavigateUri", "Clip" };

            map = new Dictionary<string, Translate>();
            map.Add("FixedPage", (reader, writer) => {
                writer.WriteStartElement("Canvas", reader.NamespaceURI);
                WriteAttributes(reader, writer);
                if (reader.IsEmptyElement) {
                    writer.WriteEndElement();
                }
            });
            map.Add("Glyphs", (reader, writer) => {
                // DEBUG: write glyphs itself (only works if fontUri is availabe as resource in assembly)
                //writer.WriteStartElement("Glyphs", reader.NamespaceURI);
                //WriteAttributes(reader, writer);
                //writer.WriteEndElement();

                string text = reader.GetAttribute("UnicodeString");
                if (text.Trim().Length == 0) {
                    // omit empty glyphs
                    reader.ReadOuterXml();
                }
                else {
                    double size = double.Parse(reader.GetAttribute("FontRenderingEmSize"), CultureInfo.InvariantCulture);
                    double y = double.Parse(reader.GetAttribute("OriginY"), CultureInfo.InvariantCulture) - size;

                    writer.WriteStartElement("TextBlock", reader.NamespaceURI);
                    writer.WriteAttributeString("Tag", reader.GetAttribute("FontUri"));     // temp store font uri
                    writer.WriteAttributeString("Canvas.Left", reader.GetAttribute("OriginX"));
                    writer.WriteAttributeString("Canvas.Top", y.ToString(CultureInfo.InvariantCulture));
                    writer.WriteAttributeString("Text", text);
                    writer.WriteAttributeString("FontSize", size.ToString(CultureInfo.InvariantCulture));
                    writer.WriteAttributeString("Foreground", reader.GetAttribute("Fill"));
                    if (reader.IsEmptyElement) {
                        writer.WriteEndElement();
                    }
                }
            });

            map.Add("ImageBrush.Transform", (reader, writer) => {
                reader.ReadOuterXml();
            });
        }

        private XpsReader(StreamResourceInfo xps)
        {
            this.xps = xps;

            StreamResourceInfo docInfo = Load("Documents/1/FixedDoc.fdoc", "Documents/1/FixedDocument.fdoc");
            if (docInfo == null) {
                throw new ArgumentException("Invalid XPS file");
            }

            using (docInfo.Stream) {
                XDocument doc = XDocument.Load(docInfo.Stream);
                var pages = from page in doc.Descendants("{http://schemas.microsoft.com/xps/2005/06}PageContent")
                            select GetPageSource(page);

                this.pages = pages.ToArray();
            }
        }

        /// <summary>
        /// Creates an instance of the XPS reader based on specified stream resource info.
        /// </summary>
        /// <param name="resourceInfo">The resource info.</param>
        /// <returns></returns>
        public static XpsReader Create(StreamResourceInfo resourceInfo)
        {
            if (resourceInfo == null) {
                throw new ArgumentNullException("resourceInfo");
            }
            return new XpsReader(resourceInfo);
        }

        /// <summary>
        /// Creates an instance of the XPS reader based on specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static XpsReader Create(Stream stream)
        {
            return new XpsReader(new StreamResourceInfo(stream, null));
        }

        /// <summary>
        /// Creates an instance of the XPS reader based on specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static XpsReader Create(FileInfo file)
        {
            if (file == null) {
                throw new ArgumentNullException("file");
            }
            return Create(file.OpenRead());
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            this.xps.Stream.Close();
        }

        private static Uri GetPageSource(XElement page)
        {
            Uri source = ParseResourceUri((string)page.Attribute("Source"));
            if (!source.OriginalString.StartsWith("Documents/1/")) {
                source = new Uri("Documents/1/" + source.OriginalString, UriKind.Relative);
            }
            return source;
        }

        /// <summary>
        /// Gets the page count.
        /// </summary>
        /// <value>The page count.</value>
        public int PageCount
        {
            get { return this.pages.Length; }
        }

        private StreamResourceInfo Load(Uri uri)
        {
            return Application.GetResourceStream(this.xps, uri);
        }

        private StreamResourceInfo Load(string uri)
        {
            return Load(ParseResourceUri(uri));
        }

        private StreamResourceInfo Load(params string[] uris)
        {
            StreamResourceInfo result = null;
            foreach (string uri in uris) {
                result = Load(uri);
                if (result != null) {
                    break;
                }
            }
            return result;
        }

        private static void WriteElement(XmlReader reader, XmlWriter writer)
        {
            writer.WriteStartElement(reader.LocalName, reader.NamespaceURI);
            WriteAttributes(reader, writer);
            if (reader.IsEmptyElement) {
                writer.WriteEndElement();
            }
        }

        private static void WriteAttributes(XmlReader reader, XmlWriter writer)
        {
            List<string> ignore;
            attributesIgnore.TryGetValue(reader.LocalName, out ignore);

            while (reader.MoveToNextAttribute()) {
                if (reader.LocalName == "xmlns" || (ignore != null && ignore.Contains(reader.LocalName))) {
                    continue;
                }

                writer.WriteAttributeString(reader.LocalName, reader.NamespaceURI, reader.Value);
            }
            reader.MoveToElement();
        }

        /// <summary>
        /// Parses specified page from the XPS package and returns a framework element.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public FrameworkElement ParsePage(int page)
        {
            if (page < 1 || page > this.pages.Length) {
                throw new ArgumentOutOfRangeException("page");
            }

            StreamResourceInfo pageInfo = Load(this.pages[page - 1]);
            if (pageInfo == null) {
                throw new ArgumentException(string.Format("Page {0} does not exist", page), "page");
            }

            StringBuilder result = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(result, settings)) {
                using (Stream stream = pageInfo.Stream) {
                    using (XmlReader reader = XmlReader.Create(stream)) {
                        while (reader.Read()) {
                            if (reader.NodeType == XmlNodeType.Element) {
                                Translate t;
                                if (map.TryGetValue(reader.LocalName, out t)) {
                                    t(reader, writer);
                                }
                                else {
                                    // no map, write as-is
                                    WriteElement(reader, writer);
                                }
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement) {
                                writer.WriteEndElement();
                            }
                        }
                    }
                }
            }

            // load element
            FrameworkElement element = (FrameworkElement)XamlReader.Load(result.ToString());

            // apply fonts and images from xps
            Dictionary<string, FontData> fonts = new Dictionary<string, FontData>();
            Dictionary<Uri, ImageData> images = new Dictionary<Uri, ImageData>();

            try {
                foreach (FrameworkElement child in GetAllChildren(element)) {
                    ApplyFontSource(child, fonts);
                    ApplyImageSource(child, images);
                }
            }
            finally {
                // properly close all open font and image streams
                foreach (FontData font in fonts.Values) {
                    font.Stream.Close();
                }
                foreach (ImageData image in images.Values) {
                    image.Stream.Close();
                }
            }

            return element;
        }

        private void ApplyFontSource(FrameworkElement element, Dictionary<string, FontData> fonts)
        {
            TextBlock text = element as TextBlock;
            if (text != null) {
                string fontUri = (string)text.Tag;  // font uri temp stored in tag
                text.Tag = null;    // clear tag

                FontData font;
                if (!fonts.TryGetValue(fontUri, out font)) {
                    StreamResourceInfo resource = Load(fontUri);
                    if (resource != null) {
                        FontParser parser = new FontParser();
                        FontInfo fontInfo = parser.ParseFont(resource.Stream, Path.GetFileName(fontUri));
                        if (fontInfo != null) {
                            font = new FontData() {
                                FamilyName = fontInfo.FamilyName,
                                Source = new FontSource(fontInfo.FontStream),
                                Stream = fontInfo.FontStream
                            };
                        }
                    }
                    fonts[fontUri] = font;
                }

                if (font != null){
                    text.FontFamily = new FontFamily(font.FamilyName);
                    text.FontSource = font.Source;
                }
            }
        }

        private void ApplyImageSource(FrameworkElement element, Dictionary<Uri, ImageData> images)
        {
            var path = element as System.Windows.Shapes.Path;
            if (path != null) {
                ImageBrush imageBrush = path.Fill as ImageBrush;
                if (imageBrush != null) {
                    BitmapImage image = imageBrush.ImageSource as BitmapImage;
                    if (image != null) {
                        ImageData imageData;
                        if (!images.TryGetValue(image.UriSource, out imageData)) {
                            if (Path.GetExtension(image.UriSource.OriginalString).ToUpper() != ".TIF") {
                                StreamResourceInfo imageInfo = Load(ParseResourceUri(image.UriSource));
                                if (imageInfo != null) {
                                    imageData = new ImageData() {
                                        Source = new BitmapImage(),
                                        Stream = imageInfo.Stream
                                    };
                                    imageData.Source.SetSource(imageInfo.Stream);
                                }
                            }

                            images[image.UriSource] = imageData;
                        }

                        if (imageData != null) {
                            imageBrush.ImageSource = imageData.Source;
                        }
                    }
                }
            }
        }

        private static Uri ParseResourceUri(string uri)
        {
            if (uri.StartsWith("/")) {
                uri = uri.Substring(1);
            }
            return new Uri(uri, UriKind.Relative);
        }

        private static Uri ParseResourceUri(Uri uri)
        {
            return ParseResourceUri(uri.OriginalString);
        }

        private static IEnumerable<DependencyObject> GetVisualChildren(DependencyObject parent)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++) {
                yield return VisualTreeHelper.GetChild(parent, i);
            }
        }

        private static IEnumerable<FrameworkElement> GetAllChildren(FrameworkElement parent)
        {
            Stack<FrameworkElement> stack = new Stack<FrameworkElement>(GetVisualChildren(parent).OfType<FrameworkElement>());
            while (stack.Count > 0) {
                FrameworkElement element = stack.Pop();
                foreach (FrameworkElement visualChild in GetVisualChildren(element).OfType<FrameworkElement>()) {
                    stack.Push(visualChild);
                }

                yield return element;
            }
        }
    }
}
