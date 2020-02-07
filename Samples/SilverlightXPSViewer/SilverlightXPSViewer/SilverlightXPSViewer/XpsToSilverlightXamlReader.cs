using System;
using System.Collections.Generic;
using System.Xml;
using System.Windows;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Resources;


namespace SilverlightXPSViewer
{
    /// <summary>
    /// Translates "XPS XAML" into "Silverlight XAML" by tweaking the structure
    /// to remove Silverlight-unsupported elements from the source and account
    /// for other similar translation issues. This is a minimal implementation
    /// that supports only the functionality needed to call .ReadOuterXml().
    /// </summary>
    class XpsToSilverlightXamlReader : XmlReader
    {
        protected readonly XmlReader _reader;
        protected readonly string _xpsDoc;
        protected readonly Dictionary<string, Stream> _imageSources;
        StreamResourceInfo _streamResourceInfo;
        protected readonly Dictionary<string, string> _elementsToTranslate = new Dictionary<string, string>();
        protected readonly Dictionary<string, List<string>> _elementAttributesToRemove = new Dictionary<string, List<string>>();
        protected readonly Dictionary<string, List<string>> _elementAttributesToTranslate = new Dictionary<string, List<string>>();
        public readonly List<string> _viewboxOfImages = new List<string>();
        public readonly List<string> _viewPortOfImages = new List<string>();
        protected string _currentElementLocalName;
        protected string _currentAttributeLocalName;
        private int i;

        public XpsToSilverlightXamlReader(XmlReader reader, string sourceFile, Dictionary<string, Stream> imageSources, List<string> viewboxOfImages, List<string> viewPortOfImages, StreamResourceInfo streamResourceInfo)
        {
            _reader = reader;
            _xpsDoc = sourceFile;
            _imageSources = imageSources;
            _viewboxOfImages = viewboxOfImages;
            _viewPortOfImages = viewPortOfImages;
            _streamResourceInfo = streamResourceInfo;
            _elementsToTranslate.Add("FixedPage", "Canvas");
            _elementAttributesToRemove.Add("Canvas", new List<string> { "RenderOptions.EdgeMode" });
            _elementAttributesToRemove.Add("FixedPage", new List<string> { "lang" });
            _elementAttributesToRemove.Add("Glyphs", new List<string> { "BidiLevel" });
            _elementAttributesToRemove.Add("ImageBrush", new List<string> { "ImageSource", "TileMode", "Viewbox", "ViewboxUnits", "Viewport", "ViewportUnits" });
            _elementAttributesToTranslate.Add("Glyphs", new List<string> { "FontUri" });
            i = -1;
        }

        public override int AttributeCount
        {
            get { throw new NotImplementedException(); }
        }

        public override string BaseURI
        {
            get { throw new NotImplementedException(); }
        }

        public override void Close()
        {
            _reader.Close();
        }

        public override int Depth
        {
            get { return _reader.Depth; }
        }

        public override bool EOF
        {
            get { throw new NotImplementedException(); }
        }

        public override string GetAttribute(int i)
        {
            throw new NotImplementedException();
        }

        public override string GetAttribute(string name, string namespaceURI)
        {
            throw new NotImplementedException();
        }

        public override string GetAttribute(string name)
        {
            throw new NotImplementedException();
        }

        public override bool IsEmptyElement
        {
            get { return _reader.IsEmptyElement; }
        }

        public override string LocalName
        {
            get
            {
                var localName = _reader.LocalName;
                // Remember the current element/attribute name
                if (XmlNodeType.Element == _reader.NodeType)
                {
                    _currentElementLocalName = localName;
                }
                else if (XmlNodeType.Attribute == _reader.NodeType)
                {
                    _currentAttributeLocalName = localName;
                }
                // Translate appropriate element names
                if ((XmlNodeType.Element == _reader.NodeType) && _elementsToTranslate.ContainsKey(localName))
                {
                    localName = _elementsToTranslate[localName];
                }
                return localName;
            }
        }

        public override string LookupNamespace(string prefix)
        {
            throw new NotImplementedException();
        }

        public override bool MoveToAttribute(string name, string ns)
        {
            throw new NotImplementedException();
        }

        public override bool MoveToAttribute(string name)
        {
            throw new NotImplementedException();
        }

        public override bool MoveToElement()
        {
            return _reader.MoveToElement();
        }

