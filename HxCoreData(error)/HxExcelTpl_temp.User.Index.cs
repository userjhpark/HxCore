using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text;

using HxCore;
using System.Data;

namespace HxCore.Data
{
    partial class HxExcelTpl_temp
    {
        #region Index Type
        public DataSet GetIndexTypeToDataTable(DataTable structDataTable)
        {
            DataSet Result = null;
            if (this.NormalSourceWorksheets == null && this.NormalSourceWorksheets.Count > 0)
            {
                DoFindSourceSheets();
            }
            if (this.NormalSourceWorksheets != null && this.NormalSourceWorksheets.Count > 0)
            {

            }
            return Result;
        }
        #endregion

        public void LoadIndex(DataTable indexData, bool bNotExistAppend = true)
        {
            //if (this.AssignVars.Count > 0 || (this.AssignDataSet != null && this.AssignDataSet.Tables.Count > 0))
            //{
            if (NormalSourceWorksheets.Count > 0)
            {
                

                foreach (ExcelWorksheet ws in NormalSourceWorksheets)
                {
                    string workSheetName = ws.Name;
                    //this.SetPrintWorksheetOld(ws);
                    //this.DoPrintWorksheet(ws);
                    this.DoPrintIndexWorksheet(ws, indexData, bNotExistAppend);
                }
            }
            //}
        }
        private void DoPrintIndexWorksheet(ExcelWorksheet worksheet, DataTable indexData, bool bNotExistAppend = true)
        {
            this.DoPrintWorksheet(worksheet);

            if (indexData == null)
                indexData = new DataTable();
            if (indexData.TableName.IsNullOrWhiteSpaceEx())
                indexData.TableName = "Index List";

            var v = GetFindDefineAreaRange(worksheet, "aa");
            
            //this.TplPageInfo
        }

        
    }
}
