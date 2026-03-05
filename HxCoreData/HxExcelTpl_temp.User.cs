using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HxCore;
using OfficeOpenXml;

namespace HxCore.Data
{
    partial class HxExcelTpl_temp
    {
        #region Const / Define
        public const string _TPL_DEF_BEGIN_ = "begin";
        public const string _TPL_DEF_END_ = "end";

        public const string _TPL_DEF_PAGE_RANGE_ = "PageRange";
        public const string _TPL_DEF_PAGE_HEAD_ = "PageHead";
        public const string _TPL_DEF_PAGE_FOOT_ = "PageFoot";

        public const string _UDEF_WORKSHEET_NAME_ = "worksheet_name";
        public const string _UDEF_BLOCK_NAME_ = "block_name";
        public const string _UDEF_BLOCK_TYPE_ = "block_type";
        public const string _UDEF_START_ROW_ = "start_row";
        public const string _UDEF_END_ROW_ = "end_row";
        public const string _UDEF_START_COLUMN_ = "start_column";
        public const string _UDEF_END_COLUMN_ = "end_column";
        public const string _UDEF_START_DEL_ROW_ = "start_del_row";
        public const string _UDEF_END_DEL_ROW_ = "end_del_row";
        public const string _UDEF_TAG_COLOR_ = "tag_color";

        public const string _UDEF_COLUMN_INDEX_ = "column_index";
        public const string _UDEF_ROW_INDEX_ = "row_index";
        public const string _UDEF_MERGE_TYPE_ = "merge_type";


        public const string _UDEF_PAGE_TOTAL_COUNT_ = "page_total_count";
        public const string _UDEF_PAGE_ITEM_COUNT_ = "page_item_count";
        public const string _UDEF_PAGE_BLANK_COUNT_ = "page_blank_count";

        public const string _UDEF_BEGIN_HEAD_ = "begin_head";
        public const string _UDEF_END_HEAD_ = "end_head";
        public const string _UDEF_BEGIN_FOOT_ = "begin_foot";
        public const string _UDEF_END_FOOT_ = "end_foot";


        public const string _UDEF_TOTAL_PAGE_ = "total_page";
        public const string _UDEF_NEW_NAME_ = "new_name";

        public const string _UDEF_REMARK_ = "remark";
        #endregion

        public DataSet AssignDataSet { get; protected set; }
        //private Dictionary<string, string> defineList { get; set; }
        public Dictionary<string, object> AssignVars { get; protected set; }


        public DataTable TplWorksheetInfo { get; protected set; }
        public DataTable TplBlockInfo { get; protected set; }
        public DataTable TplLoopStyleInfo { get; protected set;}
        public DataTable TplPageInfo { get; protected set; }
        private DataTable TplMergeCellInfo { get; set; }

        private List<ExcelWorksheet> NormalSourceWorksheets = new List<ExcelWorksheet>();
        private List<ExcelWorksheet> LoopSourceWorksheets = new List<ExcelWorksheet>();
        private List<ExcelWorksheet> OtherSourceWorksheets = new List<ExcelWorksheet>();

