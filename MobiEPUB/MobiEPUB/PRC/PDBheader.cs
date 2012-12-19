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


//	Palm Database Format

//	A Palm Database is not a file when stored on the Palm in RAM. However it will
//  be stored as a file if it is synced to a computer. The name of that file will
//  always be the database name. Multi-byte numerical fields are stored in big-endian order,
//  with the most significant byte first. The file will have the following content:
//
//	offset	bytes	content     comments
//	0	   	32	    name	    database name. This name is 0 terminated in the field
//                              and will be used as the file name on a computer. For eBooks
//                              this usually contains the title and may have the author
//                              depending on the length available.
//	32	    2	    attributes  bit field.
//	                0x0002      Read-Only
//	                0x0004      Dirty       AppInfoArea
//	                0x0008      Backup this database (i.e. no conduit exists)
//	                0x0010      (16 decimal) Okay to install newer over existing copy, if present on PalmPilot
//	                0x0020      (32 decimal) Force the PalmPilot to reset after this database is installed
//	                0x0040      (64 decimal) Don't allow copy of file to be beamed to other Pilot.
//	34	    2	    version	file version
//	36	    4	    creation date	No. of seconds since start of January 1, 1904.
//	40	    4	    modification date	 No. of seconds since start of January 1, 1904.
//	44	    4	    last backup date	No. of seconds since start of January 1, 1904.
//	48	    4	    modificationNumber	
//	52	    4	    appInfoID	offset to start of Application Info (if present) or null
//	56	    4	    sortInfoID	offset to start of Sort Info (if present) or null
//	60	    4	    type	See above table. (For Applications this data will be 'appl')
//	64	    4	    creator	See above table. This program will be launched if the file is tapped
//	68	    4	    uniqueIDseed	used internally to identify record
//	72	    4	    nextRecordListID	Only used when in-memory on Palm OS. Always set to zero in stored files.
//	76	    2	    number of Records	number of records in the file - N
//	78	    8N	    record Info List	
//	    start of record
//      info entry	 Repeat N times to end of record info entry
//	        4	    record Data Offset	 the offset of record n from the start of the PDB of this record
//	        1	    record Attributes	bit field. The least significant four bits are used to
//                  represent the category values. These are the categories used to split the
//                  databases for viewing on the screen. A few of the 16 categories are
//                  pre-defined but the user can add their own. There is an undefined category
//                  for use if the user or programmer hasn't set this.
//	                0x10 (16 decimal) Secret record bit.
//	                0x20 (32 decimal) Record in use (busy bit).
//	                0x40 (64 decimal) Dirty record bit.
//	                0x80 (128, unsigned decimal) Delete record on next HotSync.
//	        3	    UniqueID	The unique ID for this record. Often just a sequential count from 0
//	    end of record
//      info entry
//	        2?	    Gap to data	traditionally 2 zero bytes to Info or raw data
//	        ?	    Records	The actual data in the file. AppInfoArea (if present), SortInfoArea (if present) and then records sequentially
//
//
//	PDB Times
//
//	The original PDB format used times counting in seconds from 1st January, 1904.
//  This is the base time used by the original Mac OS, and there were close links
//  between Palm OS and Mac OS. Using an unsigned 32-bit integer this will overflow
//  sometime in 2040.
//
//	However, the PDB tool written for Perl says that the time should be counted
//  from 1st January 1970 (the Unix base time), and uses a signed 32-bit integer
//  which will overflow sometime in 2038.
//
//	This conflict is unfortunate, but there's a simple way to sort out what's been
//  done in a file you are examining. If the time has the top bit set, it's an
//  unsigned 32-bit number counting from 1st Jan 1904. If the time has the top bit
//  clear, it's a signed 32-bit number counting from 1st Jan 1970.
//
//	This can be stated this with some confidence, as otherwise the time would be
//  before 1972 or before 1970, depending on the interpretation and the PDB format
//  wasn't around then. For either system, overflow will occur in around 30 years
//  time. Hopefully by then everyone will be on some properly documented eBook standard.

using System;
using System.Collections;
using MobiEPUB.PRC;

namespace MobiEPUB.PRC
{
    class PDBheader
    {

        private String m_Filename;
        private int m_Version;
        private DateTime m_CreationDate;
        private DateTime m_ModificationDate;
        private String m_Type;
        private String m_Creator;
        private int m_RecordCnt;
        private ArrayList m_RecPointer;
        private Header m_Array;
        //private byte[] m_Array;

        public PDBheader(Header header)
        {
            int temp;

            if (header.Length < 80)
                throw new Exception("PDB header is too small");
            m_Array = header;

            // Filename. Bytes 0-32 null terminated ASCII
            m_Filename = m_Array.ReadString(0, 32);
            m_Filename = m_Filename.Substring(0, m_Filename.IndexOf('\0'));

            // Version - Bytes 34-2 high-endian integer;
            m_Version = m_Array.ReadShort(34);

            // Creation date - Bytes 36-4 high-endian integer. Despite the comments
            // above, the time is based on 1-Jan-1970 not 1-Jan-1904
            temp = m_Array.ReadInt(36);
            m_CreationDate = new DateTime(1970, 1, 1, 0, 0, 0);
            m_CreationDate = m_CreationDate.AddSeconds((double)temp);

            // Modification date - Bytes 40-4 high-endian integer. Despite the comments
            // above, the time is based on 1-Jan-1970 not 1-Jan-1904
            temp = m_Array.ReadInt(40);
            m_ModificationDate = new DateTime(1970, 1, 1, 0, 0, 0);
            m_ModificationDate = m_ModificationDate.AddSeconds((double)temp);

            // Type - Bytes 60-4 ASCII
            m_Type = m_Array.ReadString(60, 4);

            // Creator - Bytes 64-4 ASCII
            m_Creator = m_Array.ReadString(64, 4);

            // Record count - Bytes 76-2 high-endian integer;
            m_RecordCnt = m_Array.ReadShort(76);

            // Build a list of pointers to records.  These are held in an array
            // starting in bytes 78-8.
            m_RecPointer = new ArrayList(m_RecordCnt);
            for (int i = 0; i < m_RecordCnt; i++)
            {
                int offs = 78 + (i * 8);
                temp = m_Array.ReadInt(offs);
                m_RecPointer.Add(temp);
            }
        }

        public byte[] GetRecord(int recnum)
        {
            if ((recnum >= m_RecordCnt) || (recnum < 0))
                throw new Exception("Record no: " + recnum.ToString() + " is out of range (min = 0, max = " + m_RecordCnt + ")");

            int offset = (int)m_RecPointer[recnum];
            int len = ((recnum < m_RecordCnt) ? (int)m_RecPointer[recnum + 1] : m_Array.Length) - offset;
            byte[] result = new Byte[len];
            Array.Copy(m_Array.Array, offset, result, 0, len);
            return result;
        }

        public int GetRecordOffset(int recnum)
        {
            return (int)m_RecPointer[recnum];
        }

        public String Filename { get { return m_Filename; } }

        public int Version { get { return m_Version; } }

        public DateTime CreationDate { get { return m_CreationDate; } }

        public DateTime ModificationDate { get { return m_ModificationDate; } }

        public String Type { get { return m_Type; } }

        public String Creator { get { return m_Creator; } }

        public int RecordCnt { get { return m_RecordCnt; } }
    }
}
