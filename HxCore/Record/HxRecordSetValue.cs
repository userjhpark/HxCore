using System;
using System.Collections.Generic;
using System.Text;

namespace HxCore
{
    using Newtonsoft.Json;

    using System.Data;
    using System.Diagnostics;

    public struct HxSetValueRecord : IHxSetValueRecord<HxSetValueRecord>
    {
        #region **기본 Const & Fields**
        public const string _CDF_UNO_           = "uno";
        public const string _CDF_CNO_           = "cno";
        public const string _CDF_IS_USE_        = "is_use";
        public const string _CDF_REG_DATE_      = "reg_date";
        public const string _CDF_MOD_DATE_      = "mod_date";
        public const string _CDF_REG_AGENT_     = "reg_agent";
        public const string _CDF_MOD_AGENT_     = "mod_agent";
        public const string _CDF_REG_USER_      = "reg_user";
        public const string _CDF_MOD_USER_      = "mod_user";
        public const string _CDF_REG_UNO_       = "reg_uno";
        public const string _CDF_MOD_UNO_       = "mod_uno";

        [JsonProperty(_CDF_IS_USE_)] public string IS_USE { get; set; }
        [JsonProperty(_CDF_REG_DATE_)] public DateTime? REG_DATE { get; set; }
        [JsonProperty(_CDF_MOD_DATE_)] public DateTime? MOD_DATE { get; set; }
        [JsonProperty(_CDF_REG_AGENT_)] public string REG_AGENT { get; set; }
        [JsonProperty(_CDF_MOD_AGENT_)] public string MOD_AGENT { get; set; }
        [JsonProperty(_CDF_REG_USER_)] public string REG_USER { get; set; }
        [JsonProperty(_CDF_MOD_USER_)] public string MOD_USER { get; set; }
        [JsonProperty(_CDF_REG_UNO_)] public decimal? REG_UNO { get; set; }
        [JsonProperty(_CDF_MOD_UNO_)] public decimal? MOD_UNO { get; set; }

        public void SetMatchFieldValueByStdBase(string name, object value)
        {
            switch (name)
            {
                case _CDF_IS_USE_       : IS_USE        = value.ToStringEx(); break;
                case _CDF_REG_DATE_     : REG_DATE      = value.ToNullableDateTimeEx(); break;
                case _CDF_MOD_DATE_     : MOD_DATE      = value.ToNullableDateTimeEx(); break;
                case _CDF_REG_AGENT_    : REG_AGENT     = value.ToStringEx(); break;
                case _CDF_MOD_AGENT_    : MOD_AGENT     = value.ToStringEx(); break;
                case _CDF_REG_USER_     : REG_USER      = value.ToStringEx(); break;
                case _CDF_MOD_USER_     : MOD_USER      = value.ToStringEx(); break;
                case _CDF_REG_UNO_      : REG_UNO       = value.ToNullableDecimalEx(); break;
                case _CDF_MOD_UNO_      : MOD_UNO       = value.ToNullableDecimalEx(); break;
                default:
                    break;
            }
        }
        public void CreateStdBy(string iS_USE, DateTime? rEG_DATE, DateTime? mOD_DATE, string rEG_AGENT, string mOD_AGENT, string rEG_USER, string mOD_USER, decimal? rEG_UNO, decimal? mOD_UNO)
        {
            IS_USE      = iS_USE;
            REG_DATE    = rEG_DATE;
            MOD_DATE    = mOD_DATE;
            REG_AGENT   = rEG_AGENT;
            MOD_AGENT   = mOD_AGENT;
            REG_USER    = rEG_USER;
            MOD_USER    = mOD_USER;
            REG_UNO     = rEG_UNO;
            MOD_UNO     = mOD_UNO;
        }
        public void CopyDataByStdBase(HxSetValueRecord value)
        {
            this.IS_USE     = value.IS_USE;
            //this.RAW_GUID = param.RAW_GUID;
            this.REG_DATE   = value.REG_DATE;
            this.REG_AGENT  = value.REG_AGENT;
            this.REG_UNO    = value.REG_UNO;
            this.REG_USER   = value.REG_USER;
            this.MOD_DATE   = value.MOD_DATE;
            this.MOD_AGENT  = value.MOD_AGENT;
            this.MOD_UNO    = value.MOD_UNO;
            this.MOD_USER   = value.MOD_USER;
        }
        public void ClearByStdBase()
        {
            IS_USE        = null;
            REG_DATE      = null;
            MOD_DATE      = null;
            REG_AGENT     = null;
            MOD_AGENT     = null;
            REG_USER      = null;
            MOD_USER      = null;
            REG_UNO       = null;
            MOD_UNO       = null;
        }
        #endregion
        #region **기본 생성자 & Methods**
        public HxSetValueRecord(string iS_USE, DateTime? rEG_DATE, DateTime? mOD_DATE, string rEG_AGENT, string mOD_AGENT, string rEG_USER, string mOD_USER, decimal? rEG_UNO, decimal? mOD_UNO)
        {
            IS_USE = iS_USE;
            REG_DATE = rEG_DATE;
            MOD_DATE = mOD_DATE;
            REG_AGENT = rEG_AGENT;
            MOD_AGENT = mOD_AGENT;
            REG_USER = rEG_USER;
            MOD_USER = mOD_USER;
            REG_UNO = rEG_UNO;
            MOD_UNO = mOD_UNO;
        }

