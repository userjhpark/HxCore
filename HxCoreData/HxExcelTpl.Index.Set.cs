using OfficeOpenXml;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace HxCore.Data
{
    partial class HxExcelTpl
    {
        #region Index타입 처리
        public bool SetExcelIndexTypeData(DataTable tplData, ExcelWorksheet worksheet = null, bool isDeleteRowBlockSettings = false)
        {
            if (tplData == null || tplData.Rows.Count <= 0) return false;

            if (this.TplWorksheetInfo != null && this.TplWorksheetInfo.Rows.Count > 0)
            {
                try
                {
                    int nDataRow = tplData.Rows.Count;
                    if (worksheet == null)
                    {
                        worksheet = this.SourceWorksheets[0];
                    }
                    DataTable tagBlockData = this.GetExcelLoadIndexTagBlockData(worksheet);
                    string worksheetName = worksheet.Name;
                    IEnumerable<HxExcelTplBlock> rsTagBlockRecords = tagBlockData.ToRecordSetEx<HxExcelTplBlock>()?.Where(r => r.WORKSHEET_NAME == worksheetName);
                    //IEnumerable<HxExcelTplBlock> rsTagBlockRecords = this.GetExcelLoadIndexTagBlockRecords(worksheet)?.Where(r => r.WORKSHEET_NAME == worksheetName);
                    if (rsTagBlockRecords != null && rsTagBlockRecords.Any())
                    {
                        var qPageRange = rsTagBlockRecords.Where(r => r.BLOCK_TYPE == HxTemplateBlockType.PageRange && r.TAG_CASE == "&" && r.TAG_NAME.ToLower() == _TPL_DEF_PAGE_RANGE_.ToLower());
                        if (qPageRange == null || qPageRange.Any() != true) return false;

                        //qPageRange.FirstOrDefault().col
                        #region 반복할 행 시작 위치 찾기
                        var qStartRow = rsTagBlockRecords.Where(r => r.BLOCK_TYPE == HxTemplateBlockType.ItemVar && r.TAG_CASE == "#" && r.TAG_NAME.ToLower() == _TPL_DEF_START_ROW_.ToLower());
                        if (qStartRow == null || qStartRow.Any() != true) return false;
                        string strStartRowOptionValue = qStartRow.FirstOrDefault()?.TAG_OPTION_VALUE;
                        if (strStartRowOptionValue.IsNullOrWhiteSpaceEx() == true) return false;
                        string[] arrStartRowOptionValue = strStartRowOptionValue.SplitEx(",");

                        int? iStartRowPos = arrStartRowOptionValue?.FirstOrDefault().ToStringEx().Trim().ToIntEx();
                        if (iStartRowPos.IsNullOrZeroMinorEx() == true) return false;

                        int nStartRowCount = arrStartRowOptionValue.Length <= 1 ? 1 : arrStartRowOptionValue.LastOrDefault().ToStringEx().Trim().ToIntEx(1);
                        #endregion

                        string strTplTagPattern = HxTagTpl._DEF_TAG_ALL_PATTERN_;

                        int iPageRow = qPageRange.FirstOrDefault().START_ROW.ToIntEx(1);
                        int nPageCol = qPageRange.FirstOrDefault().END_COLUMN.ToIntEx(_MAX_EXCEL_COL_);

                        HxExcelRangeNumberRec recPageRange = new HxExcelRangeNumberRec(iPageRow, 0, iPageRow, nPageCol);
                        ExcelRange cellPageRange = worksheet.Cells[recPageRange.AreaAddress.Address];
                        int iRow = 0;
                        int iCopySourceRow = iStartRowPos.ToIntEx(0);
                        for (int index = 0; index < nDataRow; index++)
                        {
                            iRow = iStartRowPos.ToIntEx(0) + index;
                            iCopySourceRow = iRow + nStartRowCount;
                            worksheet.InsertRow(iRow, nStartRowCount, iCopySourceRow);
                            worksheet.Row(iRow).Height = worksheet.Row(iCopySourceRow).Height;
                            #region 설정된 값으로 치환
                            for (int iCol = 1; iCol <= nPageCol; iCol++)
                            {
                                bool bFormula = false;
                                ExcelRange cellSetting = worksheet.Cells[iPageRow, iCol];
                                ExcelRange cellContent = worksheet.Cells[iRow, iCol];
                                //object cellValue = cell.Value;
                                //string strCellFormula = cell.Formula;
                                string strCellText = cellSetting.Text;
                                string strValue = strCellText;
                                if (strValue.IsNullOrWhiteSpaceEx() != true)
                                {
                                    if (strValue.StartsWith("="))
                                    {
                                        bFormula = true;
                                    }
                                    HxTagTplRec[] finds = HxTagTpl.GetTagMatches(strValue, iRow, iCol, strTplTagPattern);
                                    if (finds == null || finds.Length <= 0) continue;

                                    foreach (var find in finds)
                                    {
                                        bool bExistFindColumn = tplData.Columns.Contains(find.VarName);
                                        string strFindVarName = find.Value.RegexReplaceEx("^({{\\$)", "{{\\$");
                                        string strFindColName = find.VarName;
                                        string strFindRowValue = null;
                                        
                                        if (find.VarName.IsNullOrWhiteSpaceEx() == true) continue;

                                        if (find.VarCase == "$")
                                        {
                                            if (tplData.CaseSensitive == true && bExistFindColumn == false)
                                            {
                                                foreach (DataColumn dc in tplData.Columns)
                                                {
                                                    strFindColName = dc.ColumnName;
                                                    if (strFindColName.ToLower() == find.VarName.ToLower())
                                                    {
                                                        bExistFindColumn = true;
                                                        break;
                                                    }
                                                }
                                            }

                                        }

                                        if (bExistFindColumn == true)
                                        {
                                            strFindRowValue = tplData.Rows[index][strFindColName].ToStringEx();
                                        }


                                        if (find.VarCase != "$" || bExistFindColumn != true)
                                        {
                                            strFindRowValue = string.Empty;
                                        }

                                        //strValue = strValue.RegexReplaceEx(input, strRowData, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                                        strValue = strValue.RegexReplaceEx(strFindVarName, strFindRowValue, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Multiline);
                                    }

                                    //var v = HxUtils.replace
                                    if (bFormula == true)
                                    {
                                        cellContent.Formula = strValue;
                                    }
                                    else
                                    {
                                        if(strValue.Trim().RegexReplaceEx("\\r|\\n", string.Empty) == "0")
                                        {
                                            strValue = "-";
                                        }
                                        cellContent.Value = (object)strValue;
                                    }
                                }

                            }
                            #endregion
                        }

                        #region 불필요한 TPL항목 정리(ROW 삭제 등)
                        if (iCopySourceRow > iRow)
                        {
                            worksheet.DeleteRowEx(iCopySourceRow);
                        }
                        if (isDeleteRowBlockSettings == true && qPageRange != null && qPageRange.Any())
                        {
                            try
                            {
                                //ExcelRange cells = worksheet.Cells[qPageRange.FirstOrDefault().START_ROW.ToIntEx(1), qPageRange.FirstOrDefault().START_COLUMN.ToIntEx(1), qPageRange.FirstOrDefault().END_ROW.ToIntEx(1), qPageRange.FirstOrDefault().END_COLUMN.ToIntEx(_MAX_EXCEL_COL_)];
                                worksheet.DeleteRowEx(iPageRow, nPageCol);
                            }
                            catch (Exception exDelRow)
                            {
                                Debug.WriteLine(exDelRow);
                                //throw exDelRow;
                            }

                        }
                        //worksheet.Select("C1");
                        #endregion

                        return true;
                        /*
                        var props = HxUtils.PropertyInfoList(rsTagBlockRecords?.FirstOrDefault());
                        foreach (DataColumn dc in tplData.Columns)
                        {
                            var qItem = rsTagBlockRecords.Where(r => r.BLOCK_TYPE == HxTemplateBlockType.ItemVar && r.TAG_CASE == "$" && r.TAG_NAME.ToLower() == dc.ColumnName.ToLower());
                            if (qItem != null && qItem.Any())
                            {
                                Debug.WriteLine(qItem.FirstOrDefault().START_ROW);
                            }
                            var q = props.Where(r => r.Name.ToLower() == dc.ColumnName.ToLower());
                            if(q != null && q.Any())
                            {

                            }
                        }
                        */
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return false;
        }
        #endregion
    }

}