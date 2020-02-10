using System;
using System.IO;
using System.Text;

namespace SimpleXpsViewer
{
    /// <summary>
    /// Parses TrueType fonts and returns font information. The parser supports obfuscated fonts (.odttf).
    /// </summary>
    public class FontParser
    {
        private byte[] Read(Stream stream, int count)
        {
            byte[] data = new byte[count];
            int r;
            if ((r = stream.Read(data, 0, count)) == count) {
                return data;
            }
            throw new IOException(string.Format("Read {0} bytes from stream, expecting {1} bytes", r, count));
        }

        private ushort ReadShort(byte[] data, int offset)
        {
            return (ushort)(data[offset] * 0x100 + data[offset + 1]);
        }

        private uint ReadUInt(byte[] data, int offset)
        {
            return (uint)(data[offset] * 0x1000000 + data[offset + 1] * 0x10000 + data[offset + 2] * 0x100 + data[offset + 3]);
        }

        /// <summary>
        /// Parses a TrueType font from given file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public FontInfo ParseFont(FileInfo file)
        {
            return ParseFont(file.OpenRead(), file.Name);
        }

        /// <summary>
        /// Parses a TrueType font availabe in specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="fontFileName">Name of the font file.</param>
        /// <returns></returns>
        public FontInfo ParseFont(Stream stream, string fontFileName)
        {
            if (stream == null) {
                throw new ArgumentNullException("stream");
            }
            if (fontFileName != null && Path.GetExtension(fontFileName).ToLower() == ".odttf") {
                // make sure obfuscated fonts can be read
                stream = new ObfuscatedFontStream(stream, fontFileName);
            }

            byte[] header = Read(stream, 12);
            uint version = ReadUInt(header, 0);
            if (version == 0x10000) {
                ushort numTables = ReadShort(header, 4);

                for (int i = 0; i < numTables; i++) {
                    byte[] table = Read(stream, 16);
                    if (table[0] == 'n' && table[1] == 'a' && table[2] == 'm' && table[3] == 'e') {
                        uint offset = ReadUInt(table, 8);
                        uint tableLength = ReadUInt(table, 12);

                        stream.Seek(offset, SeekOrigin.Begin);

                        byte[] namingTable = Read(stream, 6);
                        ushort format = ReadShort(namingTable, 0);
                        ushort count = ReadShort(namingTable, 2);
                        ushort stringOffset = ReadShort(namingTable, 4);
                        byte[] nameRecords = Read(stream, count * 12);

                        FontInfo info = new FontInfo();
                        info.Obfuscated = stream is ObfuscatedFontStream;
                        info.FontStream = stream;

                        for (int j = 0; j < count; j++) {
                            FontNameRecord record = new FontNameRecord() {
                                PlatformId = ReadShort(nameRecords, j * 12 + 0),
                                EncodingId = ReadShort(nameRecords, j * 12 + 2),
                                LanguageId = ReadShort(nameRecords, j * 12 + 4),
                                NameId = ReadShort(nameRecords, j * 12 + 6)
                            };

                            ushort nameLength = ReadShort(nameRecords, j * 12 + 8);
                            ushort nameOffset = ReadShort(nameRecords, j * 12 + 10);

                            stream.Seek(offset + stringOffset + nameOffset, SeekOrigin.Begin);

                            byte[] value = Read(stream, nameLength);
                            record.Value = Encoding.BigEndianUnicode.GetString(value, 0, nameLength);

                            info.AddRecord(record);
                        }

                        return info;
                    }
                }
            }
            return null;
        }
    }
}
