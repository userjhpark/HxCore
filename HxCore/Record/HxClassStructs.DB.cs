using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HxCore
{
    public struct HxDbQueryResourceRec
    {
        public string QueryString;
        public Dictionary<string, object> BindParam;
        public string WhereString;
        public string OrderByString;
        public string OptionString;
        public bool Sucess;
        public Exception CatchException;
        public string Remark;

        public HxDbQueryResourceRec(bool bInit = false)
        {
            QueryString = null;
            BindParam = null;
            WhereString = null;
            OrderByString = null;
            OptionString = null;
            Sucess = false;
            CatchException = null;
            Remark = null;
            if(bInit == true)
            {
                QueryString = string.Empty;
                BindParam = new Dictionary<string, object>();
                WhereString = string.Empty;
                OrderByString = string.Empty;
                OptionString = string.Empty;
            }
        }

        public HxDbQueryResourceRec(string queryString, Dictionary<string, object> bindParam, string whereString, string orderByString, string optionString, bool sucess, Exception catchException, string remark = null)
        {
            QueryString = queryString;
            BindParam = bindParam;
            WhereString = whereString;
            OrderByString = orderByString;
            OptionString = optionString;
            Sucess = sucess;
            CatchException = catchException;
            Remark = remark;
        }
    }

    /// <summary>
    /// Database Column Info.
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct HxDbColumnRec
    {
        /// <summary>
        /// Table Name
        /// </summary>
        public string table;    //table name
        /// <summary>
        /// Column(Field) Name
        /// </summary>
        public string name;     //field name
        /// <summary>
        /// Column(Field) Type
        /// </summary>
        public string type;     //field type
        /// <summary>
        /// Column(Field) Length
        /// </summary>
        public int len;         //field length
        /// <summary>
        /// Column(Field) flags ("NOT NULL", "INDEX")
        /// </summary>
        public string flags;    //field flags ("NOT NULL", "INDEX")
        /// <summary>
        /// precision and scale of number (eg. "10,2") or empty
        /// </summary>
        public string format;   //precision and scale of number (eg. "10,2") or empty
        /// <summary>
        /// name of index (if has one)
        /// </summary>
        public string index;    //name of index (if has one)
        /// <summary>
        /// number of chars (if any char-type)
        /// </summary>
        public int chars;    //number of chars (if any char-type)
        /// <summary>
        /// field Comments
        /// </summary>
        public string Comments; //field Comments
    }

    /// <summary>
    /// Database Table Info.
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct HxDbTableRec
    {
        /// <summary>
        /// Table Name
        /// </summary>
        public string TableName;
        /// <summary>
        /// Table Type
        /// </summary>
        public string TableType;
        /// <summary>
        /// Row(Record) Count
        /// </summary>
        public int RowCount;
        /// <summary>
        /// Column(Field) Count
        /// </summary>
        public int ColumnCount;
        /// <summary>
        /// Table Comment Or Description
        /// </summary>
        public string Comments;
    }

    /// <summary>
    /// Database Option
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct HxDbOptionRec
    {
        /// <summary>
        /// Character Set
        /// </summary>
        public string Character;
        /// <summary>
        /// Date(Time) Default Format
        /// </summary>
        public string DateFormat;
        /// <summary>
        /// Date(Time) Default Language
        /// </summary>
        public string DateLanguage;
    }
    
    public struct HxServiceHostRec
    {
        public HxServiceProviderType ProviderType;
        public string Protocol;
        public string HostName;
        public int Port;
        public string ServiceName;
        public HxServiceHostRec(bool bInit = false)
        {
            ProviderType = HxServiceProviderType.Null;
            Protocol = null;
            HostName = null;
            Port = 0;
            ServiceName = null;
        }

        public HxServiceHostRec(HxServiceProviderType providerType, string hostName, int port, string serviceName, string protocol = null)
        {
            this.ProviderType = providerType;
            this.Protocol = protocol ?? HxUtils.GetServiceProviderProtocol(providerType);
            this.HostName = hostName;
            this.Port = port;
            this.ServiceName = serviceName;
        }

        public HxServiceHostRec(HxServiceProviderType providerType, string host)
        {
            this.ProviderType = providerType;

            HxServiceHostRec rec = HxUtils.GetServiceHostInfo(host, providerType);
            this.Protocol = rec.Protocol;
            this.HostName = rec.HostName;
            this.Port = rec.Port;
            this.ServiceName = rec.ServiceName;
        }
    }
    
    public struct HxDbConnectionRec
    {
        public const string _ATTR_DB_PROVIDER_ = HxDefs._ATTR_DB_PROVIDER_;
        public const string _ATTR_DB_HOST_     = HxDefs._ATTR_DB_HOST_;
        //public const string _ATTR_DB_PORT_     = HxDefs._ATTR_DB_PORT_;
        public const string _ATTR_DB_USER_     = HxDefs._ATTR_DB_USER_;
        public const string _ATTR_DB_PASSWD_   = HxDefs._ATTR_DB_PASSWD_;
        public const string _ATTR_DB_POOLING_  = HxDefs._ATTR_DB_POOLING_;
        public const string _ATTR_DB_TITLE_    = HxDefs._ATTR_TITLE_;
        public const string _ATTR_DB_DESC_ = HxDefs._ATTR_DESC_;

        public string User;
        public string Password;
        public string HostName;
        //public int Port { get; private set; }
        public string Character; //character
        public string RemoteAddress;
        public string GlobalAddress;
        public bool? OptionModuleOpen;
        public bool? Pooling;
        public string Title;
        public string Description;
        public HxDbProviderType ProviderType;
        public string ConnectionString { get; private set; }

        public HxDbConnectionRec(string user, string password, string host, string providerType = null, string character = null, string remoteAddress = null, string globalAddress = null, bool? optionModuleOpen = null, bool? pooling = null, string title = null, string description = null)
        {
            this.User = user;
            this.Password = password;
            this.HostName = host;
            this.ProviderType = HxDbProviderType.Null;
            this.Character = character;
            this.RemoteAddress = remoteAddress;
            this.GlobalAddress = remoteAddress;
            this.OptionModuleOpen = optionModuleOpen;
            this.Pooling = pooling;
            this.Title = title;
            this.Description = description;
            if (providerType.IsNullOrWhiteSpaceEx() != true)
            {
                this.ProviderType = HxEnum.GetDbProviderType(providerType);
            }
            this.ConnectionString = HxUtils.ConnectionString(this.ProviderType, this.User, this.Password, this.HostName, this.Character, this.Pooling);
        }

        public static string GetConnectionString(HxDbConnectionRec connInfo)
        {
            return HxUtils.ConnectionString(connInfo.ProviderType, connInfo.User, connInfo.Password, connInfo.HostName, connInfo.Character, connInfo.Pooling);
        }

        public static string GetConnectionString(HxDbProviderType providerType, string userID, string password, string database, string character = null, bool? pooling = null)
        {
            return HxUtils.ConnectionString(providerType, userID, password, database, character, pooling);
        }

        internal string GetConnectionString()
        {
            return HxUtils.ConnectionString(this.ProviderType, this.User, this.Password, this.HostName, this.Character, this.Pooling);
        }

        public void SetPooling(bool? pooling)
        {
            this.Pooling = pooling;
        }

        public void SetOptionModuleOpen(bool? optionModuleOpen)
        {
            this.OptionModuleOpen = optionModuleOpen;
        }

        public static HxDbConnectionRec Create(JToken jtDbConn, string remoteAddress = null, string globalAddress = null)
        {
            HxDbConnectionRec Result;
            if (jtDbConn != null && jtDbConn[HxDbConnectionRec._ATTR_DB_HOST_].IsNullOrWhiteSpaceEx() != true)
            {
                if (remoteAddress.IsNullOrWhiteSpaceEx() == true || remoteAddress.IsRegexMatchEx(HxDefs._REGEX_IPv4_PATTERN_) != true)
                {
                    remoteAddress = HxUtils.GetUserHostAddress(true);
                }
                if (globalAddress.IsNullOrWhiteSpaceEx() == true || globalAddress.IsRegexMatchEx(HxDefs._REGEX_IPv4_PATTERN_) != true)
                {
                    remoteAddress = HxUtils.GetUserGlobalAddress(true);
                }

                Result = new HxDbConnectionRec
                {
                    ProviderType = HxEnum.GetDbProviderType(jtDbConn[_ATTR_DB_PROVIDER_].ToStringEx()),
                    HostName = jtDbConn[_ATTR_DB_HOST_].ToStringEx(),
                    User = jtDbConn[_ATTR_DB_USER_].ToStringEx(),
                    Password = jtDbConn[_ATTR_DB_PASSWD_].ToStringEx(),
                    Pooling = jtDbConn[_ATTR_DB_POOLING_]?.ToBoolEx(),
                    Title = jtDbConn[_ATTR_DB_TITLE_].ToStringEx(),
                    Description = jtDbConn[_ATTR_DB_DESC_].ToStringEx(),
                    RemoteAddress = remoteAddress,
                    GlobalAddress = globalAddress
                };
            }
            else
            {
                Result = default;
            }
            return Result;
        }
    }

    public struct HxSourceConnectionRec
    {
        public const string _MODULE_DB_NAME_                = HxDefs._MODULE_DB_NAME_;
        public const string _MODULE_API_NAME_               = HxDefs._MODULE_API_NAME_;
        public const string _MODULE_FLOW_NAME_              = HxDefs._MODULE_FLOW_NAME_;
        //public string SourceName { get; set; }

        public const string _ATTR_FILE_DOWNLOAD_            = HxDefs._ATTR_FILE_DOWNLOAD_;
        public const string _ATTR_INTRANET_PATTERN_         = HxDefs._ATTR_INTRANET_PATTERN_;
        public const string _ATTR_INTRANET_REMOTE_SERVICE_  = HxDefs._ATTR_INTRANET_REMOTE_SERVICE_;
        public const string _ATTR_EXTRANET_REMOTE_SERVICE_  = HxDefs._ATTR_EXTRANET_REMOTE_SERVICE_;

        public const string _ATTR_TITLE_                    = HxDefs._ATTR_TITLE_;
        public const string _ATTR_DESC_                     = HxDefs._ATTR_DESC_;

        public const string _CDF_NO_                        = HxDefs._ATTR_NO_;
        public const string _CDF_NAME_                      = HxDefs._ATTR_NAME_;
        public const string _CDF_DB_HOST_                   = "DB_"  + HxDbConnectionRec._ATTR_DB_HOST_;
        public const string _CDF_API_HOST_                  = "API_" + HxOpenApiJsonRec._ATTR_API_HOST_;

        public string FileDownload { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        
        public string IntranetPattern { get; set; }
        public HxRemoteServiceType IntranetRemoteService { get; set; }
        public HxRemoteServiceType ExtranetRemoteService { get; set; }


        public HxDbConnectionRec DbConnInfo { get; set; }
        public HxOpenApiJsonRec OpenApiInfo { get; set; }
        public HxFlowApiRec FlowApiInfo { get; set; }
        //public HxSourceConnectionRec() { ; ; }
        public HxSourceConnectionRec(bool bInit = false)
        {
            DbConnInfo = default;
            OpenApiInfo = default;
            FlowApiInfo = default;

            FileDownload = null;
            Title = null;
            Description = null;
            IntranetPattern = null;
            IntranetRemoteService = HxRemoteServiceType.None;
            ExtranetRemoteService = HxRemoteServiceType.None;
        }
        public HxSourceConnectionRec(HxDbConnectionRec dbConnInfo, HxOpenApiJsonRec openApiInfo, HxFlowApiRec flowApiInfo
            , string fileDownload = null, string title = null, string description = null, string intranetPattern = null, HxRemoteServiceType intranetRemoteService = HxRemoteServiceType.None, HxRemoteServiceType extranetRemoteService = HxRemoteServiceType.None)
        {
            //SourceName = name;
            DbConnInfo = dbConnInfo;
            OpenApiInfo = openApiInfo;
            FlowApiInfo = flowApiInfo;

            FileDownload = fileDownload;
            Title = title;
            Description = description;
            IntranetPattern = intranetPattern;
            IntranetRemoteService = intranetRemoteService;
            ExtranetRemoteService = extranetRemoteService;
        }

        public HxSourceConnectionRec(JToken jtFindValues, string remoteAddress = null, string globalAddress = null
            , string fileDownload = null, string title = null, string description = null, string intranetPattern = null, HxRemoteServiceType intranetRemoteService = HxRemoteServiceType.None, HxRemoteServiceType extranetRemoteService = HxRemoteServiceType.None)
        {
            //SourceName = name;
            if(jtFindValues != null)
            {
                if (remoteAddress.IsNullOrWhiteSpaceEx() == true || remoteAddress.IsRegexMatchEx(HxDefs._REGEX_IPv4_PATTERN_) != true)
                {
                    remoteAddress = HxUtils.GetUserHostAddress(true);
                }
                if (globalAddress.IsNullOrWhiteSpaceEx() == true || globalAddress.IsRegexMatchEx(HxDefs._REGEX_IPv4_PATTERN_) != true)
                {
                    remoteAddress = HxUtils.GetUserGlobalAddress(true);
                }

                JToken jtDbConn = null;
                JToken jtOpenApi = null;
                JToken jtFlowApi = null;

                foreach (JProperty jProperty in jtFindValues.Children<JProperty>())
                {
                    string name = jProperty.Name;
                    JToken jt = jProperty.Value;

                }
                DbConnInfo = HxDbConnectionRec.Create(jtDbConn, remoteAddress, globalAddress);
                OpenApiInfo = HxOpenApiJsonRec.Create(jtOpenApi, remoteAddress, globalAddress);
                FlowApiInfo = HxFlowApiRec.Create(jtFlowApi, remoteAddress, globalAddress);
            }
            else
            {
                DbConnInfo = default;
                OpenApiInfo = default;
                FlowApiInfo = default;
            }

            FileDownload = fileDownload;
            Title = title;
            Description = description;
            IntranetPattern = intranetPattern;
            IntranetRemoteService = intranetRemoteService;
            ExtranetRemoteService = extranetRemoteService;
        }

        public HxSourceConnectionRec(JToken jtDbConn, JToken jtOpenApi, JToken jtFlowApi, string remoteAddress = null, string globalAddress = null
            , string fileDownload = null, string title = null, string description = null, string intranetPattern = null, HxRemoteServiceType intranetRemoteService = HxRemoteServiceType.None, HxRemoteServiceType extranetRemoteService = HxRemoteServiceType.None)
        {
            if (remoteAddress.IsNullOrWhiteSpaceEx() == true || remoteAddress.IsRegexMatchEx(HxDefs._REGEX_IPv4_PATTERN_) != true)
            {
                remoteAddress = HxUtils.GetUserHostAddress(true);
            }
            if (globalAddress.IsNullOrWhiteSpaceEx() == true || globalAddress.IsRegexMatchEx(HxDefs._REGEX_IPv4_PATTERN_) != true)
            {
                remoteAddress = HxUtils.GetUserGlobalAddress(true);
            }
            //SourceName = name;
            DbConnInfo = HxDbConnectionRec.Create(jtDbConn, remoteAddress, globalAddress);
            OpenApiInfo = HxOpenApiJsonRec.Create(jtOpenApi, remoteAddress, globalAddress);
            FlowApiInfo = HxFlowApiRec.Create(jtFlowApi, remoteAddress, globalAddress);

            FileDownload = fileDownload;
            Title = title;
            Description = description;
            IntranetPattern = intranetPattern;
            IntranetRemoteService = intranetRemoteService;
            ExtranetRemoteService = extranetRemoteService;
        }

        public static HxSourceConnectionRec Create(JToken jtFindValues, string remoteAddress = null, string globalAddress = null
            , string fileDownload = null, string title = null, string description = null, string intranetPattern = null, HxRemoteServiceType intranetRemoteService = HxRemoteServiceType.None, HxRemoteServiceType extranetRemoteService = HxRemoteServiceType.None)
        {
            return new HxSourceConnectionRec(jtFindValues, remoteAddress, globalAddress, fileDownload, title, description, intranetPattern, intranetRemoteService, extranetRemoteService);
        }
        public static HxSourceConnectionRec Create(JToken jtDbConn, JToken jtOpenApi, JToken jtFlowApi, string remoteAddress = null, string globalAddress = null
            , string fileDownload = null, string title = null, string description = null, string intranetPattern = null, HxRemoteServiceType intranetRemoteService = HxRemoteServiceType.None, HxRemoteServiceType extranetRemoteService = HxRemoteServiceType.None)
        {
            return new HxSourceConnectionRec(jtDbConn, jtOpenApi, jtFlowApi, remoteAddress, globalAddress, fileDownload, title, description, intranetPattern, intranetRemoteService, extranetRemoteService);
        }
        public static Dictionary<string, HxSourceConnectionRec> GetSourceList(JToken jtValues, string remoteAddress = null, string globalAddress = null)
        {
            Dictionary<string, HxSourceConnectionRec> Result = new Dictionary<string, HxSourceConnectionRec>();
            if(jtValues != null)
            {
                HxSourceConnectionRec rec = new HxSourceConnectionRec();
                foreach (JProperty jProperty in jtValues.Children<JProperty>())
                {
                    string name = jProperty.Name;
                    if(name.IsNullOrWhiteSpaceEx() != true)
                    {
                        JToken jt = jProperty.Value;
                        if (jt != null)
                        {
                            rec.FileDownload = jt[_ATTR_FILE_DOWNLOAD_].ToStringEx();
                            rec.Title = jt[_ATTR_TITLE_].ToStringEx();
                            rec.Description = jt[_ATTR_DESC_].ToStringEx();
                            rec.IntranetPattern = jt[_ATTR_INTRANET_PATTERN_].ToStringEx();
                            rec.IntranetRemoteService = HxType.GetRemoteServiceType(jt[_ATTR_INTRANET_REMOTE_SERVICE_].ToStringEx());
                            rec.ExtranetRemoteService = HxType.GetRemoteServiceType(jt[_ATTR_EXTRANET_REMOTE_SERVICE_].ToStringEx());
                            if(rec.ExtranetRemoteService != HxRemoteServiceType.DirectDb && rec.ExtranetRemoteService != HxRemoteServiceType.WebApi)
                            {
                                rec.ExtranetRemoteService = rec.IntranetRemoteService;
                            }

                            if (remoteAddress.IsNullOrWhiteSpaceEx() == true || remoteAddress.IsRegexMatchEx(HxDefs._REGEX_IPv4_PATTERN_) != true)
                            {
                                remoteAddress = HxUtils.GetUserHostAddress(true);
                            }
                            if (globalAddress.IsNullOrWhiteSpaceEx() == true || globalAddress.IsRegexMatchEx(HxDefs._REGEX_IPv4_PATTERN_) != true)
                            {
                                remoteAddress = HxUtils.GetUserGlobalAddress(true);
                            }

                            if (jt[_MODULE_DB_NAME_].IsNullOrWhiteSpaceEx() != true)
                            {
                                rec.DbConnInfo = HxDbConnectionRec.Create(jt[_MODULE_DB_NAME_], remoteAddress, globalAddress);
                            }
                            if (jt[_MODULE_API_NAME_].IsNullOrWhiteSpaceEx() != true)
                            {
                                rec.OpenApiInfo = HxOpenApiJsonRec.Create(jt[_MODULE_API_NAME_], remoteAddress, globalAddress);
                            }
                            if (jt[_MODULE_FLOW_NAME_].IsNullOrWhiteSpaceEx() != true)
                            {
                                rec.FlowApiInfo = HxFlowApiRec.Create(jt[_MODULE_FLOW_NAME_], remoteAddress, globalAddress);
                            }
                        }
                        Result.AddEx(name, rec);
                    }
                }
            }
            return Result;
        }
    }

    

}