        private void InitVars()
        {
            this.TplWorksheetInfo = new DataTable("Template Worksheets");
            this.TplWorksheetInfo.Columns.AddRange(new DataColumn[]{
                      new DataColumn{ ColumnName = _UDEF_WORKSHEET_NAME_ , DataType = typeof(string)}
                    , new DataColumn{ ColumnName = _UDEF_BLOCK_NAME_ , DataType = typeof(string), DefaultValue = _TPL_DEF_PAGE_RANGE_}
                    , new DataColumn{ ColumnName = _UDEF_BLOCK_TYPE_, DataType = typeof(HxTemplateBlockType), DefaultValue = HxTemplateBlockType.PageRange }
                    , new DataColumn{ ColumnName = _UDEF_START_ROW_, DataType = typeof(int) }
                    , new DataColumn{ ColumnName = _UDEF_END_ROW_, DataType = typeof(int) }
                    , new DataColumn{ ColumnName = _UDEF_START_COLUMN_, DataType = typeof(int) }
                    , new DataColumn{ ColumnName = _UDEF_END_COLUMN_, DataType = typeof(int) }
                    , new DataColumn{ ColumnName = _UDEF_TAG_COLOR_, DataType = typeof(System.Drawing.Color), DefaultValue = System.Drawing.Color.Transparent }
                    , new DataColumn{ ColumnName = _UDEF_REMARK_, DataType = typeof(string) }
                });

            this.TplBlockInfo = new DataTable("Template Blocks");
            this.TplBlockInfo.Columns.AddRange(new DataColumn[]{
                      new DataColumn{ ColumnName = _UDEF_WORKSHEET_NAME_ , DataType = typeof(string)}
                    , new DataColumn{ ColumnName = _UDEF_BLOCK_NAME_ , DataType = typeof(string)}
                    , new DataColumn{ ColumnName = _UDEF_BLOCK_TYPE_, DataType = typeof(HxTemplateBlockType), DefaultValue = HxTemplateBlockType.None }
                    , new DataColumn{ ColumnName = _UDEF_START_ROW_, DataType = typeof(int) }
                    , new DataColumn{ ColumnName = _UDEF_END_ROW_, DataType = typeof(int) }
                    , new DataColumn{ ColumnName = _UDEF_START_COLUMN_, DataType = typeof(int) }
                    , new DataColumn{ ColumnName = _UDEF_END_COLUMN_, DataType = typeof(int) }
                    , new DataColumn{ ColumnName = _UDEF_START_DEL_ROW_, DataType = typeof(int) }
                    , new DataColumn{ ColumnName = _UDEF_END_DEL_ROW_, DataType = typeof(int) }
                    , new DataColumn{ ColumnName = _UDEF_PAGE_TOTAL_COUNT_, DataType = typeof(int), Caption = "Page Total Count"}
                    , new DataColumn{ ColumnName = _UDEF_PAGE_ITEM_COUNT_, DataType = typeof(int), Caption = "Page of Item Count"}
                    , new DataColumn{ ColumnName = _UDEF_PAGE_BLANK_COUNT_, DataType = typeof(int), Caption = "Last Page Balnk Row Count"}
                    , new DataColumn{ ColumnName = _UDEF_REMARK_, DataType = typeof(string) }
                });

            this.TplLoopStyleInfo = new DataTable("Loop Excel Cell Style");
            this.TplLoopStyleInfo.Columns.AddRange(new DataColumn[]{
                     new DataColumn{ ColumnName = _UDEF_WORKSHEET_NAME_, DataType = typeof(string)}
                    , new DataColumn{ ColumnName = _UDEF_BLOCK_NAME_, DataType = typeof(string)}
                    , new DataColumn{ ColumnName = _UDEF_COLUMN_INDEX_, DataType = typeof(int) }
                    , new DataColumn{ ColumnName = _UDEF_MERGE_TYPE_, DataType = typeof(HxValueAssignRowspanType) }
                    , new DataColumn{ ColumnName = _UDEF_REMARK_, DataType = typeof(string) }
                });

            this.TplMergeCellInfo = new DataTable("Worksheet Merge Cells Info");
            this.TplMergeCellInfo.Columns.AddRange(new DataColumn[]{
                    new DataColumn{ ColumnName = _UDEF_WORKSHEET_NAME_, DataType = typeof(string), Unique = true }
                    , new DataColumn{ ColumnName = _UDEF_START_ROW_, DataType = typeof(int) }
                    , new DataColumn{ ColumnName = _UDEF_END_ROW_, DataType = typeof(int) }
                    , new DataColumn{ ColumnName = _UDEF_START_COLUMN_, DataType = typeof(int) }
                    , new DataColumn{ ColumnName = _UDEF_END_COLUMN_, DataType = typeof(int) }
                    , new DataColumn{ ColumnName = _UDEF_REMARK_, DataType = typeof(string) }
                });

            this.TplPageInfo = new DataTable("Template Page Info");
            this.TplPageInfo.Columns.AddRange(new DataColumn[]{
                    new DataColumn{ ColumnName = _UDEF_WORKSHEET_NAME_, DataType = typeof(string), Unique = true }
                    , new DataColumn{ ColumnName = _UDEF_TOTAL_PAGE_, DataType = typeof(string) }
                    , new DataColumn{ ColumnName = _UDEF_NEW_NAME_, DataType = typeof(string) }
                });

            if (this.NormalSourceWorksheets == null)
                this.NormalSourceWorksheets = new List<ExcelWorksheet>();
            if (this.LoopSourceWorksheets == null)
                this.LoopSourceWorksheets = new List<ExcelWorksheet>();
            if (this.OtherSourceWorksheets == null)
                this.OtherSourceWorksheets = new List<ExcelWorksheet>();
            if (this.AssignVars == null)
                this.AssignVars = new Dictionary<string, object>();
            if (this.AssignDataSet == null)
                this.AssignDataSet = new DataSet();
            this.DoFindSourceSheets();
        }

        private void DoFindSourceSheets()
        {
            if (this.ExcelAppWorksheets != null)
            {
                if (this.NormalSourceWorksheets == null)
                    this.NormalSourceWorksheets = new List<ExcelWorksheet>();
                if (this.LoopSourceWorksheets == null)
                    this.LoopSourceWorksheets = new List<ExcelWorksheet>();
                if (this.OtherSourceWorksheets == null)
                    this.OtherSourceWorksheets = new List<ExcelWorksheet>();

                this.NormalSourceWorksheets.Clear();
                this.LoopSourceWorksheets.Clear();
                this.OtherSourceWorksheets.Clear();

                foreach (ExcelWorksheet ws in this.ExcelAppWorksheets)
                {
                    string strFindVar = ws.Name.Substring(0, 1);
                    if (strFindVar == "#")
                    {
                        this.NormalSourceWorksheets.Add(ws);
                    }
                    else if (strFindVar == "@")
                    {
                        this.LoopSourceWorksheets.Add(ws);
                    }
                    else
                    {
                        this.OtherSourceWorksheets.Add(ws);
                    }
                }
            }
        }

