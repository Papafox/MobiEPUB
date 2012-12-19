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



//    MOBI format was originally an extension of the PalmDOC format by adding
//    certain HTML like tags to the data. Many MOBI formatted documents still use
//    this form. However there is also a high compression version of this file
//    format that compresses data to a larger degree in a proprietary manner.
//    There are some third party programs that can read the eBooks in there MOBI 
//    format but there are only a few third party programs that can read the 
//    eBooks in the new compressed form. The higher compression mode is using a 
//    huffman coding scheme that has been called the Huff/cdic algorithm. For a 
//    description in Python check huffcdic.py available as part of the Calibre 
//    project.
//
//    From time to time features have been added to the format so new files maybe
//    problems if you try to read them with a down level reader. Currently the 
//    source files follow the guidelines in the Open eBook format.
//
//    Note that AZW for the Amazon Kindle is the same format as MOBI except that
//    it uses a different DRM scheme. Amazon owns Mobipocket. The formatting 
//    below applies to both file types.
//
//
//    Format
//
//    Like PalmDOC, the Mobipocket file format is that of a standard Palm Database
//    Format file. The header of that format includes the name of the database
//    (usually the book title and sometimes a portion of the authors name) which
//    is up to 31 bytes of data. The files are identified as Creator ID of 
//    Mobipocket a Type of BOOK.
//
//    Mobipocket have some minimal file format info, mainly about the html
//    encoding they use in the text of the book, at
//    http://www.mobipocket.com/dev/article.asp?BaseFolder=prcgen
//
//
//    PalmDOC Header
//
//    The first record in the Palm Database Format gives more information about
//    the Mobipocket file. The first 16 bytes are almost identical to the first
//    sixteen bytes of a PalmDOC format file.
//
//    offset	bytes	content	comments
//    0	2	Compression	 1 == no compression, 2 = PalmDOC compression, 17480 = HUFF/CDIC compression
//    2	2	Unused	Always zero
//    4	4	text length	Uncompressed length of the entire text of the book
//    8	2	record count	Number of PDB records used for the text of the book.
//    10	2	record size	Maximum size of each record containing text, always 4096
//    12	4	Current Position	Current reading position, as an offset into the uncompressed text
//
//    There are two differences from a Palm DOC file. There's an additional
//    compression type (17480), and the Current Position bytes are used for a
//    different purpose:
//
//    offset	bytes	content	comments
//    12	2	Encryption Type	 0 == no encryption, 1 = Old Mobipocket Encryption, 2 = Mobipocket Encryption
//    14	2	Unknown	Usually zero
//
//    The old Mobipocket Encryption scheme only allows the file to be registered
//    with one PID, unlike the current encryption scheme that allows multiple
//    PIDs to be used in a single file. Unless specifically mentioned, all the
//    encryption information on this page refers to the current scheme.
//
//
//    MOBI Header
//
//    Most Mobipocket file also have a MOBI header in record 0 that follows these
//    16 bytes, and newer formats also have an EXTH header following the 
//    Mobipocket, again all in record 0 of the PDB file format.
//
//    The MOBI header is of variable length and is not documented. Some fields
//    have been tentatively identified as follows:
//
//    offset	hex	bytes	content	comments
//    16	0x10	4	identifier	the characters M O B I
//    20	0x14	4	header length	 the length of the MOBI header, including the previous 4 bytes
//    24	0x18	4	Mobi type	The kind of Mobipocket file this is
//                    2 Mobipocket Book
//                    3 PalmDoc Book
//                    4 Audio
//                    232 mobipocket? generated by kindlegen1.2
//                    248 KF8: generated by kindlegen2
//                    257 News
//                    258 News_Feed
//                    259 News_Magazine
//                    513 PICS
//                    514 WORD
//                    515 XLS
//                    516 PPT
//    				517 TEXT
//                    518 HTML
//    28	0x1c	4	text Encoding	1252 = CP1252 (WinLatin1); 65001 = UTF-8
//    32	0x20	4	Unique-ID	Some kind of unique ID number (random?)
//    36	0x24	4	File version	Version of the Mobipocket format used in this file.
//    40	0x28	4	Ortographic index	Section number of orthographic meta index. 0xFFFFFFFF if index is not available.
//    44	0x2c	4	Inflection index	Section number of inflection meta index. 0xFFFFFFFF if index is not available.
//    48	0x30	4	Index names	0xFFFFFFFF if index is not available.
//    52	0x34	4	Index keys	0xFFFFFFFF if index is not available.
//    56	0x38	4	Extra index 0	Section number of extra 0 meta index. 0xFFFFFFFF if index is not available.
//    60	0x3c	4	Extra index 1	Section number of extra 1 meta index. 0xFFFFFFFF if index is not available.
//    64	0x40	4	Extra index 2	Section number of extra 2 meta index. 0xFFFFFFFF if index is not available.
//    68	0x44	4	Extra index 3	Section number of extra 3 meta index. 0xFFFFFFFF if index is not available.
//    72	0x48	4	Extra index 4	Section number of extra 4 meta index. 0xFFFFFFFF if index is not available.
//    76	0x4c	4	Extra index 5	Section number of extra 5 meta index. 0xFFFFFFFF if index is not available.
//    80	0x50	4	First Non-book index?	First record number (starting with 0) that's not the book's text
//    84	0x54	4	Full Name Offset	Offset in record 0 (not from start of file) of the full name of the book
//    88	0x58	4	Full Name Length	Length in bytes of the full name of the book
//    92	0x5c	4	Locale	Book locale code. Low byte is main language 09=English,
//    				next byte is dialect, 08=British, 04=US. Thus US English is
//     				1033, UK English is 2057.
//    96	0x60	4	Input Language	Input language for a dictionary
//    100	0x64	4	Output Language	Output language for a dictionary
//    104	0x68	4	Min version	Minimum mobipocket version support needed to read this file.
//    108	0x6c	4	First Image index	First record number (starting with 0)
//    				that contains an image. Image records should be sequential.
//    112	0x70	4	Huffman Record Offset	 The record number of the first huffman compression record.
//    116	0x74	4	Huffman Record Count	 The number of huffman compression records.
//    120	0x78	4	Huffman Table Offset	
//    124	0x7c	4	Huffman Table Length	
//    128	0x80	4	EXTH flags	bitfield. if bit 6 (0x40) is set, then there's an EXTH record
//    132	0x84	32	?	32 unknown bytes, if MOBI is long enough
//    164	0xa4	4	DRM Offset	Offset to DRM key info in DRMed files. 0xFFFFFFFF if no DRM
//    168	0xa8	4	DRM Count	Number of entries in DRM info. 0xFFFFFFFF if no DRM
//    172	0xac	4	DRM Size	Number of bytes in DRM info.
//    176	0xb0	4	DRM Flags	Some flags concerning the DRM info.
//    180	0xb4	12	Unknown	Bytes to the end of the MOBI header, including the
//    				    following if the header length >= 228 (244 from start of record).
//                      Use 0x000000000000000000000000.
//    192	0xc0	2	First content record number	Number of first text record. Normally 1.
//    194	0xc2	2	Last content record number	Number of last image record or
//    				    number of last text record if it contains no images.
//     				    Includes Image, DATP, HUFF, DRM.
//    196	0xc4	4	Unknown	Use 0x00000001.
//    200	0xc8	4	FCIS record number	
//    204	0xcc	4	Unknown (FCIS record count?)	Use 0x00000001.
//    208	0xd0	4	FLIS record number	
//    212	0xd4	4	Unknown (FLIS record count?)	Use 0x00000001.
//    216	0xd8	8	Unknown	Use 0x0000000000000000.
//    224	0xe0	4	Unknown	Use 0xFFFFFFFF.
//    228	0xe4	4	First Compilation data section count	Use 0x00000000.
//    232	0xe8	4	Number of Compilation data sections	Use 0xFFFFFFFF.
//    236	0xec	4	Unknown	Use 0xFFFFFFFF.
//    240	0xf0	4	Extra Record Data Flags	 A set of binary flags, some of 
//    				    which indicate extra data at the end of each text block.
//                      This only seems to be valid for Mobipocket format version 5
//     				    and 6 (and higher?), when the header length is 228 (0xE4) or 232 (0xE8).
//                      bit 1 (0x1): <extra multibyte bytes><size>
//    				    bit 2 (0x2): <TBS indexing description of this HTML record><size>
//    				    bit 3 (0x4): <uncrossable breaks><size> Setting bit 2 (0x2) disables <guide><reference type="start"> functionality.
//    244	0xf4	4	INDX Record Offset	(If not 0xFFFFFFFF)The record number of
//    				    the first INDX record created from an ncx file.
//
//
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
//
//
//    Remainder of Record 0
//
//    At the end of Record 0 of the PDB file format, we usually get the full file
//    name, the offset of which is given in the MOBI header.
//
//    There might be data of unknown use between the end of the EXTH records and the name.
//
//    The name is followed by two null bytes, and then padded with null bytes 
//    total four-byte boundary. For example, if the name is 16 bytes long, with 
//    two null bytes, that makes 18 bytes, and it then gets another two null 
//    bytes added to make it up to 20 bytes in total. However, the length stored 
//    in the header is only 16. If the name was 19 bytes, it would be followed by
//    two null bytes to make it up to 21 bytes, and then padded with three more 
//    null bytes to make it up to 24 bytes.
//
//    The name and padding is followed by more data of unknown use, usually null
//    bytes, to the end of section 0.
//
//
//    Index meta record
//
//    The first record of an index contains the meta data of the index.
//
//    offset	hex		bytes	content	comments
//    0			0x00	4		Identifier	the characters I N D X
//    4			0x04	4		header length	the length of the INDX header,
//	  							including the previous 4 bytes
//    8			0x08	4		index type	the type of the index. Known values:
//	  							0 - normal index, 2 - inflections
//    12		0x0c	4		?	?
//    16		0x10	4		?	?
//    20		0x14	4		idxt start	the offset to the IDXT section
//    24		0x18	4		index count	the number of index records
//    28		0x1c	4		index encoding	
//    32		0x20	4		index language	the language code of the index
//    36		0x24	4		total index count	the number of index entries
//    40		0x28	4		ordt start	the offset to the ORDT section
//    44		0x2c	4		ligt start	the offset to the LIGT section
//    48		0x30	4		?	?
//    52		0x34	4		?	?
//
//    The remaining INDX header values are unknown.
//
//
//    TAGX section
//
//    The TAGX section follows the INDX header and is essential for decoding 
//    these values, as it defines which how many control bytes an entry contains,
//    which bits correspond to which tag and how many values a tag requires (most
//    tag need one value, but some have two, maybe more).
//
//    offset	hex		bytes	content	comments
//    0			0x00	4		Identifier	the characters T A G X
//    4			0x04	4		header length	the length of the TAGX header,
//	  							including the previous 4 bytes
//    8			0x08	4		control byte count	the number of control bytes
//    12		0x0c	n		tag table	the tag table entries (n = header
//	  							length - 12, must be multiple of 4 bytes)
//
//    The tag table entries are multiple of 4 bytes. The first byte is the tag,
//    the second byte the number of values, the third byte the bit mask and these
//    byte indicates the end of the control byte. If the fourth byte is 0x01, 
//    all other bytes of the entry are zero.
//
//
//    Variable-width integers
//
//    Some parts of the Mobipocket format encode data as variable-width integers.
//    These integers are represented big-endian with 7 bits per byte in bits 1-7.
//    They may be either forward-encoded, in which case only the LSB has bit 8
//    set, or backward-encoded, in which case only the MSB has bit 8 set. For
//    example, the number 0x11111 would be represented forward-encoded as:
//       0x04 0x22 0x91
//    And backward-encoded as:
//       0x84 0x22 0x11
//
//
//    Trailing entries
//
//    The Extra Data Flags field of the MOBI header indicates which, if any,
//    trailing entries are appended to the end of each text record. Each set bit
//    in the field indicates a trailing entry. The entries appear to occur 
//    index-order; e.g., trailing entry 1 immediately follows the text content 
//    and entry 16 occurs at the very end of the record. The effect and exact 
//    details of most of these entries is unknown. The trailing entries indicated
//    by bits 2-16 appear to follow a common format. That format is:
//       <data><size>
//
//    Where <size> is the size of the entire trailing entry (including the size
//    of <size>) as a backward-encoded Mobipocket variable-width integer.
//
//    Only a few bits have been identified
//
//    bit	   	Data at end of records
//    0x0001	Multi-byte character overlaps
//    0x0002	Some data to help with indexing
//    0x0004	Some data about uncrossable breaks
//
//
//    Multibyte character overlap
//
//    When bit 1 of the Extra Data Flags field is set, each record is followed by
//    a trailing entry containing any extra bytes necessary to complete a
//    multibyte character which crosses the record boundary. The bytes do not
//    participate in compression regardless which compression scheme is used for
//    the file. However, unlike the trailing data bytes, the multibytes
//    (including the count byte) do get included in any encryption. The
//    overlapping bytes then re-appear as normal content at the beginning of the
//    following record. The trailing entry ends with a byte containing a count of
//    the overlapping bytes plus additional flags.
//
//    offset	bytes	content	comments
//    0		0-3	   	N terminal bytes of a multibyte character	
//    N		1	    Size & flags	bits 1-2 encode N, use of bits 3-8 is unknown
//
//
//    PalmDOC Compression
//
//    PalmDOC uses LZ77 compression techniques. DOC files can contain only
//    compressed text. The format does not allow for any text formatting. This
//    keeps files small, in keeping with the Palm philosophy. However, extensions
//    to the format can use tags, such as HTML or PML, to include formatting
//    within text. These extensions to PalmDoc are not interchangeable and are
//    the basis for most eBook Reader formats on Palm devices.
//
//    LZ77 algorithms achieve compression by replacing portions of the data with
//    references to matching data that has already passed through both encoder
//    and decoder. A match is encoded by a pair of numbers called a length-distance
//    pair, which is equivalent to the statement "each of the next length
//    characters is equal to the character exactly distance characters behind it
//    in the uncompressed stream." (The "distance" is sometimes called the
//    "offset" instead.)
//
//    In the PalmDoc format, a length-distance pair is always encoded by a
//    two-byte sequence. Of the 16 bits that make up these two bytes, 11 bits go
//    to encoding the distance, 3 go to encoding the length, and the remaining
//    two are used to make sure the decoder can identify the first byte as the
//    beginning of such a two-byte sequence. The exact algorithm needed to decode
//    the compressed text can be found on the PalmDOC page.
//
//    PalmDOC data is always divided into 4096 byte blocks (uncompressed size) and
//    the blocks are acted upon independently; no information from previous or
//    later blocks is needed when a block is being compressed or decompressed.
//    PalmDOC does have support for bookmarks. These pointers are named and refer
//    to an offset location in a file. If the file is edited these locations may
//    no longer refer to the correct locations. Some reading programs allow
//    the user to enter or edit these bookmarks while others treat them as a TOC.
//    Some reading programs may ignore them entirely. They are stored at the end
//    of the file itself so the full file needs to be scanned when loaded to find
//    them.
//
//
//    Image Records
//
//    If the file contains images, they follow the text blocks, with each image
//    using a single block. The 4096-byte record size in the PalmDoc header
//    applies only to text records; image records may be larger.
//
//
//    Magic Records
//
//    In some cases, MobiPocket Creator adds a 2-zero-byte record after the text
//    records in a file. This record is not included in the "record count" of text
//    records in the PalmDoc header, and is also not used as the "first non-book
//    index" in the MOBI header. (If the 2-zero-byte record is present, the index
//    of the following block is used as the "first non-book index".)
//
//    MobiPocket Creator also ends files with three records: 'FLIS', 'FCIS', and
//    'end-of-file', in that order. The 'FLIS' and 'FCIS' records do not seem to
//    be necessary for MobiPocket Reader or the Amazon Kindle 2 to read the file.
//    The 'end-of-file' record might be necessary.
//
//
//    FLIS Record
//
//    The FLIS record appears to have a fixed value. The meaning of the values is
//    not known.
//
//    offset	bytes	content	comments
//    0			4		identifier	the characters F L I S (0x46 0x4c 0x49 0x53)
//    4			4		?	fixed value: 8
//    8			2		?	fixed value: 65
//    10		2		?	fixed value: 0
//    12		4		?	fixed value: 0
//    16		4		?	fixed value: -1 (0xFFFFFFFF)
//    20		2		?	fixed value: 1
//    22		2		?	fixed value: 3
//    24		4		?	fixed value: 3
//    28		4		?	fixed value: 1
//    32		4		?	fixed value: -1 (0xFFFFFFFF)
//
//
//    FCIS Record
//
//    The FCIS record appears to have mostly fixed values.
//
//    offset	bytes	content	comments
//    0	 	    4		identifier	the characters F C I S (0x46 0x43 0x49 0x53)
//    4	 		4		?	fixed value: 20
//    8	 		4		?	fixed value: 16
//    12	 	4		?	fixed value: 1
//    16	 	4		?	fixed value: 0
//    20	 	4		?	text length (the same value as "text length" in the PalmDoc header)
//    24		4		?	fixed value: 0
//    28		4		?	fixed value: 32
//    32		4		?	fixed value: 8
//    36		2		?	fixed value: 1
//    38		2		?	fixed value: 1
//    40		4		?	fixed value: 0
//
//
//    End-of-file Record
//
//    The end-of-file record is a fixed 4-byte record. While the last two bytes
//    appear to be a CRLF marker, the meaning of the first two bytes is unknown.
//
//    offset	bytes	content	comments
//    0	   	1	    ?	fixed value: 233 (0xe9)
//    1	   	1	    ?	fixed value: 142 (0x8e)
//    2	   	1	    ?	fixed value: 13 (0x0d)
//    3	   	1	   	?	fixed value: 10 (0x0a)
//
//
//    Compilation Records
//
//    KindleGen creates records of the compilation source (KindleGen 1.2-2.5) and
//    the compilaion source and compiler output (Kindle Gen 2.7-) just before the
//    #End-of-file Record (KindleGen version 1.2-2.2), or just before the BOUNDARY
//    record (KindleGen version 2.3-).
//
//    MOBI files created with Mobipocket creator, Amazon's Personal Document
//    Service, or Kindle Direct Publishing (former Amazon DTP) don't include SRCS
//    record. In a past, kindlegen had an undocumented option to suppress this
//    record, but the option was removed in 2010.
//
//    A SRCS record is a record whose content is a zip archive of all source files
//    (i.e., .opf, .ncx, .htm, .jpg, ...) given to the command and puts it in the
//    generated MOBI file. The record begins with the "SRCS" signature and looks
//    as follows:
//
//    offset	bytes	content	comments
//    0			4		identifier	"SRCS" (0x53 0x52 0x43 0x53)
//    4			4		?	fixed value(?): 0x00000010
//    8			4		?	fixed value(?): 0x0000002f
//    12		4		?	fixed value(?): 0x00000001
//    16		zip		The zip archive continues to the end of this record
//
//    A CMET record is a record whose content is the output of the compilation
//    operation, and perhaps extra info. The record begins with the "CMET"
//    signature and looks as follows:
//
//    offset	bytes			content	comments
//    0	    	4	   			identifier	"CMET" (0x43 0x4D 0x45 0x54)
//    4	    	4	   			?	fixed value(?): 0x0000000C
//    8	   		4	   			text length	(big endian)
//    12	   	variable text	compilation output text, line endings are CRLF
//    variable	variable	?	unknown data to the end of the record
//
//
//    Media Records (AUDI/VIDE)
//
//    kindlegen supports embedded audio and video for some Kindle platforms. Each
//    media file is stored in a separate AUDI (audio) or VIDE (video) record.
//
//    A media record looks as follows:
//
//    offset	bytes	content	comments
//    0	  		4	   	identifier	"AUDI" (0x41 0x55 0x44 0x49) or "VIDE" (0x56 0x49 0x44 0x45)
//    4	  		4	   	?	unkown value
//    8	  		4	   	?	unknown value
//    12		media	The media data continues to the end of this record


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MobiEPUB.PRC;
using System.Drawing;

