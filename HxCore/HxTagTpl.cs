using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HxCore
{
    public class HxTagTpl : HxBase
    {
        //public const string _TAG_FORMAT_PATTERN_ = @"(?:{)(#|\$|@|\/)({0})+(?:\:([\w-~,]+))?( ?\/)?(?:})";
        //private const string _TAG_VARNAME_PATTERN_ = @"[\w.]+";
        //private static string FTagPattern = @"(?:{)(#|\$|@|\/)([\w.]+)+(?:\:([\w-~,]+))?( ?\/)?(?:})";
        public const string _DEF_TAG_ALL_PATTERN_ = @"(?:{{)(#|\$|@|\/)([\w.]+)+(?:\:([\w\-~,]+))?( ?\/)?(?:}})";
        public const string _DEF_TAG_VAR_PATTERN_ = @"(?:{{)(\$)([\w.]+)+(?:\:([\w\-~,]+))?( ?\/)?(?:}})";

        private static string _tag_all_pattern = _DEF_TAG_ALL_PATTERN_;
        public static string TAG_ALL_PATTERN => _tag_all_pattern;

        private static string _tag_var_pattern_ = _DEF_TAG_ALL_PATTERN_;
        public static string TAG_VAR_PATTERN => _tag_var_pattern_;

        private HxTagTpl(string tagPattern = HxTagTpl._DEF_TAG_ALL_PATTERN_)
            : base()
        {
            SetTagPattern(tagPattern);
        }

        private static void SetTagPattern(string tagPattern)
        {
            //FTagPattern = tagPattern;
            //if (FTagPattern.IsNullOrWhiteSpaceEx())
            //{
            //    FTagPattern = HxTagTpl._TAG_PATTERN_;
            //}
        }

        public static HxTagTplRec GetTagMatch(string inputText, string tagPattern = null)
        {
            if (tagPattern.IsNullOrWhiteSpaceEx())
            {
                tagPattern = TAG_ALL_PATTERN;
            }
            return new HxTagTplRec(inputText, tagPattern);
        }

        public static HxTagTplRec[] GetTagMatches(string inputText, string tagPattern = null)
        {
            HxTagTplRec[] Result = null;
            if (tagPattern.IsNullOrWhiteSpaceEx())
            {
                tagPattern = TAG_ALL_PATTERN;
            }
            MatchCollection matches = Regex.Matches(inputText, tagPattern);
            if (matches.Count > 0)
            {
                Result = new HxTagTplRec[matches.Count];
                int i = 0;
                foreach (Match match in matches)
                {
                    Result[i] = new HxTagTplRec(match.Value, tagPattern);
                    i++;
                }

            }
            return Result;
        }

        public static HxTagTplRec[] GetTagMatches(string inputText, int row, int col, string tagPattern = null)
        {
            HxTagTplRec[] Result = null;
            if (tagPattern.IsNullOrWhiteSpaceEx())
            {
                tagPattern = TAG_ALL_PATTERN;
            }
            MatchCollection matches = Regex.Matches(inputText, tagPattern);
            if (matches.Count > 0)
            {
                Result = new HxTagTplRec[matches.Count];
                int i = 0;
                foreach (Match match in matches)
                {
                    Result[i] = new HxTagTplRec(match.Value, row, col, tagPattern);
                    i++;
                }
            }
            return Result;
        }

        public static List<HxTagTplRec> GetTagMatchList(string inputText, string tagPattern = null)
        {
            List<HxTagTplRec> Result = null;
            HxTagTplRec[] matches = GetTagMatches(inputText, tagPattern);
            if (matches.Length > 0)
            {
                foreach (HxTagTplRec tagInfo in matches)
                {
                    Result.AddEx(tagInfo, true);
                }
                //Result.AddEx()
            }
            return Result;
        }

        public static HxTagTplRec GetDefineRecord(List<HxTagTplRec> list, string defineName)
        {
            HxTagTplRec Result = new HxTagTplRec();
            HxTagTplRec[] find = list.Where(r => r.VarCase.Equals("#") && r.VarName.ToLower().Equals(defineName.ToLower())).ToArray();
            if (find.Length >= 1)
            {
                Result = find[0];
            }
            return Result;
        }

        public static string GetDefineOption(List<HxTagTplRec> list, string defineName, string defaultValue = null)
        {
            string Result = defaultValue;
            if (!defineName.IsNullOrWhiteSpaceEx())
            {
                HxTagTplRec record = GetDefineRecord(list, defineName);
                if (!record.VarName.IsNullOrWhiteSpaceEx())
                {
                    Result = record.VarOption;
                }
            }
            return Result;
        }

        public static int GetDefineOptionStartValue(List<HxTagTplRec> list, string defineName, int defaultValue = int.MinValue)
        {
            int Result = defaultValue;
            if (!defineName.IsNullOrWhiteSpaceEx())
            {
                string strOption = GetDefineOption(list, defineName);
                if (!strOption.IsNullOrWhiteSpaceEx())
                {
                    string[] substr = Regex.Split(strOption, @"(-|,|_|~)");
                    if (substr.Length > 0)
                    {
                        Result = substr[0].ToIntEx();
                        if (Result == int.MinValue)
                        {
                            Result = defaultValue;
                        }
                    }
                }
            }
            return Result;
        }

        public static int GetDefineOptionEndValue(List<HxTagTplRec> list, string defineName, int defaultValue = int.MinValue)
        {
            int Result = defaultValue;
            if (!defineName.IsNullOrWhiteSpaceEx())
            {
                string strOption = GetDefineOption(list, defineName);
                if (!strOption.IsNullOrWhiteSpaceEx())
                {
                    string[] substr = Regex.Split(strOption, @"(-|,|_|~)");
                    if (substr.Length > 0)
                    {
                        Result = substr[(substr.Length - 1)].ToIntEx();
                        if (Result == int.MinValue)
                        {
                            Result = defaultValue;
                        }
                    }
                }
            }
            return Result;
        }
    }

    public struct HxTagTplRec
    {

        public string TagPattern;
        public string InputText;

        public string Value;
        public string VarCase;
        public string VarName;
        public string VarOption;
        //public string StartDefine;
        //public string EndDefine;
        public string ReplaceText;

        public int OptionRow;
        public int OptionColumn;


        public HxTagTplRec(string inputText, string tagPattern = HxTagTpl._DEF_TAG_ALL_PATTERN_)
        {
            this.InputText = inputText;
            this.TagPattern = tagPattern;
            this.Value = inputText;
            this.VarCase = null;
            this.VarName = null;
            this.VarOption = null;

            this.ReplaceText = inputText;

            this.OptionRow = int.MinValue;
            this.OptionColumn = int.MinValue;

            if (!this.InputText.IsNullOrWhiteSpaceEx() && !this.TagPattern.IsNullOrWhiteSpaceEx())
            {
                Regex regx = new Regex(this.TagPattern, RegexOptions.IgnoreCase);
                Match match = regx.Match(inputText);

                //foreach(match)
                if (match.Success)
                {
                    this.Value = match.Value;
                    this.VarCase = match.Groups[1].Value;
                    this.VarName = match.Groups[2].Value;
                    this.VarOption = match.Groups[3].Value;
                    //this.EndDefine = match.Groups[4].Value;
                    ReplaceText = ReplaceText.Replace(this.Value, string.Empty);
                }
            }
        }

        public HxTagTplRec(string inputText, int row, int col, string tagPattern = HxTagTpl._DEF_TAG_ALL_PATTERN_)
        {
            this.InputText = inputText;
            this.TagPattern = tagPattern;
            this.Value = inputText;
            this.VarCase = null;
            this.VarName = null;
            this.VarOption = null;

            this.ReplaceText = inputText;

            this.OptionRow = row;
            this.OptionColumn = col;

            if (!this.InputText.IsNullOrWhiteSpaceEx() && !this.TagPattern.IsNullOrWhiteSpaceEx())
            {
                Regex regx = new Regex(this.TagPattern, RegexOptions.IgnoreCase);
                Match match = regx.Match(inputText);

                //foreach(match)
                if (match.Success)
                {
                    this.Value = match.Value;
                    this.VarCase = match.Groups[1].Value;
                    this.VarName = match.Groups[2].Value;
                    this.VarOption = match.Groups[3].Value;
                    //this.EndDefine = match.Groups[4].Value;
                    ReplaceText = ReplaceText.Replace(this.Value, string.Empty);
                }
            }
        }
    }
}
