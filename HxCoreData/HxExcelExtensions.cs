using OfficeOpenXml;

using System;
using System.Collections.Generic;
using System.Text;

namespace HxCore.Data
{
    public static partial class HxExcelExtensions
    {
        /// <summary>
        /// Delete the specified row from the worksheet. (with Comments)
        /// </summary>
        /// <param name="worksheet">ExcelWorksheet Resource</param>
        /// <param name="Row">A row to be deleted</param>
        /// <returns></returns>
        public static bool DeleteRowEx(this ExcelWorksheet worksheet, int Row, int EndCol = HxExcel._MAX_EXCEL_COL_)
        {
            return HxExcelUtils.DeleteRow(worksheet, Row, EndCol);
        }
    }
}
