using System;
using System.Collections.Generic;
using System.Text;

namespace HxCore
{
    using Newtonsoft.Json;
    using System.Data;
    using System.Diagnostics;

    public class HxMemberUser : IHxSetValue
    {
        public static string _SQL_VIEW_SYS_USER_INFO_ { get; protected set; } = "SYS_USER_INFO";

        public const string _CDF_UNO_ = "uno";
        public const string _CDF_USER_ID_ = "user_id";
        public const string _CDF_USER_PWD_ = "user_pwd";
        public const string _CDF_PASSWORD_ = "password";
        public const string _CDF_MD5_ = "md5";

        [Hx(ColumnName = _CDF_UNO_)]
        [JsonProperty(_CDF_UNO_)]
        public decimal? UNo;
        [Hx(ColumnName = _CDF_USER_ID_)]
        [JsonProperty(_CDF_USER_ID_)]
        public string UserID;
        [JsonProperty("user_name")]
        public string UserName;

        [JsonProperty("dept_no")]
        public int? DeptNo;
        [JsonProperty("dept_id")]
        public int? DeptID { get => DeptNo; private set => DeptNo = value; }
        [JsonProperty("dept_cd")]
        public string DeptCode;
        [JsonProperty("dept_name")]
        public string DeptName;
        [JsonProperty("dept_name_path")]
        public string DeptNamePath;

        [JsonProperty("rank_no")]
        public int? RankNo;
        [JsonProperty("rank_id")]
        public int? RankID { get => RankNo; private set => RankNo = value; }
        [JsonProperty("rank_cd")]
        public string RankCode;
        [JsonProperty("rank_name")]
        public string RankName;

        [JsonProperty("duty_no")]
        public int? DutyNo;
        [JsonProperty("duty_id")]
        public int? DutyID { get => DutyNo; private set => DutyNo = value; }
        [JsonProperty("duty_cd")]
        public string DutyCode;
        [JsonProperty("duty_name")]
        public string DutyName;

        [JsonProperty("tel")]
        public string Tel;
        [JsonProperty("cell")]
        public string Cell;
        [JsonProperty("email")]
        public string Email;

        [JsonProperty("team_no")]
        public int? TeamNo;
        [JsonProperty("team_id")]
        public int? TeamID { get => TeamNo; private set => TeamNo = value; }
        [JsonProperty("team_cd")]
        public string TeamCode;
        [JsonProperty("team_name")]
        public string TeamName;

        [JsonProperty("jobduty_no")]
        public int? JobDutyNo;
        [JsonProperty("jobduty_id")]
        public int? JobDutyID { get => JobDutyNo; private set => JobDutyNo = value; }
        [JsonProperty("jobduty_cd")]
        public string JobDutyCode;
        [JsonProperty("jobduty_name")]
        public string JobDutyName;

        [JsonProperty("company_id")]
        public int? CompanyID;
        [JsonProperty("company_no")]
        public int? CompanyNo { get => CompanyID; private set => CompanyID = value; }
        [JsonProperty("company_name")]
        public string CompanyName;

        [JsonProperty("use_str")]
        public string IsUseStr;
        [JsonProperty("state_str")]
        public string IsStateStr;
        [JsonProperty("user_auth_type")]
        public HxUserAuthType UserAuthType;
        [JsonProperty("is_login")]
        public bool? IsLogin;

        [JsonProperty("session_no")]
        public int? SessionNo;
        [JsonProperty("session_id")]
        public string SessionID;

        //[JsonProperty("user_global_address")]
        private string UserGlobalAddress { get => HxUtils.GetUserGlobalAddress(); }
        //[JsonProperty("user_host_address")]
        private string UserHostAddress { get => HxUtils.GetUserHostAddress(); }

        [JsonProperty("remote_address")]
        public string RemoteAddress;

        [JsonProperty("user_title")]
        public string UserTitle { get => string.Format("{1} {2} ({0})", UserID, UserName, DutyName); }
        [JsonProperty("user_custom_string")]
        public string UserCustomString { get => string.Format("UNO:{0},USER_ID:{1},USER_NAME:{2},DUTY_NAME:{3},DEPT_NAME:{4},DEPT_NO:{5},DEPT_CD:{6}", UNo, UserID, UserName, DutyName, DeptName, DeptID, DeptCode); }
        [JsonProperty("user_agent_ip")]
        public string UserAgentIP { get => string.Format("{0}/{1}", UserGlobalAddress, UserHostAddress); }

        public HxMemberUser(bool bInit = false)
        {
            UNo = null;
            UserID = null;
            UserName = null;

            DeptNo = null;
            DeptCode = null;
            DeptName = null;
            DeptNamePath = null;

            RankNo = null;
            RankCode = null;
            RankName = null;

            DutyNo = null;
            DutyCode = null;
            DutyName = null;

            TeamNo = null;
            TeamCode = null;
            TeamName = null;

            JobDutyNo = null;
            JobDutyCode = null;
            JobDutyName = null;

            Tel = null;
            Cell = null;
            Email = null;

            CompanyID = null;
            CompanyName = null;

            IsUseStr = null;
            IsStateStr = null;
            UserAuthType = HxUserAuthType.None;
            IsLogin = null;

            SessionNo = null;
            SessionID = null;

            RemoteAddress = HxUtils.GetUserHostAddress(false);
        }
        public HxMemberUser(DataTable data, int rowIndex = 0)
            : this()
        {
            SetValue(data, rowIndex);
        }
        public HxMemberUser(DataRow row)
            : this()
        {
            SetValue(row);
        }