        public void Assign(Dictionary<string, object> args)
        {
            if(this.AssignVars == null)
            {
                this.AssignVars = new Dictionary<string, object>();
            }
            foreach (KeyValuePair<string, object> pair in args)
            {
                if (!this.AssignVars.ContainsKey(pair.Key))
                {
                    this.AssignVars.Add(pair.Key, pair.Value);
                }
                else
                {
                    this.AssignVars[pair.Key] = pair.Value;
                }
            }
            //this.assignVars.ContainsKey(
        }

        public void Assign(DataSet assignData)
        {
            this.AssignDataSet = assignData;
        }

        private void DoAssingDataKeysNameToLower()
        {
            if (this.AssignVars != null && this.AssignVars.Count > 0)
            {
                foreach (KeyValuePair<string, object> pair in this.AssignVars)
                {
                    string keyName = pair.Key.ToLower();
                    if (!this.AssignVars.ContainsKey(keyName))
                    {
                        this.AssignVars.Add(keyName, pair.Value);
                    }
                }
            }
            if (this.AssignDataSet != null && this.AssignDataSet.Tables.Count > 0)
            {
                foreach (DataTable dt in this.AssignDataSet.Tables)
                {
                    dt.TableName = dt.TableName.ToLower();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        dc.ColumnName = dc.ColumnName.ToLower();
                    }
                }
            }
        }

        private void Assign(DataTable assingData)
        {
            if (this.AssignDataSet == null)
                this.AssignDataSet = new DataSet();
            if (!this.AssignDataSet.Tables.Contains(assingData.TableName))
            {
                this.AssignDataSet.Tables.Add(assingData);
            }
        }
        private void Assign(string tableName, DataTable assingData)
        {
            assingData.TableName = tableName;
            this.Assign(assingData);
        }

        public void Print_()
        {
            this.DoAssingDataKeysNameToLower();
            //this.PrintIndex();
        }

        

        private void DoPrintWorksheet(ExcelWorksheet worksheet)
        {
            //this.blockInfo.Rows.Clear();
            //this.templateWorksheetInfo.Clear();
            #region Page Range
            int PAGE_ROW_BEGIN = 1;
            int PAGE_ROW_END = worksheet.Dimension.End.Row;
            int PAGE_COL_BEGIN = 1;
            int PAGE_COL_END = worksheet.Dimension.End.Column;

            HxTempateWorkseetRec Result = GetWorksheetPageRange(worksheet, ref PAGE_ROW_BEGIN, ref PAGE_ROW_END, ref PAGE_COL_BEGIN, ref PAGE_COL_END);

            DataRow drPageRange = this.TplWorksheetInfo.NewRow();

            drPageRange[_UDEF_WORKSHEET_NAME_] = worksheet.Name;

            drPageRange[_UDEF_BLOCK_NAME_] = _TPL_DEF_PAGE_RANGE_;
            drPageRange[_UDEF_BLOCK_TYPE_] = HxTemplateBlockType.PageRange;

            drPageRange[_UDEF_START_ROW_] = PAGE_ROW_BEGIN;
            drPageRange[_UDEF_END_ROW_] = PAGE_ROW_END;
            drPageRange[_UDEF_START_COLUMN_] = PAGE_COL_BEGIN;
            drPageRange[_UDEF_END_COLUMN_] = PAGE_COL_END;

            drPageRange[_UDEF_TAG_COLOR_] = worksheet.TabColor;
            drPageRange[_UDEF_REMARK_] = worksheet.TabColor.ToStringEx();

            this.TplWorksheetInfo.Rows.Add(drPageRange);
            #endregion

            #region Page Head
            //ExcelRange PageHead = GetFindBlockDefineRange(worksheet, _UDEF_BEGIN_HEAD_, PAGE_ROW_BEGIN, PAGE_ROW_END, PAGE_COL_END);
            ExcelRange PageHead = GetFindDefineAreaRange(worksheet, _UDEF_BEGIN_HEAD_);
            if (PageHead != null)
            {
                DataRow drPageHead = this.TplBlockInfo.NewRow();
                drPageHead[_UDEF_WORKSHEET_NAME_] = worksheet.Name;
                drPageHead[_UDEF_BLOCK_NAME_] = _TPL_DEF_PAGE_HEAD_;
                drPageHead[_UDEF_BLOCK_TYPE_] = HxTemplateBlockType.PageHead;
                drPageHead[_UDEF_START_ROW_] = PAGE_ROW_BEGIN;
                drPageHead[_UDEF_END_ROW_] = PAGE_ROW_END;
                drPageHead[_UDEF_START_COLUMN_] = PAGE_COL_BEGIN;
                drPageHead[_UDEF_END_COLUMN_] = PAGE_COL_END;
                this.TplBlockInfo.Rows.Add(drPageHead);
            }
            #endregion

            #region Page End
            ExcelRange PageFoot = GetFindDefineAreaRange(worksheet, _UDEF_BEGIN_FOOT_, PAGE_ROW_BEGIN, PAGE_ROW_END, PAGE_COL_END);
            if (PageHead != null)
            {
                DataRow drPageFoot = this.TplBlockInfo.NewRow();
                drPageFoot[_UDEF_WORKSHEET_NAME_] = worksheet.Name;
                drPageFoot[_UDEF_BLOCK_NAME_] = _TPL_DEF_PAGE_FOOT_;
                drPageFoot[_UDEF_BLOCK_TYPE_] = HxTemplateBlockType.PageFoot;
                drPageFoot[_UDEF_START_ROW_] = PAGE_ROW_BEGIN;
                drPageFoot[_UDEF_END_ROW_] = PAGE_ROW_END;
                drPageFoot[_UDEF_START_COLUMN_] = PAGE_COL_BEGIN;
                drPageFoot[_UDEF_END_COLUMN_] = PAGE_COL_END;
                this.TplBlockInfo.Rows.Add(drPageFoot);
            }
            #endregion
                        
        }

