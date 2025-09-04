using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace HxCore
{

    public struct HxBoolResultRec
    {
        public bool? Result;
        public object Value;
        //public string Code;
        public string Message;
        public HxResultType ResultType;

        public HxBoolResultRec(bool? result = null)
        {
            Result = result;
            Value = null;
            //Code = null;
            Message = null;
            ResultType = HxResultType.None;
        }

        public HxBoolResultRec(bool result, object value, string message, HxResultType type = HxResultType.None)
        {
            Result = result;
            Value = value;
            //Code = code;
            Message = message;
            ResultType = type;
        }
    }

    public struct HxHostURIRec
    {
        //^(?:(\w+):\/\/)?([^:\/\s]+)(?:\:([^\/]*))?((\/[^\s/\/]+)*)?\/([^#\s\?]*)(?:\?([^#\s]*))?(#\w*)?$
        //^(?:(\w+):\/\/)?([^:\/\s]+)(?:\:([^\/]*))?((\/[^\s/\/]+)*)?\/([^#\s\?]*)(?:\?([^#\s]*))?(?:#(\w*))?$

        public const string _REGEX_PATTERN_ = @"^(?:(\w+):\/\/)?([^:\/\s]+)(?:\:([^\/]*))?((\/[^\s/\/]+)*)?\/([^#\s\?]*)(?:\?([^#\s]*))?(?:#(\w*))?$";

        public string Protocol;
        public string Domain;
        public string Port;
        public string PathDir; //디렉토리 경로
        public string PathFile; //파일명
        public string Parameter; //파라미터 (Parameter, Query String)
        public string Fragment; //플래그먼트(Fragment, Hashtag, Anchor)

        public string PathFull => $"{PathDir}/{PathFile}";
        public int PortNo
        {
            get
            {
                int Result = Port.ToIntEx(int.MinValue);
                if (Port.ToIntEx(0).IsZeroMinorValueEx() == true && Protocol.IsNullOrWhiteSpaceEx() != true)
                {
                    Result = HxUtils.GetServiceDefaultPort(Protocol);
                    if (Result.IsZeroMinorValueEx() != true)
                    {
                        Port = Result.ToStringEx();
                    }
                }
                return Result;
            }
        }
        public string QueryString => Parameter;

        public string URI
        {
            get
            {
                string Result = null;
                string strProtocol = Protocol;
                string strDomain = Domain;
                string strPort = Port;
                string strPathFull = PathFull;
                string strParameter = Parameter;
                string strFragment = Fragment;

                if (strProtocol.IsNullOrWhiteSpaceEx() == true && strPort.IsNullOrWhiteSpaceEx() != true)
                {
                    strProtocol = HxUtils.GetServiceProviderProtocol(strPort.ToIntEx());
                }
                if (strProtocol.IsNullOrWhiteSpaceEx() != true && strProtocol.EndsWith(@"://") != true)
                {
                    strProtocol += @"://";
                }

                if (strDomain.IsNullOrWhiteSpaceEx() != true && strDomain.EndsWith(@"/"))
                {
                    strDomain = strDomain.RegexReplaceEx(@"(\/+)$", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                }

                if (strPort.IsNullOrWhiteSpaceEx() != true)
                {
                    if (strProtocol.IsNullOrWhiteSpaceEx() != true)
                    {
                        string strDefaultPort = HxUtils.GetServiceDefaultPort(strProtocol).ToStringEx();
                        if (strPort == strDefaultPort)
                        {
                            strPort = null;
                        }
                    }
                    if (strPort.IsNullOrWhiteSpaceEx() != true && strPort.StartsWith(@":") != true)
                    {
                        strPort = $":{strPort}";
                    }
                }

                if (strPathFull.IsNullOrWhiteSpaceEx() != true && strPathFull.StartsWith(@"/") != true)
                {
                    strPathFull = $"/{strPathFull}";
                }

                if (strParameter.IsNullOrWhiteSpaceEx() != true)
                {

                    if (strParameter.StartsWith(@"&") == true)
                    {
                        strParameter = strParameter.RegexReplaceEx(@"^(\&+)", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    }
                    if (strParameter.StartsWith(@"?") != true)
                    {
                        strParameter = $"?{strParameter}";
                    }
                }

                if (strFragment.IsNullOrWhiteSpaceEx() != true)
                {
                    if (strFragment.StartsWith(@"&") == true)
                    {
                        strFragment = strFragment.RegexReplaceEx(@"^(\#+)", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    }
                    if (strFragment.StartsWith(@"#") != true)
                    {
                        strFragment = $"#{strFragment}";
                    }
                }
                Result = $"{strProtocol}{strDomain}{strPort}{strPathFull}{strParameter}{strFragment}";
                return Result;
            }
        }

        public bool IsLoad => (Domain.IsNullOrWhiteSpaceEx() != true ? true : false);

        public HxHostURIRec(bool bInit = false)
        {
            this.Protocol = null;
            this.Domain = null;
            this.Port = null;
            this.PathDir = null;
            this.PathFile = null;
            this.Parameter = null;
            this.Fragment = null;

            if(bInit == true)
            {
                Clear();
            }
        }

        public HxHostURIRec(string protocol, string domain, string port, string pathDir, string pathFile, string parameter, string fragment)
            : this()
        {
            this.Protocol = protocol;
            this.Domain = domain;
            this.Port = port;
            this.PathDir = pathDir;
            this.PathFile = pathFile;
            this.Parameter = parameter;
            this.Fragment = fragment;
        }
        
        public HxHostURIRec(string host)
            : this()
        {
            string strPattern = @"^(?:(\w+):\/\/)?([^:\/\s]+)(?:(\:)([^\/]*))?((\/[^\s/\/]+)*)?\/([^#\s\?]*)(?:\?([^#\s]*))?(#\w*)?$";
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(host, strPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (match.Success)
            {
                Protocol = match.Groups[1].Value;
                Domain = match.Groups[2].Value;
                //string strDbPortDelimiter = match.Groups[3].Value;
                Port = match.Groups[3].Value;
                PathDir = match.Groups[4].Value;
                string strPathDirFirstService = PathDir?.SplitEx("/").FirstOrDefault(r => r.IsNullOrWhiteSpaceEx() != true);
                string strPathDirLast = match.Groups[5].Value ?? PathDir?.SplitEx("/").LastOrDefault(r => r.IsNullOrWhiteSpaceEx() != true);
                PathFile = match.Groups[6].Value;
                Parameter = match.Groups[7].Value;
                Fragment = match.Groups[8].Value;
                if (Port.IsNullOrWhiteSpaceEx() == true && Protocol.IsNullOrWhiteSpaceEx() != true)
                {
                    Port = HxUtils.GetServiceDefaultPort(Protocol).ToStringEx();
                }
            }
        }

        public void Clear()
        {
            this.Protocol = string.Empty;
            this.Domain = string.Empty;
            this.Port = string.Empty;
            this.PathDir = string.Empty;
            this.PathFile = string.Empty;
            this.Parameter = string.Empty;
            this.Fragment = string.Empty;
        }
        public static HxHostURIRec Create(string host)
        {
            HxHostURIRec Result = new HxHostURIRec(host);
            return Result;
        }
    }

    public struct HxOpenApiJsonRec
    {

        public const string _ATTR_API_KEY_ = HxDefs._ATTR_API_KEY_;
        public const string _ATTR_API_PASSWD_ = HxDefs._ATTR_API_PASSWD_;
        public const string _ATTR_API_HOST_ = HxDefs._ATTR_API_HOST_;
        public const string _ATTR_API_REMOTE_SERVICE_ = HxDefs._ATTR_API_REMOTE_SERVICE_;

        //public string MODULE_NAME;
        public string API_KEY { get; set; }
        public string API_PASS { get; set; }
        public string API_HOST { get; set; }
        public string REMOTE_ADDR { get; set; }
        public string GLOBAL_ADDR { get; set; }
        public string REMOTE_SERVICE_STR
        {
            get => REMOTE_SERVICE_TYPE.ToString();
            set
            {
                string strValue = ( value.IsNullOrWhiteSpaceEx() == true ? "DIRECTDB" : value.ToUpper() ) ;
                REMOTE_SERVICE_TYPE = HxType.GetRemoteServiceType(strValue);
                /**
                switch (strValue)
                {
                    case "WEBAPI":
                    case "OPENAPI":
                    case "HTTP":
                    case "REST":
                        REMOTE_SERVICE_TYPE = HxRemoteServiceType.WebApi;
                        break;
                    case "DIRECTDB":
                    case "DATABASE":
                    case "DB":
                    case "DBMS":
                        REMOTE_SERVICE_TYPE = HxRemoteServiceType.DirectDb;
                        break;
                    case "CONNECTIONFAIL":
                    case "FAIL":
                    case "ERROR":
                        REMOTE_SERVICE_TYPE = HxRemoteServiceType.ConnectionFail;
                        break;
                    case "NONE":
                    case "NULL":
                    case null:
                    case "":
                        REMOTE_SERVICE_TYPE = HxRemoteServiceType.None;
                        break;
                    default:
                        REMOTE_SERVICE_TYPE = HxRemoteServiceType.ETC;
                        break;
                }
                * */
            }
        }
        public HxRemoteServiceType REMOTE_SERVICE_TYPE { get; set; }
        public HxOpenApiJsonRec(bool bInit = false)
        {
            //MODULE_NAME = null;
            API_KEY = null;
            API_PASS = null;
            API_HOST = null;
            REMOTE_ADDR = null;
            GLOBAL_ADDR = null;
            REMOTE_SERVICE_TYPE = HxRemoteServiceType.None;

            if (bInit == true)
            {
                //MODULE_NAME = string.Empty;
                API_KEY = string.Empty;
                API_PASS = string.Empty;
                API_HOST = string.Empty;
                REMOTE_ADDR = string.Empty;
                GLOBAL_ADDR = string.Empty;
                REMOTE_SERVICE_TYPE = HxRemoteServiceType.None;
            }
        }

        public static HxOpenApiJsonRec Create(JToken jtOpenApi, string remoteAddress = null, string globalAddress = null)
        {
            if (remoteAddress.IsNullOrWhiteSpaceEx() == true || remoteAddress.IsRegexMatchEx(HxDefs._REGEX_IPv4_PATTERN_) != true)
            {
                remoteAddress = HxUtils.GetUserHostAddress(true);
            }
            if (globalAddress.IsNullOrWhiteSpaceEx() == true || globalAddress.IsRegexMatchEx(HxDefs._REGEX_IPv4_PATTERN_) != true)
            {
                remoteAddress = HxUtils.GetUserGlobalAddress(true);
            }

            HxOpenApiJsonRec Result = new HxOpenApiJsonRec
            {
                API_KEY = jtOpenApi[_ATTR_API_KEY_]?.ToStringEx(),
                API_PASS = jtOpenApi[_ATTR_API_PASSWD_]?.ToStringEx(),
                API_HOST = jtOpenApi[_ATTR_API_HOST_]?.ToStringEx(),
                REMOTE_SERVICE_STR = jtOpenApi[_ATTR_API_REMOTE_SERVICE_]?.ToStringEx(),
                REMOTE_ADDR = remoteAddress,
                GLOBAL_ADDR = globalAddress
            };
            return Result;
        }
    }

    public struct HxOpenApiDbRec
    {
        public static string _SQL_TABLE_NAME_ { get; private set; } = "COMM_OAPI_INFO";
        public static string _SQL_VIEW_NAME_ { get; private set; } = "V_COMM_OAPI_INFO";

        public const string _SQL_TABLE_COMM_OAPI_KEY_ = "COMMON.COMM_OAPI_KEY";
        public const string _SQL_VIEW_COMM_OAPI_KEY_ = "COMMON.V_COMM_OAPI_KEY";

        public const string _CDF_NO_ = "api_no";
        public const string _CDF_MODULE_ = "api_module";
        public const string _CDF_KEY_NAME_   = HxDefs._API_KEY_NAME_;
        public const string _CDF_PASS_NAME_  = HxDefs._API_PASS_NAME_;
        public const string _CDF_NAME_ = "api_name";
        public const string _CDF_DESC_ = "api_desc";
        public const string _CDF_AUTH_ = "api_auth";
        public const string _CDF_PARENT_NO_ = "parent_no";
        public const string _CDF_REGEX_PATTERN_ = "regex_pattern";
        public const string _CDF_REGEX_FLAGS_ = "regex_flags";
        public const string _CDF_REGEX_OPTION_ = "regex_option";

        public const string _CDF_CASE_PAGE_ = "case_page";
        public const string _CDF_CASE_MAJOR_ = "case_major";
        public const string _CDF_CASE_MINOR_ = "case_minor";
        public const string _CDF_CASE_OPTION_ = "case_option";


        public const string _CDF_IS_USE_ = "is_use";
        public int? ApiNo;
        public string ApiModule;
        public string ApiKey;
        public string ApiPass;
        public string ApiName;
        public string ApiDesc;
        public string ApiAuth;
        public string RegexPattern;
        public string RegexFlags;
        public string RegexOption;

        public string CasePage;
        public string CaseMajor;
        public string CaseMinor;
        public string CaseOption;

        public int? ParentNo;
        public bool? IsUse;

        public HxOpenApiDbRec(bool bInit = false)
        {
            ApiNo = null;
            ApiModule = null;
            ApiKey = null;

            ApiPass = null;
            ApiName = null;
            ApiDesc = null;
            ApiAuth = null;
            RegexPattern = null;
            RegexFlags = null;
            RegexOption = null;

            CasePage = null;
            CaseMajor = null;
            CaseMinor = null;
            CaseOption = null;

            ParentNo = null;
            IsUse = null;
        }
        public HxOpenApiDbRec(DataTable data)
            : this()
        {
            Load(data);
        }
        public HxOpenApiDbRec(DataRow row)
            : this()
        {
            Load(row);
        }

        public void Load(DataTable data)
        {
            if (data != null && data.Rows.Count > 0)
            {
                Load(data.Rows[0]);
            }
        }

        public void Load(DataRow row)
        {
            if (row != null && row.Table.Columns.Count > 0)
            {
                DataTable dt = row.Table;
                this.IsUse = false;
                foreach (DataColumn dc in dt.Columns)
                {
                    string name = dc.ColumnName.ToLower();
                    object value = row[name];
                    switch (name)
                    {
                        case _CDF_NO_:
                            ApiNo = value?.ToIntEx();
                            break;
                        case _CDF_MODULE_:
                            ApiModule = value.ToStringEx();
                            break;
                        case _CDF_KEY_NAME_:
                            ApiKey = value.ToStringEx();
                            break;
                        case _CDF_PASS_NAME_:
                            ApiPass = value.ToStringEx();
                            break;

                        case _CDF_NAME_:
                            ApiName = value.ToStringEx();
                            break;
                        case _CDF_DESC_:
                            ApiDesc = value.ToStringEx();
                            break;
                        case _CDF_AUTH_:
                            ApiAuth = value.ToStringEx();
                            break;
                        case _CDF_REGEX_PATTERN_:
                            RegexPattern = value.ToStringEx();
                            break;
                        case _CDF_REGEX_FLAGS_:
                            RegexFlags = value.ToStringEx();
                            break;
                        case _CDF_REGEX_OPTION_:
                            RegexOption = value.ToStringEx();
                            break;

                        case _CDF_CASE_PAGE_:
                            CasePage = value.ToStringEx();
                            break;
                        case _CDF_CASE_MAJOR_:
                            CaseMajor = value.ToStringEx();
                            break;
                        case _CDF_CASE_MINOR_:
                            CaseMinor = value.ToStringEx();
                            break;
                        case _CDF_CASE_OPTION_:
                            CaseOption = value.ToStringEx();
                            break;

                        case _CDF_PARENT_NO_:
                            if (value == null || (value != null && value == DBNull.Value))
                            {
                                ParentNo = null;
                            }
                            else
                            {
                                ParentNo = value?.ToIntEx();
                            }
                            break;
                        case _CDF_IS_USE_:
                            string strIsUse = value.ToStringEx().ToUpper().Trim();
                            if (strIsUse.Substring(0, 1) == "Y" || strIsUse == "TRUE" || strIsUse == "1")
                            {
                                IsUse = true;
                            }
                            else
                            {
                                IsUse = false;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                IsUse = null;
            }
        }

        public void SetTableName(string value)
        {
            _SQL_TABLE_NAME_ = value;
        }
        public void SetViewName(string value)
        {
            _SQL_VIEW_NAME_ = value;
        }

        public void Clear()
        {
            ApiNo = null;
            ApiModule = null;
            ApiKey = null;
            ParentNo = null;
            ApiPass = null;
            ApiName = null;
            ApiDesc = null;
            ApiAuth = null;
            RegexPattern = null;
            RegexFlags = null;
            RegexOption = null;
            IsUse = null;
        }
    }

    public struct HxDateTimeRange
    {
        private readonly DateTime FStart;
        private readonly DateTime FEnd;

        public DateTime Start
        {
            get { return FStart; }
        }
        public DateTime End
        {
            get { return FEnd; }
        }

        public HxDateTimeRange(DateTime start, DateTime end)
        {
            this.FStart = start;
            this.FEnd = end;
        }
    }

    public struct HxDecimalRange
    {
        readonly decimal FStart;
        readonly decimal FEnd;

        public decimal Start
        {
            get { return FStart; }
        }
        public decimal End
        {
            get { return FEnd; }
        }

        public HxDecimalRange(decimal start, decimal end)
        {
            this.FStart = start;
            this.FEnd = end;
        }
    }

    public struct HxFileSimpleLocalRemoteRec
    {
        public string REMOTE_FILE_NAME;
        public string REMOTE_FILE_PATH;
        public string REMOTE_FILE_SAVE;

        public long?  REMOTE_FILE_SIZE;

        public string LOCAL_FILE_NAME;
        public string LOCAL_FILE_PATH;
        public string LOCAL_FILE_SAVE;
        public long?  LOCAL_FILE_SIZE;

        public string ORIGINAL_FILE_NAME;
        public string WORK_TYPE;
        public string DOC_STATUS;
        public string REMARK;

        public string REMOTE_FILE_FULL
        {
            get
            {
                if(REMOTE_FILE_PATH.IsNullOrWhiteSpaceEx() != true && REMOTE_FILE_SAVE.IsNullOrWhiteSpaceEx() != true)
                {
                    return Path.Combine(REMOTE_FILE_PATH, REMOTE_FILE_SAVE);
                }
                return null;
            }
        }
        public string LOCAL_FILE_FULL
        {
            get
            {
                if (LOCAL_FILE_PATH.IsNullOrWhiteSpaceEx() != true && LOCAL_FILE_SAVE.IsNullOrWhiteSpaceEx() != true)
                {
                    return Path.Combine(LOCAL_FILE_PATH, LOCAL_FILE_SAVE);
                }
                return null;
            }
        }

        public HxFileSimpleLocalRemoteRec(string rEMOTE_FILE_NAME, string rEMOTE_FILE_PATH, string rEMOTE_FILE_SAVE, long? rEMOTE_FILE_SIZE, string lOCAL_FILE_NAME, string lOCAL_FILE_PATH, string lOCAL_FILE_SAVE, long? lOCAL_FILE_SIZE, string oRIGINAL_FILE_NAME, string wORK_TYPE, string dOC_STATUS, string rEMARK)
        {
            this.REMOTE_FILE_NAME = rEMOTE_FILE_NAME;
            this.REMOTE_FILE_PATH = rEMOTE_FILE_PATH;
            this.REMOTE_FILE_SAVE = rEMOTE_FILE_SAVE;
            this.REMOTE_FILE_SIZE = rEMOTE_FILE_SIZE;
            this.LOCAL_FILE_NAME = lOCAL_FILE_NAME;
            this.LOCAL_FILE_PATH = lOCAL_FILE_PATH;
            this.LOCAL_FILE_SAVE = lOCAL_FILE_SAVE;
            this.LOCAL_FILE_SIZE = lOCAL_FILE_SIZE;
            this.ORIGINAL_FILE_NAME = oRIGINAL_FILE_NAME;
            this.WORK_TYPE = wORK_TYPE;
            this.DOC_STATUS = dOC_STATUS;
            this.REMARK = rEMARK;
        }
    }

    public struct HxFilePropLocalRemoteRec
    {
        public decimal? GROUP_NO;
        public decimal? FILE_NO;

        public string FILE_NAME;
        public string FILE_PATH;
        public string FILE_SAVE;

        public long? FILE_SIZE;

        public string FILE_TITLE;
        public string FILE_DESC;
        public string FILE_CHECK;
        public decimal? FILE_VER;
        public string FILE_URL;
        public string FILE_NUM;
        public string FILE_TYPE;
        public DateTime? FILE_DATE;

        public string LOCAL_TEMP_NAME;
        public string LOCAL_TEMP_PATH;
        public string LOCAL_TEMP_CHECK;
        public string LOCAL_TEMP_FULL_NAME
        {
            get
            {
                string Result = Path.Combine(LOCAL_TEMP_PATH, LOCAL_TEMP_NAME);
                //if(Result.IsNullOrWhiteSpaceEx() != true && File.Exists(Result) != true)
                //{
                //    Result = null;
                //}
                return Result;
            }
        }

        public string REMOTE_FULL_NAME;
        public string REMOTE_SAVE_NAME;
        public string REMOTE_SAVE_PATH;
        public string REMOTE_SAVE_EXT;
        public string REMOTE_SAVE_TYPE;

        public string ORIGINAL_NAME;

        //public string LOCAL_SAVE_NAME;

        public HxFilePropLocalRemoteRec(bool bInit = true)
        {
            GROUP_NO = null;
            FILE_NO = null;
            FILE_TITLE = null;

            FILE_NAME = null;
            FILE_SIZE = null;
            FILE_SAVE = null;
            FILE_PATH = null;

            FILE_CHECK = null;
            FILE_DATE = null;

            FILE_DESC = null;
            FILE_TYPE = null;
            FILE_URL = null;
            FILE_NUM = null;
            FILE_VER = null;


            LOCAL_TEMP_NAME = null;
            LOCAL_TEMP_PATH = null;
            LOCAL_TEMP_CHECK = null;

            REMOTE_FULL_NAME = null;
            REMOTE_SAVE_NAME = null;
            REMOTE_SAVE_PATH = null;
            REMOTE_SAVE_EXT = null;
            REMOTE_SAVE_TYPE = null;

            ORIGINAL_NAME = null;
        }

        public HxFilePropLocalRemoteRec(string fileName, decimal file_no, decimal? group_no = null, string remoteName = null)
            : this(new FileInfo(fileName), file_no, group_no, remoteName)
        {
            ; ;
        }

        public HxFilePropLocalRemoteRec(FileInfo info, decimal file_no, decimal? group_no = null, string remoteName = null, string originalName = null)
            : this()
        {
            if (info.Exists)
            {
                GROUP_NO = group_no;
                FILE_NO = file_no;
                FILE_NAME = info.Name;
                FILE_PATH = info.DirectoryName;
                FILE_SAVE = info.Name;
                FILE_SIZE = info.Length;
                FILE_DATE = info.CreationTime;
                FILE_CHECK = HxFile.MD5CheckSum(info);
                FILE_TYPE = HxFile.GetFileMimeType(info.Name);

                LOCAL_TEMP_NAME = info.Name;
                LOCAL_TEMP_PATH = info.DirectoryName;
                LOCAL_TEMP_CHECK = FILE_CHECK;

                REMOTE_FULL_NAME = remoteName;
                REMOTE_SAVE_NAME = REMOTE_FULL_NAME.IsNullOrWhiteSpaceEx() == true ? null : HxFile.GetFileName(REMOTE_FULL_NAME);
                REMOTE_SAVE_PATH = REMOTE_FULL_NAME.IsNullOrWhiteSpaceEx() == true ? null : HxFile.GetFileDirPath(REMOTE_FULL_NAME);
                REMOTE_SAVE_EXT  = REMOTE_FULL_NAME.IsNullOrWhiteSpaceEx() == true ? null : HxFile.GetFileNameExt(REMOTE_FULL_NAME);
                REMOTE_SAVE_TYPE = REMOTE_FULL_NAME.IsNullOrWhiteSpaceEx() == true ? FILE_TYPE : HxFile.GetMime(REMOTE_FULL_NAME);

                ORIGINAL_NAME = originalName??info.Name;
            }
        }
    }
    public struct HxFileInfoRec
    {
        public string FULL_NAME { get; set; }
        public string PARENT_NAME { get; set; }
        public long FILE_SIZE { get; set; }
        public long? ORIGINAL_SIZE { get; set; }

        private string _FILE_CHECK;
        public string FILE_CHECK
        {
            get
            {
                if(_FILE_CHECK.IsNullOrWhiteSpaceEx() == true && FULL_NAME.IsNullOrWhiteSpaceEx() != true && HxFile.IsFileExists(FULL_NAME))
                {
                    _FILE_CHECK = HxFile.GetCheckSum<System.Security.Cryptography.MD5>(FULL_NAME);
                }
                return _FILE_CHECK;
            }
            set
            {
                _FILE_CHECK = value;
            }
        }
        public string FILE_NAME
        {
            get {
                if (FULL_NAME.IsNullOrWhiteSpaceEx() != true)
                {
                    return HxFile.GetFileName(FULL_NAME);
                }
                return null;
            }
        }
        public string FILE_EXT
        {
            get
            {
                if (FULL_NAME.IsNullOrWhiteSpaceEx() != true)
                {
                    return HxFile.GetFileNameExt(FULL_NAME);
                }
                return null;
            }
        }
        public string FILE_NAME2
        {
            get
            {
                if (FULL_NAME.IsNullOrWhiteSpaceEx() != true)
                {
                    return HxFile.GetFileNameWithOutExt(FULL_NAME);
                }
                return null;
            }
        }
        public string DIR_NAME
        {
            get
            {
                if (FULL_NAME.IsNullOrWhiteSpaceEx() != true)
                {
                    return HxFile.GetFileDirPath(FULL_NAME);
                }
                return null;
            }
        }
        public decimal? SORT_NO { get; set; }

        public HxFileInfoRec(string fULL_NAME, string pARENT_NAME, long fILE_SIZE = -1, long? fILE_SIZE2 = null, decimal? sORT_NO = null, string fILE_CHECK = null)
        {
            FULL_NAME = fULL_NAME;
            PARENT_NAME = pARENT_NAME;
            FILE_SIZE = fILE_SIZE;
            if (fILE_SIZE <= 0)
            {
                FileInfo fi = new FileInfo(FULL_NAME);
                if (fi.Exists == true)
                {
                    this.FILE_SIZE = fi.Length;
                }
            }
            ORIGINAL_SIZE = fILE_SIZE2;
            SORT_NO = sORT_NO;
            _FILE_CHECK = fILE_CHECK;
        }
    }
    public struct HxFilePatternToDoc5Rec
    {
        public const string _PATTERN_MAIN_ = @"([a-zA-Z0-9\-\(\)\,\&\~＃＄％＆／＋ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩⅰⅱⅲⅳⅴⅵⅶⅷⅸⅹ]{4,})(?:(?:_|\ )(.{3,}))?(?:_|\.)(?:Rev|R)(?:\.)?([0-9a-zA-Z]{1,2})+(.{2,})?(?:\.)(\w+)$";
        public const string _PATTERN_SUB_ = @"^([\w\-\(\)\,]+)(.{2,})?(?:\.)(\w+)$";
        public string FileFullName  { get; set; }
        public string FileDirPath { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public string DocNum { get; set; }
        public string DocTitle { get; set; }
        public string DocRevNum { get; set; }
        public string DocDesc { get; set; }
        public string DocExt { get; set; }

        public HxFilePatternToDoc5Rec(string inputFileName,
                                       string pattern = _PATTERN_MAIN_,
                                       string subPattern = _PATTERN_SUB_
        )
        {
            FileFullName = null;
            FileDirPath = null;
            FileName = null;
            FileExt = null;

            DocNum = null;
            DocTitle = null;
            DocRevNum = null;
            DocDesc = null;
            DocExt = null;

            if (inputFileName.IsNullOrWhiteSpaceEx() != true)
            {
                FileFullName = HxFile.GetFileFullPath(inputFileName)??inputFileName;
                FileDirPath  = HxFile.GetFileDirPath(FileFullName);
                FileName     = HxFile.GetFileName(FileFullName);
                FileExt      = HxFile.GetFileNameExt(FileFullName);

                DocNum = string.Empty;
                DocTitle = string.Empty;
                DocRevNum = string.Empty;
                DocDesc = string.Empty;
                DocExt = string.Empty;
                #region //정규식을 이용한 파일룰 가져오기
                Match match = Regex.Match(FileName, pattern, RegexOptions.IgnoreCase);
                if (match != null && match.Success == true && match.Value.IsNullOrWhiteSpaceEx() != true)
                {
                    DocNum = match.Groups.Count >= 1 ? match.Groups[1].Value.Trim() : string.Empty;
                    DocTitle = match.Groups.Count >= 2 ? match.Groups[2].Value.Trim() : string.Empty;
                    DocRevNum = match.Groups.Count >= 3 ? match.Groups[3].Value.Trim() : string.Empty;
                    DocDesc = match.Groups.Count >= 4 ? match.Groups[4].Value.Trim() : string.Empty;
                    DocExt = match.Groups.Count >= 5 ? match.Groups[5].Value.Trim() : string.Empty;
                    if (FileExt.ToUpper() != DocExt.ToUpper())
                    {
                        DocExt = FileExt;
                    }
                }
                if (DocNum.IsNullOrWhiteSpaceEx() == true && subPattern.IsNullOrWhiteSpaceEx() != true)
                {
                    DocNum = string.Empty;
                    DocTitle = string.Empty;
                    DocRevNum = string.Empty;
                    DocDesc = string.Empty;
                    DocExt = string.Empty;
                    match = Regex.Match(FileName, subPattern);
                    if (match != null && match.Success == true && match.Value.IsNullOrWhiteSpaceEx() != true)
                    {
                        DocNum = match.Groups.Count >= 1 ? match.Groups[1].Value.Trim() : string.Empty;
                        DocDesc = match.Groups.Count >= 2 ? match.Groups[2].Value.Trim() : string.Empty;
                        DocExt = match.Groups.Count >= 3 ? match.Groups[3].Value.Trim() : string.Empty;
                        if (FileExt.ToUpper() != DocExt.ToUpper())
                        {
                            DocExt = FileExt;
                        }

                    }
                }
                #endregion
            }
        }
    }

    public struct HxFilePatternToDoc6Rec
    {
        /**
Cover & List.pdf
00.Cover & DWG. List.pdf
00.HTE-G-1000-000^Cover & DWG. List.pdf
00.HTE-G-1000-000^Cover.pdf
00.HTE-G-1000-000^DWG. List.pdf

001.HTE-G-1000-001.pdf
001.HTE-G-1000-001^R1 Drawing List.pdf

1.1_2in-AD133-01-A045-1[ALK-P-0210]_Sht_1.dwg
0001.1.1_2in-AD133-01-A045-1[ALK-P-0210]_Sht_1^R1 배관 ISO 에어리퀴드 사례.dwg

00001.D-150029-C-111^ANILINE PRODUCTION UNIT FOUNDATION PLAN & SECTION FOR A-GA255A(B) 토건.pdf
00001.D-150029-C-111^R1_ANILINE PRODUCTION UNIT FOUNDATION PLAN & SECTION FOR A-GA255A(B) 토건.pdf

01.PP.BPAIII-G-7000-000_Drawing List^C GAD.pdf
01.PP.BPAIII-G-7000-000_Drawing List^Rev12 GAD.pdf

01.MR.BPAIII-G-7000-000_Drawing List^R12^GAD.pdf
01.MR.BPAIII-G-7000-000_Drawing List^aaaaggg ddfkjsdkfjlskjf ajdlfjaskldfj safjdkl jsdlfkj as.pdf

01.MR.BPAIII-G-7000-000_Drawing List^R12 GAD.pdf
01.MR.BPAIII-G-7000-000_Drawing List^R12.GAD.pdf
01.MR.BPAIII-G-7000-000_Drawing List^R12_GAD.pdf
01.MR.BPAIII-G-7000-000_Drawing List^R12^GAD.pdf
01.MR.BPAIII-G-7000-000_Drawing List^R0 GAD.pdf
01.MR.BPAIII-G-7000-000_Drawing List^R0 VPIS.pdf

01.MR.BPAIII-G-7000-000_Drawing List^R0.pdf
01.MR.BPAIII-G-7000-000_Drawing List^Rev0.pdf
01.MR.BPAIII-G-7000-000_Drawing List^R0.pdf
01.MR.BPAIII-G-7000-000_Drawing List.pdf

Cover_for_Plot_Plan(PCS).pdf
00.Cover_for_Plot_Plan(PCS).pdf
00.Cover_for_Plot_Plan(PCS)^Rev0.pdf
00.B-8-5010-S000 COVER^Rev0.pdf
00.MR.B-8-5010-S000 COVER^Rev0.pdf

BPAIII-G-7000-001 (OVERALL PLOT PLAN)^R0.pdf
BPAIII-G-7000-002 (general plot plan).pdf
KE3-CI-3101-01A_LER(III) PROJECT PAVING PLAN_0.dwg
B-8-4010-S07_1 T.O.S EL.+9000 FRAMING PLAN.xls
B-8-4010-S20_1 (FRAMING ELEVATION-1).pdf
B-8-5010-S000_0 DWG LIST.txt

D-150029-C-111^R1_ANILINE PRODUCTION UNIT FOUNDATION PLAN & SECTION FOR A-GA255A(B).pdf
D-070042-C111_7_DRAINAGE PLAN-3.txt

1.1_2in-AD133-01-A045-1[ALK-P-0210]_Sht_1.dwg
0.75_2in-AD133-01-A045-1[ALK-P-0210]_Sht_1^R0.dwg
        */

        //public const string _PATTERN_MAIN_ = @"^(?:(\d{2,5})(?:\.))*(?:([a-zA-Z]{2})(?:\.))*([a-zA-Z0-9\-\(\)\[\]\,\&\~＃＄％＆_／＋ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩⅰⅱⅲⅳⅴⅵⅶⅷⅸⅹ\.\+]{4,})(?:[\ \^'#@&%]{1}(?:(?:Rev|R)([0-9a-zA-Z]{1,2}))*|(?:\.|\ |_|\^|\-)*(.{1,}))*(?:(?:\.)(\w+))$";
        //public const string _PATTERN_MAIN_ = @"^(?:(\d{2,5})(?:\.))*(?:([a-zA-Z]{2})(?:\.))*([a-zA-Z0-9\-\(\)\[\]\,\&\~＃＄％＆＿／＋ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩⅰⅱⅲⅳⅴⅵⅶⅷⅸⅹ\.\+]{4,})(?:[\ _\^'#@&%]{1})(?:Rev|R){0,1}([0-9a-zA-Z]{1,2})(?:(?:[\ _\^'#@&%\-]{1})(.{1,}))*(\.\w+)$";
        public const string _PATTERN_MAIN_ = @"^(?:(\d{1,5}(?:\.\d{1,5})*)(?:\.))*(?:([a-zA-Z]{2})(?:\.))*([a-zA-Z0-9_\-\(\)\[\]\,\&\~＃＄％＆＿／＋ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩⅰⅱⅲⅳⅴⅵⅶⅷⅸⅹ\.\+]{4,})(?:[\.\ _\^'#@&%]{1})(?:Rev|R){0,1}([0-9a-zA-Z]{1,2})(?:(?:[\ _\^'#@&%\-]{1})(.{1,}))*(\.\w+)$";
        public const string _PATTERN_SUB_ = @" ^ (?:(\d{2,5})(?:\.))*(?:([a-zA-Z]{2})(?:\.))*([\w\-\(\)\,\.]+)(.{2,})?(?:\.)(\w+)$";
        public string FileFullName { get; set; }
        public string FileDirPath { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }
        //1
        public string DocPrefix { get; set; }
        //2
        public string DocDisc { get; set; }
        //3
        public string DocNum { get; set; }
        //4
        public string DocRevNum { get; set; }
        //5
        public string DocTitle { get; set; }
        //6
        public string DocExt { get; set; }

        public HxFilePatternToDoc6Rec(string inputFileName,
                                       string pattern = _PATTERN_MAIN_,
                                       string subPattern = _PATTERN_SUB_
        )
        {
            FileFullName = null;
            FileDirPath = null;
            FileName = null;
            FileExt = null;

            DocPrefix = null;
            DocDisc = null;
            DocNum = null;
            DocRevNum = null;
            DocTitle = null;
            DocExt = null;

            if (inputFileName.IsNullOrWhiteSpaceEx() != true)
            {
                FileFullName = HxFile.GetFileFullPath(inputFileName) ?? inputFileName;
                FileDirPath = HxFile.GetFileDirPath(FileFullName);
                FileName = HxFile.GetFileName(FileFullName);
                FileExt = HxFile.GetFileNameExt(FileFullName);

                DocPrefix = string.Empty;
                DocDisc = string.Empty;
                DocNum = string.Empty;
                DocRevNum = string.Empty;
                DocTitle = string.Empty;
                DocExt = string.Empty;
                #region //정규식을 이용한 파일룰 가져오기
                try
                {
                    var match = Regex.Match(FileName, pattern, RegexOptions.IgnoreCase);
                    if (match != null && match.Success == true && match.Value.IsNullOrWhiteSpaceEx() != true)
                    {
                        DocPrefix = match.Groups.Count >= 1 ? match.Groups[1].Value.Trim() : string.Empty;
                        DocDisc = match.Groups.Count >= 2 ? match.Groups[2].Value.Trim() : string.Empty;
                        DocNum = match.Groups.Count >= 3 ? match.Groups[3].Value.Trim() : string.Empty;
                        DocRevNum = match.Groups.Count >= 4 ? match.Groups[4].Value.Trim() : string.Empty;
                        DocTitle = match.Groups.Count >= 5 ? match.Groups[5].Value.Trim() : string.Empty;
                        DocExt = match.Groups.Count >= 6 ? match.Groups[6].Value.Trim() : string.Empty;
                        if (FileExt.ToUpper() != DocExt.ToUpper())
                        {
                            DocExt = FileExt;
                        }
                    }
                    if (DocNum.IsNullOrWhiteSpaceEx() == true && subPattern.IsNullOrWhiteSpaceEx() != true)
                    {
                        DocPrefix = string.Empty;
                        DocDisc = string.Empty;
                        DocNum = string.Empty;
                        DocRevNum = string.Empty;
                        DocTitle = string.Empty;
                        DocExt = string.Empty;
                        match = Regex.Match(FileName, subPattern);
                        if (match != null && match.Success == true && match.Value.IsNullOrWhiteSpaceEx() != true)
                        {
                            DocPrefix = match.Groups.Count >= 1 ? match.Groups[1].Value.Trim() : string.Empty;
                            DocDisc = match.Groups.Count >= 2 ? match.Groups[2].Value.Trim() : string.Empty;
                            DocNum = match.Groups.Count >= 3 ? match.Groups[3].Value.Trim() : string.Empty;
                            DocRevNum = string.Empty;
                            DocTitle = match.Groups.Count >= 4 ? match.Groups[4].Value.Trim() : string.Empty;
                            DocExt = match.Groups.Count >= 5 ? match.Groups[5].Value.Trim() : string.Empty;
                            if (FileExt.ToUpper() != DocExt.ToUpper())
                            {
                                DocExt = FileExt;
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw ex;
                }
                
                #endregion
            }
        }
    }

    
    public struct HxURIStructRec
    {
        public const string _REGEX_URI_PATTERN_ = HxDefs._REGEX_URI_PATTERN_;
        public string Protocal;
        public string Host;
        public int? Port;
        public string Path;
        public string QueryString;
        public string PageTab;
        public HxURIStructRec(bool bInit = false)
        {
            Protocal = null;
            Host = null;
            Port = null;
            Path = null;
            QueryString = null;
            PageTab = null;
        }
        public HxURIStructRec(string protocal, string host, int? port, string path, string queryString, string pageTab)
            :this()
        {
            Protocal = protocal;
            Host = host;
            Port = port;
            Path = path;
            QueryString = queryString;
            PageTab = pageTab;
        }
        public HxURIStructRec(string inputURI, int? defaultPort = null)
            :this()
        {

            System.Text.RegularExpressions.Match match = HxString.RegexMatch(inputURI, _REGEX_URI_PATTERN_, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if(match != null && match.Success == true && match.Value.IsNullOrWhiteSpaceEx() != true)
            {
                int i = 0;
                foreach(System.Text.RegularExpressions.Group grp in match.Groups)
                {
                    string strValue = grp.Value;
                    switch (i)
                    {
                        case 1:
                            this.Protocal = strValue;
                            break;
                        case 2:
                            this.Host = strValue;
                            break;
                        case 3:
                            this.Port = strValue.IsNullOrWhiteSpaceEx() == true ? defaultPort : strValue.ToNullableIntEx(defaultPort) ;
                            break;
                        case 4:
                            this.Path = strValue;
                            break;
                        case 5:
                            this.QueryString = strValue;
                            break;
                        case 6:
                            this.PageTab = strValue;
                            break;
                    }
                    i++;
                }
            }
        }
    }

    

    
}
