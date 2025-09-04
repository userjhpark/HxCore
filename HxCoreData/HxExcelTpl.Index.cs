using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;

using HxCore;
using HxCore.Data;
using System.Linq;

namespace HxCore.Data
{
    partial class HxExcelTpl
    {
        #region Index타입 처리
        public DataTable GetExcelIndexTypeData(ExcelWorksheet worksheet = null)
        {
            DataTable Result = null;
            try
            {
                if (this.TplWorksheetInfo != null && this.TplWorksheetInfo.Rows.Count > 0)
                {
                    //string workspaceName = this.TplWorksheetInfo.Rows[0][_UDEF_WORKSHEET_NAME_].ToStringEx();
                    DataTable tagBlockData = this.GetExcelLoadIndexTagBlockData(worksheet);
                    if (tagBlockData != null && tagBlockData.Columns.Count > 0 && tagBlockData.Rows.Count > 0)
                    {
                        //DataRow[] rows = tagBlock.Select(string.Format("{0} = '{1}'", ));
                        Result = GetWorksheetIndexTypeData(tagBlockData);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public int GetFindBlockStartRowValue(DataTable tagBlock, string worksheetName)
        {
            if (tagBlock != null && worksheetName.IsNullOrWhiteSpaceEx() != true)
            {
                try
                {
                    var query = (
                                from tag in tagBlock.AsEnumerable()
                                where tag.Field<string>(_UDEF_WORKSHEET_NAME_).Equals(worksheetName) && tag.Field<string>(_UDEF_TAG_NAME_).ToLower().Equals(_TPL_DEF_START_ROW_.ToLower())
                                orderby tag.Field<string>(_UDEF_TAG_OPTION_VALUE_) descending
                                select tag
                                );

                    if (query != null)
                    {
                        var q = query.First();
                        return q.Field<string>(_UDEF_TAG_OPTION_VALUE_).ToIntEx();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    //throw ex;
                }
            }
            return -1;
        }

        private DataTable GetWorksheetIndexTypeData(DataTable tagBlockData)
        {
            DataTable Result = null;
            if(this.ExcelAppWorksheets != null && this.SourceWorksheetCount > 0)
            {
                Result = GetWorksheetIndexTypeData(SourceWorksheets[0], tagBlockData);
            }
            return Result;
        }

        private DataTable GetWorksheetIndexTypeData(ExcelWorksheet worksheet, DataTable tagBlockData)
        {
            DataTable Result = null;
            try
            {
                if (worksheet != null && tagBlockData != null && tagBlockData.Columns.Count > 0 && tagBlockData.Rows.Count > 0)
                {
                    //string mConfWhere = string.Format("{0} = '{1}'", _UDEF_WORKSHEET_NAME_, worksheet.Name);
                    //mConfWhere += string.Format(" AND {0} = '{1}'", _UDEF_TAG_CASE_, "&");
                    //mConfWhere += string.Format(" AND {0} = '{1}'", _UDEF_TAG_NAME_, HxTemplateBlockType.PageRange.ToStringEx());
                    //DataRow[] rowConf = tagBlock.Select(mConfWhere);
                    //DataTable dtConf = null; //= tagBlock.Clone();
                    //bool bConf = HxUtils.MergeDataRow(dtConf, rowConf);
                    HxExcelRangeNumberRec wsRange = this[worksheet.Name];

                    if (wsRange.EndRow > 0)
                    {
                        //string mStartRow = string.Format("{0} = '{1}'", _UDEF_WORKSHEET_NAME_, worksheet.Name);
                        //mStartRow += string.Format(" AND {0} = '{1}'", _UDEF_TAG_CASE_, "#");
                        //mStartRow += string.Format(" AND {0} = '{1}'", _UDEF_TAG_NAME_, "");

                        int startRow = this.GetFindBlockStartRowValue(tagBlockData, worksheet.Name);
                        if (startRow <= 0)
                            startRow = wsRange.StartRow;
                        if (startRow <= 0)
                            startRow = 1;

                        string mVarWhere = string.Format("{0} = '{1}'", _UDEF_WORKSHEET_NAME_, worksheet.Name);
                        mVarWhere += string.Format(" AND {0} = '{1}'", _UDEF_TAG_CASE_, "$");
                        DataRow[] rowVars = tagBlockData.Select(mVarWhere);
                        DataTable dtVar = null; //= tagBlock.Clone();
                        bool bVar = HxUtils.MergeDataRow(ref dtVar, rowVars);
                        if (dtVar != null && bVar == true)
                        {
                            Result = new DataTable();
                            Dictionary<string, int> varColumnIndex = new Dictionary<string, int>();
                            foreach (DataRow dr in dtVar.Rows)
                            {
                                string colName = dr[_UDEF_TAG_NAME_].ToStringEx().ToLower();
                                int colIndex = dr[_UDEF_START_COLUMN_].ToIntEx();
                                if (Result.Columns.Contains(colName) != true)
                                {
                                    Result.Columns.Add(colName);
                                    varColumnIndex.AddEx(colName, colIndex);
                                }
                            }

                            for (int iRow = startRow; iRow <= wsRange.EndRow; iRow++)
                            {
                                DataRow row = Result.NewRow();
                                bool bInput = false;
                                foreach (KeyValuePair<string, int> pair in varColumnIndex)
                                {
                                    string colName = pair.Key.ToLower();
                                    int colIndex = pair.Value.ToIntEx();
                                    if (colName.IsNullOrWhiteSpaceEx() != true && Result.Columns.Contains(colName) && colIndex > 0)
                                    {
                                        object value = worksheet.Cells[iRow, colIndex]?.Value;
                                        string text = worksheet.Cells[iRow, colIndex]?.Text;
                                        string formula = worksheet.Cells[iRow, colIndex]?.Formula;
                                        string value2 = worksheet.Cells[iRow, colIndex]?.GetValue<string>();
                                        string formulaR1C1 = worksheet.Cells[iRow, colIndex]?.FormulaR1C1;
                                        //var formulaR1C1 = worksheet.Cells[iRow, colIndex].;
                                        row[colName] = text;
                                        bInput = true;
                                    }
                                }
                                if (bInput == true)
                                {
                                    Result.Rows.Add(row);
                                }
                                //for (int iCol = wsRange.StartColumn; iRow <= wsRange.EndColumn; iCol++)
                                //{

                                //}
                            }
                        }
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

        public DataTable GetExcelLoadIndexTagBlockData(ExcelWorksheet worksheet = null)
        {
            DataTable Result = null;
            try
            {
                bool bSuccess = false;
                if (worksheet == null)
                {
                    bSuccess = DoExcelLoadIndexTagBlock(ref Result);
                }
                else
                {
                    bSuccess = DoWorksheetLoadIndexTagBlock(worksheet, ref Result);
                }
                if (!bSuccess)
                    Result = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public IEnumerable<HxExcelTplBlock> GetExcelLoadIndexTagBlockRecords(ExcelWorksheet worksheet = null)
        {
            IEnumerable<HxExcelTplBlock> Result = null;
            DataTable tagBlockData = GetExcelLoadIndexTagBlockData(worksheet);
            if (tagBlockData != null && tagBlockData.Columns.Count > 0 && tagBlockData.Rows.Count > 0)
            {
                Result = tagBlockData.ToRecordSetEx<HxExcelTplBlock>();
            }
            return Result;
        }
        protected virtual bool DoExcelLoadIndexTagBlock(ref DataTable tagBlockInfo)
        {
            bool Result = false;
            try
            {
                foreach (ExcelWorksheet sheet in this.ExcelAppWorksheets)
                {
                    bool bWorked = DoWorksheetLoadIndexTagBlock(sheet, ref tagBlockInfo);
                    if (bWorked == true && Result != true)
                    {
                        Result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Result = false;
                Debug.WriteLine(ex);
                //throw;
            }

            return Result;
        }


        protected virtual bool DoWorksheetLoadIndexTagBlock(ExcelWorksheet worksheet, ref DataTable tagBlockInfo)
        {
            bool Result = false;
            try
            {
                if (worksheet != null)
                {
                    string worksheet_name = worksheet.Name;
                    if (tagBlockInfo == null || tagBlockInfo?.Columns?.Count <= 0)
                    {
                        //data = new DataTable();
                        tagBlockInfo = CreateTplTagBlockDataTable();
                    }
                    if (tagBlockInfo != null)
                    {
                        if (!tagBlockInfo.Columns.Contains(_UDEF_WORKSHEET_NAME_))
                        {
                            tagBlockInfo.Columns.Add(new DataColumn(_UDEF_WORKSHEET_NAME_, typeof(string)));
                        }
                        try
                        {
                            HxExcelRangeNumberRec sheetAreaLimit = GetWorksheetTplPageRange(worksheet);
                            HxExcelRangeNumberRec findAreaLimit = new HxExcelRangeNumberRec(1, 1, 1, sheetAreaLimit.EndColumn);
                            DataTable dt = GetWorkseetFindTagBlockDataTable(worksheet, findAreaLimit, HxTagTpl._DEF_TAG_ALL_PATTERN_);
                            Result = HxUtils.MergeDataTable(tagBlockInfo, dt);
                            //Result = true;
                        }
                        catch (Exception ex)
                        {
                            Result = false;
                            Debug.WriteLine(ex);
                            throw ex;
                        }
                    }

                    //ExcelCellAddress 
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
            return Result;
        }
        #endregion
    }
}
