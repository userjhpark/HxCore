using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace HxCore.Data
{
    public class HxExcelTplBlock : HxSetValueObject, IHxSetValue
    {
        public static new string _CUSTOM_USER_AGENT_ => HxUtils.GetOSCustomUserAgent(); //$"[{HxUtils.UserGlobalAddress()}]HOST:{HxUtils.UserHostName()}/OS_USER:{HxUtils.OSCustomUserAgent}";

        #region Column & Fields
        public const string _CDF_NO_                             = HxExcelTpl._UDEF_NO_                  ;
        public const string _CDF_WORKSHEET_NAME_                 = HxExcelTpl._UDEF_WORKSHEET_NAME_      ;
        public const string _CDF_BLOCK_NAME_                     = HxExcelTpl._UDEF_BLOCK_NAME_          ;
        public const string _CDF_BLOCK_TYPE_                     = HxExcelTpl._UDEF_BLOCK_TYPE_          ;
        public const string _CDF_START_ROW_                      = HxExcelTpl._UDEF_START_ROW_           ;
        public const string _CDF_START_COLUMN_                   = HxExcelTpl._UDEF_START_COLUMN_        ;
        public const string _CDF_END_ROW_                        = HxExcelTpl._UDEF_END_ROW_             ;
        public const string _CDF_END_COLUMN_                     = HxExcelTpl._UDEF_END_COLUMN_          ;
        public const string _CDF_START_ADDR_                     = HxExcelTpl._UDEF_START_ADDR_          ;
        public const string _CDF_END_ADDR_                       = HxExcelTpl._UDEF_END_ADDR_            ;
        public const string _CDF_TAG_INPUT_                      = HxExcelTpl._UDEF_TAG_INPUT_           ;
        public const string _CDF_TAG_PATTERN_                    = HxExcelTpl._UDEF_TAG_PATTERN_         ;
        public const string _CDF_TAG_VALUE_                      = HxExcelTpl._UDEF_TAG_VALUE_           ;
        public const string _CDF_TAG_CASE_                       = HxExcelTpl._UDEF_TAG_CASE_            ;
        public const string _CDF_TAG_NAME_                       = HxExcelTpl._UDEF_TAG_NAME_            ;
        public const string _CDF_TAG_OUTPUT_                     = HxExcelTpl._UDEF_TAG_OUTPUT_          ;
        public const string _CDF_TAG_OPTION_VALUE_               = HxExcelTpl._UDEF_TAG_OPTION_VALUE_    ;
        public const string _CDF_TAG_OPTION_ROW_                 = HxExcelTpl._UDEF_TAG_OPTION_ROW_      ;
        public const string _CDF_TAG_OPTION_COL_                 = HxExcelTpl._UDEF_TAG_OPTION_COL_      ;
        public const string _CDF_START_DEL_ROW_                  = HxExcelTpl._UDEF_START_DEL_ROW_       ;
        public const string _CDF_END_DEL_ROW_                    = HxExcelTpl._UDEF_END_DEL_ROW_         ;
        public const string _CDF_PAGE_TOTAL_COUNT_               = HxExcelTpl._UDEF_PAGE_TOTAL_COUNT_    ;
        public const string _CDF_PAGE_ITEM_COUNT_                = HxExcelTpl._UDEF_PAGE_ITEM_COUNT_     ;
        public const string _CDF_PAGE_BLANK_COUNT_               = HxExcelTpl._UDEF_PAGE_BLANK_COUNT_    ;
        public const string _CDF_REMARK_                         = HxExcelTpl._UDEF_REMARK_              ;

        [JsonProperty(_CDF_NO_              )] public int?                  NO                      { get; set; }
        [JsonProperty(_CDF_WORKSHEET_NAME_  )] public string                WORKSHEET_NAME          { get; set; }
        [JsonProperty(_CDF_BLOCK_NAME_      )] public string                BLOCK_NAME              { get; set; }
        [JsonProperty(_CDF_BLOCK_TYPE_      )] public HxTemplateBlockType   BLOCK_TYPE              { get; set; }
        [JsonProperty(_CDF_START_ROW_       )] public int?                  START_ROW               { get; set; }
        [JsonProperty(_CDF_START_COLUMN_    )] public int?                  START_COLUMN            { get; set; }
        [JsonProperty(_CDF_END_ROW_         )] public int?                  END_ROW                 { get; set; }
        [JsonProperty(_CDF_END_COLUMN_      )] public int?                  END_COLUMN              { get; set; }
        [JsonProperty(_CDF_START_ADDR_      )] public string                START_ADDR              { get; set; }
        [JsonProperty(_CDF_END_ADDR_        )] public string                END_ADDR                { get; set; }
        [JsonProperty(_CDF_TAG_INPUT_       )] public string                TAG_INPUT               { get; set; }
        [JsonProperty(_CDF_TAG_PATTERN_     )] public string                TAG_PATTERN             { get; set; }
        [JsonProperty(_CDF_TAG_VALUE_       )] public string                TAG_VALUE               { get; set; }
        [JsonProperty(_CDF_TAG_CASE_        )] public string                TAG_CASE                { get; set; }
        [JsonProperty(_CDF_TAG_NAME_        )] public string                TAG_NAME                { get; set; }
        [JsonProperty(_CDF_TAG_OUTPUT_      )] public string                TAG_OUTPUT              { get; set; }
        [JsonProperty(_CDF_TAG_OPTION_VALUE_)] public string                TAG_OPTION_VALUE        { get; set; }
        [JsonProperty(_CDF_TAG_OPTION_ROW_  )] public int?                  TAG_OPTION_ROW          { get; set; }
        [JsonProperty(_CDF_TAG_OPTION_COL_  )] public int?                  TAG_OPTION_COL          { get; set; }
        [JsonProperty(_CDF_START_DEL_ROW_   )] public int?                  START_DEL_ROW           { get; set; }
        [JsonProperty(_CDF_END_DEL_ROW_     )] public int?                  END_DEL_ROW             { get; set; }
        [JsonProperty(_CDF_PAGE_TOTAL_COUNT_)] public int?                  PAGE_TOTAL_COUNT        { get; set; }
        [JsonProperty(_CDF_PAGE_ITEM_COUNT_ )] public int?                  PAGE_ITEM_COUNT         { get; set; }
        [JsonProperty(_CDF_PAGE_BLANK_COUNT_)] public int?                  PAGE_BLANK_COUNT        { get; set; }
        [JsonProperty(_CDF_REMARK_          )] public string                REMARK                  { get; set; }
        #endregion

        public override void SetMatchFieldValue(string name, object value)
        {
            switch (name)
            {
                case _CDF_BLOCK_TYPE_:
                    try
                    {
                        if (value == null)
                        {
                            BLOCK_TYPE = HxTemplateBlockType.None;
                        }
                        else
                        {
                            BLOCK_TYPE = (HxTemplateBlockType)value;
                        }
                    }
                    catch (Exception ex)
                    {
                        BLOCK_TYPE = HxTemplateBlockType.None;
                        Debug.WriteLine(ex);
                        //throw;
                    }
                    
                    break;
                default:
                    base.SetMatchFieldValue(name, value);
                    break;
            }
            
        }
    }
}
