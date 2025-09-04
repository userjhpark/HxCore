using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HxCore.Data
{
    public struct HxExcelRangeNumberRec
    {
        public int StartRow;
        public int StartColumn;
        public int EndRow;
        public int EndColumn;
        public ExcelAddress StartAddress { get => GetStartAddress(); }
        public ExcelAddress EndAddress { get => GetEndAddress(); }
        public ExcelAddress AreaAddress { get => GetAreaAddress(); }
        public HxExcelRangeNumberRec(bool bInit = true)
        {
            StartRow = -1;
            EndRow = -1;
            StartColumn = -1;
            EndColumn = HxExcel._MAX_EXCEL_COL_;
            if (bInit == true)
            {
                //StartRow = 1;
                //EndRow = HxExcel._MAX_EXCEL_ROW_;
                //StartColumn = 1;
                //EndColumn = HxExcel._MAX_EXCEL_COL_;
                SetValue();
            }
        }
        public HxExcelRangeNumberRec(int startRow = 0, int startColumn = 0, int endRow = HxExcel._MAX_EXCEL_ROW_, int endColumn = HxExcel._MAX_EXCEL_COL_)
            : this()
        {
            SetValue(startRow, startColumn, endRow, endColumn);
        }

        public void SetValue(int startRow = 0, int startColumn = 0, int endRow = HxExcel._MAX_EXCEL_ROW_, int endColumn = HxExcel._MAX_EXCEL_COL_)
        {
            StartRow = startRow;
            StartColumn = startColumn;
            EndRow = endRow;
            EndColumn = endColumn;
            if (StartRow <= 0)
                StartRow = 1;
            if (StartColumn <= 0)
                StartColumn = 1;
            if (EndRow <= 0)
                EndRow = HxExcel._MAX_EXCEL_ROW_;
            if (EndColumn <= 0)
                EndColumn = HxExcel._MAX_EXCEL_COL_;
        }

        private ExcelAddress GetStartAddress()
        {
            return new ExcelAddress(StartRow, StartColumn, StartRow, StartColumn);
        }
        private ExcelAddress GetEndAddress()
        {
            return new ExcelAddress(EndRow, EndColumn, EndRow, EndColumn);
        }
        private ExcelAddress GetAreaAddress()
        {
            return new ExcelAddress(StartRow, StartColumn, EndRow, EndColumn);
        }


    }
}
