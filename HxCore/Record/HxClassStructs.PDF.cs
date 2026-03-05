using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Linq;

namespace HxCore
{
    public struct HxAnnotsRec
    {
        public string Name;

        public string ParentName;

        public string AnnotType;
        public string Intent;

        public string Author;

        public string Title;
        public int Page;
        public string Subject;

        public string CreateDate;
        public string ModifyDate;

        public string Start;
        public string End;
        public string Rect;

        public string Flags;
        public bool? FlagsPrint;
        public bool? FlagsLocked;

        public string Color;

        public string Contents;

        public string XmlData;

        public HxAnnotsRec(System.Xml.Linq.XElement element = null, string authorString = null)
        {
            this.Name = null;

            this.ParentName = null;

            this.AnnotType = null;
            this.Intent = null;
            this.Title = null;

            this.Author = authorString;

            this.Page = int.MinValue;
            this.Subject = null;

            this.CreateDate = null;
            this.ModifyDate = null;

            this.Start = null;
            this.End = null;
            this.Rect = null;

            this.Flags = null;
            this.FlagsPrint = null;
            this.FlagsLocked = null;

            this.Color = null;

            this.XmlData = null;

            //this.
            this.Contents = null;
            if (element != null)
            {
                //SysEnv.Core.DebugConsoleWrite(element.Name.LocalName.ToString());
                this.Name = element.Attribute("name").Value;

                this.ParentName = element.Attribute("inreplyto") != null ? element.Attribute("inreplyto").Value : null;

                this.AnnotType = element.Name.LocalName;
                this.Title = element.Attribute("title") != null ? element.Attribute("title").Value : null;
                this.Author = this.Title;
                if (authorString.IsNullOrWhiteSpaceEx() != true)
                {
                    this.Author = authorString;
                }

                //this.Title = string.Format("[0]{1} {2} {3}", SysCore.LoginID, SysEnv.Core.LoginName, SysEnv.Core.LoginDutyName, SysEnv.Core.LoginDeptName);
                //VpcsCore.SysCore.User.logi

                this.Intent = element.Attribute("intent") != null ? element.Attribute("intent").Value : null;
                this.Subject = element.Attribute("subject") != null ? element.Attribute("subject").Value : null;
                this.Page = element.Attribute("page") != null ? element.Attribute("page").Value.ToIntEx() : int.MinValue;

                this.CreateDate = element.Attribute("creationdate") != null ? element.Attribute("creationdate").Value : null;
                this.ModifyDate = element.Attribute("date") != null ? element.Attribute("date").Value : null;

                this.Start = element.Attribute("start") != null ? element.Attribute("start").ToStringEx() : null;
                this.End = element.Attribute("end") != null ? element.Attribute("end").Value : null;
                this.Rect = element.Attribute("rect") != null ? element.Attribute("rect").Value : null;

                this.Flags = element.Attribute("flags") != null ? element.Attribute("flags").Value : null;
                if (this.Flags.IsNullOrWhiteSpaceEx())
                {
                    this.FlagsPrint = null;
                    this.FlagsLocked = null;
                }
                else
                {
                    this.FlagsPrint = false;
                    this.FlagsLocked = false;
                    string[] strFlags = this.Flags.SplitEx(",");
                    foreach (string sFlageVal in strFlags)
                    {
                        switch (sFlageVal.Trim().ToLower())
                        {
                            case "print":
                                this.FlagsPrint = true;
                                break;
                            case "locked":
                                this.FlagsLocked = true;
                                break;
                        }
                    }
                }

                this.Color = element.Attribute("color") != null ? element.Attribute("color").Value : null; //FFFF00

                string strContents = null;
                //XDocument doc = XDocument.Parse(@"<?xml version=""1.0"" encoding=""UTF-8""?>"+ XmlData);
                var Items = element
                        .Elements(XName.Get("contents-richtext", "http://ns.adobe.com/xfdf/"))
                        .Elements(XName.Get("body", "http://www.w3.org/1999/xhtml"))
                        .Elements(XName.Get("p", "http://www.w3.org/1999/xhtml"))
                        .Elements(XName.Get("span", "http://www.w3.org/1999/xhtml"))
                        //.Select(x => (string)x).ToList()
                        ;
                foreach (var item in Items)
                {
                    strContents += item.Value != null ? item.Value : null;
                }
                this.Contents = strContents;
                this.XmlData = element.ToString(SaveOptions.DisableFormatting);
                //this.XmlData = XElement.Parse(this.XmlData).ToString(SaveOptions.DisableFormatting);
                //this.XmlData = this.XmlData.Replace("\r\n", "<br />");
            }
        }

