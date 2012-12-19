//------------------------------------------------------------------------------------
//    This file is part of MobiEPUB.
//
//    MobiEPUB is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    MobiEPUB is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with MobiEPUB.  If not, see <http://www.gnu.org/licenses/>.
//------------------------------------------------------------------------------------
//    Copyright 2012, Matthew Donald
//------------------------------------------------------------------------------------


//    PalmDOC Header
//
//    The first record in the Palm Database Format gives more information about
//    the Mobipocket file. The first 16 bytes are almost identical to the first
//    sixteen bytes of a PalmDOC format file.
//
//    offset	bytes	content	comments
//    0         2	    Compression	 1 == no compression, 2 = PalmDOC compression, 17480 = HUFF/CDIC compression
//    2	        2	    Unused	Always zero
//    4	        4	    text length	Uncompressed length of the entire text of the book
//    8	        2	    record count	Number of PDB records used for the text of the book.
//    10	    2	    record size	Maximum size of each record containing text, always 4096
//    12	    4	    Current Position	Current reading position, as an offset into the uncompressed text
//
//    There are two differences from a Palm DOC file. There's an additional
//    compression type (17480), and the Current Position bytes are used for a
//    different purpose:
//
//    offset	bytes	content	comments
//    12	    2	    Encryption Type	 0 == no encryption, 1 = Old Mobipocket Encryption, 2 = Mobipocket Encryption
//    14	    2	    Unknown	Usually zero
//
//    The old Mobipocket Encryption scheme only allows the file to be registered
//    with one PID, unlike the current encryption scheme that allows multiple
//    PIDs to be used in a single file. Unless specifically mentioned, all the
//    encryption information on this page refers to the current scheme.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobiEPUB.PRC;
namespace MobiEPUB.PRC
{
    class PRCheader
    {
        private int m_Compression;
        private int m_UncompTextLen;
        private int m_PRCrecCnt;
        private int m_MaxRecLen;
        private int m_Encryption;

        public enum CompressionMode
        {
            None,
            PalmDoc,
            Huffman,
            Unknown
        }

        public enum EncryptionMode
        {
            None,
            OldMobi,
            NewMobi,
            Unknown
        }

        public PRCheader(Header header)
        {
            // Compression : Bytes 0-2 big-endian integer
            m_Compression = header.ReadShort(0);

            // Uncompressed Text Length: Bytes 4-4 big-endian integer
            m_UncompTextLen = header.ReadInt(4);
            m_UncompTextLen += 2; // allow for trailing CRLF

            // record Count : Bytes 8-2 big-endian integer
            m_PRCrecCnt = header.ReadShort(8);

            // Max record lenth : Bytes 10-2 big-endian integer
            m_MaxRecLen = header.ReadShort(10);

            // Encryption : Bytes 12-2 big-endian integer
            m_Encryption = header.ReadShort(12);
        }

        public CompressionMode Compression
        {
            get
            {
                CompressionMode result;
                switch (m_Compression)
                {
                    case 1:
                        result = CompressionMode.None;
                        break;
                    case 2:
                        result = CompressionMode.PalmDoc;
                        break;
                    case 17480:
                        result = CompressionMode.Huffman;
                        break;
                    default:
                        result = CompressionMode.Unknown;
                        break;
                }
                return result;
            }
        }

        public int TextLenth { get { return m_UncompTextLen; } }

        public int RecordCnt { get { return m_PRCrecCnt; } }

        public int MaxRecLenth { get { return m_MaxRecLen; } }

        public EncryptionMode Encryption
        {
            get
            {
                EncryptionMode result;
                switch (m_Encryption)
                {
                    case 0:
                        result = EncryptionMode.None;
                        break;
                    case 1:
                        result = EncryptionMode.OldMobi;
                        break;
                    case 2:
                        result = EncryptionMode.NewMobi;
                        break;
                    default:
                        result = EncryptionMode.Unknown;
                        break;
                }

                return result;
            }
        }
    }
}
