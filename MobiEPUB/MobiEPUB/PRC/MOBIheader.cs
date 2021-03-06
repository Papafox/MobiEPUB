﻿//------------------------------------------------------------------------------------
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


//    MOBI Header
//
//    Most Mobipocket file also have a MOBI header in record 0 that follows these
//    16 bytes, and newer formats also have an EXTH header following the 
//    Mobipocket, again all in record 0 of the PDB file format.
//
//    The MOBI header is of variable length and is not documented. Some fields
//    have been tentatively identified as follows:
//
//    offset	hex	    bytes	content	comments
//    16	    0x10	4	identifier	the characters M O B I
//    20	    0x14	4	header length	 the length of the MOBI header, including the previous 4 bytes
//    24	    0x18	4	Mobi type	The kind of Mobipocket file this is
//                          2 Mobipocket Book
//                          3 PalmDoc Book
//                          4 Audio
//                          232 mobipocket? generated by kindlegen1.2
//                          248 KF8: generated by kindlegen2
//                          257 News
//                          258 News_Feed
//                          259 News_Magazine
//                          513 PICS
//                          514 WORD
//                          515 XLS
//                          516 PPT
//    				        517 TEXT
//                          518 HTML
//    28	    0x1c	4	text Encoding	1252 = CP1252 (WinLatin1); 65001 = UTF-8
//    32	    0x20	4	Unique-ID	Some kind of unique ID number (random?)
//    36	    0x24	4	File version	Version of the Mobipocket format used in this file.
//    40	    0x28	4	Ortographic index	Section number of orthographic meta index. 0xFFFFFFFF if index is not available.
//    44	    0x2c	4	Inflection index	Section number of inflection meta index. 0xFFFFFFFF if index is not available.
//    48	    0x30	4	Index names	0xFFFFFFFF if index is not available.
//    52	    0x34	4	Index keys	0xFFFFFFFF if index is not available.
//    56	    0x38	4	Extra index 0	Section number of extra 0 meta index. 0xFFFFFFFF if index is not available.
//    60	    0x3c	4	Extra index 1	Section number of extra 1 meta index. 0xFFFFFFFF if index is not available.
//    64	    0x40	4	Extra index 2	Section number of extra 2 meta index. 0xFFFFFFFF if index is not available.
//    68	    0x44	4	Extra index 3	Section number of extra 3 meta index. 0xFFFFFFFF if index is not available.
//    72	    0x48	4	Extra index 4	Section number of extra 4 meta index. 0xFFFFFFFF if index is not available.
//    76	    0x4c	4	Extra index 5	Section number of extra 5 meta index. 0xFFFFFFFF if index is not available.
//    80	    0x50	4	First Non-book index?	First record number (starting with 0) that's not the book's text
//    84	    0x54	4	Full Name Offset	Offset in record 0 (not from start of file) of the full name of the book
//    88	    0x58	4	Full Name Length	Length in bytes of the full name of the book
//    92	    0x5c	4	Locale	Book locale code. Low byte is main language 09=English,
//    				        next byte is dialect, 08=British, 04=US. Thus US English is
//     				        1033, UK English is 2057.
//    96	    0x60	4	Input Language	Input language for a dictionary
//    100	    0x64	4	Output Language	Output language for a dictionary
//    104	    0x68	4	Min version	Minimum mobipocket version support needed to read this file.
//    108	    0x6c	4	First Image index	First record number (starting with 0)
//    		        		that contains an image. Image records should be sequential.
//    112	    0x70	4	Huffman Record Offset	 The record number of the first huffman compression record.
//    116	    0x74	4	Huffman Record Count	 The number of huffman compression records.
//    120	    0x78	4	Huffman Table Offset	
//    124	    0x7c	4	Huffman Table Length	
//    128	    0x80	4	EXTH flags	bitfield. if bit 6 (0x40) is set, then there's an EXTH record
//    132	    0x84	32	?	32 unknown bytes, if MOBI is long enough
//    164	    0xa4	4	DRM Offset	Offset to DRM key info in DRMed files. 0xFFFFFFFF if no DRM
//    168	    0xa8	4	DRM Count	Number of entries in DRM info. 0xFFFFFFFF if no DRM
//    172	    0xac	4	DRM Size	Number of bytes in DRM info.
//    176	    0xb0	4	DRM Flags	Some flags concerning the DRM info.
//    180	    0xb4	12	Unknown	Bytes to the end of the MOBI header, including the
//    		    		    following if the header length >= 228 (244 from start of record).
//                          Use 0x000000000000000000000000.
//    192	    0xc0	2	First content record number	Number of first text record. Normally 1.
//    194	    0xc2	2	Last content record number	Number of last image record or
//    				        number of last text record if it contains no images.
//     			    	    Includes Image, DATP, HUFF, DRM.
//    196	    0xc4	4	Unknown	Use 0x00000001.
//    200	    0xc8	4	FCIS record number	
//    204	    0xcc	4	Unknown (FCIS record count?)	Use 0x00000001.
//    208	    0xd0	4	FLIS record number	
//    212	    0xd4	4	Unknown (FLIS record count?)	Use 0x00000001.
//    216	    0xd8	8	Unknown	Use 0x0000000000000000.
//    224	    0xe0	4	Unknown	Use 0xFFFFFFFF.
//    228	    0xe4	4	First Compilation data section count	Use 0x00000000.
//    232	    0xe8	4	Number of Compilation data sections	Use 0xFFFFFFFF.
//    236	    0xec	4	Unknown	Use 0xFFFFFFFF.
//    240	    0xf0	4	Extra Record Data Flags	 A set of binary flags, some of 
//    			    	    which indicate extra data at the end of each text block.
//                          This only seems to be valid for Mobipocket format version 5
//     				        and 6 (and higher?), when the header length is 228 (0xE4) or 232 (0xE8).
//                          bit 1 (0x1): <extra multibyte bytes><size>
//    				        bit 2 (0x2): <TBS indexing description of this HTML record><size>
//    				        bit 3 (0x4): <uncrossable breaks><size> Setting bit 2 (0x2) disables <guide><reference type="start"> functionality.
//    244	    0xf4	4	INDX Record Offset	(If not 0xFFFFFFFF)The record number of
//    			    	    the first INDX record created from an ncx file.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobiEPUB.PRC;