        private HxTempateWorkseetRec GetWorksheetPageRange(ExcelWorksheet worksheet, ref int AStartRow, ref int AEndRow, ref int AStartColumn, ref int AEndColumn)
        {
            HxTempateWorkseetRec Result = new HxTempateWorkseetRec();
            AStartRow = 1;
            if(AEndRow <= 0)
                AEndRow = _MAX_EXCEL_ROW_;

            AStartColumn = 1;
            if(AEndColumn <= 0)
                AEndColumn = _MAX_EXCEL_COL_;

            bool isFindBegin = false;
            //bool isFindEnd = false;

            #region BEGIN(시작) 위치 찾기
            for (int iRow = AStartRow; iRow <= AEndRow; iRow++)
            {
                for (int iCol = AStartColumn; iCol <= AEndColumn; iCol++)
                {
                    ExcelRange cell = worksheet.Cells[iRow, iCol];
                    if (!cell.Text.IsNullOrWhiteSpaceEx() && cell.Text.ToLower().Contains("{# " + _TPL_DEF_BEGIN_ + "}"))
                    {
                        //Match match = Regex.Match(cell.Value.ToStringEx(), "{#begin}", RegexOptions.IgnoreCase);
                        AStartRow = cell.Start.Row;
                        AEndColumn = cell.End.Column;
                        isFindBegin = true;
                        break;
                    }
                }
                if (isFindBegin == true)
                    break;
            }
            #endregion
            #region END(끝) 위치 찾기
            if (isFindBegin == true)
            {
                for (int iRow = AStartRow; iRow <= AEndRow; iRow++)
                {
                    ExcelRange cell = worksheet.Cells[iRow, AEndColumn];
                    if (!cell.Text.IsNullOrWhiteSpaceEx() && cell.Text.ToLower().Contains("{# " + _TPL_DEF_END_ + "}"))
                    {
                        AEndRow = cell.End.Row;
                        //isFindEnd = true;
                        break;
                    }
                }
            }
            #endregion

            Result.WorksheetName = worksheet.Name;
            Result.StartRow = AStartRow;
            Result.StartColumn = AStartColumn;
            Result.EndRow = AEndRow;
            Result.EndColumn = AEndColumn;
            return Result;
        }

