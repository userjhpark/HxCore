using System;
using System.Collections.Generic;
using System.Text;

namespace HxCore
{
    public enum HxPDFAlignmentNumberType
    {
        Left_aligned = 0
        , Centered = 1
        , Right_aligned = 2
    }

    public enum HxPDFNoteIconStringType
    {
        //참조 : https://help.adobe.com/en_US/acrobat/acrobat_dc_sdk/2015/HTMLHelp/#t=Acro12_MasterBook%2FJS_API_AcroJS%2FAnnotation_properties.htm%23XREF_14440_noteIcon&rhsearch=Annotation%20types&rhsyns=%20
        Note //(default)
        , Check
        , Circle
        , Comment
        , Cross
        , Help
        , Insert
        , Key
        , NewParagraph
        , Paragraph
        , RightArrow
        , RightPointer
        , Star
        , UpArrow
        , UpLeftArrow
    }

    public enum HxPDFLineEndingStringType
    {
        None// (default)
        , OpenArrow
        , ClosedArrow
        , ROpenArrow        // Acrobat 6.0
        , RClosedArrow      // Acrobat 6.0
        , Butt              // Acrobat 6.0
        , Diamond
        , Circle
        , Square
        , Slash             // Acrobat 7.0
    }
    public enum HxPdfFormDataFormatType
    {
        None = 0,
        //
        // 요약:
        //     Data is represented as FDF (Forms Data Format).
        Fdf = 1,
        //
        // 요약:
        //     Data is represented as XML.
        Xml,
        //
        // 요약:
        //     Data is represented as XFDF (XML Forms Data Format).
        Xfdf,
        //
        // 요약:
        //     Data is represented as text.
        Txt
    }
    public enum HxPdfCompatibilityType
    {
        None = 0,
        //
        // 요약:
        //     The document supports the ISO 32000-1:2008 standard.
        Pdf = 1,
        //
        // 요약:
        //     The document supports the ISO 19005-1:2005 standard.
        PdfA1b, //GTS_PDFA1
        //
        // 요약:
        //     The document supports the ISO 19005-2:2011 standard.
        PdfA2b,
        //
        // 요약:
        //     The document supports the ISO 19005-3:2012 standard.
        PdfA3b
    }
}