namespace MobiEPUB
{
    class PRCebook : Ebook
    {
        private PDBheader m_PDBheader;
        private PRCheader m_PRCheader;
        private MOBIheader m_MOBIheader;
        private EXTHheader m_EXTHheader;
        private PrcImages m_Images;

        private byte[] m_Array;
        private FileStream m_File;
        private String m_Document;
        private Header m_Rec0;

        public PRCebook(String fn)
            : base()
        {
            Open(fn);
        }

        public override void Open(string fn)
        {
            base.Open(fn);
            LoadPRC(fn);
            _meta.Title = m_MOBIheader.Fullname;
            _meta.Author = m_EXTHheader.Author;
            _meta.Language = m_EXTHheader.Language;
            _meta.Subject = m_EXTHheader.Subject;
            _meta.Publisher = m_EXTHheader.Publisher;
        }

        private void LoadPRC(string fn)
        {
            m_File = new FileStream(fn, FileMode.Open, FileAccess.Read);
            m_Array = new Byte[m_File.Length];
            m_File.Read(m_Array, 0, m_Array.Length);
            Header m_Header = new Header(m_Array);
            m_PDBheader = new PDBheader(m_Header);
            byte[] prcRec = m_PDBheader.GetRecord(0);
            m_Rec0 = new Header(prcRec);

            m_PRCheader = new PRCheader(m_Rec0);
            m_MOBIheader = new MOBIheader(m_Rec0);

            // Does an EXTH header exist? Byte 126 bit 6 (0x40) 
            bool m_EXTHexists = (((int)prcRec[126] & 0x40) == 0x00);

            if (m_EXTHexists)
            {
                m_EXTHheader = new EXTHheader(m_Rec0);
            }

            // Images
            m_Images = new PrcImages(m_Rec0, m_PDBheader);
        }

