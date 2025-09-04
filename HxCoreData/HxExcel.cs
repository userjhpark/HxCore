using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HxCore.Data
{
    public class HxExcel : HxBase
    {
        //LIMIT : 1,048,576 rows by 16,384 columns
        //https://support.office.com/en-us/article/excel-specifications-and-limits-1672b34d-7043-467e-8e27-269d656771c3#ID0EBABAAA=Office_2007

        /// <summary>
        /// LIMIT : MAX ROWS
        /// </summary>
        public const int _MAX_EXCEL_ROW_ = 65535; //1048576
        /// <summary>
        /// LIMIT : MAX COLUMNS
        /// </summary>
        public const int _MAX_EXCEL_COL_ = 16384; //16,384 

        //public const string _TAG_PATTERN_ = @"(?:{)(#|\$|@|\/)(\w+)(?:\:(\d))?(\/)?(?:})";
        //const string _TAG_PATTERN_ = @"(?:{)(#|\$|@|\/)(\w+)(?:\:(\d))?(\/)?(?:})";
        //const string _TAG_PATTERN_ = @"^(?:{)(#|\$|@|\/)(\w+)(?:\:(\d))?(\/)?(?:})$";

        #region Static / Excel Utils
        /// <summary>
        /// Worksheet 사용 범위 가져오기
        /// </summary>
        /// <param name="worksheet">Excel Worksheet Resource</param>
        /// <param name="pageStartRow">시작 ROW</param>
        /// <param name="pageStartColumn">시작 COLUMN</param>
        /// <param name="pageEndRow">끝 ROW</param>
        /// <param name="pageEndColumn">끝 COLUMN</param>
        /// <returns>범위</returns>
        public static HxExcelRangeNumberRec GetWorksheetDimensionRange(ExcelWorksheet worksheet, int pageEndRow = -1, int pageEndColumn = -1, int pageStartRow = -1, int pageStartColumn = -1)
        {
            //int pageStartRow = -1;
            //int pageEndRow = -1;
            //int pageStartColumn = -1;
            //int pageEndColumn = -1;
            if (worksheet != null && worksheet.Dimension != null)
            {
                if (pageStartRow <= 0)
                    pageStartRow = worksheet.Dimension.Start.Row;
                if (pageStartColumn <= 0)
                    pageStartColumn = worksheet.Dimension.Start.Column;

                if (pageEndRow <= 0)
                    pageEndRow = worksheet.Dimension.End.Row;
                if (pageEndColumn <= 0)
                    pageEndColumn = worksheet.Dimension.End.Column;
            }
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
        /// <summary>
        /// Worksheet 사용 범위 가져오기
        /// </summary>
        /// <param name="worksheet">Excel Worksheet Resource</param>
        /// <param name="pageRange">[Struct]Range Number Type</param>
        /// <returns>범위</returns>
        public static HxExcelRangeNumberRec GetWorksheetDimensionRange(ExcelWorksheet worksheet, HxExcelRangeNumberRec pageRange)
        {
            return new HxExcelRangeNumberRec { StartRow = pageRange.StartRow, EndRow = pageRange.EndRow, StartColumn = pageRange.StartColumn, EndColumn = pageRange.EndColumn };
        }
        /// <summary>
        /// Worksheet 사용 범위 가져오기
        /// </summary>
        /// <param name="worksheet">Excel Worksheet Resource</param>
        /// <param name="endAddress">End - Cell Address Resource</param>
        /// <param name="startAddress">Start - Cell Address Resource</param>
        /// <returns>범위</returns>
        public static HxExcelRangeNumberRec GetWorksheetDimensionRange(ExcelWorksheet worksheet, ExcelAddress endAddress, ExcelAddress startAddress = null)
        {
            HxExcelRangeNumberRec Result;
            int pageStartRow = -1;
            int pageEndRow = -1;
            int pageStartColumn = -1;
            int pageEndColumn = -1;
            if (endAddress != null)
            {
                pageEndRow = endAddress.End.Row;
                pageEndColumn = endAddress.End.Column;
            }
            if(startAddress != null)
            {
                pageStartRow = startAddress.Start.Row;
                pageStartColumn = startAddress.End.Column;
            }
            Result = GetWorksheetDimensionRange(worksheet, pageEndRow, pageEndColumn, pageStartRow, pageStartColumn);
            return Result;
        }
        /// <summary>
        /// Worksheet 사용 범위 가져오기
        /// </summary>
        /// <param name="worksheet">Excel Worksheet Resource</param>
        /// <param name="strEndAddress">End - Cell Address String</param>
        /// <param name="strStartAddress">Start - Cell Address String</param>
        /// <returns>범위</returns>
        public static HxExcelRangeNumberRec GetWorksheetDimensionRange(ExcelWorksheet worksheet, string strEndAddress, string strStartAddress = null)
        {
            HxExcelRangeNumberRec Result;
            int pageStartRow = -1;
            int pageEndRow = -1;
            int pageStartColumn = -1;
            int pageEndColumn = -1;
            if (!strEndAddress.IsNullOrWhiteSpaceEx())
            {
                ExcelAddress endAddress = new ExcelAddress(strEndAddress);
                pageEndRow = endAddress.End.Row;
                pageEndColumn = endAddress.End.Column;
            }
            if (!strStartAddress.IsNullOrWhiteSpaceEx())
            {
                ExcelAddress startAddress = new ExcelAddress(strStartAddress);
                pageStartRow = startAddress.Start.Row;
                pageStartColumn = startAddress.End.Column;
            }
            Result = GetWorksheetDimensionRange(worksheet, pageEndRow, pageEndColumn, pageStartRow, pageStartColumn);
            return Result;
        }

        [Obsolete("미 구현!")]
        internal static List<MatchCollection> GetFindCellRegexMatch(ExcelWorksheet worksheet, HxExcelRangeNumberRec cellRangeNumber, string findPattern = null, RegexOptions optRegexOptions = (RegexOptions.IgnoreCase | RegexOptions.Multiline))
        {
            if (worksheet == null) return null;

            if (findPattern.IsNullOrWhiteSpaceEx())
            {
                findPattern = HxTagTpl._DEF_TAG_ALL_PATTERN_;
            }
            Regex rx = new Regex(findPattern, optRegexOptions);

            ExcelRange cells = worksheet.Cells[cellRangeNumber.StartRow, cellRangeNumber.StartColumn, cellRangeNumber.EndRow, cellRangeNumber.EndColumn];
            IEnumerable<ExcelRangeBase> findCells = cells?.Where( r => rx.IsMatch( r.Value.ToStringEx() ) );

            /*
            var query = from ce in worksheet.Cells[cellRangeNumber.StartRow, cellRangeNumber.StartColumn, cellRangeNumber.EndRow, cellRangeNumber.EndColumn]
                        where rx.IsMatch(ce.Value.ToStringEx())
                        select ce;
            */
            if (findCells != null && findCells.Any() == true)
            {
                List<MatchCollection> Result = new List<MatchCollection>();
                foreach(ExcelRange cell in findCells)
                {

                    if (cell == null && cell.Value == null) continue;
                    MatchCollection matches = HxString.RegexMatches(cell.Value.ToStringEx(), findPattern, optRegexOptions);
                    //#warning 여기서 부터....구현 필요
                }
                return Result;
            }

            return null;
        }

        /// <summary>
        /// Cell의 Value값을 정규식으로 찾은 ExcelRange 목록(Array)
        /// </summary>
        /// <param name="worksheet">Excel Worksheet Resource</param>
        /// <param name="findPattern">찾을 패턴</param>
        /// <param name="cellRange">찾을 대상 Cell 영역</param>
        /// <param name="optMultiPosition">다중 찾기 옵션</param>
        /// <param name="optRegexOptions">정규식 옵션</param>
        /// <returns>찾은 ExcelRange Array</returns>
        public static ExcelRange[] GetFindCellRegexMatchRanges(ExcelWorksheet worksheet, string findPattern, HxExcelRangeNumberRec cellRange, HxMultiplePosition optMultiPosition = HxMultiplePosition.All, RegexOptions optRegexOptions = (RegexOptions.IgnoreCase | RegexOptions.Multiline))
        {
            ExcelRange[] Result = null;
            if (worksheet != null)
            {
                if (findPattern.IsNullOrWhiteSpaceEx())
                {
                    findPattern = HxTagTpl._DEF_TAG_ALL_PATTERN_;
                }
                Regex rx = new Regex(findPattern, optRegexOptions);
                var query = from ce in worksheet.Cells[cellRange.StartRow, cellRange.StartColumn, cellRange.EndRow, cellRange.EndColumn]
                               where rx.IsMatch(ce.Value.ToStringEx())
                               select ce;
                if (query != null && query.Count() > 0)
                {
                    int n = query.Count();
                    switch (optMultiPosition)
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
        /// <summary>
        /// Cell의 Value값을 정규식으로 찾은 ExcelRange 목록(List)
        /// </summary>
        /// <param name="worksheet">Excel Worksheet Resource</param>
        /// <param name="findPattern">찾을 패턴</param>
        /// <param name="cellRange">찾을 대상 Cell 영역</param>
        /// <param name="optMultiPosition">다중 찾기 옵션</param>
        /// <param name="optRegexOptions">정규식 옵션</param>
        /// <returns>찾은 ExcelRange List</returns>
        public static List<ExcelRange> GetFindCellRegexMatchRangeList(ExcelWorksheet worksheet, string findPattern, HxExcelRangeNumberRec cellRange, HxMultiplePosition optMultiPosition = HxMultiplePosition.All, RegexOptions optRegexOptions = (RegexOptions.IgnoreCase | RegexOptions.Multiline))
        {
            List<ExcelRange> Result = null;
            ExcelRange[] ranges = GetFindCellRegexMatchRanges(worksheet, findPattern, cellRange, optMultiPosition, optRegexOptions);
            if (ranges != null && ranges.Length > 0)
            {
                Result = ranges.ToListEx();
            }
            return Result;
        }
        /// <summary>
        /// Cell의 Value값과 일치하는 ExcelRange 목록
        /// </summary>
        /// <param name="worksheet">Excel Worksheet Resource</param>
        /// <param name="findPattern">찾을 패턴</param>
        /// <param name="cellRange">찾을 대상 Cell 영역</param>
        /// <param name="optMultiPosition">다중 찾기 옵션</param>
        /// <param name="optRegexOptions">정규식 옵션</param>
        /// <returns>찾은 ExcelRange Array</returns>
        public static ExcelRange[] GetFindCellValueMatchRanges(ExcelWorksheet worksheet, string value, HxExcelRangeNumberRec cellRange, HxMultiplePosition optMultiPosition = HxMultiplePosition.All, RegexOptions optRegexOptions = RegexOptions.IgnoreCase)
        {
            if (worksheet != null && value != null) {
                string findPattern = value;

                if (!findPattern.TrimStart().StartsWith("^"))
                {
                    findPattern = "^" + findPattern;
                }
                if (!findPattern.TrimEnd().EndsWith("$"))
                {
                    findPattern += "$";
                }
                return GetFindCellRegexMatchRanges(worksheet, findPattern, cellRange, optMultiPosition, optRegexOptions);
            }
            return null;
        }
        /// <summary>
        /// Cell의 Value값을 정규식으로 찾은 첫번째(First) ExcelRange
        /// </summary>
        /// <param name="worksheet">Excel Worksheet Resource</param>
        /// <param name="findPattern">찾을 패턴</param>
        /// <param name="cellRange">찾을 대상 Cell 영역</param>
        /// <param name="optRegexOptions">정규식 옵션</param>
        /// <returns>찾은 첫번째(First) ExcelRange</returns>
        public static ExcelRange GetFindCellRegexMatchFirstRange(ExcelWorksheet worksheet, string findPattern, HxExcelRangeNumberRec cellRange, RegexOptions optRegexOptions = (RegexOptions.IgnoreCase | RegexOptions.Multiline))
        {
            HxMultiplePosition optMultiPosition = HxMultiplePosition.First;
            ExcelRange[] ranges = GetFindCellRegexMatchRanges(worksheet, findPattern, cellRange, optMultiPosition, optRegexOptions);
            if(ranges != null && ranges.Length > 0)
            {
                return ranges.First();
            }
            return null;
        }
        /// <summary>
        /// Cell의 Value값을 정규식으로 찾은 마지막(Last) ExcelRange
        /// </summary>
        /// <param name="worksheet">Excel Worksheet Resource</param>
        /// <param name="findPattern">찾을 패턴</param>
        /// <param name="cellRange">찾을 대상 Cell 영역</param>
        /// <param name="optRegexOptions">정규식 옵션</param>
        /// <returns>찾은 마지막(Last) ExcelRange</returns>
        public static ExcelRange GetFindCellRegexMatchLastRange(ExcelWorksheet worksheet, string findPattern, HxExcelRangeNumberRec cellRange, RegexOptions optRegexOptions = (RegexOptions.IgnoreCase | RegexOptions.Multiline))
        {
            HxMultiplePosition optMultiPosition = HxMultiplePosition.Last;
            ExcelRange[] ranges = GetFindCellRegexMatchRanges(worksheet, findPattern, cellRange, optMultiPosition, optRegexOptions);
            if (ranges != null && ranges.Length > 0)
            {
                return ranges.Last();
            }
            return null;
        }
        
        #endregion


        protected string LoadUserFileName
        {
            get;
            private set;
        }
        protected FileInfo ExcelLoadFileInfo
        {
            get;
            private set;
        }

        protected bool IsSaveOverWrite
        {
            get;
            private set;
        }

        protected string SaveUserFileName
        {
            get;
            private set;
        }

        protected FileInfo ExcelSaveFileInfo
        {
            get;
            private set;
        }

        public ExcelPackage ExcelApp
        {
            get;
            protected set;
        }
        public ExcelWorkbook ExcelAppWorkbook
        {
            get
            {
                if (this.ExcelApp != null)
                    return this.ExcelApp.Workbook;
                else
                    return null;
            }
        }
        public ExcelWorksheets ExcelAppWorksheets
        {
            get
            {
                if (this.ExcelAppWorkbook != null)
                    return this.ExcelAppWorkbook.Worksheets;
                else
                    return null;
            }
        }

        public HxExcel(string openFileName, string saveFileName = null, bool isOverWrite = false)
        {
            //this.excelPackage = new OfficeOpenXml.ExcelPackage();
            this.LoadUserFileName = openFileName;
            this.SaveUserFileName = saveFileName;
            this.IsSaveOverWrite = isOverWrite;
            this.CreateExcelPackage(openFileName);
        }

        public HxExcel(Stream loadStream, string saveFileName = null, bool isOverWrite = false)
        {
            this.LoadUserFileName = null;
            this.SaveUserFileName = saveFileName;
            this.IsSaveOverWrite = isOverWrite;
            this.CreateExcelPackage(loadStream);
        }


        protected override void FreeAndNull()
        {
            try
            {
                if (ExcelApp != null)
                {
                    ExcelApp.Dispose();
                    ExcelApp = null;
                }
            }
            catch (Exception)
            {

                //throw;
            }
            
            base.FreeAndNull();
        }

        private bool CreateExcelPackage(string fileName, string password = null)
        {

            bool Result = false;

            try
            {
                this.ExcelLoadFileInfo = null;
                this.ExcelSaveFileInfo = null;
                //this.excelPackage.lo
                FileInfo xlsxFileInfo = new FileInfo(fileName);
                if (xlsxFileInfo.Exists && (xlsxFileInfo.Extension == ".xlsx" || xlsxFileInfo.Extension == ".xltx"))
                {
                    this.ExcelLoadFileInfo = xlsxFileInfo;
                    this.ExcelSaveFileInfo = null;
                    try
                    {
                        this.ExcelApp = new ExcelPackage(xlsxFileInfo, true, password);
                        
                        //this.ExcelApp = new ExcelPackage(xlsxFileInfo.OpenRead(), password);
                        
                        //using (FileStream stream = new FileStream(xlsxFileInfo.FullName, FileMode.Open))
                        //{
                        //    //this.ExcelApp = new ExcelPackage();
                        //    ExcelApp.Load(stream, password);
                        //}
                    }
                    catch (IOException ex)
                    {
                        throw ex;
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }

                }
                //System.Reflection.Assembly assm;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Result;
        }

        private bool CreateExcelPackage(Stream loadStream, string password = null)
        {
            bool Result = false;
            this.ExcelLoadFileInfo = null;
            this.ExcelSaveFileInfo = null;
            try
            {
                this.ExcelApp = new ExcelPackage(loadStream, password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public override void Dispose()
        {
            if (this.ExcelApp != null)
            {
                this.ExcelApp.Dispose();
            }
            base.Dispose();
        }

        public void DoExcelSave(string saveFileName = null)
        {
            if (this.ExcelApp != null)
            {
                string fileName;//= this.SaveUserFileName;
                if (!saveFileName.IsNullOrWhiteSpaceEx() || !this.SaveUserFileName.IsNullOrWhiteSpaceEx())
                {
                    if (!saveFileName.IsNullOrWhiteSpaceEx())
                    {
                        fileName = saveFileName;
                    }
                    else
                    {
                        fileName = this.SaveUserFileName;
                    }
                    FileInfo fiSave = new FileInfo(fileName);
                    this.ExcelApp.SaveAs(fiSave);
                    ExcelSaveFileInfo = fiSave;
                    //System.Diagnostics.Process.Start(fi.FullName);

                }
                else
                {
                    this.ExcelApp.Save();
                    ExcelSaveFileInfo = ExcelApp.File;
                }
            }
        }


        
    }
}
