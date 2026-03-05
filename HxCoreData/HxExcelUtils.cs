using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace HxCore.Data
{
    public class HxExcelUtils : HxUtils
    {
        public static HxExcelRangeNumberRec GetWorksheetDimensionRange(ExcelWorksheet worksheet, int pageStartRow = -1, int pageStartColumn = -1, int pageEndRow = -1, int pageEndColumn = -1)
        {
            //int pageStartRow = -1;
            //int pageEndRow = -1;
            //int pageStartColumn = -1;
            //int pageEndColumn = -1;
            if(worksheet != null && worksheet.Dimension != null)
            if (pageStartRow <= 0)
                pageStartRow = worksheet.Dimension.Start.Row;
            if (pageEndRow <= 0)
                pageEndRow = worksheet.Dimension.End.Row;
            if (pageStartColumn <= 0)
                pageStartColumn = worksheet.Dimension.Start.Column;
            if (pageEndColumn <= 0)
                pageEndColumn = worksheet.Dimension.End.Column;
            //if (pageStartRow <= 0)
            //    pageStartRow = 1;
            //if (pageEndRow <= 0)
            //    pageEndRow = HxExcel._MAX_EXCEL_ROW_;
            //if (pageStartColumn <= 0)
            //    pageStartColumn = 1;
            //if (pageEndColumn <= 0)
            //    pageEndColumn = HxExcel._MAX_EXCEL_COL_;

            return new HxExcelRangeNumberRec { StartRow = pageStartRow, EndRow = pageEndRow, StartColumn = pageStartColumn, EndColumn = pageEndColumn };
        }
        public static HxExcelRangeNumberRec GetWorksheetDimensionRange(ExcelWorksheet worksheet, HxExcelRangeNumberRec pageRange)
        {
            return new HxExcelRangeNumberRec { StartRow = pageRange.StartRow, EndRow = pageRange.EndRow, StartColumn = pageRange.StartColumn, EndColumn = pageRange.EndColumn };
        }

        private static ExcelRange[] GetFindValueRange(ExcelWorksheet worksheet, HxExcelRangeNumberRec pageRange, bool bRegexMatch = true)
        {
            ExcelRange[] Result = null;
            if (worksheet != null)
            {
                pageRange = GetWorksheetDimensionRange(worksheet, pageRange);
                try
                {

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            } else
            {
                //Result = new ExcelRange[0];
            }
            return Result;
        }

        public static bool DeleteRow(ExcelWorksheet worksheet, HxExcelRangeNumberRec pageRange)
        {
            try
            {
                ExcelRange cells = worksheet.Cells[pageRange.StartRow, pageRange.StartColumn, pageRange.EndRow, pageRange.EndColumn];
                if (cells != null && cells.Count() > 0)
                {
                    foreach (var cell in cells)
                    {
                        var cmt = cell.Comment;
                        if (cmt != null)
                        {
                            worksheet.Comments.Remove(cmt);
                        }
                    }
                    for (int i = pageRange.EndRow; i >= pageRange.StartRow; i--)
                    {
                        worksheet.DeleteRow(i);
                    }
                    return true;
                }
            }
            catch (Exception exDelRow)
            {
                Debug.WriteLine(exDelRow);
                throw exDelRow;
            }
            return false;
        }
        public static bool DeleteRow(ExcelWorksheet worksheet, ExcelRange cells)
        {
            try
            {
                if (cells != null && cells.Count() > 0)
                {
                    foreach (var cell in cells)
                    {
                        var cmt = cell.Comment;
                        if (cmt != null)
                        {
                            worksheet.Comments.Remove(cmt);
                        }
                    }
                    for (int i = cells.End.Row; i >= cells.Start.Row; i--)
                    {
                        worksheet.DeleteRow(i);
                    }
                    return true;
                }
            }
            catch (Exception exDelRow)
            {
                Debug.WriteLine(exDelRow);
                throw exDelRow;
            }
            return false;
        }
        public static bool DeleteRow(ExcelWorksheet worksheet, int Row, int EndCol = HxExcel._MAX_EXCEL_COL_)
        {
            HxExcelRangeNumberRec pageRange = new HxExcelRangeNumberRec(Row, 1, Row, EndCol);
            return DeleteRow(worksheet, pageRange);
        }

        public static DataTable GetIndexTypeData(string excelFileName)
        {
            DataTable Result = null;
            if (excelFileName.IsNullOrWhiteSpaceEx() == true || HxFile.FileExists(excelFileName) != true) return Result;

            try
            {
                string filePath = HxFile.GetLongFileName(excelFileName);
                using (HxExcelTpl excel = new HxExcelTpl(filePath))
                {
                    if (excel != null && excel.ExcelApp != null && excel.SourceWorksheetCount > 0)
                    {
                        Result = excel.GetExcelIndexTypeData();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            return Result;
        }
    }
}
