using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace HxCore.Data
{
    partial class HxExcelTpl
    {
        #region Static / Excel Template Utils
        public static DataTable CreateTplWorksheetDataTable(string tableName = null)
        {
            if (tableName.IsNullOrWhiteSpaceEx())
                tableName = "Template Worksheets";
            DataTable Result = new DataTable(tableName);
            Result.Columns.AddRange(new DataColumn[]{
                    new DataColumn{ ColumnName = _UDEF_NO_ , DataType = typeof(int), Unique = true, AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 }
                    , new DataColumn{ ColumnName = _UDEF_WORKSHEET_NAME_ , DataType = typeof(string) }
                    , new DataColumn{ ColumnName = _UDEF_BLOCK_NAME_ , DataType = typeof(string), DefaultValue = _TPL_DEF_PAGE_RANGE_}
                    , new DataColumn{ ColumnName = _UDEF_BLOCK_TYPE_, DataType = typeof(HxTemplateBlockType), DefaultValue = HxTemplateBlockType.PageRange }
                    , new DataColumn{ ColumnName = _UDEF_START_ROW_, DataType = typeof(int) }
                    , new DataColumn{ ColumnName = _UDEF_START_COLUMN_, DataType = typeof(int) }
                    , new DataColumn{ ColumnName = _UDEF_END_ROW_, DataType = typeof(int) }
                    , new DataColumn{ ColumnName = _UDEF_END_COLUMN_, DataType = typeof(int) }

                    , new DataColumn{ ColumnName = _UDEF_START_ADDR_ }
                    , new DataColumn{ ColumnName = _UDEF_END_ADDR_ }

                    , new DataColumn{ ColumnName = _UDEF_TAG_COLOR_, DataType = typeof(System.Drawing.Color), DefaultValue = System.Drawing.Color.Transparent }
                    , new DataColumn{ ColumnName = _UDEF_REMARK_, DataType = typeof(string) }
                });
            return Result;
        }
        public static DataTable CreateTplTagBlockDataTable(string tableName = null)
        {
            if (tableName.IsNullOrWhiteSpaceEx())
                tableName = "Template Blocks";
            DataTable Result = new DataTable(tableName);
            Result.Columns.AddRange(new DataColumn[]{
                    new DataColumn{ ColumnName = _UDEF_NO_ , DataType = typeof(int), Unique = true, AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1 }
                    , new DataColumn{ ColumnName = _UDEF_WORKSHEET_NAME_ , DataType = typeof(string)}
                    , new DataColumn{ ColumnName = _UDEF_BLOCK_NAME_ , DataType = typeof(string), DefaultValue = HxTemplateBlockType.ItemVar.ToStringEx()}
                    , new DataColumn{ ColumnName = _UDEF_BLOCK_TYPE_, DataType = typeof(HxTemplateBlockType), DefaultValue = HxTemplateBlockType.ItemVar }
                    , new DataColumn{ ColumnName = _UDEF_START_ROW_, DataType = typeof(int) }
                    , new DataColumn{ ColumnName = _UDEF_START_COLUMN_, DataType = typeof(int) }
                    , new DataColumn{ ColumnName = _UDEF_END_ROW_, DataType = typeof(int) }
                    , new DataColumn{ ColumnName = _UDEF_END_COLUMN_, DataType = typeof(int) }

                    , new DataColumn{ ColumnName = _UDEF_START_ADDR_ }
                    , new DataColumn{ ColumnName = _UDEF_END_ADDR_ }
                    
                    , new DataColumn{ ColumnName = _UDEF_TAG_INPUT_, DataType = typeof(string) }
                    , new DataColumn{ ColumnName = _UDEF_TAG_PATTERN_, DataType = typeof(string) }
                    , new DataColumn{ ColumnName = _UDEF_TAG_VALUE_, DataType = typeof(string) }
                    , new DataColumn{ ColumnName = _UDEF_TAG_CASE_, DataType = typeof(string) }
                    , new DataColumn{ ColumnName = _UDEF_TAG_NAME_, DataType = typeof(string) }
                    , new DataColumn{ ColumnName = _UDEF_TAG_OUTPUT_, DataType = typeof(string) }
                    , new DataColumn{ ColumnName = _UDEF_TAG_OPTION_VALUE_, DataType = typeof(string) }
                    , new DataColumn{ ColumnName = _UDEF_TAG_OPTION_ROW_, DataType = typeof(int) }
                    , new DataColumn{ ColumnName = _UDEF_TAG_OPTION_COL_, DataType = typeof(int) }

                    , new DataColumn{ ColumnName = _UDEF_START_DEL_ROW_, DataType = typeof(int) }
                    , new DataColumn{ ColumnName = _UDEF_END_DEL_ROW_, DataType = typeof(int) }

                    , new DataColumn{ ColumnName = _UDEF_PAGE_TOTAL_COUNT_, DataType = typeof(int), Caption = "Page Total Count"}
                    , new DataColumn{ ColumnName = _UDEF_PAGE_ITEM_COUNT_, DataType = typeof(int), Caption = "Page of Item Count"}
                    , new DataColumn{ ColumnName = _UDEF_PAGE_BLANK_COUNT_, DataType = typeof(int), Caption = "Last Page Balnk Row Count"}
                    , new DataColumn{ ColumnName = _UDEF_REMARK_, DataType = typeof(string) }
                });
            return Result;
        }

        public static ExcelRange[] GetWorkseetFindTagBlockRanges(ExcelWorksheet worksheet, HxExcelRangeNumberRec cellRange, string findPattern = _TPL_TAG_PATTERN_, RegexOptions optRegexOptions = (RegexOptions.IgnoreCase | RegexOptions.Multiline))
        {
            ExcelRange[] Result = null;
            if (worksheet != null)
            {
                if (findPattern.IsNullOrWhiteSpaceEx())
                {
                    findPattern = _TPL_TAG_PATTERN_;
                }
                Result = GetFindCellRegexMatchRanges(worksheet, findPattern, cellRange, HxMultiplePosition.All, optRegexOptions);
            }
            return Result;
        }

        public static DataTable GetWorkseetFindTagBlockDataTable(ExcelWorksheet worksheet, HxExcelRangeNumberRec cellRange, string findPattern = _TPL_TAG_PATTERN_, RegexOptions optRegexOptions = (RegexOptions.IgnoreCase | RegexOptions.Multiline))
        {
            DataTable Result = null;
            try
            {
                if (worksheet != null)
                {
                    string worksheetName = worksheet.Name;
                    Result = CreateTplTagBlockDataTable(worksheetName);

                    ExcelRange[] ranges = GetWorkseetFindTagBlockRanges(worksheet, cellRange, findPattern, optRegexOptions);
                    if (ranges != null && ranges.Length > 0)
                    {
                        DataRow pageRangeRow = Result.NewRow();
                        pageRangeRow[_UDEF_WORKSHEET_NAME_] = worksheetName;
                        pageRangeRow[_UDEF_BLOCK_NAME_] = HxTemplateBlockType.PageRange.ToStringEx();
                        pageRangeRow[_UDEF_BLOCK_TYPE_] = HxTemplateBlockType.PageRange;
                        pageRangeRow[_UDEF_START_ROW_] = cellRange.StartRow;
                        pageRangeRow[_UDEF_START_COLUMN_] = cellRange.StartColumn;
                        pageRangeRow[_UDEF_END_ROW_] = cellRange.EndRow;
                        pageRangeRow[_UDEF_END_COLUMN_] = cellRange.EndColumn;
                        pageRangeRow[_UDEF_START_ADDR_] = cellRange.StartAddress;
                        pageRangeRow[_UDEF_END_ADDR_] = cellRange.EndAddress;
                        //row[_UDEF_TAG_INPUT_] = HxTemplateBlockType.PageRange.ToStringEx();
                        //row[_UDEF_TAG_VALUE_] = HxTemplateBlockType.PageRange.ToStringEx();
                        pageRangeRow[_UDEF_TAG_CASE_] = "&";
                        pageRangeRow[_UDEF_TAG_NAME_] = HxTemplateBlockType.PageRange.ToStringEx();
                        Result.Rows.Add(pageRangeRow);

                        foreach (ExcelRange range in ranges)
                        {
                            string input = worksheet.Cells[range.Address].Value.ToStringEx();
                            if (!input.IsNullOrWhiteSpaceEx())
                            {
                                MatchCollection matches = Regex.Matches(input, findPattern, optRegexOptions);
                                foreach (Match match in matches)
                                {
                                    HxTagTplRec tagTplRec = new HxTagTplRec(match.Value, findPattern);

                                    DataRow dr = Result.NewRow();

                                    dr[_UDEF_WORKSHEET_NAME_] = worksheetName;
                                    dr[_UDEF_BLOCK_NAME_] = HxTemplateBlockType.ItemVar.ToStringEx();
                                    dr[_UDEF_BLOCK_TYPE_] = HxTemplateBlockType.ItemVar;

                                    dr[_UDEF_START_ROW_] = range.Start.Row;
                                    dr[_UDEF_START_COLUMN_] = range.Start.Column;
                                    dr[_UDEF_END_ROW_] = range.End.Row;
                                    dr[_UDEF_END_COLUMN_] = range.End.Column;
                                    dr[_UDEF_START_ADDR_] = range.Start.Address;
                                    dr[_UDEF_END_ADDR_] = range.End.Address;

                                    dr[_UDEF_TAG_INPUT_] = tagTplRec.InputText;
                                    dr[_UDEF_TAG_PATTERN_] = tagTplRec.TagPattern;
                                    dr[_UDEF_TAG_VALUE_] = tagTplRec.Value;
                                    dr[_UDEF_TAG_CASE_] = tagTplRec.VarCase;
                                    dr[_UDEF_TAG_NAME_] = tagTplRec.VarName;
                                    dr[_UDEF_TAG_OUTPUT_] = tagTplRec.ReplaceText;
                                    dr[_UDEF_TAG_OPTION_VALUE_] = tagTplRec.VarOption;
                                    dr[_UDEF_TAG_OPTION_ROW_] = tagTplRec.OptionRow;
                                    dr[_UDEF_TAG_OPTION_COL_] = tagTplRec.OptionColumn;

                                    Result.Rows.Add(dr);
                                }
                            }
                        }
                    }
                }
                if (Result != null)
                {
                    Result.TableName = worksheet.Name;
                    Result.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return Result;
        }

        //public 

        #endregion
    }
}
