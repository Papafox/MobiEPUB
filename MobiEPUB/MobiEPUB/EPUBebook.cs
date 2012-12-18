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


//    Definition
//
//    Quoted from the IDPF web site:
//    "'.epub' is the file extension of an XML format for reflowable digital books
//    and publications. '.epub' is composed of three open standards, the Open
//    Publication Structure (OPS), Open Packaging Format (OPF) and Open Container
//    Format (OCF), produced by the IDPF. '.epub' allows publishers to produce and
//    send a single digital publication file through distribution and offers
//    consumers interoperability between software/hardware for unencrypted
//    reflowable digital books and other publications. The Open eBook Publication
//    Structure or 'OEB', originally produced in 1999, is the precursor to OPS."
//
//
//    Usage
//
//    The intent of ePub is to serve both as a source file format and an end user
//    format. For this reason the files are collected into a container for easy
//    dissemination and use. This container is generally a zip file but the
//    extension has been renamed to .epub. It has special requirements by including
//    an uncompressed mime type file while the rest of the data in the file is
//    compressed. An ePub reader should be capable of reading the content in its
//    compressed format.
//
//    Mime Type: Multipurpose Internet Mail Extensions (RFC 2045). “MIME media types”
//    provide a standard methodology for specifying the content type of objects.
//
//
//    Specifications
//
//    The IDPF specification page contains the specifications for this format. In
//    particular check the version 2.01 OPS and OPF specifications and the version
//    1.01 OCF specifications. The informational documents are also quite useful
//    in understanding the standard's intent and content.
//
//
//    OCF
//
//    A typical OCF is a zip file that might look like:
//    	mimetype
//    	META-INF/
//    		container.xml
//      	[manifest.xml]
//      	[metadata.xml]
//      	[signatures.xml]
//      	[encryption.xml]
//      	[rights.xml]
//    	OEBPS/
//      	Great Expectations.opf
//      	cover.html
//    		chapters/
//         		chapter01.html
//         		chapter02.html
//         		… other HTML files for the remaining chapters …
//
//    mimetype
//
//    The first file in the ZIP Container MUST be a file by the ASCII name of
//    ‘mimetype’ which holds the MIME type for the ZIP Container (i.e.,
//    “application/epub+zip” as a 20 character ASCII string; no padding, CR/LF,
//    white-space or case change). The file MUST NOT be compressed nor encrypted
//    and there MUST NOT be an extra field in its ZIP header.
//
//    container.xml
//
//    The container.xml is a required file with a required name. It must be in the
//    META-INF folder. All other folders are optional and can be any name the user
//    chooses. The container.xml file shows the filename and location of the OPF file.
//
//    OPF
//
//    The Open Packaging Format (OPF) Specification, defines the mechanism by which
//    the various components of an OPS publication are tied together and provides
//    additional structure and semantics to the electronic publication.
//
//    Specifically, OPF:
//    *	Describes and references all components of the electronic publication (e.g.
//    	markup files, images, navigation structures).
//    *	Provides publication-level metadata. Specifically it should include:
//    	dublin core formatted data
//    *	Specifies the linear reading-order of the publication.
//    *	Provides fallback information to use when unsupported extensions to OPS
//    	are employed.
//    *	Provides a mechanism to specify a declarative table of contents (the NCX).
//    *	May provide pointers to additional optional elements such as embedded fonts.
//
//    An example:
//    	<package version="2.0" xmlns="http://www.idpf.org/2007/opf"
//             unique-identifier="BookId">
//    		 <metadata xmlns:dc="http://purl.org/dc/elements/1.1/"
//                    xmlns:opf="http://www.idpf.org/2007/opf">
//    				<dc:title>Alice in Wonderland</dc:title>
//             		<dc:language>en</dc:language>
//             		<dc:identifier id="BookId" opf:scheme="ISBN">
//                		123456789X
//    				</dc:identifier>
//             		<dc:creator opf:role="aut">Lewis Carroll</dc:creator>
//    		 </metadata>
//         	 <manifest>
//             		<item id="intro" href="introduction.html" media-type="application/xhtml+xml" />
//    				<item id="c1" href="chapter-1.html" media-type="application/xhtml+xml" />
//            		<item id="c2" href="chapter-2.html" media-type="application/xhtml+xml" />
//            		<item id="toc" href="contents.xml" media-type="application/xhtml+xml" />
//            		<item id="oview" href="arch.png" media-type="image/png" />
//    		 </manifest>
//         	 <spine toc="ncx">
//             		<itemref idref="intro" />
//            		<itemref idref="toc" />
//            		<itemref idref="c1" />
//            		<itemref idref="c2" />
//            		<itemref idref="oview" linear="no" />
//    		 </spine>
//    	</package>
//
//    Unique ID
//
//    A unique id is a required element in the OPF. Here is one way to accomplish
//    this. Just click on the site and it will give you an id: http://www.famkruithof.net/uuid/uuidgen
//
//
//    OPS
//
//    The Open Publication Structure (OPS) Specification describes a standard for
//    representing the content of electronic publications.
//
//    Specifically:
//    *	The specification is intended to give content providers (e.g. publishers,
//    	authors, and others who have content to be displayed) and publication tool
//     	providers, minimal and common guidelines that ensure fidelity, accuracy,
//     	accessibility, and adequate presentation of electronic content over various
//     	Reading Systems.
//    *	The specification seeks to reflect established content format standards.
//    *	The goal of this specification is to define a standard means of content
//    	description for use by purveyors of electronic books (publishers, agents,
//     	authors et al.) allowing such content to be provided to multiple Reading
//     	Systems and to insure maximum presentational equivalence across Reading
//     	Systems.
//
//    XHTML
//
//    XHTML is predefined XML and as such it should begin with the line:
//    	<?xml version="1.0" encoding="ISO-8859-1"?>
//    where the character set to be used in the book is defined in the encoding entry.
//    The default is Unicode UTF-8. UTF-16 must also be supported but all the glyphs
//    need not be present in the font set.
//
//    A conforming OPS document must support the following XHTML constructions.
//
//    XHTML 1.1 Module Name	Elements (non-normative)	Notes
//    Structure	   	 	   	body, head, html, title		the default rendering for body is consistent
//    													with the CSS property page-break-before having
//     													been set to right (which behaves like always on
//     													one-page Reading Systems), but may be overridden
//     													by an appropriate style sheet declaration.
//    Text	  	 	 	   	abbr, acronym, address,		The optional attribute cite may be used in blockquote,
//    						blockquote, br, cite,		q, del and ins to provide a URI citation for the
//    						code, dfn, div, em, h1,		element contents. Reading Systems are not required
//     						h2, h3, h4, h5, h6, kbd,	to process or use the referenced URI resource, whether
//    						p, pre, q, samp, span,		or not the resource is listed in the Manifest.
//     						strong, var	
//    Hypertext	 			a	  	   					Reading Systems may use or render a URI referenced
//    													physical resource not listed in the Manifest (i.e.,
//     													it is not a component of the Publication), but they
//     													are not required to do so.
//    List	 				dl, dt, dd, ol, ul, li	
//    Object	 				object, param				The object element is the preferred method for generic
//    													object inclusion. When adding objects whose data media
//    													type is not drawn from the OPS Core Media Type list or
//     													which reference an object implementation using the
//     													classid attribute, the object element must specify
//     													fallback information for the object, such as another
//     													object, an img element, or descriptive text.
//    Presentation	 		b, big, hr, i, small,
//    						sub, sup, tt	
//    Edit	 				del, ins
//    Bidirectional Text	 	bdo	
//    Table	 	  			caption, col, colgroup,
//    						table, tbody, td, tfoot,
//     						th, thead, tr	
//    Image		  			img	   	   	  				The inline element img should only be used to refer
//    													to images with OPS Core Media Types of GIF
//     													(http://www.w3.org/Graphics/GIF/spec-gif89a.txt),
//     													PNG (RFC 2083), JPG/JFIF (http://www.w3.org/Graphics/JPEG)
//     													or SVG (http://www.w3.org/TR/SVG11/). The required URI
//     													attribute, src, is used to reference the image resource,
//     													which must be listed in the Manifest.
//    													The required alt attribute should contain a brief and
//     													informative textual description of the image. This text
//     													may be used by Reading Systems as an alternative to, or
//     													in addition to, displaying the image. The text is also an
//     													acceptable fallback for an img with src referencing a
//     													non-OPS Core Media Type for which no viable fallback was
//     													found in the manifest.
//    Client-Side Image Map	 area, map	
//    Meta-Information		 meta	
//    Style Sheet	 	  		 style						The type attribute of the style element is required and
//    													must be given the value of text/css or the deprecated
//     													text/x-oeb1-css.
//    Style Attribute   		 (deprecated)	 			style attribute	
//    Link	 		  		 link						The link element allows for the specification of various
//    													relationships with other documents. Reading Systems must
//     													recognize external style sheet references specified via
//     													the href attribute and the associated rel attribute (for
//     													the values rel="stylesheet" and rel="alternate stylesheet".)
//    Base	 				 base						The root of an ePUB file is the top of the file hierarchy
//    													inside the container.
//
//