        public void SetMatchFieldValue(string name, object value)
        {
            switch (name)
            {
                default:
                    SetMatchFieldValueByStdBase(name, value);
                    break;
            }
        }
        public void CopyData(HxSetValueRecord value)
        {
            CopyData(value);
        }
        public void Clear()
        {
            ClearByStdBase();
        }
        #endregion

        #region 생성자
        public HxSetValueRecord(bool bInit = false)
        {
            IS_USE = null;
            REG_DATE = null;
            MOD_DATE = null;
            REG_AGENT = null;
            MOD_AGENT = null;
            REG_USER = null;
            MOD_USER = null;
            REG_UNO = null;
            MOD_UNO = null;
        }

        public HxSetValueRecord(DataView dv, int index = 0)
            : this(true)
        {
            this.SetValue(dv, index);
        }
        public HxSetValueRecord(DataTable dt, int index = 0)
            : this(true)
        {
            this.SetValue(dt, index);
        }

        public HxSetValueRecord(DataRow dr)
           : this(true)
        {
            this.SetValue(dr);
        }
        #endregion
        #region 공통 Property 및 Methods
        public string COL_CUSTOM_USER_AGENT
        {
            get => HxSetValueObject.CUSTOM_USER_AGENT;
        }
        public void SetValue(DataRow row)
        {
            if (row != null && row.Table.Columns.Count > 0)
            {
                DataTable dt = row.Table;
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        string name = dc.ColumnName.ToLower();
                        object value = row[name];
                        SetMatchFieldValue(name, value);
                    }
                }
            }
        }
        public void SetValue(DataTable dt, int index = 0)
        {
            if (dt != null && index >= 0)
            {
                int nRow = dt.Rows.Count;
                if (index >= nRow)
                {
                    index = nRow - 1;
                }
                DataRow row = dt.Rows[index];
                SetValue(row);
            }
        }
        public void SetValue(DataView dv, int index = 0)
        {
            if (dv != null && index >= 0)
            {
                int nRow = dv.Count;
                if (index >= nRow)
                {
                    index = nRow - 1;
                }
                DataRowView rowv = dv[index];
                DataRow dr = rowv.Row;
                SetValue(dr);
            }
        }
        public string GetCustomUserAgentString(IHxDb db)
        {
            string Result = null;
            try
            {
                if (db != null && db.Open())
                {
                    string SQL = string.Format("SELECT {0} FROM DUAL", COL_CUSTOM_USER_AGENT);
                    Result = db.QueryOne(SQL).ToStringEx();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw ex;
            }
            return Result;
        }
        //public abstract string _QUERY_OBJECT_ { get; }
        internal static DataTable GetQueryDataTable(IHxDb db, string queryString, string pWhere = null, Dictionary<string, object> pBind = null)
        {
            DataTable Result = null;
            try
            {
                if (db != null)
                {
                    if (pBind == null)
                        pBind = new Dictionary<string, object>();
                    if (queryString.IsNullOrWhiteSpaceEx() != true)
                    {
                        string SQL = GetQueryString(queryString, pWhere);
                        Result = db.QueryDataTable(SQL, pBind);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw ex;
            }
            return Result;
        }
        public static string GetQueryString(string queryString, string pWhere)
        {
            return HxUtils.SelectQueryString(queryString, pWhere);
        }

        public static string GetSelectQueryString(string queryString, string pWhere)
        {
            return HxUtils.SelectQueryString(queryString, pWhere);
        }
        #endregion
        
    }
}