        //private MarkupAnnotsRec(PDFXEdit.IPXC_Annotation annot)
        //    : this()
        //{
        //    if (annot != null)
        //    {
        //        this.Name = annot.Name;
        //        this.AnnotType = annot.Type.ToStringEx();
        //        //this.Flags = annot.Flags;

        //        this.Contents = annot.Data.Contents;
        //        //this.Color = annot.Data.Color;
        //    }
        //}


    }

    public struct HxPDFPointRec //Text, Sound, FileAttachment
    {
        //참조 : https://help.adobe.com/en_US/acrobat/acrobat_dc_sdk/2015/HTMLHelp/#t=Acro12_MasterBook%2FJS_API_AcroJS%2FAnnotation_properties.htm%23XREF_25272_popupOpen&rhsearch=Annotation%20types&rhsyns=%20
        public decimal X, Y; //Xul, Yul . ul : upper left-hand

    }
    public struct HxPDFRectRec
    {
        public decimal XllLeft, YllBottom, XurRight, YurTop; //ll : lower-left, ur : upper-right // left, bottom, right, top

        public HxPDFRectRec(double left, double bottom, double right, double top) : this()
        {
            Left = left;
            Bottom = bottom;
            Right = right;
            Top = top;
        }

        public HxPDFRectRec(decimal left, decimal bottom, decimal right, decimal top) : this()
        {
            XllLeft = left;
            YllBottom = bottom;
            XurRight = right;
            YurTop = top;
        }

        public double Left
        {
            get => XllLeft.ToDoubleEx();
            set
            {
                XllLeft = value.ToDecimalEx();
            }
        }
        public double Bottom
        {
            get => YllBottom.ToDoubleEx();
            set
            {
                YllBottom = value.ToDecimalEx();
            }
        }
        public double Right
        {
            get => XurRight.ToDoubleEx();
            set
            {
                XurRight = value.ToDecimalEx();
            }
        }
        public double Top
        {
            get => YurTop.ToDoubleEx();
            set
            {
                YurTop = value.ToDecimalEx();
            }
        }
    }
    public struct HxPDFSpanRec
    {
        public string text;
        public Color textColor;
        public decimal textSize;
    }
    public struct HxPDFTextFontRec
    {
        public string fontFamily;
        public string fontStyle;
        public decimal fontWeight;
    }
    public struct HxPDFTextStyleRec
    {
        public string alignment;
        public HxPDFTextFontRec textFont;
        public Color textColor;
        public decimal textSize;
    }


    //Text
    //FreeText
    //Line
    //Square
    //Circle
    //Polygon
    //PolyLine
    //Highlight
    //Underline
    //Squiggly
    //StrikeOut
    //Stamp
    //Caret
    //Ink
    //FileAttachment
    //Sound
    public class HxPDFAnnotationAll
    {
        //참조 : https://help.adobe.com/en_US/acrobat/acrobat_dc_sdk/2015/HTMLHelp/#t=Acro12_MasterBook%2FJS_API_AcroJS%2FAnnotation_types1.htm&rhsearch=Annotation%20types&rhhlterm=Annotation%20types&rhsyns=%20