//    DRM
//
//    The ePUB standard does not endorse any particular DRM scheme but allows for the
//    creation of DRM. The most popular DRM scheme at this time is the one made and
//    used as part of Adobe Digital Editions. This scheme has been licensed by
//    Overdrive for Library use. Many other publishers also use this scheme. The DRM
//    is applied to individual files within the ePub container. Other DRM systems
//    include an offshoot from Barnes and Noble that is supported by the Adobe DRM
//    server and the Apple FairPlay DRM scheme used on iPad and iPhone devices
//    running iBooks.
//
//    It is also possible to have DRM on embedded fonts that are part of an ePUB
//    formatted file by applying DRM directly to the internal file. In addition there
//    are schemes to obfuscate the embedded fonts file. The standard defines one
//    method but Adobe has defined a slightly different method and it seems to dominate
//    at this point. Obfuscated fonts use an XOR (exclusive or) technique to obscure
//    the fonts in an embedded font set so that it cannot be extracted and used by
//    itself. This is done to meet the copyright requirements imposed by the font
//    designer.
//
//
//    Relationships
//
//    Relationship to NVDL
//
//    This specification uses the NVDL language (see
//    http://standards.iso.org/ittf/PubliclyAvailableStandards/c038615_ISO_IEC_19757-4_2006(E).zip)
//    as a means to unambiguously define the interaction between the various schemas
//    used in this specification. NVDL allows for interaction and validation between
//    various XML schema languages. See Appendix A for a normative NVDL definition
//    of OPS.
//
//    This specification does not require the use of NVDL tools to validate OPS
//    documents, although such tools are available and may be used for validation.
//
//    Relationship to XHTML and DTBook
//
//    This specification recognizes the importance of current software tools, legacy data,
//    publication practices, and market conditions, and has therefore incorporated
//    certain XHTML 1.1 Document Type Modules and DTBook as Preferred Vocabularies.
//    This approach allows content providers to exploit current XHTML and DTBook
//    content, tools, and expertise.
//
//    To minimize the implementation burden on Reading System implementers (who may
//    be working with devices that have power and display constraints), the Preferred
//    Vocabularies do not include all XHTML 1.1 elements and attributes. Further, the
//    modules selected from the XHTML 1.1 specification were chosen to be consistent
//    with current directions in XHTML.
//
//    Any construct deprecated in XHTML 1.1 is either deprecated or omitted from this
//    specification; CSS-based equivalents are provided in most such cases. Style sheet
//    constructs are also used for new presentational functionality beyond that provided
//    in XHTML.
//
//    Relationship to CSS
//
//    This specification defines a style language based on CSS 2 (see
//    http://www.w3.org/TR/CSS2/.) The style sheet MIME type text/x-oeb1-css has been
//    deprecated in favor of text/css. Note that not all of CSS 2 is supported and
//    there are additional extensions which are prefixed with oeb-. (oeb-page-head,
//    oeb-page-foot, oeb-column-number, however, many ePub readers do not support
//    these features anyway.)
//
//    Relationship to XML
//
//    OPS is based on XML because of its generality and simplicity, and because XML
//    documents are likely to adapt well to future technologies and uses. XML also
//    provides well-defined rules for the syntax of documents, which decreases the
//    cost to implementers and reduces incompatibility across systems. Further, XML
//    is extensible: it is not tied to any particular type of document or set of
//    element types, it supports internationalization, and it encourages document
//    markup that can represent a document’s internal parts more directly, making them
//    amenable to automated formatting and other types of computer processing.
//
//    Reading Systems must be XML processors as defined in XML 1.1. All OPS Content
//    Documents must be valid XML documents according to their respective schemas.
//
//    Relationship to XML Namespaces
//
//    Reading Systems must process XML namespaces according to the XML Namespaces
//    Recommendation at http://www.w3.org/TR/xml-names11/. For example:
//    	xmlns:ops="http://www.idpf.org/2007/ops"
//
//    Relationship to Dublin Core
//
//    Dublin Core is the defined standard for all metadata used in the ePub document.
//    Only the id, title, and language are required but other entries are encouraged.
//    Certainly author should be entered if known.



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Ionic.Zip;

