using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace HxCore.Data
{
    public partial class HxExcelTpl_temp : HxExcel
    {
        public HxTemplateType TemplateType
        {
            get;
            protected set;
        }

        public HxExcelTpl_temp(string sourceFileName, string targetFileName = null, bool isOverWrite = false)
            : base(sourceFileName, targetFileName, isOverWrite)
        {
            this.Init();
        }

        public HxExcelTpl_temp(Stream loadStream, string saveFileName = null, bool isOverWrite = false) 
            : base(loadStream, saveFileName, isOverWrite)
        {
            this.Init();
        }

        private void Init(bool bInit = false)
        {
            if(IsCreated != true || bInit == true)
            {
                TemplateType = HxTemplateType.None;

                InitVars();

                IsCreated = true;
            }
        }
    }

    /// <summary>
    /// 양식(Tempate) 종류
    /// </summary>
    public enum HxTemplateType
    {
        None = 0,
        /// <summary>
        /// List 형식(Index)
        /// </summary>
        Index = 1,
        /// <summary>
        /// Form 형식(Datasheet)
        /// </summary>
        Datasheet = 2,
        /// <summary>
        /// Report 형식(Print)
        /// </summary>
        Report = 3
    }

    //public struct HxTagTemplateRec
    //{

    //    public string TagPattern;
    //    public string InputText;

    //    public string Value;
    //    public string VarCase;
    //    public string VarName;
    //    public string VarOption;
    //    //public string StartDefine;
    //    //public string EndDefine;
    //    public string ReplaceText;

    //    public int OptionRow;
    //    public int OptionColumn;


    //    public HxTagTemplateRec(string inputText, string tagPattern = HxTagTpl._TAG_PATTERN_)
    //    {
    //        this.InputText = inputText;
    //        this.TagPattern = tagPattern;
    //        this.Value = inputText;
    //        this.VarCase = null;
    //        this.VarName = null;
    //        this.VarOption = null;

    //        this.ReplaceText = inputText;

    //        this.OptionRow = int.MinValue;
    //        this.OptionColumn = int.MinValue;

    //        if (!this.InputText.IsNullOrWhiteSpaceEx() && !this.TagPattern.IsNullOrWhiteSpaceEx())
    //        {
    //            Regex regx = new Regex(this.TagPattern, RegexOptions.IgnoreCase);
    //            Match match = regx.Match(inputText);

    //            //foreach(match)
    //            if (match.Success)
    //            {
    //                this.Value = match.Value;
    //                this.VarCase = match.Groups[1].Value;
    //                this.VarName = match.Groups[2].Value;
    //                this.VarOption = match.Groups[3].Value;
    //                //this.EndDefine = match.Groups[4].Value;
    //                ReplaceText = ReplaceText.Replace(this.Value, string.Empty);
    //            }
    //        }
    //    }

    //    public HxTagTemplateRec(string inputText, int row, int col, string tagPattern = HxTagTpl._TAG_PATTERN_)
    //    {
    //        this.InputText = inputText;
    //        this.TagPattern = tagPattern;
    //        this.Value = inputText;
    //        this.VarCase = null;
    //        this.VarName = null;
    //        this.VarOption = null;

    //        this.ReplaceText = inputText;

    //        this.OptionRow = row;
    //        this.OptionColumn = col;

    //        if (!this.InputText.IsNullOrWhiteSpaceEx() && !this.TagPattern.IsNullOrWhiteSpaceEx())
    //        {
    //            Regex regx = new Regex(this.TagPattern, RegexOptions.IgnoreCase);
    //            Match match = regx.Match(inputText);

    //            //foreach(match)
    //            if (match.Success)
    //            {
    //                this.Value = match.Value;
    //                this.VarCase = match.Groups[1].Value;
    //                this.VarName = match.Groups[2].Value;
    //                this.VarOption = match.Groups[3].Value;
    //                //this.EndDefine = match.Groups[4].Value;
    //                ReplaceText = ReplaceText.Replace(this.Value, string.Empty);
    //            }
    //        }
    //    }
    //}

    public enum HxValueAssignRowspanType
    {
        None
        , Tag
        , Value
    }

    public struct HxTempateWorkseetRec
    {
        public string WorksheetName;
        public int StartRow;
        public int EndRow;
        public int StartColumn;
        public int EndColumn;
    }
    public enum HxTemplateBlockType
    {
        None
        , PageRange
        , PageHead
        , PageFoot
        , ItemVar
        , ItemLoop
    }

}
