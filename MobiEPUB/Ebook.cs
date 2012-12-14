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
using System.IO;

namespace MobiEPUB
{
    abstract class Ebook
    {
        protected Metadata _meta;
        protected String _filename;
        protected Stream _stream;


        public Ebook()
        {
            _meta = new Metadata();
        }

        public String Filename { get { return _filename; } }

        public void Save()
        {
            // Check that a filename has been specified
            if (_filename == null || _filename.Length == 0)
                throw new MobiEPUBexception("Save failed: no filename specifed");

            // Check that the metadata is valid:
            // 1. Has a title been specifed?
            if (_meta.Title == null || _meta.Title.Length == 0)
                throw new MobiEPUBexception("Save failed: no title specified");
        }
    }
}
