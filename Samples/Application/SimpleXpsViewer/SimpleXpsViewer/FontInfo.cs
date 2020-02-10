using System;
using System.Collections.Generic;
using System.IO;

namespace SimpleXpsViewer
{
    /// <summary>
    /// Provides TrueType font information.
    /// </summary>
    public class FontInfo
        : IEnumerable<FontNameRecord>
    {
        private List<FontNameRecord> records = new List<FontNameRecord>();

        internal void AddRecord(FontNameRecord record)
        {
            this.records.Add(record);
        }

        private string GetValue(FontNameId id)
        {
            FontNameRecord record = GetRecord(id, 3);
            if (record != null){
                return record.Value;
            }
            return null;
        }

        /// <summary>
        /// Gets a value indicating whether the source of this font is obfuscated (odttf fonts).
        /// </summary>
        /// <value><c>true</c> if obfuscated; otherwise, <c>false</c>.</value>
        public bool Obfuscated { get; internal set; }
        /// <summary>
        /// (Macintosh only) On the Macintosh, the menu name is constructed using the FOND resource. This usually matches the Full Name. If you want the name of the font to appear differently than the Full Name, you can insert the Compatible Full Name.
        /// </summary>
        /// <value>The compatible full.</value>
        public string CompatibleFull { get { return GetValue(FontNameId.CompatibleFull); } }
        /// <summary>
        /// Copyright notice.
        /// </summary>
        /// <value>The copyright.</value>
        public string Copyright { get { return GetValue(FontNameId.Copyright); } }
        /// <summary>
        /// Description; description of the typeface. Can contain revision information, usage recommendations, history, features, etc.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get { return GetValue(FontNameId.Description); } }
        /// <summary>
        /// Name of the designer of the typeface.
        /// </summary>
        /// <value>The designer.</value>
        public string Designer { get { return GetValue(FontNameId.Designer); } }
        /// <summary>
        /// URL of typeface designer.
        /// </summary>
        /// <value>The designer URL.</value>
        public string DesignerUrl { get { return GetValue(FontNameId.DesignerUrl); } }
        /// <summary>
        /// Font Family name.
        /// </summary>
        /// <value>The name of the family.</value>
        public string FamilyName { get { return GetValue(FontNameId.FamilyName); } }
        /// <summary>
        /// Full font name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName { get { return GetValue(FontNameId.FullName); } }
        /// <summary>
        /// Unique font identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Identifier { get { return GetValue(FontNameId.Identifier); } }
        /// <summary>
        /// Description of how the font may be legally used, or different example scenarios for licensed use. 
        /// </summary>
        /// <value>The license.</value>
        public string License { get { return GetValue(FontNameId.License); } }
        /// <summary>
        /// URL where additional licensing information can be found.
        /// </summary>
        /// <value>The license URL.</value>
        public string LicenseUrl { get { return GetValue(FontNameId.LicenseUrl); } }
        /// <summary>
        /// Manufacturer Name.
        /// </summary>
        /// <value>The manufacturer.</value>
        public string Manufacturer { get { return GetValue(FontNameId.Manufacturer); } }
        /// <summary>
        /// Its presence in a font means that the PostScriptName holds a PostScript font name that is meant to be used with the "composefont" invocation in order to invoke the font in a PostScript interpreter. 
        /// </summary>
        /// <value>The postscript CID.</value>
        public string PostscriptCID { get { return GetValue(FontNameId.PostscriptCID); } }
        /// <summary>
        /// Postscript name for the font.
        /// </summary>
        /// <value>The name of the postscript.</value>
        public string PostscriptName { get { return GetValue(FontNameId.PostscriptName); } }
        /// <summary>
        /// For historical reasons, font families have contained a maximum of four styles, but font designers may group more than four fonts to a single family. The Preferred Family allows font designers to include the preferred family grouping which contains more than four fonts.
        /// </summary>
        /// <value>The preferred family.</value>
        public string PreferredFamily { get { return GetValue(FontNameId.PreferredFamily); } }
        /// <summary>
        /// Allows font designers to include the preferred subfamily grouping that is more descriptive than the font sub family.
        /// </summary>
        /// <value>The preferred sub family.</value>
        public string PreferredSubFamily { get { return GetValue(FontNameId.PreferredSubFamily); } }
        /// <summary>
        /// This can be the font name, or any other text that the designer thinks is the best sample to display the font in. 
        /// </summary>
        /// <value>The sample text.</value>
        public string SampleText { get { return GetValue(FontNameId.SampleText); } }
        /// <summary>
        /// Font Subfamily name. The Font Subfamily name distiguishes the font in a group with the same Font Family name.
        /// </summary>
        /// <value>The name of the sub family.</value>
        public string SubFamilyName { get { return GetValue(FontNameId.SubFamilyName); } }
        /// <summary>
        /// This is used to save any trademark notice/information for this font. Such information should be based on legal advice. This is distinctly separate from the copyright. 
        /// </summary>
        /// <value>The trademark.</value>
        public string Trademark { get { return GetValue(FontNameId.Trademark); } }
        /// <summary>
        /// URL Vendor; URL of font vendor.
        /// </summary>
        /// <value>The vendor URL.</value>
        public string VendorUrl { get { return GetValue(FontNameId.VendorUrl); } }
        /// <summary>
        /// Version string.
        /// </summary>
        /// <value>The identifier.</value>
        public string Version { get { return GetValue(FontNameId.Version); } }
        /// <summary>
        /// Used to provide a WWS-conformant family name.
        /// </summary>
        /// <value>The identifier.</value>
        public string WWSFamilyName { get { return GetValue(FontNameId.WWSFamilyName); } }
        /// <summary>
        /// This ID provides a WWS-conformant subfamily name 
        /// </summary>
        /// <value>The identifier.</value>
        public string WWSSubFamilyName { get { return GetValue(FontNameId.WWSSubFamilyName); } }

        /// <summary>
        /// Gets the unobfuscated font stream.
        /// </summary>
        /// <value>The font stream.</value>
        public Stream FontStream { get; internal set; }

        /// <summary>
        /// Gets the record.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public FontNameRecord GetRecord(FontNameId nameId, int platformId)
        {
            foreach (FontNameRecord record in this.records) {
                if (record.NameId == (ushort)nameId && record.PlatformId == platformId) {
                    return record;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<FontNameRecord> GetEnumerator()
        {
            return this.records.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.records.GetEnumerator();
        }
    }
}
