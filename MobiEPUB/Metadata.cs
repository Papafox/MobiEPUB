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

namespace MobiEPUB
{
    class Metadata
    {
        private String m_Title;
        private String m_Author;
        private String m_Language;
        private String m_Subject;

        public Metadata()
        {
        }

        public String Title { get { return m_Title; } set { m_Title = value; } }
        public String Author { get { return m_Author; } set { m_Author = value; } }
        public String Language { get { return m_Language; } set { m_Language = value; } }
        public String Subject { get { return m_Subject; } set { m_Subject = value; } }
    }
}