        public string author { get; set; }
        public decimal borderEffectIntensity { get; set; }
        public string borderEffectStyle { get; set; }

        public string contents;
        public DateTime creationDate { get; protected set; }
        public bool delay { get; set; }
        public bool hidden { get; set; }
        public string inReplyTo { get; set; }
        public string intent { get; set; }
        public bool locked { get; set; } //lock
        public DateTime modDate { get; set; }
        public string name { get; set; }
        public bool noView { get; set; }
        public decimal opacity { get; set; }
        public int page { get; set; }
        public bool popupOpen { get; set; } //All except FreeText, Sound, FileAttachment
        public HxPDFRectRec popupRect { get; set; }
        public bool print { get; set; }
        public bool readOnly { get; set; }
        public HxPDFRectRec rect { get; set; }
        public string refType { get; set; }
        public List<HxPDFSpanRec> richContents { get; set; } //All except Sound, FileAttachment
        public int seqNum { get; protected set; }
        public Color strokeColor { get; set; }
        public string style { get; set; }
        public string subject { get; set; }
        public bool toggleNoView { get; set; }
        public string type { get; protected set; }


        //, borderEffectIntensity, borderEffectStyle, 
        //caretSymbol, contents, creationDate, delay, hidden, inReplyTo, 
        //intent, lock, modDate, name, noView, opacity, page, popupOpen, 
        //popupRect, print, readOnly, rect, refType, richContents, rotate, 
        //seqNum, strokeColor, style, subject, toggleNoView, type, width
    }

    public class HxPDFAnnotationText : HxPDFAnnotationAll
    {
        public HxPDFNoteIconStringType noteIcon { get; set; }
        public HxPDFPointRec point { get; set; }
        public string state { get; set; } //Text
        public string stateModel { get; set; } //Text
    }

    public class HxPDFAnnotationFreeText : HxPDFAnnotationAll
    {
        ////0 : Left aligned, 1: Centered, 2 : Right aligned, https://help.adobe.com/en_US/acrobat/acrobat_dc_sdk/2015/HTMLHelp/index.html#t=Acro12_MasterBook%2FJS_API_AcroJS%2FAnnotation_properties.htm%23XREF_99809_alignment
        public HxPDFAlignmentNumberType alignment { get; set; } //FreeText, Redact
        public List<decimal> callout { get; set; }
        public HxPDFPointRec dash { get; set; } //FreeText, Line, PolyLine, Polygon, Circle, Square, Ink
        public string fillColor { get; set; }//Circle, Square, Line, Polygon, PolyLine, FreeText
        public HxPDFLineEndingStringType lineEnding { get; set; }

        public List<HxPDFSpanRec> richDefaults { get; set; }

        public int rotate { get; set; } //All, FreeText

        public string textFont { get; set; } //FreeText
        public string textSize { get; set; } //FreeText

        public decimal width { get; set; } //Square, Circle, Line, Ink, FreeText
    }

    public class HxPDFAnnotationLine : HxPDFAnnotationAll
    {
        public HxPDFLineEndingStringType arrowBegin { get; set; } //Line, PolyLine
        public HxPDFLineEndingStringType arrowEnd { get; set; } //Line, PolyLine
        public bool doCaption { get; set; } //Line
        public string fillColor { get; set; }//Circle, Square, Line, Polygon, PolyLine, FreeText
        public decimal leaderExtend { get; set; } //Line
        public decimal leaderLength { get; set; } //Line

        public List<HxPDFPointRec> points { get; set; } //Line
        public decimal width { get; set; } //Square, Circle, Line, Ink, FreeText
    }
    
    

