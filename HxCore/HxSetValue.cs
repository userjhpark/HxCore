using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace HxCore
{

    public abstract class HxSetValue : IHxSetValue
    {

        public virtual void SetValue(string name, object value, BindingFlags flags = (BindingFlags.Public | BindingFlags.Instance), bool bIgnoreCaseToUpperOrToLower = true, bool bThrowException = false)
        {
            HxUtils.SetMemberPropertyValue(this, name, value, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
        }

        public abstract void SetValue(DataRow row);

        public abstract void SetValue(DataTable data, int rowIndex = 0);
    }

    public abstract class HxSetValueObject : IHxSetValue
    {
        public const string _CUSTOM_USER_AGENT_ = "'[' || SYS_CONTEXT('USERENV', 'IP_ADDRESS', 15) || ']HOST:' || sys_context('USERENV','HOST') || '/OS_USER:' || SYS_CONTEXT('USERENV','OS_USER') || '/MODULE:' || SYS_CONTEXT('USERENV','module')";
        //public static string CUSTOM_USER_AGENT => "'[' || SYS_CONTEXT('USERENV', 'IP_ADDRESS', 15) || ']HOST:' || sys_context('USERENV','HOST') || '/OS_USER:' || SYS_CONTEXT('USERENV','OS_USER') || '/MODULE:' || SYS_CONTEXT('USERENV','module')";
        public static string CUSTOM_USER_AGENT => _CUSTOM_USER_AGENT_;


        public HxSetValueObject()
        {
            ; ;
        }

        public HxSetValueObject(DataView dv, int index = 0)
            : this()
        {
            this.SetValue(dv, index);
        }
        public HxSetValueObject(DataTable dt, int index = 0)
            : this()
        {
            this.SetValue(dt, index);
        }

        public HxSetValueObject(DataRow dr)
           : this()
        {
            this.SetValue(dr);
        }

        

        public virtual void SetValue(DataRow row)
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

        public virtual void SetValue(DataTable dt, int index = 0)
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
        public virtual void SetValue(DataView dv, int index = 0)
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
        //public abstract void SetMatchFieldValue(string name, object value);
        public virtual void SetMatchFieldValue(string name, object value)
        {
            SetMemberFieldValue(name, value);
        }


        protected static DataTable GetQueryDataTable(IHxDb db, string pQueryString, string pWhere = null, Dictionary<string, object> pBind = null, string pOrderBy = null)
        {
            return GetQueryDataTable(db, pQueryString, pWhere, pBind, pOrderBy, null);
        }
        //public abstract string _QUERY_OBJECT_ { get; }
        protected static DataTable GetQueryDataTable(IHxDb db, string pQueryString, string pWhere, Dictionary<string, object> pBind, string pOrderBy, string pTableNameStr = null)
        {
            DataTable Result = null;
            try
            {
                if (pBind == null)
                    pBind = new Dictionary<string, object>();
                if (pQueryString.IsNullOrWhiteSpaceEx() != true)
                {
                    string SQL = GetQueryString(pQueryString, pWhere);

                    if(pOrderBy.IsNullOrWhiteSpaceEx() != true)
                    {
                        SQL = HxUtils.OrderByQueryString(SQL, pOrderBy);
                    }
                    Result = GetQueryDataTable(db, SQL, pBind);
                    if(Result != null && pTableNameStr.IsNullOrWhiteSpaceEx() != true)
                    {
                        Result.TableName = pTableNameStr;
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
        protected static DataTable GetQueryDataTable(IHxDb db, string queryString, Dictionary<string, object> pBind = null)
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
                        string SQL = queryString.Replace("\r\n", Environment.NewLine);
                        SQL = SQL.Replace(Environment.NewLine, "\n");
                        SQL = SQL.Replace("\t", "    ");
                        //string SQL = queryString.ToStringEx().Trim().RegexReplaceEx("\\r\\n", "\n", System.Text.RegularExpressions.RegexOptions.Multiline);
                        Result = db.QueryDataTable(SQL, pBind, false);
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

        public static string SelectQueryString(string queryString, string mWhere)
        {
            return HxUtils.SelectQueryString(queryString, mWhere);
        }
        public static string GetQueryString(string queryString, string pWhere)
        {
            return HxUtils.SelectQueryString(queryString, pWhere);
        }

        public static string GetSelectQueryString(string queryString, string pWhere)
        {
            return HxUtils.SelectQueryString(queryString, pWhere);
        }

        public static string OrderByQueryString(string queryString, string mOrderBy)
        {
            return HxUtils.OrderByQueryString(queryString, mOrderBy);
        }

        protected virtual object GetCustomPropertyValue(string name, bool bIgnoreCaseToUpperOrToLower = true, bool bThrowException = false)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            return HxUtils.GetCustomPropertyValue(this, name, null, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
        }

        protected virtual bool SetMemberFieldValue(string name, object value, bool bIgnoreCaseToUpperOrToLower = true, bool bThrowException = false)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            return HxUtils.SetMemberPropertyValue(this, name, value, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
            /**
            //bool Result = false;
            //try
            //{
            //    var v = this;
            //    if (v != null)
            //    {
            //        var memberField = v.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            //        if (bIgnoreCaseToUpperOrToLower == true && memberField == null)
            //        {
            //            memberField = v.GetType().GetProperty(name.ToUpper(), BindingFlags.Public | BindingFlags.Instance);
            //            if (memberField == null)
            //            {
            //                memberField = v.GetType().GetProperty(name.ToLower(), BindingFlags.Public | BindingFlags.Instance);
            //            }
            //        }
            //        if (memberField != null)
            //        {
            //            Type t = Nullable.GetUnderlyingType(memberField.PropertyType) ?? memberField.PropertyType;
            //            object safeValue = (value == null) ? null : Convert.ChangeType(value, t);

            //            if(value == null)
            //            {
            //                safeValue = null;
            //            }
            //            else if(t == typeof(Nullable<DateTime>))
            //            {
            //                safeValue = value.ToNullableDateTimeEx();
            //            }
            //            else if (t == typeof(Nullable<decimal>))
            //            {
            //                safeValue = value.ToNullableDecimalEx();
            //            }
            //            else if (t == typeof(Nullable<long>))
            //            {
            //                safeValue = value.ToNullableLongEx();
            //            }
            //            else if (t == typeof(Nullable<int>))
            //            {
            //                safeValue = value.ToNullableIntEx();
            //            }
            //            else
            //            {
            //                //safeValue = (value == null) ? null : Convert.ChangeType(value, t);
            //                safeValue = Convert.ChangeType(value, t);
            //            }

            //            var member = memberField.GetValue(v);
            //            memberField.SetValue(v, safeValue);
            //            //member = memberField.GetValue(v);
            //            /*
            //            // no flags necessary for a public property
            //            var prop = member.GetType().GetProperty(name);
            //            if (prop == null)
            //            {
            //                prop = member.GetType().GetProperty(name?.ToUpper());
            //            }
            //            if (prop != null)
            //            {
            //                prop.SetValue(member, value);
            //            }
            //            * /
            //            Result = true;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex);
            //    if (bThrowException == true)
            //    {
            //        throw ex;
            //    }
            //}
            //return Result;
            * */
        }

        protected virtual bool SetJsonPropertyValue(string name, object value, bool bIgnoreCaseToUpperOrToLower = true, bool bThrowException = false)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            return HxUtils.SetJsonPropertyValue(this, name, value, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
            /**
            //bool Result = false;
            //try
            //{
            //    var v = this;
            //    if (v != null)
            //    {
            //        var props = v.GetType().GetProperties(BindingFlags.Instance | BindingFlags.SetProperty);
            //        foreach (PropertyInfo prop in props)
            //        {
            //            foreach (object attr in prop.GetCustomAttributes(true))
            //            {
            //                JsonPropertyAttribute customAttr = (attr as JsonPropertyAttribute);
            //                if(customAttr != null)
            //                {
            //                    if (customAttr.PropertyName == name)
            //                    {
            //                        Result = SetMemberPropertyValue(prop.Name, value, bIgnoreCaseToUpperOrToLower, bThrowException);
            //                    }
            //                    else if (customAttr.PropertyName == name.ToUpper())
            //                    {
            //                        Result = SetMemberPropertyValue(prop.Name, value, bIgnoreCaseToUpperOrToLower, bThrowException);
            //                    }
            //                    else if (customAttr.PropertyName == name.ToLower())
            //                    {
            //                        Result = SetMemberPropertyValue(prop.Name, value, bIgnoreCaseToUpperOrToLower, bThrowException);
            //                    }
            //                }
            //                //result += (attr as JsonPropertyAttribute).PropertyName;
            //            }
            //        }
            //    }
            //    Result = true;
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex);
            //    if (bThrowException == true)
            //    {
            //        throw ex;
            //    }
            //}
            //return Result;
            * */
        }

        protected virtual bool SetHxAttributePropertyValue(string name, object value, bool bIgnoreCaseToUpperOrToLower = true, bool bThrowException = false)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            return HxUtils.SetHxAttributePropertyValue(this, name, value, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
            /**
            //bool Result = false;
            //try
            //{
            //    var v = this;
            //    if (v != null)
            //    {
            //        var props = v.GetType().GetProperties(BindingFlags.Instance | BindingFlags.SetProperty);
            //        foreach (PropertyInfo prop in props)
            //        {
            //            foreach (object attr in prop.GetCustomAttributes(true))
            //            {
            //                HxAttribute customAttr = (attr as HxAttribute);
            //                if (customAttr != null)
            //                {
            //                    if (customAttr != null)
            //                    {
            //                        if (customAttr.ColumnName == name)
            //                        {
            //                            Result = SetMemberPropertyValue(prop.Name, value, bIgnoreCaseToUpperOrToLower, bThrowException);
            //                        }
            //                        else if (bIgnoreCaseToUpperOrToLower == true && customAttr.ColumnName == name.ToUpper())
            //                        {
            //                            Result = SetMemberPropertyValue(prop.Name, value, bIgnoreCaseToUpperOrToLower, bThrowException);
            //                        }
            //                        else if (bIgnoreCaseToUpperOrToLower == true && customAttr.ColumnName == name.ToLower())
            //                        {
            //                            Result = SetMemberPropertyValue(prop.Name, value, bIgnoreCaseToUpperOrToLower, bThrowException);
            //                        }
            //                        else if (bIgnoreCaseToUpperOrToLower && customAttr.Name == name)
            //                        {
            //                            Result = SetMemberPropertyValue(prop.Name, value, bIgnoreCaseToUpperOrToLower, bThrowException);
            //                        }
            //                        else if (bIgnoreCaseToUpperOrToLower == true && customAttr.Name == name.ToUpper())
            //                        {
            //                            Result = SetMemberPropertyValue(prop.Name, value, bIgnoreCaseToUpperOrToLower, bThrowException);
            //                        }
            //                        else if (bIgnoreCaseToUpperOrToLower == true && customAttr.Name == name.ToLower())
            //                        {
            //                            Result = SetMemberPropertyValue(prop.Name, value, bIgnoreCaseToUpperOrToLower, bThrowException);
            //                        }
            //                    }
            //                }
            //                //result += (attr as JsonPropertyAttribute).PropertyName;
            //            }
            //        }
            //    }
            //    Result = true;
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex);
            //    if (bThrowException == true)
            //    {
            //        throw ex;
            //    }
            //}
            //return Result;
            **/
        }

        protected virtual bool SetCustomPropertyValue(string name, object value, bool bIgnoreCaseToUpperOrToLower = true, bool bThrowException = false)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            return HxUtils.SetCustomPropertyValue(this, name, value, flags, bIgnoreCaseToUpperOrToLower, bThrowException);
            /**
            //bool Result = false;
            //try
            //{
            //    Result = SetMemberPropertyValue(name, value, bIgnoreCaseToUpperOrToLower, bThrowException);
            //    if(Result != true)
            //    {
            //        Result = SetJsonPropertyValue(name, value, bIgnoreCaseToUpperOrToLower, bThrowException);
            //    }
            //    if (Result != true)
            //    {
            //        Result = SetHxAttributePropertyValue(name, value, bIgnoreCaseToUpperOrToLower, bThrowException);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    if (bThrowException == true)
            //    {
            //        throw ex;
            //    }
            //}
            //return Result;
            */
        }

        public static string GetCustomUserAgentString(IHxDb db)
        {
            string Result = null;
            try
            {
                if (db != null && db.Open())
                {
                    string SQL = string.Format("SELECT {0} FROM DUAL", CUSTOM_USER_AGENT);
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

        
    }

    public enum HxDefaultColumnsType
    {
        Default,
        WithAgent,
        WithAgentUNo,
    }

    public abstract class HxSetValueDbTable : HxSetValueObject, IHxSetValue
    {
        //public abstract static string _SQL_QUERY_OBJECT_ = null;

        //public const string _CDF_CNO_ = "cno";
        //public const string _CDF_UNO_ = "uno";

        public const string _CDF_IS_USE_ = "is_use";



        public const string _CDF_REG_DATE_ = "reg_date";
        public const string _CDF_MOD_DATE_ = "mod_date";
        public const string _CDF_REG_AGENT_ = "reg_agent";
        public const string _CDF_MOD_AGENT_ = "mod_agent";
        public const string _CDF_REG_USER_ = "reg_user";
        public const string _CDF_MOD_USER_ = "mod_user";
        public const string _CDF_REG_UNO_ = "reg_uno";
        public const string _CDF_MOD_UNO_ = "mod_uno";



        protected static DataTable GetData(IHxDb db, string pQueryString, Dictionary<string, object> pBind = null, string pWhere = null)
        {
            DataTable Result = null;
            try
            {
                if (db != null && pQueryString.IsNullOrWhiteSpaceEx() != true)
                {
                    string SQL = HxUtils.SelectQueryString(pQueryString, pWhere);
                    Result = db.QueryDataTable(pQueryString, pBind);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            return Result;
        }

        protected static DataTable GetData(IHxDb db, string pQueryString, string pWhere = null, Dictionary<string, object> pBind = null)
        {
            return GetData(db, pQueryString, pBind, pWhere);
        }

        #region Member 관련 Static
        public static HxMemberUser MemberUserInfo(IHxDb db, decimal uno, string cryptColName = null, string cryptKey = null)
        {
            HxMemberUser Result = null;
            try
            {
                if (db != null)
                {
                    DataTable dt = HxMemberUser.GetData(db, uno);
                    //Result = dt.ToRecordEx<MemberUserRec>();
                    Result = new HxMemberUser(dt);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            return Result;
        }
        public static string MemberCustomString(IHxDb db, decimal uno)
        {
            string Result = uno.ToStringEx();
            HxMemberUser mem = MemberUserInfo(db, uno);
            if (mem != null)
            {
                Result = string.Format("UNO:{0},USER_ID:{1},USER_NAME:{2},DUTY_NAME:{3},DEPT_NAME:{4},DEPT_NO:{5},DEPT_CD:{6}", mem?.UNo, mem?.UserID, mem?.UserName, mem?.DutyName, mem?.DeptName, mem?.DeptID, mem?.DeptCode);
            }
            return Result;
        }

        public static string MemberCustomString(HxMemberUser mem)
        {
            string Result = null;
            if (mem != null)
            {
                Result = string.Format("UNO:{0},USER_ID:{1},USER_NAME:{2},DUTY_NAME:{3},DEPT_NAME:{4},DEPT_NO:{5},DEPT_CD:{6}", mem?.UNo, mem?.UserID, mem?.UserName, mem?.DutyName, mem?.DeptName, mem?.DeptID, mem?.DeptCode);
            }
            return Result;
        }

        public static bool InsertDefaultColumnMatch(string strCustomUser, ref StringBuilder sbrCol, ref StringBuilder sbrVal, ref Dictionary<string, object> bind, decimal? uno = null, HxDefaultColumnsType columnsType = HxDefaultColumnsType.Default)
        {
            bool Result;
            string colName;
            object value;
            try
            {
                colName = _CDF_IS_USE_;
                value = HxUseType.Y.ToStringEx();
                sbrCol.AppendFormat(", {0}", colName);
                sbrVal.AppendFormat(",:{0}", colName);
                bind.AddDbEx(colName, value);

                colName = _CDF_REG_USER_;
                value = strCustomUser;
                sbrCol.AppendFormat(", {0}", colName);
                sbrVal.AppendFormat(",:{0}", colName);
                bind.AddDbEx(colName, value);
                colName = _CDF_MOD_USER_;
                value = strCustomUser;
                sbrCol.AppendFormat(", {0}", colName);
                sbrVal.AppendFormat(",:{0}", colName);
                bind.AddDbEx(colName, value);

                colName = _CDF_REG_DATE_;
                value = "SYSDATE";
                sbrCol.AppendFormat(", {0}", colName);
                sbrVal.AppendFormat(", {0}", value);
                colName = _CDF_MOD_DATE_;
                value = "SYSDATE";
                sbrCol.AppendFormat(", {0}", colName);
                sbrVal.AppendFormat(", {0}", value);

                if (columnsType == HxDefaultColumnsType.WithAgent)
                {
                    colName = _CDF_REG_AGENT_;
                    value = "FUNC_USER_AGENT()";
                    sbrCol.AppendFormat(", {0}", colName);
                    sbrVal.AppendFormat(", {0}", value);
                    colName = _CDF_MOD_AGENT_;
                    value = "FUNC_USER_AGENT()";
                    sbrCol.AppendFormat(", {0}", colName);
                    sbrVal.AppendFormat(", {0}", value);
                }
                
                if (uno != null && columnsType == HxDefaultColumnsType.WithAgentUNo)
                {
                    colName = _CDF_REG_UNO_;
                    value = uno;
                    sbrCol.AppendFormat(", {0}", colName);
                    sbrVal.AppendFormat(",:{0}", colName);
                    bind.AddDbEx(colName, value);
                    colName = _CDF_MOD_UNO_;
                    value = uno;
                    sbrCol.AppendFormat(", {0}", colName);
                    sbrVal.AppendFormat(",:{0}", colName);
                    bind.AddDbEx(colName, value);
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Result = false;
                //throw;
            }
            return Result;

        }
        public static bool InsertDefaultColumnMatch(IHxDb db, decimal? uno, ref StringBuilder sbrCol, ref StringBuilder sbrVal, ref Dictionary<string, object> bind, HxDefaultColumnsType columnsType = HxDefaultColumnsType.Default)
        {

            string strCustomUser = null;
            if (uno != null)
                strCustomUser = MemberCustomString(db, uno.ToDecimalEx());
            return InsertDefaultColumnMatch(strCustomUser, ref sbrCol, ref sbrVal, ref bind, uno, columnsType);
        }
        public static bool UpdateDefaultColumnMatch(string strCustomUser, ref StringBuilder sbrRow, ref Dictionary<string, object> bind, decimal? uno = null, HxDefaultColumnsType columnsType = HxDefaultColumnsType.Default)
        {
            bool Result;
            string colName;
            object value;
            try
            {
                //colName = "IS_USE";
                //value = SbUseType.Y.ToStringEx();
                //sbrRow.AppendFormat(", {0} = :{0}", colName);
                //bind.AddDbEx(colName, value);

                colName = _CDF_MOD_USER_;
                value = strCustomUser;
                sbrRow.AppendFormat("\n\t, {0} = :{0}", colName);
                bind.AddDbEx(colName, value);

                colName = _CDF_MOD_DATE_;
                value = "SYSDATE";
                sbrRow.AppendFormat("\n\t, {0} = {1}", colName, value);

                if (columnsType == HxDefaultColumnsType.WithAgent)
                {
                    colName = _CDF_MOD_AGENT_;
                    value = "FUNC_USER_AGENT()";
                    sbrRow.AppendFormat("\n\t, {0} = {1}", colName, value);
                }
                else if (uno != null && columnsType == HxDefaultColumnsType.WithAgentUNo)
                {
                    colName = _CDF_MOD_UNO_;
                    value = uno;
                    sbrRow.AppendFormat("\n\t, {0} = {1}", colName, value);
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Result = false;
                //throw;
            }
            return Result;

        }
        public static bool UpdateDefaultColumnMatch(IHxDb db, decimal? uno, ref StringBuilder sbrRow, ref Dictionary<string, object> bind, HxDefaultColumnsType columnsType = HxDefaultColumnsType.Default)
        {
            string strCustomUser = uno != null ? MemberCustomString(db, uno.ToDecimalEx()) : null;
            return UpdateDefaultColumnMatch(strCustomUser, ref sbrRow, ref bind, uno, columnsType);

        }
        #endregion
    }

    

    public abstract class HxDbStdBaseSetValue : HxSetValueDbTable, IHxSetValue
    {
        public void Create(string is_use = null, DateTime? reg_date = null, DateTime? mod_date = null, string reg_agent = null, string mod_agent = null, string reg_user = null, string mod_user = null, decimal? reg_uno = null, decimal? mod_uno = null)
        {
            IS_USE = is_use;
            REG_DATE = reg_date;
            MOD_DATE = mod_date;
            REG_AGENT = reg_agent;
            MOD_AGENT = mod_agent;
            REG_USER = reg_user;
            MOD_USER = mod_user;
            REG_UNO = reg_uno;
            MOD_UNO = mod_uno;
        }
        public void Create(
            bool? is_use = null
            , decimal? uno = null, string user_custom = null, string user_agent = null
            , DateTime? workDateTime = null
            , bool InsertCreateType = true
        )
        {
            if (workDateTime == null || workDateTime.IsNullOrWhiteSpaceEx() == true)
            {
                workDateTime = DateTime.Now;
            }
            IS_USE = is_use.ToStringEx(true);
            MOD_AGENT = user_agent;
            MOD_USER = user_custom;
            MOD_UNO = uno;
            MOD_DATE = workDateTime;

            if (InsertCreateType == true)
            {
                REG_AGENT = user_agent;
                REG_USER = user_custom;
                REG_UNO = uno;
                REG_DATE = workDateTime;
            }
        }

        //public const string _CDF_IS_USE_    = "is_use";
        //public const string _CDF_REG_DATE_  = "reg_date";
        //public const string _CDF_MOD_DATE_  = "mod_date";
        //public const string _CDF_REG_AGENT_ = "reg_agent";
        //public const string _CDF_MOD_AGENT_ = "mod_agent";
        //public const string _CDF_REG_USER_  = "reg_user";
        //public const string _CDF_MOD_USER_  = "mod_user";
        //public const string _CDF_REG_UNO_   = "reg_uno";
        //public const string _CDF_MOD_UNO_   = "mod_uno";

        [JsonProperty(_CDF_IS_USE_)] public string IS_USE { get; set; }
        [JsonProperty(_CDF_REG_DATE_)] public DateTime? REG_DATE { get; set; }
        [JsonProperty(_CDF_MOD_DATE_)] public DateTime? MOD_DATE { get; set; }
        [JsonProperty(_CDF_REG_AGENT_)] public string REG_AGENT { get; set; }
        [JsonProperty(_CDF_MOD_AGENT_)] public string MOD_AGENT { get; set; }
        [JsonProperty(_CDF_REG_USER_)] public string REG_USER { get; set; }
        [JsonProperty(_CDF_MOD_USER_)] public string MOD_USER { get; set; }
        [JsonProperty(_CDF_REG_UNO_)] public decimal? REG_UNO { get; set; }
        [JsonProperty(_CDF_MOD_UNO_)] public decimal? MOD_UNO { get; set; }

        public virtual void CopyData(HxDbStdBaseSetValue param)
        {
            this.IS_USE = param.IS_USE;
            //this.RAW_GUID = param.RAW_GUID;
            this.REG_DATE = param.REG_DATE;
            this.REG_AGENT = param.REG_AGENT;
            this.REG_UNO = param.REG_UNO;
            this.REG_USER = param.REG_USER;
            this.MOD_DATE = param.MOD_DATE;
            this.MOD_AGENT = param.MOD_AGENT;
            this.MOD_UNO = param.MOD_UNO;
            this.MOD_USER = param.MOD_USER;
        }
        public virtual object GetPropertyValue(string name)
        {
            object Result;
            switch (name)
            {
                case _CDF_IS_USE_: Result = IS_USE; break;
                case _CDF_REG_DATE_: Result = REG_DATE; break;
                case _CDF_MOD_DATE_: Result = MOD_DATE; break;
                case _CDF_REG_AGENT_: Result = REG_AGENT; break;
                case _CDF_MOD_AGENT_: Result = MOD_AGENT; break;
                case _CDF_REG_USER_: Result = REG_USER; break;
                case _CDF_MOD_USER_: Result = MOD_USER; break;
                case _CDF_REG_UNO_: Result = REG_UNO; break;
                case _CDF_MOD_UNO_: Result = MOD_UNO; break;
                default:
                    Result = GetCustomPropertyValue(name, true, true);
                    break;
            }
            return Result;
        }
        public virtual void SetPropertyValue(string name, object value) //SetMatchFieldValue
        {
            //base.SetMatchFieldValue(name, value);
            switch (name)
            {
                case _CDF_IS_USE_: IS_USE = value.ToStringEx(); break;
                case _CDF_REG_DATE_: REG_DATE = value.ToNullableDateTimeEx(); break;
                case _CDF_MOD_DATE_: MOD_DATE = value.ToNullableDateTimeEx(); break;
                case _CDF_REG_AGENT_: REG_AGENT = value.ToStringEx(); break;
                case _CDF_MOD_AGENT_: MOD_AGENT = value.ToStringEx(); break;
                case _CDF_REG_USER_: REG_USER = value.ToStringEx(); break;
                case _CDF_MOD_USER_: MOD_USER = value.ToStringEx(); break;
                case _CDF_REG_UNO_: REG_UNO = value.ToNullableDecimalEx(); break;
                case _CDF_MOD_UNO_: MOD_UNO = value.ToNullableDecimalEx(); break;
                default:
                    SetCustomPropertyValue(name, value, true, true);
                    break;
            }
        }

        public virtual void SetValueToFieldByStd(
            bool? is_use = null
            , decimal? uno = null, string user_custom = null, string user_agent = null
            , DateTime? workDateTime = null
            , bool InsertCreateType = true
        )
        {
            SetValueToFieldByStd(is_use.ToStringEx(true), uno, user_custom, user_agent, workDateTime, InsertCreateType);
        }
        public virtual void SetValueToFieldByStd(
            string is_use = null
            , decimal? uno = null, string user_custom = null, string user_agent = null
            , DateTime? workDateTime = null
            , bool InsertCreateType = true
        )
        {
            if (workDateTime == null || workDateTime.IsNullOrWhiteSpaceEx() == true)
            {
                workDateTime = DateTime.Now;
            }
            this.IS_USE = is_use.ToStringEx(true);
            this.MOD_AGENT = user_agent;
            this.MOD_USER = user_custom;
            this.MOD_UNO = uno;
            this.MOD_DATE = workDateTime;

            if (InsertCreateType == true)
            {
                this.REG_AGENT = user_agent;
                this.REG_USER = user_custom;
                this.REG_UNO = uno;
                this.REG_DATE = workDateTime;
            }
        }

        public static HxResultValue SetArrangeDbInsertKeyValuePairsType(string inputColName, object inputObjValue, StringBuilder builderCols, StringBuilder builderVals, Dictionary<string, object> pairsBind, bool? IsNotParamBindAndCustomValueWithQuotesUse = null, HxDbParamValueType paramValueType = HxDbParamValueType.Default, bool? bNotUseValueIsNull = null)
        {
            return HxUtils.SetArrangeDbInsertKeyValuePairsType(inputColName, inputObjValue, builderCols, builderVals, pairsBind, IsNotParamBindAndCustomValueWithQuotesUse, paramValueType);
        }
        public static HxResultValue SetArrangeDbInsertDateTimeType(string inputColName, DateTime? inputDateValue, StringBuilder builderCols, StringBuilder builderVals, Dictionary<string, object> pairsBind, HxDbProviderType providerType = HxDbProviderType.Oracle)
        {
            return HxUtils.SetArrangeDbInsertDateTimeType(inputColName, inputDateValue, builderCols, builderVals, pairsBind, providerType);
        }

        public static string GetUpdateQuery(string updateQueryStr, HxMemberUser member, string agentOSCustomStr)
        {
            return GetUpdateQuery(updateQueryStr, member.UNo, member.UserCustomString, agentOSCustomStr, null);
        }
        private static string GetUpdateQuery(string updateQueryString, decimal? uno, string userMemberCustomStr, string agentOSCustomStr, IHxDb db = null)
        {
            string Result = null;
            string SQL = updateQueryString?.Trim();
            if (SQL.IsNullOrWhiteSpaceEx() != true && SQL.IsRegexMatchEx(@"^(UPDATE)(?:\s+)([a-zA-Z0-9_]+)(?:\s+)(SET)(?:\s)", System.Text.RegularExpressions.RegexOptions.IgnoreCase) == true)
            {
                string mUpdateSQL = null;

                if (SQL.IsRegexMatchEx($@"(,)(?:\s+)*({_CDF_MOD_DATE_})(?:\s+)*(=)(?:\s+)*([a-zA-Z0-9_\-\:]+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase) != true)
                {
                    mUpdateSQL += $", {_CDF_MOD_DATE_} = SYSDATE";
                }
                if (uno != null && SQL.IsRegexMatchEx($@"(,)(?:\s+)*({_CDF_MOD_UNO_})(?:\s+)*(=)(?:\s+)*([a-zA-Z0-9_\-\:]+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase) != true)
                {
                    mUpdateSQL += $", {_CDF_MOD_UNO_} = {uno}";
                }

                if (SQL.IsRegexMatchEx($@"(,)(?:\s+)*({_CDF_MOD_USER_})(?:\s+)*(=)(?:\s+)*([a-zA-Z0-9_\-\:]+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase) != true)
                {
                    if (userMemberCustomStr == null && uno != null && db != null && db.Open() == true)
                    {
                        userMemberCustomStr = HxMemberUser.GetUserInfo(db, uno)?.UserCustomString;
                    }
                    if (userMemberCustomStr.IsNullOrWhiteSpaceEx() != true)
                    {
                        mUpdateSQL += $", {_CDF_MOD_USER_} = '{userMemberCustomStr}'";
                    }
                }
                if (agentOSCustomStr != null && SQL.IsRegexMatchEx($@"(,)(?:\s+)*({_CDF_MOD_AGENT_})(?:\s+)*(=)(?:\s+)*([a-zA-Z0-9_\-\:]+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase) != true)
                {
                    agentOSCustomStr = agentOSCustomStr.IsNullOrWhiteSpaceEx() == true ? HxUtils.GetOSCustomUserAgent() : agentOSCustomStr.Trim();
                    mUpdateSQL += $", {_CDF_MOD_AGENT_} = '{agentOSCustomStr}'";
                }

                List<string> listLine = SQL.RegexReplaceEx("where", "WHERE", System.Text.RegularExpressions.RegexOptions.IgnoreCase).SplitEx("WHERE").ToListEx<string>();
                StringBuilder sbRow = new StringBuilder();
                if (listLine != null && listLine.Count > 0)
                {
                    for (int i = 0; i < listLine.Count - 1; i++)
                    {
                        sbRow.AppendLine(listLine[i]);
                    }
                    sbRow.AppendLine(mUpdateSQL);
                    if (listLine.Count > 1)
                    {
                        sbRow.AppendLine(listLine[listLine.Count - 1]);
                    }
                }
                Result = sbRow.ToStringEx();
            }
            return Result;
        }
        protected static string GetInsertQuery(string insertQueryStr, decimal? uno, string userMemberCustomStr, string agentOSCustomStr, IHxDb db = null)
        {
            string Result = null;
            string SQL = insertQueryStr?.Trim();
            if (SQL.IsNullOrWhiteSpaceEx() != true && SQL.IsRegexMatchEx(@"^(INSERT)(?:\s+)(INTO)(?:\s+)(\()", System.Text.RegularExpressions.RegexOptions.IgnoreCase) == true)
            {

            }
            return Result;
        }
    }



}
