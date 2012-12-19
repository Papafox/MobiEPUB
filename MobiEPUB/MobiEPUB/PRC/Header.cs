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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobiEPUB.PRC
{
    class Header
    {
        private Byte[] m_Header;
        private int m_CurrPos;

        public Header(Byte[] header)
        {
            m_Header = header;
            m_CurrPos = 0;
        }

        public int Length { get { return m_Header.Length; } }

        public Byte[] Array { get { return m_Header; } }

        public void Position(int pos)
        {
            if (pos < 0 || pos > m_Header.Length)
                throw new Exception("Position out of range: Requested " + pos.ToString() + " Max " + m_Header.Length.ToString());
            m_CurrPos = pos;
        }

        public void Get(Byte[] array)
        {
            System.Array.Copy(m_Header, m_CurrPos, array, 0, array.Length);
        }

        public void Get(Byte[] array, int offset, int len)
        {
            if ((offset + len) >= array.Length)
                throw new Exception("Invalid length");
            System.Array.Copy(m_Header, m_CurrPos, array, offset, len);
        }

        public int ReadInt()
        {
            int result = ReadInt(m_CurrPos);
            m_CurrPos += 4;
            return result;
        }

        public int ReadInt(int pos)
        {
            int result = ((int)m_Header[pos] << 24) | ((int)m_Header[pos + 1] << 16) | ((int)m_Header[pos + 2] << 8) | ((int)m_Header[pos + 3]);
            return result;
        }

        public long ReadLong()
        {
            long result = ReadLong(m_CurrPos);
            m_CurrPos += 8;
            return result;
        }

        public long ReadLong(int pos)
        {
            long result = ((long)m_Header[pos] << 56) | ((long)m_Header[pos + 1] << 48) | ((long)m_Header[pos + 2] << 40) | ((long)m_Header[pos + 3] << 32) |
                          ((long)m_Header[pos + 4] << 24) | ((long)m_Header[pos + 5] << 16) | ((long)m_Header[pos + 6] << 8) | ((long)m_Header[pos + 7]);
            return result;
        }

        public int ReadShort()
        {
            int result = ReadShort(m_CurrPos);
            m_CurrPos += 2;
            return result;
        }

        public int ReadShort(int pos)
        {
            int result = ((int)m_Header[pos] << 8) | (int)m_Header[pos + 1];
            return result;
        }

        public String ReadString(int len)
        {
            String result = ReadString(m_CurrPos, len);
            m_CurrPos += len;
            return result;
        }


        public String ReadString(int pos, int len)
        {
            return System.Text.Encoding.ASCII.GetString(m_Header, pos, len);
        }
    }
}