    public class HxPDFAnnotationSquare : HxPDFAnnotationAll
    {
        public HxPDFPointRec dash { get; set; } //FreeText, Line, PolyLine, Polygon, Circle, Square, Ink
        public string fillColor { get; set; }//Circle, Square, Line, Polygon, PolyLine, FreeText
        public int rotate { get; set; } //All, FreeText
        public decimal width { get; set; } //Square, Circle, Line, Ink, FreeText
    }
    public class HxPDFAnnotationCircle : HxPDFAnnotationSquare
    {
        //public string fillColor { get; set; }
    }
    /*
    public class HxPDFAnnotationCircle : HxPDFAnnotationAll
    {
        public HxPDFPointRec dash { get; set; } //FreeText, Line, PolyLine, Polygon, Circle, Square, Ink
        public string fillColor { get; set; }//Circle, Square, Line, Polygon, PolyLine, FreeText
        public int rotate { get; set; } //FreeText, Square, Circle
        public decimal width { get; set; } //Square, Circle, Line, Ink, FreeText
    }*/
    public class HxPDFAnnotationPolygon : HxPDFAnnotationSquare
    {
        public string vertices { get; set; } //Polygon, PolyLine
    }
    /*
    public class HxPDFAnnotationPolygon : HxPDFAnnotationAll
    {
        public HxPDFPointRec dash { get; set; } //FreeText, Line, PolyLine, Polygon, Circle, Square, Ink
        public string fillColor { get; set; }//Circle, Square, Line, Polygon, PolyLine, FreeText
        public int rotate { get; set; } //FreeText, Square, Circle, Polygon
        public string vertices { get; set; } //Polygon, PolyLine
        public decimal width { get; set; } //Square, Circle, Line, Ink, FreeText, Polygon
    }
    */

    public class HxPDFAnnotationPolyLine : HxPDFAnnotationPolygon
    {
        public HxPDFLineEndingStringType arrowBegin { get; set; } //Line, PolyLine
        public HxPDFLineEndingStringType arrowEnd { get; set; } //Line, PolyLine
        //public HxPDFPointRec dash { get; set; } //FreeText, Line, PolyLine, Polygon, Circle, Square, Ink
        //public decimal width { get; set; } //Square, Circle, Line, Ink, FreeText
    }
    

    public class HxPDFAnnotationHighlight : HxPDFAnnotationAll
    {
        public string quads { get; set; } //Highlight, StrikeOut, Underline, Squiggly,Redact
        public int rotate { get; set; } //All, FreeText
        public decimal width { get; set; } //Highlight, Square, Circle, Line, Ink, FreeText
    }

    public class HxPDFAnnotationUnderline : HxPDFAnnotationHighlight
    {
        
    }

    public class HxPDFAnnotationSquiggly : HxPDFAnnotationHighlight
    {

    }
    public class HxPDFAnnotationStrikeOut : HxPDFAnnotationHighlight
    {

    }

    public class HxPDFAnnotationStamp : HxPDFAnnotationAll
    {
        public string AP { get; set; } //Stamp
        public int rotate { get; set; } //FreeText
    }
    public class HxPDFAnnotationCaret : HxPDFAnnotationFreeText
    {
        public string caretSymbol { get; set; }
    }
    public class HxPDFAnnotationInk : HxPDFAnnotationAll
    {
        public decimal width { get; set; } //Ink, Highlight, Square, Circle, Line, Ink, FreeText
    }
    public class HxPDFAnnotationFileAttachment : HxPDFAnnotationAll
    {
        public string attachIcon { get; set; } //FileAttachment

        public object attachment { get; set; } //FileAttachment //https://help.adobe.com/en_US/acrobat/acrobat_dc_sdk/2015/HTMLHelp/index.html#t=Acro12_MasterBook%2FJS_API_AcroJS%2FAnnotation_properties.htm%23XREF_75359_attachment
        public string cAttachmentPath { protected get; set; }
        protected new List<HxPDFSpanRec> richContents { get; private set; } //All except Sound, FileAttachment