        public void SetValue(DataTable data, int rowIndex = 0)
        {
            if (data != null && data.Rows.Count > 0)
            {
                int n = data.Rows.Count;
                if (rowIndex < 0)
                {
                    rowIndex = 0;
                }
                else if (rowIndex >= n)
                {
                    rowIndex = n - 1;
                }
                SetValue(data.Rows[rowIndex]);
            }
        }

        public void SetValue(DataRow row)
        {
            if (row != null && row.Table.Columns.Count > 0)
            {
                DataTable dt = row.Table;

                foreach (DataColumn dc in dt.Columns)
                {
                    string name = dc.ColumnName.ToLower();
                    object value = row[name];
                    switch (name)
                    {
                        case "uno":
                            UNo = value.ToIntEx();
                            break;
                        case "user_id":
                            UserID = value.ToStringEx();
                            break;
                        case "user_name":
                            UserName = value.ToStringEx();
                            break;

                        case "dept_id":
                            DeptNo = value.ToIntEx();
                            break;
                        case "dept_cd":
                            DeptCode = value.ToStringEx();
                            break;
                        case "dept_name":
                            DeptName = value.ToStringEx();
                            break;
                        case "dept_name_path":
                            DeptNamePath = value.ToStringEx();
                            break;

                        case "rank_id":
                        case "rank_no":
                            RankNo = value.ToIntEx();
                            break;
                        case "rank_cd":
                        case "rank_code":
                            RankCode = value.ToStringEx();
                            break;
                        case "rank_name":
                            RankName = value.ToStringEx();
                            break;

                        case "duty_id":
                        case "duty_no":
                            DutyNo = value.ToIntEx();
                            break;
                        case "duty_cd":
                        case "duty_code":
                            DutyCode = value.ToStringEx();
                            break;
                        case "duty_name":
                            DutyName = value.ToStringEx();
                            break;

                        case "team_id":
                            TeamNo = value.ToIntEx();
                            break;
                        case "team_cd":
                            TeamCode = value.ToStringEx();
                            break;
                        case "team_name":
                            TeamName = value.ToStringEx();
                            break;

                        case "jobduty_id":
                        case "jobduty_no":
                            JobDutyNo = value.ToIntEx();
                            break;
                        case "jobduty_cd":
                        case "jobduty_code":
                            JobDutyCode = value.ToStringEx();
                            break;
                        case "jobduty_name":
                            JobDutyName = value.ToStringEx();
                            break;

                        case "tel":
                        case "phone":
                            Tel = value.ToStringEx();
                            break;
                        case "cell":
                        case "mobile":
                            Cell = value.ToStringEx();
                            break;
                        case "email":
                            Email = value.ToStringEx();
                            break;

                        case "company_id":
                        case "company_no":
                            CompanyID = value.ToIntEx();
                            break;
                        case "company_name":
                            CompanyName = value.ToStringEx();
                            break;

                        case "is_use":
                            IsUseStr = value.ToStringEx();
                            break;
                        case "is_state":
                            IsStateStr = value.ToStringEx();
                            break;
                        case "is_admin":
                            if (UserAuthType == HxUserAuthType.None)
                            {
                                string strManager = value.ToStringEx();
                                if (strManager == "Y")
                                {
                                    UserAuthType = HxUserAuthType.Admin;
                                }
                            }
                            break;
                        case "is_manager":
                            if (UserAuthType == HxUserAuthType.None || UserAuthType == HxUserAuthType.Guest)
                            {
                                string strManager = value.ToStringEx();
                                if (strManager == "Y")
                                {
                                    UserAuthType = HxUserAuthType.Admin;
                                }
                                else
                                {
                                    UserAuthType = HxUserAuthType.None;
                                    string strAttend = dt.Columns.Contains("is_attend") == true ? row["is_attend"].ToStringEx() : null;
                                    if (strAttend.IsNullOrWhiteSpaceEx() != true)
                                    {
                                        switch (strAttend)
                                        {
                                            case "N":
                                                UserAuthType = HxUserAuthType.Guest;
                                                break;
                                            default:
                                                UserAuthType = HxUserAuthType.Member;
                                                //string strDeptNo = dt.Columns.Contains("dept_name") == true ? row["dept_no"].ToStringEx() : null;
                                                //string strDeptCD = dt.Columns.Contains("dept_cd") == true ? row["dept_cd"].ToStringEx() : null;
                                                //string strDeptName = dt.Columns.Contains("dept_name") == true ? row["dept_name"].ToStringEx() : null;
                                                //switch (strDeptName)
                                                //{
                                                //    case "사업관리팀":
                                                //        UserAuthType = HxUserAuthType.PM;
                                                //        break;
                                                //    case "경영지원팀":
                                                //    case "관리팀":
                                                //        UserAuthType = HxUserAuthType.Management;
                                                //        break;
                                                //    case "기술연구소":
                                                //        UserAuthType = HxUserAuthType.Manager;
                                                //        break;
                                                //    default:
                                                //        UserAuthType = HxUserAuthType.Member;
                                                //        break;
                                                //}
                                                break;
                                        }
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                if (UNo.IsNullOrMinValueEx() != true && UserID.IsNullOrWhiteSpaceEx() != true && UserName.IsNullOrWhiteSpaceEx() != true
                    && (UserAuthType != HxUserAuthType.None && UserAuthType != HxUserAuthType.Guest)
                )
                {
                    IsLogin = true;
                }
                else
                {
                    IsLogin = false;
                }
            }
            else
            {
                IsLogin = false;
            }
        }

        public void Clear()
        {
            UNo = null;
            UserID = null;
            UserName = null;

            DeptNo = null;
            DeptCode = null;
            DeptName = null;
            DeptNamePath = null;

            RankNo = null;
            RankCode = null;
            RankName = null;

            DutyNo = null;
            DutyCode = null;
            DutyName = null;

            TeamNo = null;
            TeamCode = null;
            TeamName = null;

            JobDutyNo = null;
            JobDutyCode = null;
            JobDutyName = null;

            CompanyID = null;
            CompanyName = null;

            IsUseStr = null;
            IsStateStr = null;
            UserAuthType = HxUserAuthType.None;
            IsLogin = null;
        }

        public void SetViewName(string viewNameStr)
        {
            _SQL_VIEW_SYS_USER_INFO_ = viewNameStr;
        }

        public static DataTable GetData(IHxDb db, decimal? pUNo, string pWhere = null, Dictionary<string, object> bind = null)
        {
            DataTable Result = null;
            try
            {
                if (db != null)
                {
                    if (pUNo == null && pWhere.IsNullOrWhiteSpaceEx() == true)
                    {
                        pWhere += " AND IS_USE = 'Y'";
                    }

                    if (pUNo != null)
                    {
                        decimal keyValue = pUNo.ToDecimalEx();
                        string colName = _CDF_UNO_;

                        if (bind == null)
                            bind = new Dictionary<string, object>();

                        if (pWhere.IsNullOrWhiteSpaceEx() != true)
                        {
                            pWhere += " AND ";
                        }
                        pWhere += string.Format("{0} = :{0}", colName);
                        bind.AddEx(colName, keyValue);
                    }

                    string SQL = _SQL_VIEW_SYS_USER_INFO_;
                    SQL = HxUtils.SelectQueryString(SQL, pWhere);
                    Result = db.QueryDataTable(SQL, bind);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //throw ex;
            }
            return Result;
        }

        public static HxMemberUser GetUserInfo(IHxDb db, decimal? pUNo, string pWhere = null, Dictionary<string, object> bind = null)
        {
            HxMemberUser Result = new HxMemberUser();
            DataTable dt = GetData(db, pUNo, pWhere, bind);
            if (dt != null && dt.Rows.Count > 0)
            {
                Result = new HxMemberUser(dt);
                // dt.ToConvertEx<MemberUser>();
            }
            return Result;
        }

        public static HxMemberUser[] ConvertToArray(DataTable dataSource)
        {
            HxMemberUser[] Result = null;
            try
            {
                Result = new HxMemberUser[dataSource.Rows.Count];
                int i = 0;
                foreach (DataRow dr in dataSource.Rows)
                {
                    Result[i] = new HxMemberUser(dr);
                    i++;
                }
            }
            catch (Exception ex)
            {
                Result = null;
                Debug.WriteLine(ex);
                throw ex;
            }
            return Result;
        }
        public static IEnumerable<HxMemberUser> ConvertToRecordset(DataTable dataSource)
        {
            IEnumerable<HxMemberUser> Result = new HxMemberUser[dataSource.Rows.Count];
            try
            {
                Result = new HxMemberUser[dataSource.Rows.Count];
                int i = 0;
                foreach (DataRow dr in dataSource.Rows)
                {
                    HxMemberUser mem = new HxMemberUser(dr);
                    Result = Result.AddEx(mem);
                    i++;
                }
            }
            catch (Exception ex)
            {
                Result = null;
                Debug.WriteLine(ex);
                throw ex;
            }
            return Result;
        }
        public static HxMemberUser ConvertToRecord(DataTable dataSource, int index = 0)
        {
            HxMemberUser Result = null;
            try
            {
                if(dataSource != null && dataSource.Rows.Count > 0)
                {
                    if (index < 0) index = 0;
                    Result = new HxMemberUser(dataSource, index);
                }
            }
            catch (Exception ex)
            {
                Result = null;
                Debug.WriteLine(ex);
                throw ex;
            }
            return Result;
        }

    }
}