namespace MobiEPUB
{
    class EPUBebook : Ebook
    {
        private String opfFilename;                 // zip path of the OPF entry
        private String uniqueId;                    // UniqueID attribute of the OPF
        private XmlNamespaceManager xmlnsManager;

        public EPUBebook(String fn)
            : base()
        {
            _filename = fn;
            if (!ZipFile.IsZipFile(_filename))
                throw new MobiEPUBexception("invalid EPUB file: " + _filename + " (reason 01)");
            setupNameSpaces();
            Open(_filename);
        }

        public EPUBebook()
            : base()
        {
            setupNameSpaces();
        }

        private void setupNameSpaces()
        {
            xmlnsManager = new XmlNamespaceManager(new NameTable());
            xmlnsManager.AddNamespace("def", "urn:oasis:names:tc:opendocument:xmlns:container");
        }

        //=================================================================================================================================
        //      L o a d   a n   E P U B   f i l e
        //=================================================================================================================================
        public override void Open(String fn)
        {
            base.Open(fn);
            if (!ZipFile.IsZipFile(fn))
                throw new MobiEPUBexception("File '" + fn + "' is not a valid EPUB file (reason 01)");
            ZipFile epub = ZipFile.Read(fn);
            LoadEPUB(epub);
        }

        public override void Open(Stream str)
        {
            base.Open(str);
            ZipFile epub = ZipFile.Read(str);
            LoadEPUB(epub);
        }