namespace MobiEPUB.PRC
{
    class MOBIheader
    {
        private String m_MOBI;
        private int m_MOBIlen;
        private int m_MOBItype;
        private int m_Encoding;
        private int m_Locale;
        private int m_PrcVersion;
        private String m_Fullname;
        private int m_FirstImageRec;
        private int m_FirstHuffmanRec;
        private int m_HuffmanRecCnt;
        private int m_HuffmanTableOffset;
        private int m_HuffmanTableLen;

        public MOBIheader(Header header)
        {
            // MOBI eye catcher - Bytes 16-4 ASCII
            m_MOBI = header.ReadString(16, 4);
            if (!m_MOBI.Equals("MOBI"))
                throw new Exception("Invalid PRC file (reason 01)");

            // Header length, including eye catcher : Bytes 20-4 big-endian integer
            m_MOBIlen = header.ReadInt(20);

            // MOBI type : Bytes 24-4 big-endian integer
            m_MOBItype = header.ReadInt(24);

            // Encoding 1252=CP1252 65001=UTF-8 : Bytes 28-4 big-endian integer
            m_Encoding = header.ReadInt(28);

            // PRC version - mobipocket format : Bytes 36-4 big-endian integer
            m_PrcVersion = header.ReadInt(36);

            // Document Fullname: Offset into rec 0 at bytes 84-4 length 88-4
            int fnOffset = header.ReadInt(84);
            int fnLength = header.ReadInt(88);
            m_Fullname = header.ReadString(fnOffset, fnLength);

            // Locale : Bytes 92-4 big-endian integer
            m_Locale = header.ReadInt(92);

            // First image rec: Bytes 108-4 big-endian integer
            m_FirstImageRec = header.ReadInt(108);

            // First huffman rec: Bytes 112-4 big-endian integer
            m_FirstHuffmanRec = header.ReadInt(112);

            // Huffman rec count: Bytes 116-4 big-endian integer
            m_HuffmanRecCnt = header.ReadInt(116);

            // Huffman table offset: Bytes 120-4 big-endian integer
            m_HuffmanTableOffset = header.ReadInt(120);

            // Huffman table length: Bytes 124-4 big-endian integer
            m_HuffmanTableLen = header.ReadInt(124);
        }

        public int MOBItype { get { return m_MOBItype; } }

        public int Encoding { get { return m_Encoding; } }

        public int Version { get { return m_PrcVersion; } }

        public int Locale { get { return m_Locale; } }

        public int FirstImageRec { get { return m_FirstImageRec; } }

        public String Fullname { get { return m_Fullname; } }

        public bool ImagesExist { get { return (m_FirstImageRec > 0); } }
    }
}