        private String GetDocument()
        {
            // Extract the document 
            StringBuilder doc = new StringBuilder(m_PRCheader.TextLenth + 2);
            Decompression decoder = null;
            switch (m_PRCheader.Compression)
            {
                case PRCheader.CompressionMode.None:
                    decoder = new Decompression();
                    break;
                case PRCheader.CompressionMode.PalmDoc:
                    decoder = new LZ77decompress();
                    break;
                case PRCheader.CompressionMode.Huffman:
                    //decoder = new HuffmanDecompress(m_Rec0);
                    break;
                default:
                    throw new Exception("Invalid compression");
            }

            for (int i = 1; i < m_MOBIheader.FirstImageRec; i++)
            {
                byte[] bo = GetRecord(i);
                doc.Append(decoder.Decompress(bo));
            }
            return doc.ToString();
        }
        //------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------

        public PRCheader.CompressionMode Compression { get { return m_PRCheader.Compression; } }

        public int TextLenth { get { return m_PRCheader.TextLenth; } }

        public int PrcRecordCnt { get { return m_PRCheader.RecordCnt; } }

        public int MaxRecLenth { get { return m_PRCheader.MaxRecLenth; } }

        public PRCheader.EncryptionMode Encryption { get { return m_PRCheader.Encryption; } }

