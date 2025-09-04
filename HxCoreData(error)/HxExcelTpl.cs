using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace HxCore.Data
{
    using HxCore;
    using System.Linq;

    public partial class HxExcelTpl : HxExcel
    {
        #region Const / Define

        public const string _TPL_TAG_PATTERN_ = HxTagTpl._DEF_TAG_ALL_PATTERN_;

        public const string _TPL_TAG_BEGIN_ = "{{#BEGIN}}";
        public const string _TPL_TAG_END_ = "{{#END}}";

        public const string _TPL_DEF_PAGE_RANGE_ = "PageRange";
        public const string _TPL_DEF_PAGE_HEAD_ = "PageHead";
        public const string _TPL_DEF_PAGE_FOOT_ = "PageFoot";
        public const string _TPL_DEF_START_ROW_ = "StartRow";

        public const string _UDEF_NO_ = "no";

        public const string _UDEF_WORKSHEET_NAME_ = "worksheet_name";
        public const string _UDEF_BLOCK_NAME_ = "block_name";
        public const string _UDEF_BLOCK_TYPE_ = "block_type";

        public const string _UDEF_START_RANGE_ = "start_range";
        public const string _UDEF_END_RANGE_ = "end_range";

        public const string _UDEF_START_ADDR_ = "start_address";
        public const string _UDEF_END_ADDR_ = "end_address";

        public const string _UDEF_START_ROW_ = "start_row";
        public const string _UDEF_END_ROW_ = "end_row";
        public const string _UDEF_START_COLUMN_ = "start_column";
        public const string _UDEF_END_COLUMN_ = "end_column";

        public const string _UDEF_START_DEL_ROW_ = "start_del_row";
        public const string _UDEF_END_DEL_ROW_ = "end_del_row";

        public const string _UDEF_COLUMN_INDEX_ = "column_index";
        public const string _UDEF_ROW_INDEX_ = "row_index";

        public const string _UDEF_MERGE_TYPE_ = "merge_type";
        public const string _UDEF_TAG_COLOR_ = "tag_color";


        public const string _UDEF_PAGE_TOTAL_COUNT_ = "page_total_count";
        public const string _UDEF_PAGE_ITEM_COUNT_ = "page_item_count";
        public const string _UDEF_PAGE_BLANK_COUNT_ = "page_blank_count";

        public const string _UDEF_BEGIN_HEAD_ = "begin_head";
        public const string _UDEF_END_HEAD_ = "end_head";
        public const string _UDEF_BEGIN_FOOT_ = "begin_foot";
        public const string _UDEF_END_FOOT_ = "end_foot";


        public const string _UDEF_TOTAL_PAGE_ = "total_page";
        public const string _UDEF_NEW_NAME_ = "new_name";



        public const string _UDEF_TAG_INPUT_ = "tag_input";
        public const string _UDEF_TAG_OUTPUT_ = "tag_output";
        public const string _UDEF_TAG_PATTERN_ = "tag_pattern";
        public const string _UDEF_TAG_VALUE_ = "tag_value";
        public const string _UDEF_TAG_NAME_ = "tag_name";
        public const string _UDEF_TAG_CASE_ = "tag_case";
        public const string _UDEF_TAG_OPTION_VALUE_ = "tag_option_value";
        public const string _UDEF_TAG_OPTION_ROW_ = "tag_option_row";
        public const string _UDEF_TAG_OPTION_COL_ = "tag_option_col";

        public const string _UDEF_REMARK_ = "remark";
        #endregion

        #region Static / ExcelTemplate Utils
        
        #endregion

        #region Class Member 변수
        public HxTemplateType TemplateType
        {
            get;
            protected set;
        }
        public DataTable TplWorksheetInfo { get; protected set; }
        public DataTable TplBlockInfo { get; protected set; }
        //public DataTable TplBlockInfo { get; protected set; }
        private List<ExcelWorksheet> SourceWorksheets = new List<ExcelWorksheet>();

        public int ExcelWorksheetCount
        {
            get
            {
                if (this.ExcelApp == null)
                    return -1;
                else if (this.ExcelAppWorksheets == null)
                    return -1;
                else
                    return this.ExcelAppWorksheets.Count;
            }
        }

        public int SourceWorksheetCount
        {
            get
            {
                if (this.ExcelApp == null)
                    return -1;
                else if (this.SourceWorksheets == null)
                    return -1;
                else
                    return this.SourceWorksheets.Count;
            }
        }

        public HxExcelRangeNumberRec this[int index]
        {
            get
            {
                if(this.TplWorksheetInfo != null && this.TplWorksheetInfo.Rows.Count > index)
                {
                    return GetWorksheetTplPageRange(this.ExcelAppWorksheets[index]);
                }
                return new HxExcelRangeNumberRec(false);
            }
        }
        public HxExcelRangeNumberRec this[string name]
        {
            get
            {
                if (this.TplWorksheetInfo != null && this.TplWorksheetInfo.Rows.Count > 0)
                {
                    return GetWorksheetTplPageRange(this.ExcelAppWorksheets[name]);
                }
                return new HxExcelRangeNumberRec(false);
            }
        }

        #endregion

        #region 생성자 및 초기화
        public HxExcelTpl(string openFileName, string saveFileName = null, bool isOverWrite = false)
            : base(openFileName, saveFileName, isOverWrite)
        {
            this.Init();
        }

        public HxExcelTpl(Stream loadStream, string saveFileName = null, bool isOverWrite = false)
            : base(loadStream, saveFileName, isOverWrite)
        {
            this.Init();
        }

        public void Init(bool bInit = false)
        {
            if (IsCreated != true || bInit == true)
            {
                TemplateType = HxTemplateType.None;

                InitVars();

                IsCreated = true;
            }
        }
        private void InitVars()
        {
            this.TplWorksheetInfo = CreateTplWorksheetDataTable("Worksheet Info");

            if (this.SourceWorksheets == null)
                this.SourceWorksheets = new List<ExcelWorksheet>();
            this.SourceWorksheets.Clear();

            foreach (ExcelWorksheet ws in this.ExcelAppWorksheets)
            {
                string strFindVar = ws.Name.Substring(0, 1);
                if (strFindVar == "#")
                {
                    this.SourceWorksheets.Add(ws);
                }
            }
            if(this.SourceWorksheets.Count == 0 && this.ExcelAppWorksheets.Count > 0)
            {
                this.SourceWorksheets.Add(this.ExcelAppWorksheets.First());
            }

            if(this.TplWorksheetInfo != null && this.SourceWorksheets != null && this.SourceWorksheets.Count > 0)
            {
                foreach (ExcelWorksheet ws in this.SourceWorksheets)
                {
                    try
                    {
                        HxExcelRangeNumberRec wsPageRangeInfo = GetWorksheetDimensionRange(ws);
                        HxExcelRangeNumberRec wsFindRangeInfo = wsPageRangeInfo; //GetWorksheetDimensionRange(ws);
                        ExcelRange matchRange = GetFindCellRegexMatchFirstRange(ws, _TPL_TAG_BEGIN_, wsFindRangeInfo);
                        if (matchRange != null)
                        {
                            if (matchRange.End.Column >= 1)
                            {
                                wsFindRangeInfo.StartColumn = matchRange.End.Column;
                                wsFindRangeInfo.EndColumn = matchRange.End.Column;

                                wsPageRangeInfo.EndColumn = matchRange.End.Column - 1;
                            }
                        }
                        matchRange = null;
                        matchRange = GetFindCellRegexMatchFirstRange(ws, _TPL_TAG_END_, wsFindRangeInfo);
                        if (matchRange != null)
                        {
                            if (matchRange.End.Row >= 1)
                            {
                                wsFindRangeInfo.EndRow = matchRange.End.Row;

                                wsPageRangeInfo.EndRow = matchRange.End.Row;
                            }
                            //if (wsRange.EndRow > wsPageEndRange..Row)
                            //{
                            //    wsRange.EndRow = wsPageEndRange.Start.Row;
                            //}
                            //if (wsRange.EndColumn > wsPageEndRange.Start.Column)
                            //{
                            //    wsRange.EndColumn = wsPageEndRange.Start.Column;
                            //}
                        }
                        DataRow dr = this.TplWorksheetInfo.NewRow();
                        dr[_UDEF_WORKSHEET_NAME_] = ws.Name;
                        dr[_UDEF_BLOCK_NAME_] = _TPL_DEF_PAGE_RANGE_;
                        dr[_UDEF_BLOCK_TYPE_] = HxTemplateBlockType.PageRange;

                        dr[_UDEF_START_ROW_] = wsPageRangeInfo.StartRow;
                        dr[_UDEF_START_COLUMN_] = wsPageRangeInfo.StartColumn;
                        dr[_UDEF_END_ROW_] = wsPageRangeInfo.EndRow;
                        dr[_UDEF_END_COLUMN_] = wsPageRangeInfo.EndColumn;

                        //dr[_UDEF_START_ADDR_] = new ExcelAddress(wsPageRangeInfo.StartRow, wsPageRangeInfo.StartColumn, wsPageRangeInfo.StartRow, wsPageRangeInfo.StartColumn);
                        //dr[_UDEF_END_ADDR_] = new ExcelAddress(wsPageRangeInfo.EndRow, wsPageRangeInfo.EndColumn, wsPageRangeInfo.EndRow, wsPageRangeInfo.EndColumn);

                        dr[_UDEF_TAG_COLOR_] = ws.TabColor;
                        dr[_UDEF_REMARK_] = ws.TabColor.ToStringEx();

                        this.TplWorksheetInfo.Rows.Add(dr);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        throw ex;
                    }
                    
                }
            }
        }

        public HxExcelRangeNumberRec GetWorksheetTplPageRange(ExcelWorksheet worksheet)
        {
            HxExcelRangeNumberRec Result = new HxExcelRangeNumberRec(1, 1, 1, 1);
            if (worksheet != null)
            {
                Result = GetWorksheetDimensionRange(worksheet);
                if (TplWorksheetInfo != null && TplWorksheetInfo.Rows.Count > 0 &&
                    TplWorksheetInfo.Columns.Contains(_UDEF_WORKSHEET_NAME_) && TplWorksheetInfo.Columns.Contains(_UDEF_BLOCK_TYPE_) &&
                    (
                        (TplWorksheetInfo.Columns.Contains(_UDEF_START_ROW_) && TplWorksheetInfo.Columns.Contains(_UDEF_START_COLUMN_) && TplWorksheetInfo.Columns.Contains(_UDEF_END_ROW_) && TplWorksheetInfo.Columns.Contains(_UDEF_END_COLUMN_))
                        || (TplWorksheetInfo.Columns.Contains(_TPL_DEF_PAGE_RANGE_))
                    )
                )
                {
                    var query = (
                                from ws in TplWorksheetInfo.AsEnumerable()
                                where ws.Field<string>(_UDEF_WORKSHEET_NAME_).Equals(worksheet.Name) && ws.Field<HxTemplateBlockType>(_UDEF_BLOCK_TYPE_).Equals(HxTemplateBlockType.PageRange)
                                orderby ws.Field<int>(_UDEF_NO_)
                                select ws
                                );

                    //var varTemp = TplWorksheetInfo.AsEnumerable().Where(x => x[_UDEF_WORKSHEET_NAME_].Equals(worksheet.Name));



                    if (query != null && query.Any() == true)
                    {
                        var q = query.Last();

                        Result.StartRow = q.Field<int>(_UDEF_START_ROW_);
                        Result.StartColumn = q.Field<int>(_UDEF_START_COLUMN_);
                        Result.EndRow = q.Field<int>(_UDEF_END_ROW_);
                        Result.EndColumn = q.Field<int>(_UDEF_END_COLUMN_);
                    }
                }
            }
            return Result;
        }
        #endregion
    }
}