        private Dictionary<string, ExcelRange> GetWorksheetLoopBlocks(ExcelWorksheet worksheet, HxTempateWorkseetRec worksheetInfo)
        {
            Dictionary<string, ExcelRange> Result = new Dictionary<string, ExcelRange>();

            string strLoopPattern = @"({)(@)(\w+)(:[0-9]{1,})*(\s*\/\s*)*(})";
            Regex loopRegex = new Regex(strLoopPattern, RegexOptions.IgnoreCase);
            var queryLoops = (from ce in worksheet.Cells[worksheetInfo.StartRow, worksheetInfo.EndColumn, worksheetInfo.EndRow, worksheetInfo.EndColumn]
                              where ce.Value is string && (loopRegex.IsMatch(ce.Value.ToStringEx()) == true)
                              select ce
                            );
            if (queryLoops != null)
            {
                foreach (var loop in queryLoops)
                {
                    Match match = Regex.Match(loop.Value.ToStringEx(), strLoopPattern);
                    //int idx = 0;
                    if (match.Success)
                    {
                        int loopStartRow = -1;
                        int loopItemEndRow = -1;
                        int loopBlankStartRow = -1;
                        int loopBlockEndRow = -1;
                        string strFullText = match.Groups[0].ToStringEx();
                        string strLoopName = match.Groups[3].ToStringEx();
                        string strLoopCount = match.Groups[4].ToStringEx();
                        string strLoopEnd = match.Groups[5].ToStringEx();
                        loopStartRow = loop.Start.Row;
                        if (!strLoopEnd.IsNullOrWhiteSpaceEx())
                        {
                            loopItemEndRow = loop.End.Row;
                        }
                        else
                        {
                            var queryLoopElse = (from ce in worksheet.Cells[loopStartRow, worksheetInfo.EndColumn, worksheetInfo.EndRow, worksheetInfo.EndColumn]
                                                 where ce.Value is string && (ce.Value.ToStringEx().Contains("{:}") == true)
                                                 select ce
                                );
                            if (queryLoopElse != null && queryLoopElse.Count() > 0)
                            {
                                loopBlankStartRow = queryLoopElse.First().Start.Row;
                                loopItemEndRow = loopBlankStartRow - 1;

                            }

                            var queryLoopEnd = (from ce in worksheet.Cells[loopStartRow, worksheetInfo.EndColumn, worksheetInfo.EndRow, worksheetInfo.EndColumn]
                                                where ce.Value is string && (ce.Value.ToStringEx().Contains("{/}") == true)
                                                select ce
                                );
                            if (queryLoopEnd != null && queryLoopEnd.Count() > 0)
                            {

                                loopBlockEndRow = queryLoopEnd.First().End.Row;
                                loopItemEndRow = loopBlockEndRow;
                            }
                        }

                        int nPageItemCount = strLoopCount.ToIntEx();
                        int loopDataRowCount = -1;
                        if (this.AssignDataSet.Tables.Contains(strLoopName))
                        {
                            loopDataRowCount = this.AssignDataSet.Tables[strLoopName].Rows.Count;
                        }
                        //int loopDataRowCount = this.assignDataSet[
                        int nPageQuotient = loopDataRowCount / nPageItemCount;
                        int nPageRemainder = loopDataRowCount % nPageItemCount;
                        int nPageTotal = nPageQuotient;
                        int nPageBlank = 0;
                        if (nPageRemainder > 0)
                        {
                            nPageTotal += 1;
                            nPageBlank = (nPageItemCount - nPageRemainder);
                        }


                        DataRow dr = this.TplBlockInfo.NewRow();
                        dr["worksheet_name"] = worksheet.Name;
                        dr["block_name"] = strLoopName;
                        dr["block_type"] = HxTemplateBlockType.ItemLoop;
                        dr["start_row"] = loopStartRow;
                        dr["end_row"] = loopItemEndRow;
                        dr["start_column"] = worksheetInfo.StartColumn;
                        dr["end_column"] = worksheetInfo.EndColumn;
                        dr["start_del_row"] = loopBlankStartRow;
                        dr["end_del_row"] = loopBlockEndRow;
                        dr["page_total_count"] = nPageTotal;
                        dr["page_blank_count"] = nPageBlank;
                        dr["page_item_count"] = nPageItemCount;
                        //dr["
                        this.TplBlockInfo.Rows.Add(dr);
                        Result.Add(strLoopName, worksheet.Cells[loopStartRow, worksheetInfo.StartColumn, loopItemEndRow, worksheetInfo.EndColumn]);
                    }
                }
            }
            return Result;
        }