        public int rotate { get; set; } //FreeText
        public decimal width { get; set; } //FileAttachment, Ink, Highlight, Square, Circle, Line, Ink, FreeText
    }
    public class HxPDFAnnotationSound : HxPDFAnnotationAll
    {
        public int rotate { get; set; } //All
        protected new List<HxPDFSpanRec> richContents { get; private set; } //All except Sound, FileAttachment
        public string soundIcon { get; set; } //Sound
        public decimal width { get; set; } //Sound, FileAttachment, Ink, Highlight, Square, Circle, Line, Ink, FreeText
    }


    #region Append Annots
    public struct HxPDFAnnotsMarkupSquare
    {
        //const uint AnnotType
        const string _DefaultSubjectENG_ = "Square";
        const string _DefaultSubjectKOR_ = "사각형";

        public uint PageIndex; //1

        public double Left;
        public double Bottom;
        public double Right;
        public double Top;
        public HxPDFRectRec Rect
        {
            get
            {
                HxPDFRectRec Result = new HxPDFRectRec
                {
                    XllLeft = Left.ToDecimalEx(),
                    YllBottom = Bottom.ToDecimalEx(),
                    XurRight = Right.ToDecimalEx(),
                    YurTop = Top.ToDecimalEx()
                };
                return Result;
            }
            set
            {
                Left = value.XllLeft.ToDoubleEx();
                Bottom = value.YllBottom.ToDoubleEx();
                Right = value.XurRight.ToDoubleEx();
                Top = value.YurTop.ToDoubleEx();
            }
        }

        public string Title; //author;// Environment.UserName;
        public string Subject; //사각형
        public double Rotation; //0.0
        public string DateString; //creationdate = "D:20240610094458+09'00'"