        public override bool MoveToFirstAttribute()
        {
            // Move to the first *valid* attribute (skipping all invalid ones)
            var hadAttributes = _reader.HasAttributes;
            var result = TrackLastAttribute(SkipInvalidAttributes(_reader.MoveToFirstAttribute()));
            if (hadAttributes && !result)
            {
                _reader.MoveToElement();
            }
            return result;
        }

        public override bool MoveToNextAttribute()
        {
            return TrackLastAttribute(SkipInvalidAttributes(_reader.MoveToNextAttribute()));
        }

        protected bool SkipInvalidAttributes(bool result)
        {
            // Skip invalid elements
            while (result && _elementAttributesToRemove.ContainsKey(_currentElementLocalName) && _elementAttributesToRemove[_currentElementLocalName].Contains(_reader.LocalName))
            {
                // Track the ImageBrush/ImageSources for use by SetImageBrushSource
                if (("ImageBrush" == _currentElementLocalName) && ("ImageSource" == _reader.LocalName))
                {
                    i++;
                    var stream = Application.GetResourceStream(_streamResourceInfo, ConvertPartName(_reader.Value)).Stream;
                    _imageSources.Add(_reader.Value + "%" + i, stream);
                    
                }
                else if (("ImageBrush" == _currentElementLocalName) && ("Viewbox" == _reader.LocalName))
                {
                    _viewboxOfImages.Add(_reader.Value);
                }
                else if (("ImageBrush" == _currentElementLocalName) && ("Viewport" == _reader.LocalName))
                {
                    _viewPortOfImages.Add(_reader.Value);
                }
                result = _reader.MoveToNextAttribute();
            }
            return result;
        }

        protected bool TrackLastAttribute(bool result)
        {
            if (!result)
            {
                _currentAttributeLocalName = null;
            }
            return result;
        }

        public override XmlNameTable NameTable
        {
            get { throw new NotImplementedException(); }
        }

        public override string NamespaceURI
        {
            get { return _reader.NamespaceURI; }
        }

        public override XmlNodeType NodeType
        {
            get { return _reader.NodeType; }
        }

        public override string Prefix
        {
            get { return _reader.Prefix; }
        }

        public override bool Read()
        {
            return _reader.Read();
        }

        public override bool ReadAttributeValue()
        {
            return _reader.ReadAttributeValue();
        }

        public override ReadState ReadState
        {
            get { return _reader.ReadState; }
        }

        public override void ResolveEntity()
        {
            throw new NotImplementedException();
        }

        internal static Uri ConvertPartName(string partName)
        {
            // Remove the leading '/' from part names since Silverlight doesn't seem to like them there
            return new Uri(partName.TrimStart('/'), UriKind.RelativeOrAbsolute);
        }

        public override string Value
        {
            get
            {
                var value = _reader.Value;
                // Translate the attribute's value to a Uri based off the document Uri
                if (_elementAttributesToTranslate.ContainsKey(_currentElementLocalName) && _elementAttributesToTranslate[_currentElementLocalName].Contains(_currentAttributeLocalName))
                {
                    //string fileName = System.IO.Path.GetFileNameWithoutExtension(_xpsDoc);
                    //string fontUri = string.Format("/{0};component/{1}", fileName + "_font", System.IO.Path.GetFileName(value).Replace(".odttf", ".ttf"));
                    
                    //var stream = Application.GetResourceStream(_streamResourceInfo, ConvertPartName(value)).Stream;
                    ////AssemblyPart asmPart = new AssemblyPart();
                    //asmPart.Load(stream);

                    //App.Current.Resources.Add(System.IO.Path.GetFileName(value), stream);

                    //var abc = App.GetResourceStream(ConvertPartName(value));
                    //var myassembly =  AppDomain.CurrentDomain.DefineDynamicAssembly(new System.Reflection.AssemblyName("DynamicAssembly"), System.Reflection.Emit.AssemblyBuilderAccess.Run);
                    //var module = myassembly.DefineDynamicModule("Fonts");
                    //module.DefineManifestResource(System.IO.Path.GetFileName(value), stream, System.Reflection.ResourceAttributes.Public);

                    //var abc = App.GetResourceStream(ConvertPartName(value));

                    //string fontUri = string.Format("/{0};component/{1}", fileName + "_font", System.IO.Path.GetFileName(value));
                    //value = (new Uri(fontUri, UriKind.Relative)).ToString();
                }
                return value;
            }
        }
    }
}
