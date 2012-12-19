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

//   * offset 0: record identifier  4 bytes, always 'huff'
//   * offset 4: header length 4 bytes, big-endian long int, always = 24
//   * offset 8: cache table (big-endian) offset 4 bytes, big-endian long int, always = 24
//   * offset 12: base table (big-endian) offset 4 bytes, big-endian long int, always = 1048
//   * offset 16: cache table (little-endian) offset  4 bytes, big-endian long int, always = 1304
//   * offset 20: base table (little-endian) offset 4 bytes, big-endian long int, always = 2328
//
//   * offset 24: cache table (big-endian) 1024 bytes, 256 big-endian long ints
//        this is a look up table for the length and decoding of short codewords. 
//        if the codeword represented by the 8 bits is unique, then bit 7 (0x80) 
//        will be set, and the low 5 bits are the length in bits of the code. 
//        the high three bytes partially represent the final symbol.
//        if bit 7 is clear, then the code is looked up in the base table
//
//   * offset 1048: base table (big-endian)
//        256 bytes, 64 big-endian long ints
//        this is where the codeword is looked up if it isn't found in the cache table.
//
//   * offset 1304: cache table (little-endian)
//        1024 bytes, 256 little-endian long ints.
//        this contains exactly the same data as in the cache table at offset 24, except that all of the values are stored in little-endian format instead of big-endian.
//        presumably this is for a speed advantage on slow little-endian processors. this module uses only the big-endian tables.
//
//   * offset 2328: base table (little-endian)
//        256 bytes, 64 little-endian long ints
//        this contains exactly the same data as in the base table at offset 1048, except that all of the values are stored in little-endian format instead of big-endian.
//        presumably this is for a speed advantage on slow little-endian processors. this module uses only the big-endian tables.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobiEPUB.PRC
{
    class HuffmanDecompress : Decompression
    {
        public HuffmanDecompress(Header header)
            : base()
        {
        }

        public override String Decompress(Byte[] array)
        {
            throw new Exception("Not implemented");
        }

    }
}