        private void DoWorksheetLoopStyle(ExcelWorksheet worksheet, string blockName, HxTempateWorkseetRec tplWorksheetInfo)
        {
            if (this.AssignDataSet.Tables.Contains(blockName))
            {
                DataTable item = this.AssignDataSet.Tables[blockName];
                int itemTotal = item.Rows.Count;
                DataRow[] loopBlockRows = this.TplBlockInfo.Select(string.Format("block_name = '{0}'", blockName));
                if (loopBlockRows != null && loopBlockRows.Count() > 0)
                {
                    #region Block Style
                    string strStyleBlockPattern = @"({)(\#)\s*(STYLE)\s*(:)\s*(" + blockName + @")\s*(\/)\s*(})";
                    var queryStyles = (from ce in worksheet.Cells[tplWorksheetInfo.EndRow, tplWorksheetInfo.EndColumn, tplWorksheetInfo.EndRow + 10, tplWorksheetInfo.EndColumn]
                                       where ce.Value is string && (Regex.IsMatch(ce.Text, strStyleBlockPattern) == true)
                                       select ce
                            );
                    if (queryStyles != null && queryStyles.Count() > 0)
                    {
                        var styleBlock = queryStyles.First();
                        if (styleBlock != null)
                        {
                            string strStyleTagPattern = @"({)(\#)\s*(ROWSPAN)\s*(:)\s*([\w\-]+)\s*(})";
                            for (int iCol = tplWorksheetInfo.StartColumn; iCol < tplWorksheetInfo.EndColumn; iCol++)
                            {
                                ExcelRange cell = worksheet.Cells[styleBlock.Start.Row, iCol];
                                Match match = Regex.Match(cell.Text, strStyleTagPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                                if (match.Success)
                                {
                                    string strMatchFullText = match.Groups[0].ToStringEx();
                                    string strMatchModeName = match.Groups[3].ToStringEx();
                                    string strMatchModeType = match.Groups[5].ToStringEx();
                                    HxValueAssignRowspanType rowspanType = HxValueAssignRowspanType.None;

                                    DataRow dr = this.TplLoopStyleInfo.NewRow();
                                    dr["worksheet_name"] = worksheet.Name;
                                    dr["block_name"] = blockName;
                                    dr["column_index"] = iCol;
                                    //dr["merge_type"]

                                    switch (strMatchModeType.Trim().ToLower())
                                    {
                                        case "tag":
                                            rowspanType = HxValueAssignRowspanType.Tag;
                                            break;
                                        case "value":
                                            rowspanType = HxValueAssignRowspanType.Value;
                                            break;
                                        default:
                                            rowspanType = HxValueAssignRowspanType.None;
                                            break;
                                    }
                                    dr["merge_type"] = rowspanType;
                                    this.TplLoopStyleInfo.Rows.Add(dr);
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
        }

        private ExcelRange[] GetFindCellValueEqualRanges(ExcelWorksheet worksheet, string value, int pageStartRow, int pageEndRow, int pageStartColumn, int pageEndColumn, HxMultiplePosition optPosition = HxMultiplePosition.All)
        {
            ExcelRange[] Result = null;
            if (worksheet != null && !value.IsNullOrWhiteSpaceEx())
            {
                if (pageStartRow <= 0 || pageStartColumn <= 0)
                if (pageStartRow <= 0)
                    pageStartRow = worksheet.Dimension.Start.Row;
                if (pageStartRow <= 0)
                    pageStartRow = 1;
                if (pageEndRow <= 0)
                    pageEndRow = worksheet.Dimension.End.Row;
                if (pageStartRow <= 0)
                    pageStartRow = _MAX_EXCEL_ROW_;

                if (pageStartColumn <= 0)
                    pageStartColumn = worksheet.Dimension.Start.Column;
                if (pageStartColumn <= 0)
                    pageStartColumn = 1;
                if (pageEndColumn <= 0)
                    pageEndColumn = worksheet.Dimension.End.Column;
                if (pageEndColumn <= 0)
                    pageEndColumn = _MAX_EXCEL_COL_;
                Regex rx = new Regex(HxTagTpl._DEF_TAG_ALL_PATTERN_, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                var selected = from ce in worksheet.Cells[pageStartRow, pageStartColumn, pageEndRow, pageEndColumn]
                               where rx.IsMatch(ce.Value.ToStringEx())
                               select ce;


                var query = (from ce in worksheet.Cells[pageStartRow, pageStartColumn, pageEndRow, pageEndColumn]
                             where ce.Value is string && (ce.Value.ToStringEx().ToLower().Contains(value) == true)
                             select ce
                            );
                if (query != null && query.Count() > 0)
                {
                    int n = query.Count();
                    switch (optPosition)
                    {
                        case HxMultiplePosition.First:
                            Result = new ExcelRange[1];
                            var firstQuery = query.First();
                            Result[0] = worksheet.Cells[firstQuery.Start.Row, firstQuery.Start.Column, firstQuery.End.Row, firstQuery.End.Column];
                            break;
                        case HxMultiplePosition.Last:
                            Result = new ExcelRange[1];
                            var lastQuery = query.Last();
                            Result[0] = worksheet.Cells[lastQuery.Start.Row, lastQuery.Start.Column, lastQuery.End.Row, lastQuery.End.Column];
                            break;
                        default:
                            Result = new ExcelRange[n];
                            int i = 0;
                            foreach (var currQuery in query)
                            {
                                Result[i] = worksheet.Cells[currQuery.Start.Row, currQuery.Start.Column, currQuery.End.Row, currQuery.End.Column];
                                i++;
                            }
                            break;
                    }
                    //var queryReulst = query.First();
                    //Result[0] = worksheet.Cells[queryReulst.Start.Row, queryReulst.Start.Column, queryReulst.End.Row, queryReulst.End.Column];
                }

            }
            return Result;
        }

        private ExcelRange[] GetFindCellValueRegxMatchRange(ExcelWorksheet worksheet, string value, HxMultiplePosition optPosition = HxMultiplePosition.All)
        {
            ExcelRange[] Result = null;
            if (worksheet != null && !value.IsNullOrWhiteSpaceEx())
            {
                HxExcelRangeNumberRec pageRange = GetWorksheetDimensionRange(worksheet);
                try
                {

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return Result;
        }
        private ExcelRange[] GetFindCellValueRegxMatchRange(ExcelWorksheet worksheet, string value, int pageStartRow, int pageStartColumn, int pageEndRow, int pageEndColumn, HxMultiplePosition optPosition = HxMultiplePosition.All)
        {
            ExcelRange[] Result = null;
            if (worksheet != null && !value.IsNullOrWhiteSpaceEx())
            {
                HxExcelRangeNumberRec pageRange = GetWorksheetDimensionRange(worksheet, pageStartRow, pageStartColumn, pageEndRow, pageEndColumn);
                try
                {

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return Result;
        }

        private ExcelRange[] GetFindCellValueRegxMatchRange(ExcelWorksheet worksheet, string value, HxExcelRangeNumberRec pageRange, HxMultiplePosition optPosition = HxMultiplePosition.All)
        {
            ExcelRange[] Result = null;
            if (worksheet != null && !value.IsNullOrWhiteSpaceEx())
            {
                try
                {

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return Result;
        }

        private List<ExcelRange> GetFindCellValueRangeList(ExcelWorksheet worksheet, string value, int pageStartRow, int pageEndRow, int pageStartColumn, int pageEndColumn, HxMultiplePosition optPosition = HxMultiplePosition.All)
        {
            ExcelRange[] tempVars = GetFindCellValueEqualRanges(worksheet, value, pageStartRow, pageEndRow, pageStartColumn, pageEndColumn, optPosition);
            if (tempVars != null && tempVars.Length > 0)
            {
                return tempVars.ToListEx();
            }
            return null;
        }
        private ExcelRange GetFindCellValueFirstRange(ExcelWorksheet worksheet, string value, int pageStartRow, int pageEndRow, int pageStartColumn, int pageEndColumn)
        {
            ExcelRange[] tempVars = GetFindCellValueEqualRanges(worksheet, value, pageStartRow, pageEndRow, pageStartColumn, pageEndColumn);
            if(tempVars != null && tempVars.Length > 0)
            {
                return tempVars.FirstOrDefault();
            }
            return null;
        }
        private ExcelRange GetFindCellValueLastRange(ExcelWorksheet worksheet, string value, int pageStartRow, int pageEndRow, int pageStartColumn, int pageEndColumn)
        {
            ExcelRange[] tempVars = GetFindCellValueEqualRanges(worksheet, value, pageStartRow, pageEndRow, pageStartColumn, pageEndColumn);
            if (tempVars != null && tempVars.Length > 0)
            {
                return tempVars.LastOrDefault();
            }
            return null;
        }

        private ExcelRange[] GetFindBlockVarRanges(ExcelWorksheet worksheet, string blockVarName, int pageStartRow, int pageEndRow, int pageStartColumn, int pageEndColumn, HxMultiplePosition optPosition = HxMultiplePosition.All)
        {
            string strBlockVarName = "{$" + blockVarName + "}";
            return GetFindCellValueEqualRanges(worksheet, strBlockVarName, pageStartRow, pageEndRow, pageStartColumn, pageEndColumn, optPosition);
        }
        private List<ExcelRange> GetFindBlockVarRangeList(ExcelWorksheet worksheet, string blockVarName, int pageStartRow, int pageEndRow, int pageStartColumn, int pageEndColumn, HxMultiplePosition optPosition = HxMultiplePosition.All)
        {
            ExcelRange[] tempVars = GetFindBlockVarRanges(worksheet, blockVarName, pageStartRow, pageEndRow, pageStartColumn, pageEndColumn, optPosition);
            if (tempVars != null && tempVars.Length > 0)
            {
                return tempVars.ToListEx();
            }
            return null;
        }

        private ExcelRange GetFindBlockVarFirstRange(ExcelWorksheet worksheet, string blockVarName, int pageStartRow, int pageEndRow, int pageStartColumn, int pageEndColumn)
        {
            ExcelRange[] tempVars = GetFindBlockVarRanges(worksheet, blockVarName, pageStartRow, pageEndRow, pageStartColumn, pageEndColumn);
            if (tempVars != null && tempVars.Length > 0)
            {
                return tempVars.FirstOrDefault();
            }
            return null;
        }
        private ExcelRange GetFindBlockVarLastRange(ExcelWorksheet worksheet, string value, int pageStartRow, int pageEndRow, int pageStartColumn, int pageEndColumn)
        {
            ExcelRange[] tempVars = GetFindBlockVarRanges(worksheet, value, pageStartRow, pageEndRow, pageStartColumn, pageEndColumn);
            if (tempVars != null && tempVars.Length > 0)
            {
                return tempVars.LastOrDefault();
            }
            return null;
        }

        private HxExcelRangeNumberRec GetWorksheetDimensionRange2(ExcelWorksheet worksheet, int pageStartRow = -1, int pageStartColumn = -1, int pageEndRow = -1, int pageEndColumn = -1)
        {
            if ((this.TplWorksheetInfo != null && this.TplWorksheetInfo.Rows.Count > 0) && (pageStartRow <= 0 || pageEndRow <= 0 || pageStartColumn <= 0 || pageEndColumn <= 0))
            {
                DataRow wsInfo = this.TplWorksheetInfo.Select(string.Format("{0} = '{1}'", _UDEF_WORKSHEET_NAME_, worksheet.Name))?.LastOrDefault();
                if (wsInfo != null)
                {
                    if (pageStartRow <= 0)
                        pageStartRow = wsInfo[_UDEF_START_ROW_].ToIntEx();
                    if (pageStartColumn <= 0)
                        pageStartColumn = wsInfo[_UDEF_START_COLUMN_].ToIntEx();
                    if (pageEndRow <= 0)
                        pageEndRow = wsInfo[_UDEF_END_ROW_].ToIntEx();
                    if (pageEndColumn <= 0)
                        pageEndColumn = wsInfo[_UDEF_END_COLUMN_].ToIntEx();
                }
            }

            if (pageStartRow <= 0 || pageStartColumn <= 0 || pageEndRow <= 0 || pageEndColumn <= 0)
            {
                HxExcelRangeNumberRec wsRange = HxExcelUtils.GetWorksheetDimensionRange(worksheet);
                if (pageStartRow <= 0)
                    pageStartRow = wsRange.StartRow;
                if (pageStartColumn <= 0)
                    pageStartColumn = wsRange.StartColumn;
                if (pageEndRow <= 0)
                    pageEndRow = wsRange.EndRow;
                if (pageEndColumn <= 0)
                    pageEndColumn = wsRange.EndColumn;
            }

            return new HxExcelRangeNumberRec { StartRow = pageStartRow, EndRow = pageEndRow, StartColumn = pageStartColumn, EndColumn = pageEndColumn };

        }
        
        private ExcelRange GetFindDefineAreaRange(ExcelWorksheet worksheet, string blockDefineAreaName, int pageStartRow = int.MinValue, int pageEndRow = int.MinValue, int pageColumn = int.MinValue)
        {
            ExcelRange Result = null;

            if((this.TplWorksheetInfo != null && this.TplWorksheetInfo.Rows.Count > 0) && (pageStartRow <=0 || pageEndRow <= 0 || pageColumn <= 0) )
            {
                DataRow wsInfo = this.TplWorksheetInfo.Select(string.Format("{0} = '{1}'", _UDEF_WORKSHEET_NAME_, worksheet.Name))?.LastOrDefault();
                if (wsInfo != null)
                {
                    if(pageStartRow <= 0)
                        pageStartRow = wsInfo[_UDEF_START_ROW_].ToIntEx();
                    if(pageEndRow <= 0)
                        pageEndRow = wsInfo[_UDEF_END_ROW_].ToIntEx();
                    if(pageColumn <= 0)
                        pageColumn = wsInfo[_UDEF_END_COLUMN_].ToIntEx();
                }
            }

            if (pageStartRow <= 0)
                pageStartRow = worksheet.Dimension.Start.Row;
            if (pageEndRow <= 0)
                pageEndRow = worksheet.Dimension.End.Row;
            if (pageColumn <= 0)
                pageColumn = worksheet.Dimension.End.Column;

            if (pageStartRow <= 0)
                pageStartRow = 1;
            if (pageEndRow <= 0)
                pageEndRow = _MAX_EXCEL_ROW_;
            if (pageColumn <= 0)
                pageColumn = _MAX_EXCEL_COL_;


            string strAreaBlockName = "{#" + blockDefineAreaName + "/}";
            string strAreaBlockName2 = "{#" + blockDefineAreaName + " /}";


            string strStartBlockName = "{#" + blockDefineAreaName + "}";
            string strEndBlockName = "{/" + blockDefineAreaName + "}";

            var queryBegins = (from ce in worksheet.Cells[pageStartRow, pageColumn, pageEndRow, pageColumn]
                               where ce.Value is string && (ce.Value.ToStringEx().ToLower().Contains(strStartBlockName) == true)
                               select ce
                        );
            if (queryBegins != null && queryBegins.Count() > 0)
            {
                var queryBegin = queryBegins.First();
                var queryEnds = (from ce in worksheet.Cells[pageStartRow, pageColumn, pageEndRow, pageColumn]
                                 where ce.Value is string && (ce.Value.ToStringEx().ToLower().Contains(strEndBlockName) == true)
                                 select ce
                        );
                if (queryEnds != null && queryEnds.Count() > 0)
                {
                    var queryEnd = queryEnds.Last();
                    Result = worksheet.Cells[queryBegin.Start.Row, queryBegin.Start.Column, queryEnd.End.Row, queryEnd.End.Column];
                }
            }

            return Result;
        }
    }
}
