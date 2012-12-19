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
using System.Collections;
using System.Drawing;
using System.IO;

namespace MobiEPUB.PRC
{
    class PrcImages
    {
        private ArrayList m_ImageArray;
        private int m_FirstImageRec;

        public PrcImages(Header header, PDBheader pdb)
        {
            m_FirstImageRec = header.ReadInt(108);

            // First image rec: Bytes 108-4 big-endian integer
            if (m_FirstImageRec > 0)
            {
                m_ImageArray = new ArrayList();

                // Loop though images.  Ignore last two records (FLIS and FCIS)
                for (int i = m_FirstImageRec; i < pdb.RecordCnt - 3; i++)
                {
                    Image newImage;

                    // get the next image
                    Byte[] imageRec = pdb.GetRecord(i);

                    // Check for various image formats (JPG,GIF,BMP etc)
                    String jpgFlag = ReadString(imageRec, 6, 4);
                    if (jpgFlag.Equals("JFIF"))
                    {
                        newImage = LoadJPEG(imageRec);
                        m_ImageArray.Add(newImage);
                    }
                    String gifFlag = ReadString(imageRec, 0, 3);
                    if (gifFlag.Equals("GIF"))
                    {
                        newImage = LoadGIF(imageRec);
                        m_ImageArray.Add(newImage);
                    }
                }
            }
        }

        public Image[] Images { get { return m_ImageArray.ToArray(typeof(Image)) as Image[]; } }

        //------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------

        private Image LoadJPEG(Byte[] imageRec)
        {
            MemoryStream mem = new MemoryStream(imageRec.Length);
            mem.Write(imageRec, 0, imageRec.Length);
            System.Drawing.Image result = System.Drawing.Image.FromStream(mem);
            return result;
        }

        private Image LoadGIF(Byte[] imageRec)
        {
            return LoadJPEG(imageRec);
        }

        //------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------

        private String ReadString(Byte[] array, int pos, int len)
        {
            return Encoding.ASCII.GetString(array, pos, len);
        }

        public int ReadShort(Byte[] array, int pos)
        {
            int result = ((int)array[pos] << 8) | (int)array[pos + 1];
            return result;
        }
    }
}