        //------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------

        public String Title { get { return m_PDBheader.Filename; } }

        public int Version { get { return m_PDBheader.Version; } }

        public DateTime Created { get { return m_PDBheader.CreationDate; } }

        public DateTime Modified { get { return m_PDBheader.ModificationDate; } }

        public String Type { get { return m_PDBheader.Type; } }

        public String Creator { get { return m_PDBheader.Creator; } }

        public int PdbRecordCnt { get { return m_PDBheader.RecordCnt; } }

        //------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------

        //public String MOBI { get { return m_MOBI; } }

        public int MOBItype { get { return m_MOBIheader.MOBItype; } }

        public int PrcEncoding { get { return m_MOBIheader.Encoding; } }

        public int PrcVersion { get { return m_MOBIheader.Version; } }

        public String Fullname { get { return m_MOBIheader.Fullname; } }

        public bool ImagesExist { get { return m_MOBIheader.ImagesExist; } }

        //------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------

        public bool EXTHexists { get { return (m_EXTHheader != null); } }

        //------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------

        public Image[] Images { get { return m_Images.Images; } }

        public String Document
        {
            get
            {
                if (m_Document == null) m_Document = GetDocument();
                return m_Document;
            }
        }

        //------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------

        private int Read(byte[] array, int offset)
        {
            int result = ((offset + 32) < m_Array.Length) ? 32 : m_Array.Length - offset;
            Array.Copy(m_Array, offset, array, 0, result);
            return result;
        }

        public byte[] GetRecord(int recnum)
        {
            return m_PDBheader.GetRecord(recnum);
        }

        public String DecompressRecord(int recnum)
        {
            Decompression decoder = null;
            switch (m_PRCheader.Compression)
            {
                case PRCheader.CompressionMode.None:
                    decoder = new Decompression();
                    break;
                case PRCheader.CompressionMode.PalmDoc:
                    decoder = new LZ77decompress();
                    break;
                case PRCheader.CompressionMode.Huffman:
                    //decoder = new HuffmanDecompress(m_Rec0);
                    break;
                default:
                    throw new Exception("Invalid compression");
            }
            Byte[] bo = GetRecord(recnum);
            return decoder.Decompress(bo);
        }

        public int GetRecordOffset(int recnum)
        {
            return m_PDBheader.GetRecordOffset(recnum);
        }

    }
}