        private void LoadEPUB(ZipFile zip)
        {
            if (!IsValidEpub(zip))
                throw new MobiEPUBexception("File '" + zip.Name + "' is not a valid EPUB file (reason 02)");
            XmlDocument container = GetContainer(zip);
            opfFilename = GetOPFfilename(container);
            XmlDocument opf = GetOPF(zip, opfFilename);
            String opfVersion = GetOPFversion(opf);
            if (opfVersion != "2.0")
                throw new Exception("Error: OPF unsupported version. Found:" + opfVersion + " expected:2.0");
            LoadOPF(opf);
        }

        // Validate that the EPUB contains a valid mimetype "application/epub+zip"
        private bool IsValidEpub(ZipFile epub)
        {
            ZipEntry ze = epub["mimetype"];
            int len = (int)ze.UncompressedSize + 4;
            byte[] buff = new byte[len];
            MemoryStream st = new MemoryStream(buff, true);
            ze.Extract(st);
            String mimetype = Encoding.UTF8.GetString(buff, 0, (int)ze.UncompressedSize);
            bool result = (mimetype == "application/epub+zip");
            st.Close();
            return result;
        }

        // Extract the container.xml document
        private XmlDocument GetContainer(ZipFile zip)
        {
            XmlDocument result = new XmlDocument();
            ZipEntry ze = zip["META-INF/container.xml"];
            int len = (int)ze.UncompressedSize;
            byte[] buff = new byte[len];
            MemoryStream st = new MemoryStream(buff, true);
            ze.Extract(st);
            st.Position = 0;
            st.Seek(0, SeekOrigin.Begin);
            result.Load(st);
            st.Close();
            return result;
        }

        // Extract the OPF document file name from the container.xml
        //   <?xml version="1.0" encoding="utf-8" standalone="no" ?>
        //   <container version="1.0">
        //      <rootfiles>
        //         <rootfile full-path="OEBPS/content.opf" media-type="application/oebps-package+xml">
        //      </rootfiles>
        //   </container>
        private String GetOPFfilename(XmlDocument container)
        {
            XmlNode node;

            // create ns manager
            XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(container.NameTable);
            xmlnsManager.AddNamespace("def", "urn:oasis:names:tc:opendocument:xmlns:container");

            // Validate version 1.0
            node = container.SelectSingleNode("/def:container", xmlnsManager);
            XmlAttribute version = node.Attributes["version"];
            if (version.Value != "1.0")
                throw new Exception("Error: container.xml unsupported version. Found:" + version.Value + " expected:1.0");

            // Extract the OPF file path.  Check that it has a valid mimetype
            node = container.SelectSingleNode("/def:container/def:rootfiles/def:rootfile", xmlnsManager);
            XmlAttribute path = node.Attributes["full-path"];
            XmlAttribute type = node.Attributes["media-type"];
            if ((type.Value != null) && (type.Value != "application/oebps-package+xml"))
                throw new Exception("Error: container.xml unsupported media-type. Found:'" + type.Value + "' expected:'application/oebps-package+xml'");


            // Check a path exists
            if (path == null)
                throw new Exception("Error: container.xml does not contain the path of the OPF");

            return path.Value;
        }

