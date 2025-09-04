using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HxCore;
using System.Diagnostics;

namespace HxCore.Win.PDFXE
{
    partial class HxPDFXE
    {
        /// <summary>
        /// Enum (Selection Menu / Comment / Event) ID Array 리스트
        /// </summary>
        public int[] nIDS;
        /// <summary>
        /// Get StringID to NumberID
        /// </summary>
        /// <param name="value">String ID</param>
        /// <returns>Number ID</returns>
        public int? GetStr2NumberID(string value)
        {
            try
            {
                return AxPXVCtl.Inst.Str2ID(value);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //return null;
                //throw ex;
            }
            return null;
        }
        /// <summary>
        /// Get NumberID to StringID
        /// </summary>
        /// <param name="value">Number ID</param>
        /// <returns>String ID</returns>
        public string GetNumber2StrID(int? value)
        {
            try
            {
                if (!value.IsNullOrZeroMinEx())
                {
                    return AxPXVCtl.Inst.ID2Str((int)value);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //throw ex;
            }
            return null;
        }

        /// <summary>
        /// Get IDS(Enum) Array List
        /// </summary>
        private int[] GetIDSArray()
        {
            int[] Result = new int[(int)HxIDS._last_];
            for (HxIDS i = 0; i < HxIDS._last_; i++)
            {
                string sid = Enum.GetName(typeof(HxIDS), i);
                if (sid[0] == '_') // skip all like '_op_first_', '_op_last_', '_e_first_', etc..
                {
                    Result[(int)i] = 0;
                    continue;
                }
                sid = sid.Replace('_', '.');
                Result[(int)i] = AxPXVCtl.Inst.Str2ID(sid);
            }
            return Result;
        }
        /// <summary>
        /// GET IDS(Enum)항목의 Command String ID
        /// </summary>
        /// <param name="value">Enum IDS</param>
        /// <returns>String ID</returns>
        public string GetIDS2StrID(HxIDS value)
        {
            string Result = null;
            string strID = Enum.GetName(typeof(HxIDS), value);
            if (strID[0] != '_') // skip all like '_op_first_', '_op_last_', '_e_first_', etc..
            {
                strID = strID.Replace('_', '.');
            }
            return Result;
        }
        /// <summary>
        /// GET IDS(Enum)항목의 Command Number ID
        /// </summary>
        /// <param name="value">Enum IDS</param>
        /// <returns>Number ID</returns>
        public int? GetIDS2NumberID(HxIDS value)
        {
            string strID = GetIDS2StrID(value);
            if (strID.IsNullOrWhiteSpaceEx())
            {
                return (int)AxPXVCtl.Inst.Str2ID(strID);
            }
            return null;
        }
        
        /// <summary>
        /// PDFXE에 이벤트 등록 또는 해제
        /// </summary>
        /// <param name="nID">Number ID</param>
        /// <param name="bRegister">Register?</param>
        public void SetRegisterEvents(int nID, bool bRegister)
        {
            AxPXVCtl.EnableEventListening2(nID, bRegister);
        }
        /// <summary>
        /// PDFXE에 이벤트 등록 또는 해제
        /// </summary>
        /// <param name="strID">String ID</param>
        /// <param name="bRegister">Register?</param>
        public void SetRegisterEvents(string strID, bool bRegister)
        {
            int? nID = this.GetStr2NumberID(strID);
            if (!nID.IsNullOrMinValueEx())
            {
                this.SetRegisterEvents((int)nID, bRegister);
            }
        }
        /// <summary>
        /// PDFXE에 IDS(Enum)에서 정의된 Event 등록 또는 해제
        /// </summary>
        /// <param name="bRegister">Boolean</param>
        public void SetRegisterEvents(bool bRegister)
        {
            if(this.nIDS.Length <= 0)
            {
                this.nIDS = this.GetIDSArray();
            }
            for (HxIDS i = HxIDS._e_begin_ + 1; i < HxIDS._e_end_; i++)
            {
                //PXVCtl.EnableEventListening2(nIDS[(int)i], bRegister);
                this.SetRegisterEvents(nIDS[(int)i], bRegister);
            }
        }
        



    }
}
