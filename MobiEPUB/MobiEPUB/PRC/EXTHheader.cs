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


//    EXTH Header
//
//    If the MOBI header indicates that there's an EXTH header, it follows
//    immediately after the MOBI header. Since the MOBI header is of variable
//    length, this isn't at any fixed offset in record 0. Note that some readers
//    will ignore any EXTH header info if the mobipocket version number specified
//    in the MOBI header is 2 or less (perhaps 3 or less).
//
//    The EXTH header is also undocumented, so some of this is guesswork.
//
//    bytes	content	   	  	comments
//    4	   	identifier	    the characters "E X T H"
//    4	   	header length	the length of the EXTH header, including the previous
//    						4 bytes - but not including the final padding.
//    4	   	record Count	The number of records in the EXTH header. the rest
//    						of the EXTH header consists of repeated EXTH records
//     						to the end of the EXTH length.
//
//    EXTH record start	Repeat until done.
//    4	   	record type		Exth Record type. Just a number identifying what's stored in the record
//    4	   	record length	length of EXTH record = L , including the 8 bytes
//    						in the type and length fields L-8	record data	Data.
//
//    EXTH record end	Repeat until done.
//    p	 	padding		   	Null bytes to pad the EXTH header to a multiple of
//    						four bytes (none if the header is already a
//     						multiple of four). This padding is not included in
//     						the EXTH header length.
//
//    There are lots of different EXTH Records types. Ones found so far in
//    Mobipocket files are listed here, with possible meanings. Hopefully these 
//    will be filled in as more information comes to light.
//
//    record type	usual length	name	comments	opf meta tag
//    1		drm_server_id		
//    2	   	drm_commerce_id		
//    3		drm_ebookbase_book_id		
//    100		author		   	 <dc:Creator>
//    101		publisher	 	 <dc:Publisher>
//    102		imprint		 	 <Imprint>
//    103		description		 <dc:Description>
//    104		isbn		 	 <dc:Identifier scheme='ISBN'>
//    105		subject			 <dc:Subject>
//    106		publishingdate	 <dc:Date>
//    107		review			 <Review>
//    108		contributor		 <dc:Contributor>
//    109		rights			 <dc:Rights>
//    110		subjectcode		 <dc:Subject BASICCode="subjectcode">
//    111		type			 <dc:Type>
//    112		source			 <dc:Source>
//    113		asin		
//    114		versionnumber		
//    115	4	sample			 0x0001 if the book content is only a sample of the full book	
//    116		startreading	 Position (4-byte offset) in file at which to open when first opened	
//    117	3	adult			 Mobipocket Creator adds this if Adult only is checked on its GUI; contents: "yes"	 <Adult>
//    118		retail price	 As text, e.g. "4.99"	<SRP>
//    119		retail price currency	As text, e.g. "USD"	<SRP Currency="currency">
//    200	3	Dictionary short name	As text	<DictionaryVeryShortName>
//    201	4	coveroffset	 	 Add to first image field in Mobi Header to find PDB record containing the cover image	<EmbeddedCover>
//    202	4	thumboffset	 	 Add to first image field in Mobi Header to find PDB record containing the thumbnail cover image	
//    203		hasfakecover		
//    204	4	Creator Software Known Values: 1=mobigen, 2=Mobipocket Creator, 
//    						     200=kindlegen (Windows), 201=kindlegen (Linux), 202=kindlegen (Mac).
//    						     Warning: Calibre creates fake creator entries, pretending to be a Linux
//     						     kindlegen 1.2 (201, 1, 2, 33307) for normal ebooks and a non-public Linux 
//    						     kindlegen 2.0 (201, 2, 0, 101) for periodicals.	
//    205	4	Creator Major Version		
//    206	4	Creator Minor Version		
//    207	4	Creator Build Number		
//    208		watermark		
//    209		tamper proof keys	Used by the Kindle (and Android app) for generating book-specific PIDs.	
//    300		fontsignature		
//    401	1	clippinglimit	Integer percentage of the text allowed to be clipped. Usually 10.	
//    402		publisherlimit		
//    403		Unknown		
//    404	1	ttsflag	1 - Text to Speech disabled; 0 - Text to Speech enabled	
//    405	1	Unknown		
//    406	8	Unknown		
//    407	8	Unknown		
//    450	4	Unknown		
//    451	4	Unknown		
//    452	4	Unknown		
//    453	4	Unknown		
//    501	4	cdetype	 PDOC - Personal Doc; EBOK - ebook; EBSP - ebook sample;	
//    502		lastupdatetime		
//    503		updatedtitle		
//    504		asin	 I found a copy of ASIN in this record.	
//    524		language		<dc:language>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobiEPUB.PRC;

namespace MobiEPUB.PRC
{
    class EXTHheader
    {
        private int m_MOBIlen;
        private bool m_EXTHexists;
        private int m_EXTHoffset;
        private String m_EXTH;
        private Dictionary<int, string> m_EXTHrecs = new Dictionary<int, string>();

        public EXTHheader(Header header)
        {
            // Header length, including eye catcher : Bytes 20-4 big-endian integer
            m_MOBIlen = header.ReadInt(20);

            // EXTH follows immediately after MOBI
            m_EXTHoffset = m_MOBIlen + 16;

            // EXTH eye catcher - Bytes EXTH+0-4 ASCII
            m_EXTH = header.ReadString(m_EXTHoffset, 4);
            if (!m_EXTH.Equals("EXTH"))
                throw new Exception("Invalid PRC file (reason 02)");

            // Get the total length of the EXTH record
            int totLen = header.ReadInt(m_EXTHoffset + 4);

            // Get the count of EXTH records
            int recCnt = header.ReadInt(m_EXTHoffset + 8);

            // Loop through the rest of the EXTH records saving them
            int endOffset = m_EXTHoffset + totLen;
            int pos = m_EXTHoffset + 12;
            while (pos < endOffset - 1)
            {
                String value;

                int recType = header.ReadInt(pos);
                int recLen = header.ReadInt(pos + 4);
                switch (recType)
                {
                    case 115:
                    case 116:
                    case 201:
                    case 202:
                    case 204:
                    case 205:
                    case 206:
                    case 207:
                        value = header.ReadInt(pos + 8).ToString();
                        break;
                    default:
                        value = header.ReadString(pos + 8, recLen - 8);
                        break;
                }
                m_EXTHrecs.Add(recType, value);
                pos += recLen;
            }
        }

        private String Lookup(int key)
        {
            String result = null;
            return (m_EXTHrecs.TryGetValue(100, out result)) ? result : null;
        }

        private int LookupInt(int key)
        {
            String result = null;
            return ((m_EXTHrecs.TryGetValue(100, out result))) ? int.Parse(result) : -1;
        }

        public String Author { get { return Lookup(100); } }

        public String Publisher { get { return Lookup(101); } }

        public String Decription { get { return Lookup(103); } }

        public String ISBN { get { return Lookup(104); } }

        public String Subject { get { return Lookup(105); } }

        public String PublishDate { get { return Lookup(106); } }

        public String Contributor { get { return Lookup(108); } }

        public String Rights { get { return Lookup(109); } }

        public String Source { get { return Lookup(112); } }

        public String ASIN { get { return Lookup(113); } }

        public bool Sample { get { return (LookupInt(115) == 1); } }

        public int Software { get { return LookupInt(204); } }

        public String Language { get { return Lookup(524); } }
    }
}