        // Extract the OPF document
        private XmlDocument GetOPF(ZipFile zip, String opfFile)
        {
            XmlDocument result = new XmlDocument();
            ZipEntry ze = zip[opfFile];
            int len = (int)ze.UncompressedSize;
            byte[] buff = new byte[len];
            MemoryStream st = new MemoryStream(buff, true);
            ze.Extract(st);
            st.Position = 0;
            st.Seek(0, SeekOrigin.Begin);
            result.Load(st);
            st.Close();

            return result;
        }

        // Validate the OPF
        // Check that version is 2.0
        // Check UniqueID exists
        private String GetOPFversion(XmlDocument opf)
        {
            XmlNode node;

            // create ns manager
            XmlNamespaceManager opfnsManager = new XmlNamespaceManager(opf.NameTable);
            opfnsManager.AddNamespace("def", "http://www.idpf.org/2007/opf");
            opfnsManager.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");

            // Validate version 2.0
            node = opf.SelectSingleNode("/def:package", opfnsManager);
            XmlAttribute version = node.Attributes["version"];
            return version.Value;
        }

        // Parse the OPF into various parts
        //   <?xml version="1.0" ?>
        //   <package xmlns="http://www.idpf.org/2007/opf" unique-identifier="BookId" version="2.0">
        //       <metadata xmlns:dc="http://purl.org/dc/elements/1.1/">
        //       ...
        //       </metadata>
        //       <manifest>
        //           <item href="9781118180501f01.xhtml" id="f01" media-type="application/xhtml+xml" />
        //           <item href="9781118180501toc.xhtml" id="toc" media-type="application/xhtml+xml" />
        //           <item href="images/c07uf002.jpg" id="c07uf002" media-type="image/jpeg" />
        //           ...
        //       </manifest>
        //       <spine toc="ncx">
        //           <itemref idref="xcover" linear="yes" />
        //           ...
        //       </spine>
        //       <guide>
        //          <reference href="9781118180501cover.xhtml" title="Cover" type="cover" /> 
        //          ...
        //       </guide>
        //   </package>
        private void LoadOPF(XmlDocument opf)
        {
            // Extract the unique-identifier attribute from <package>
            XmlNamespaceManager opfnsManager = new XmlNamespaceManager(opf.NameTable);
            opfnsManager.AddNamespace("def", "http://www.idpf.org/2007/opf");
            XmlNode package = opf.SelectSingleNode("/def:package", opfnsManager);
            XmlAttribute unique = package.Attributes["unique-identifier"];
            if (unique != null)
                uniqueId = unique.Value;

            // Load the metadata
            LoadMetadata(opf);

            // Load the manifest
            LoadManifest(opf);

            // Load the spine

            // Load the guide
        }


        // Load the metadata and extract title/author etc
        // Use the UniqueID loaded from the <package> to extract the value
        //    	<metadata xmlns:dc="http://purl.org/dc/elements/1.1/"
        //                  xmlns:opf="http://www.idpf.org/2007/opf">
        // 			<dc:title>Alice in Wonderland</dc:title>
        //      	<dc:language>en</dc:language>
        //      	<dc:identifier id="BookId" opf:scheme="ISBN">123456789X</dc:identifier>
        //         	<dc:creator opf:role="aut">Lewis Carroll</dc:creator>
        //    	</metadata>
        private void LoadMetadata(XmlDocument opf)
        {
            XmlNamespaceManager opfnsManager = new XmlNamespaceManager(opf.NameTable);
            opfnsManager.AddNamespace("def", "http://www.idpf.org/2007/opf");
            opfnsManager.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
            opfnsManager.AddNamespace("dcterms", "http://purl.org/dc/terms/");

            XmlNode metadata = opf.SelectSingleNode("/def:package/def:metadata", opfnsManager);
            XmlNodeList terms = metadata.SelectNodes("dc:*", opfnsManager);
            foreach (XmlNode item in terms)
            {
                if (item.Name.Equals("dc:title"))
                    _meta.Title = item.InnerText;
                if (item.Name.Equals("dc:language"))
                    _meta.Language = item.InnerText;
                if (item.Name.Equals("dc:publisher"))
                    _meta.Publisher = item.InnerText;

                XmlAttribute id = item.Attributes["id"];
                if (id != null && id.Value.Equals(uniqueId))
                {
                    _meta.UniqueIdName = uniqueId;
                    _meta.UniqueId = item.InnerText;
                }

            }
        }

