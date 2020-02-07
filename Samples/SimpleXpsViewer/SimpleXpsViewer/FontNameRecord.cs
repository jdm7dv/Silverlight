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

namespace SimpleXpsViewer
{
    /// <summary>
    /// Represents a single font name record found in a TrueType font.
    /// </summary>
    public class FontNameRecord
    {
        /// <summary>
        /// Gets the platform id.
        /// </summary>
        /// <value>The platform id.</value>
        public ushort PlatformId { get; internal set; }
        /// <summary>
        /// Gets the platform-specific encoding ID.
        /// </summary>
        /// <value>The encoding id.</value>
        public ushort EncodingId { get; internal set; }
        /// <summary>
        /// Gets the language id.
        /// </summary>
        /// <value>The language id.</value>
        public ushort LanguageId { get; internal set; }
        /// <summary>
        /// Gets the name id.
        /// </summary>
        /// <value>The name id.</value>
        public ushort NameId { get; internal set; }
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; internal set; }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return this.Value;
        }
    }
}
