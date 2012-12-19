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


//    This is a detailed description about the Palm OS implementation of Lz77
//    compression, which is available in version 4.0 and later of the Palm OS. The
//    compression scheme has the following components:
//
//    	an indicator bit
//    	an index value (11 bits long)
//    	a length indicator value (4 bits long)
//
//    Component	 	 Length 	   	Description
//    indicator bit	 1 bit 		   	If the value of this bit is 1, then the
//    								next 8 bits represents a byte value.
//
//    								If the value of this bit is 0, then the
//    								next 11 bits represents an index, followed
//    								by 4 bits of length information.
//
//    index value	  11 bits	 	Points into the data that has already been
//    								parsed in the buffer. The buffer is 2048 bytes long.
//
//    length	  	  4 bits		The number of bytes to retrieve from the buffer
//    information					(starting at the index byte) is 3 + the value of
//    								this field. For example:
//    									if the value is 0, retrieve 3 bytes
//    									if the value is 1, retrieve 4 bytes
//    										...
//    									if the value is 15, retrieve 18 bytes
//
//    Note that this implementation yields a maximum compression ratio of 9, based on
//    the following computation:
//    		18(bytes) * 8 bits / 16 bits  using System;


using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System;

namespace MobiEPUB.PRC
{
    class LZ77decompress : Decompression
    {
        public LZ77decompress()
            : base()
        {
        }


        // Decompress a byte array encoded with PalmDoc's version of LZ77
        // There are three types of compressed data:
        //      type A: Uncompressed
        //      type B: 0x80 Sliding window
        //      type C: 0xC0 a leading space has been stripped
        public override String Decompress(Byte[] array)
        {
            int outputSize = 4096;  // Max possible output buffer for PRC
            Byte[] output = new Byte[outputSize];
            int inPtr = 0;
            int outPtr = 0;

            while (inPtr < array.Length)
            {
                // Get the next compressed input byte
                int currChar = ((int)array[inPtr++]) & 0x00FF;

                if (currChar >= 0x00C0)
                {
                    // type C command (space + char)
                    output[outPtr++] = (byte)' ';
                    output[outPtr++] = (byte)(currChar & 0x007F);
                }
                else if (currChar >= 0x0080)
                {
                    // type B command (sliding window sequence)

                    // Move this to high bits and read low bits
                    currChar = (currChar << 8) | (((int)array[inPtr++]) & 0x00FF);
                    // 3 + low 3 bits (Beirne's 'n'+3)
                    int windowLen = 3 + (currChar & 0x0007);
                    // next 11 bits (Beirne's 'm')
                    int windowDist = (currChar >> 3) & 0x07FF;
                    int windowCopyFrom = outPtr - windowDist;

                    windowLen = Math.Min(windowLen, outputSize - outPtr);
                    while (windowLen-- > 0)
                        output[outPtr++] = output[windowCopyFrom++];
                }
                else if (currChar >= 0x0009)
                {
                    // self-representing, no command
                    output[outPtr++] = (byte)currChar;
                }
                else if (currChar >= 0x0001)
                {
                    // type A command (next c chars are literal)
                    currChar = Math.Min(currChar, outputSize - outPtr);
                    while (currChar-- > 0)
                        output[outPtr++] = array[inPtr++];
                }
                else
                {
                    // c == 0, also self-representing
                    output[outPtr++] = (byte)currChar;
                }
            }

            return Encoding.ASCII.GetString(output, 0, outPtr);
        }
    }
}