        // Load the manifest
        //       <manifest>
        //           <item href="9781118180501f01.xhtml" id="f01" media-type="application/xhtml+xml" />
        //           <item href="9781118180501toc.xhtml" id="toc" media-type="application/xhtml+xml" />
        //           <item href="images/c07uf002.jpg" id="c07uf002" media-type="image/jpeg" />
        //           ...
        //       </manifest>
        private void LoadManifest(XmlDocument opf)
        {
            XmlNamespaceManager opfnsManager = new XmlNamespaceManager(opf.NameTable);
            opfnsManager.AddNamespace("def", "http://www.idpf.org/2007/opf");
            opfnsManager.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");

            XmlNodeList items = opf.SelectNodes("/def:package/def:manifest/def:item", opfnsManager);
            foreach (XmlNode item in items)
            {
                XmlAttribute href = item.Attributes["href"];
                XmlAttribute id = item.Attributes["id"];
                _meta.AddFile(id.Value, href.Value);
            }
        }


        //=================================================================================================================================
        //      B u i l d    a n    E P U B   f i l e
        //
        //      1.  Create the EPUB zip file
        //      2.  Add the internal directories (META-INF and OEBPS)
        //      3.  Create the mimetype file
        //      4.  Create the container.xml control file
        //      5.  Add all the documents to the OEBPS directory
        //      6.  Create the OPF control file
        //=================================================================================================================================

        //-------------------------------------------------------------------------------------------------------------------------
        //   Create the EPUB zip file and load it's various entries
        //-------------------------------------------------------------------------------------------------------------------------
        public override void Save()
        {
            base.Save();
            opfFilename = "/OEBPS/" + OPF_filename(_meta.Title);
            ZipFile tempZip = new ZipFile(_filename);
            tempZip.AddDirectory("/META-INF");
            tempZip.AddDirectory("/OEBPS");
            tempZip.AddEntry("/mimetype", "application/epub+zip");
            tempZip.AddEntry("/META-INF/container.xml", ContainerXml());
            tempZip.AddEntry(opfFilename, OPF());

            if (File.Exists(_filename))
            {
                if (File.Exists(_filename + ".old"))
                    File.Delete(_filename + ".old");
                File.Move(_filename, _filename + ".old");
            }
            tempZip.Save(_filename);
        }

        //-------------------------------------------------------------------------------------------------------------------------
        //   Create the container.xml control file
        //
        //   <?xml version="1.0" encoding="utf-8" standalone="no" ?>
        //   <container version="1.0">
        //      <rootfiles>
        //         <rootfile full-path="OEBPS/content.opf" media-type="application/oebps-package+xml">
        //      </rootfiles>
        //   </container>
        //-------------------------------------------------------------------------------------------------------------------------
        private String ContainerXml()
        {
            // Creat an empty document and add an XMl declaration
            XmlDocument result = new XmlDocument(xmlnsManager.NameTable);
            XmlDeclaration dec = result.CreateXmlDeclaration("1.0", "utf-8", "no");
            result.AppendChild(dec);

            // Create the root element "rootfiles"
            XmlElement rootfiles = result.CreateElement("rootfiles");
            result.AppendChild(rootfiles);

            // Create the inner element "rootfile" and add two attributes "full-path" and "media-type"
            XmlElement rootfile = result.CreateElement("rootfile");
            XmlAttribute opf_path = BuildAttribute(result, "full-path", opfFilename);
            XmlAttribute media_type = BuildAttribute(result, "media-type", "application/oebps-package+xml");
            rootfile.AppendChild(opf_path);
            rootfile.AppendChild(media_type);

            rootfiles.AppendChild(rootfile);
            return result.OuterXml;
        }

