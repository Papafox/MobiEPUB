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

using System;
using System.Collections;

namespace MobiEPUB
{
    class Metadata
    {
        private String m_Title;
        private String m_Author;
        private String m_Language;
        private String m_Subject;

        private ArrayList m_FileList;

        public Metadata()
        {
            m_FileList = new ArrayList();
        }

        public String Title { get { return m_Title; } set { m_Title = value; } }
        public String Author { get { return m_Author; } set { m_Author = value; } }
        public String Language { get { return m_Language; } set { m_Language = value; } }
        public String Subject { get { return m_Subject; } set { m_Subject = value; } }

        public DocumentFile[] Files
        {
            get
            {
                if (m_FileList.Count == 0)
                    return null;
                DocumentFile[] result = m_FileList.ToArray(typeof(DocumentFile)) as DocumentFile[];
                return result;
            }
        }

        public DocumentFile AddFile(String fn)
        {
            DocumentFile doc = new DocumentFile();
            foreach (DocumentFile d in m_FileList)
            {
                if (d.ItemID == doc.ItemID)
                    throw new MobiEPUBexception("Duplicate document itemid '" + doc.ItemID + "' used for files '" + fn + "' and '" + d.Filename);
            }
            doc.Filename = fn;
            m_FileList.Add(doc);
            return doc;
        }

        public DocumentFile AddFile(String id, String fn)
        {
            DocumentFile doc = new DocumentFile(id);
            foreach (DocumentFile d in m_FileList)
            {
                if (d.ItemID == doc.ItemID)
                    throw new MobiEPUBexception("Duplicate document itemid '" + doc.ItemID + "' used for files '" + fn + "' and '" + d.Filename);
            }
            doc.Filename = fn;
            m_FileList.Add(doc);
            return doc;
        }

    }

    class DocumentFile
    {
        private static int m_id_count = 0;
        private String m_Filename;
        private String m_Mimetype;
        private String m_Id;

        public DocumentFile()
        {
            m_Id = "id" + (m_id_count++).ToString("D2");
        }

        public DocumentFile(String id)
        {
            m_Id = id;
        }

        public String Filename { get { return m_Filename; } set { m_Filename = value; } }
        public String Mimetype { get { return m_Mimetype; } set { m_Mimetype = value; } }
        public String ItemID { get { return m_Id; } }
    }
}