        public Color FillColor;
        public double FillOpacity;
        public Color BoardColor;
        public float BoardWidth;
        public bool IsBorderUse
        {
            get
            {
                if (this.BoardWidth <= 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            set
            {
                if (value == true && BoardWidth <= 0)
                {
                    BoardWidth = 1.0f;
                }
                else if (value == false)
                {
                    BoardWidth = 0.0f;
                }
            }
        }


        public HxPDFAnnotsMarkupSquare(uint pageIndex, double left, double bottom, double right, double top, string title, string subject, double rotation, string dateString, Color backgroundColor, double backgroundOpacity, Color boardColor, double boardWidth)
        {
            PageIndex = pageIndex;
            Left = left;
            Bottom = bottom;
            Right = right;
            Top = top;
            Title = title;
            Subject = subject;
            Rotation = rotation;
            DateString = dateString;
            FillColor = backgroundColor;
            FillOpacity = backgroundOpacity;
            BoardColor = boardColor;
            BoardWidth = boardWidth.ToFloatEx();
        }

        public HxPDFAnnotsMarkupSquare(bool bInit = true)
        {
            PageIndex = 0;
            Left = 0.0;
            Bottom = 0.0;
            Right = 0.0;
            Top = 0.0;
            Title = string.Empty;
            Subject = string.Empty;
            Rotation = 0.0;
            DateString = string.Empty;
            FillColor = Color.Empty;
            FillOpacity = 1.0;
            BoardColor = Color.Empty;
            BoardWidth = 1.0f;

            if (bInit == true)
            {
                Title = _DefaultSubjectENG_;
                Subject = Environment.UserName;
                //TimeSpan difference = (DateTimeOffset.Now - DateTimeOffset.UtcNow);
                //DateString = $"D:{DateTime.Now.ToStringEx("yyyyMMddHHmmss")}{(difference.Hours >= 0 ? "+" : "-")}{difference.Hours}'{difference.Minutes}'";
            }

        }
    }

    public struct HxPDFAnnotsMarkupFreeText
    {
        //const uint AnnotType
        const string _DefaultSubjectENG_ = "FreeText";
        const string _DefaultSubjectKOR_ = "FreeText";

        public uint PageIndex; //1

        public double Left;
        public double Bottom;
        public double Right;
        public double Top;
        public HxPDFRectRec Rect
        {
            get
            {
                HxPDFRectRec Result = new HxPDFRectRec
                {
                    XllLeft = Left.ToDecimalEx(),
                    YllBottom = Bottom.ToDecimalEx(),
                    XurRight = Right.ToDecimalEx(),
                    YurTop = Top.ToDecimalEx()
                };
                return Result;
            }
            set
            {
                Left = value.XllLeft.ToDoubleEx();
                Bottom = value.YllBottom.ToDoubleEx();
                Right = value.XurRight.ToDoubleEx();
                Top = value.YurTop.ToDoubleEx();
            }
        }

        public string Title; //author;// Environment.UserName;
        public string Subject; //사각형
        public double Rotation; //0.0
        public string DateString; //creationdate = "D:20240610094458+09'00'"

        public Color FillColor;
        public double FillOpacity;
        public Color BoardColor;
        public float BoardWidth;
        public bool IsBorderUse
        {
            get
            {
                if (this.BoardWidth <= 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            set
            {
                if (value == true && BoardWidth <= 0)
                {
                    BoardWidth = 1.0f;
                }
                else if (value == false)
                {
                    BoardWidth = 0.0f;
                }
            }
        }

        public string Contents;

        public Color TextColor;
        //public Font TextFont;
        public int TextAlign;
        public string TextFontName;
        public double TextFontSize;

        public HxPDFAnnotsMarkupFreeText(uint pageIndex, double left, double bottom, double right, double top, string title, string subject, double rotation, string dateString, Color backgroundColor, double backgroundOpacity, Color boardColor, double boardWidth, string contents, Color textColor, string textFontName, double textFontSize, int textAlign)
        {
            PageIndex = pageIndex;
            Left = left;
            Bottom = bottom;
            Right = right;
            Top = top;
            Title = title;
            Subject = subject;
            Rotation = rotation;
            DateString = dateString;
            FillColor = backgroundColor;
            FillOpacity = backgroundOpacity;
            BoardColor = boardColor;
            BoardWidth = boardWidth.ToFloatEx();
            Contents = contents;
            TextColor = textColor;
            //TextFont = textFont;
            TextFontName = textFontName;
            TextFontSize = textFontSize;
            TextAlign = textAlign;
        }

        public HxPDFAnnotsMarkupFreeText(bool bInit = true)
        {
            PageIndex = 0;
            Left = 0.0;
            Bottom = 0.0;
            Right = 0.0;
            Top = 0.0;
            Title = string.Empty;
            Subject = string.Empty;
            Rotation = 0.0;
            DateString = string.Empty;
            FillColor = Color.Empty;
            FillOpacity = 1.0;
            BoardColor = Color.Empty;
            BoardWidth = 1.0f;
            Contents = string.Empty;
            TextColor = Color.Black;
            //TextFont = new Font("Arial", 11, FontStyle.Regular);
            TextFontName = "Arial";
            TextFontSize = 11;
            TextAlign = -1;
            /*
            public enum PXC_TextJustification
            {
                TJ_Default = -1,
                TJ_Left = 0,
                TJ_Top = 0,
                TJ_Center = 1,
                TJ_Middle = 1,
                TJ_Right = 2,
                TJ_Bottom = 2,
                TJ_Justify = 3,
                TJ_JustifyAll = 4,
                TJ_Radix = 5
            }
             * */
            if (bInit == true)
            {
                Title = _DefaultSubjectENG_;
                Subject = Environment.UserName;
                //TimeSpan difference = (DateTimeOffset.Now - DateTimeOffset.UtcNow);
                //DateString = $"D:{DateTime.Now.ToStringEx("yyyyMMddHHmmss")}{(difference.Hours >= 0 ? "+" : "-")}{difference.Hours}'{difference.Minutes}'";
            }

        }
    }
    #endregion
}