        //-------------------------------------------------------------------------------------------------------------------------
        //   Create the OPF control file
        //
        //   <?xml version="1.0" ?>
        //   <package xmlns="http://www.idpf.org/2007/opf" unique-identifier="BookId" version="2.0">
        //       <metadata xmlns:dc="http://purl.org/dc/elements/1.1/">
        //       ...
        //       </metadata>
        //       <manifest>
        //           <item href="9781118180501f01.xhtml" id="f01" media-type="application/xhtml+xml" />
        //           <item href="9781118180501toc.xhtml" id="toc" media-type="application/xhtml+xml" />
        //           <item href="images/c07uf002.jpg" id="c07uf002" media-type="image/jpeg" />
        //           ...
        //       </manifest>
        //       <spine toc="ncx">
        //           <itemref idref="xcover" linear="yes" />
        //           ...
        //       </spine>
        //       <guide>
        //          <reference href="9781118180501cover.xhtml" title="Cover" type="cover" /> 
        //          ...
        //       </guide>
        //   </package>
        //-------------------------------------------------------------------------------------------------------------------------
        private String OPF()
        {
            // Create an empty OPF document with a namespace and add an XML declaration
            XmlNamespaceManager nsManager = new XmlNamespaceManager(new NameTable());
            nsManager.AddNamespace("opf", "http://www.idpf.org/2007/opf");
            nsManager.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
            XmlDocument result = new XmlDocument(nsManager.NameTable);
            XmlDeclaration dec = result.CreateXmlDeclaration("1.0", null, null);
            result.AppendChild(dec);

            // Create document root "package" and add namespace
            XmlElement package = result.CreateElement("package", "http://www.idpf.org/2007/opf");
            XmlAttribute unique_identifier = BuildAttribute(result, "unique-identifier", "BookId");
            XmlAttribute version = BuildAttribute(result, "version", "2.0");
            package.AppendChild(unique_identifier);
            package.AppendChild(version);

            // Create the Dublin Core metadata
            XmlElement metadata = result.CreateElement("dc", "metadata", "http://purl.org/dc/elements/1.1/");
            result.AppendChild(metadata);

            // Create the Manifest
            XmlElement manifest = BuildManifest(result);
            result.AppendChild(manifest);

            // Create the Spine
            XmlElement spine = BuildSpine(result);
            result.AppendChild(spine);

            // Create the Guide
            XmlElement guide = BuildGuide(result);
            result.AppendChild(guide);

            // Return the XML as text
            return result.OuterXml;
        }

        // Generate a filename based on the title with any spaces replaced by '_'
        private String OPF_filename(String title)
        {
            String result = title;
            result.Replace(' ', '_');

            return result;
        }

        // Create the metadata
        //       <metadata xmlns:dc="http://purl.org/dc/elements/1.1/">
        //       ...
        //       </metadata>
        private XmlElement BuildMetadata(XmlDocument doc)
        {
            XmlElement result = doc.CreateElement("dc", "metadata", "http://purl.org/dc/elements/1.1/");

            return result;
        }


        // Create the manifest structure
        //       <manifest>
        //           <item href="9781118180501f01.xhtml" id="f01" media-type="application/xhtml+xml" />
        //           <item href="9781118180501toc.xhtml" id="toc" media-type="application/xhtml+xml" />
        //           <item href="images/c07uf002.jpg" id="c07uf002" media-type="image/jpeg" />
        //           ...
        //       </manifest>
        private XmlElement BuildManifest(XmlDocument doc)
        {
            XmlElement result = doc.CreateElement("manifest");

            return result;
        }

        // Create the spine structure
        //       <spine toc="ncx">
        //           <itemref idref="xcover" linear="yes" />
        //           ...
        //       </spine>
        private XmlElement BuildSpine(XmlDocument doc)
        {
            XmlElement result = doc.CreateElement("spine");
            XmlAttribute toc = BuildAttribute(doc, "toc", "ncx");
            result.AppendChild(toc);

            return result;
        }

        // Create the guide structure
        //       <guide>
        //          <reference href="9781118180501cover.xhtml" title="Cover" type="cover" /> 
        //          ...
        //       </guide>
        private XmlElement BuildGuide(XmlDocument doc)
        {
            XmlElement result = doc.CreateElement("guide");

            return result;
        }
        
        // Create an attribute given a document + name + value
        private XmlAttribute BuildAttribute(XmlDocument doc, String name, String text)
        {
            XmlAttribute result = doc.CreateAttribute(name);
            XmlText attr_text = doc.CreateTextNode(text);
            result.AppendChild(attr_text);

            return result;
        }
    }
}
